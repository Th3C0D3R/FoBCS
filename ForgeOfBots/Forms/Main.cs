using CefSharp;
//using CefSharp.OffScreen;
using CefSharp.WinForms;
using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.DataHandler;
using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses;
using ForgeOfBots.Utils;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static CefSharp.LogSeverity;
using static ForgeOfBots.Utils.Helper;
using WebClient = System.Net.WebClient;

namespace ForgeOfBots
{
   public partial class Main : Form
   {
      public ChromiumWebBrowser cwb;
      public static ResourceManager resMgr = new ResourceManager("ForgeOfBots.Properties.Resources", Assembly.GetExecutingAssembly());
      public Dictionary<string, string> AllCookies = new Dictionary<string, string>();
      public RequestBuilder ReqBuilder = new RequestBuilder();
      public BotData BotData = new BotData();
      public SettingData SettingData = new SettingData();
      public Settings UserData;
      UserDataInput usi = null;
      public Browser Browser = null;
      public static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      public static string ProgramPath = Path.Combine(AppDataPath, Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().ManifestModule.Name));

      public int CurrentState = 0;
      public string ForgeHX_FilePath = "";
      public bool LoginLoaded = false, ForgeHXLoaded = false, UIDLoaded = false, SECRET_LOADED = false;

      private bool blockedLogin = false;

      public Main()
      {
         InitializeComponent();
         Log("[DEBUG] Starting Bot");
         Browser = new Browser(this);
         Browser.Show();

         var settings = new CefSettings
         {
            LogSeverity = Verbose,
            CachePath = "cache",
            PersistSessionCookies = false,
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0",
            WindowlessRenderingEnabled = true,

         };
         Cef.Initialize(settings);

         var browserSettings = new BrowserSettings
         {
            DefaultEncoding = "UTF-8",
            Javascript = CefState.Enabled,
            JavascriptAccessClipboard = CefState.Enabled,
            LocalStorage = CefState.Enabled,
            WebSecurity = CefState.Disabled,
         };
         Log("[DEBUG] Browser Settings and Cef Settings loaded");

         if (Settings.SettingsExists())
            UserData = Settings.ReadSettings();
         else
         {
            Log("[DEBUG] No Userdata found, creating new one!");
            UserData = new Settings();
            usi = new UserDataInput(ListClass.ServerList);
            usi.UserDataEntered += Usi_UserDataEntered;
            usi.ShowDialog();
            Browser.Hide();
            return;
         }
         ContinueExecution();
         Browser.Hide();
      }

      private void ContinueExecution()
      {
         cwb = new ChromiumWebBrowser("https://" + UserData.WorldServer + ".forgeofempires.com/")
         {
            RequestHandler = new CustomRequestHandler(),
            MenuHandler = new CustomMenu(),
            Dock = DockStyle.Fill,
         };
         ResponseHandler.browser = cwb;
         var jsInterface = new jsMapInterface
         {
            onHookEvent = ResponseHandler.HookEventHandler
         };
         cwb.JavascriptObjectRepository.ResolveObject += (sender, e) =>
         {
            var repo = e.ObjectRepository;
            if (e.ObjectName == "jsInterface")
            {
               BindingOptions bindingOptions = BindingOptions.DefaultBinder;
               repo.Register("jsInterface", new jsMapInterface() { onHookEvent = ResponseHandler.HookEventHandler }, isAsync: true, options: bindingOptions);
            }
         };
         cwb.FrameLoadEnd += Cwb_FrameLoadEnd;
         Browser.Controls.Add(cwb);
      }

      private void Usi_UserDataEntered(string username, string password, string server)
      {
         Log("[DEBUG] Userdata loaded");
         UserData.Username = username;
         UserData.Password = password;
         UserData.WorldServer = server;
         usi.Close();
         ContinueExecution();
      }

      private void Cwb_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
      {
         //cwb.ShowDevTools();
         CookieHandler.CookiesLoaded += OnCookiesLoaded;
         CookieHandler.GetCookies();
      }

      private void OnCookiesLoaded(object sender, CookieLoadedEventArgs e)
      {
         AllCookies = e.AllCookies;
         if (AllCookies.ContainsKey("CID".ToLower()))
         {
            BotData.CID = AllCookies["CID".ToLower()];
         }
         if (AllCookies.ContainsKey("CSRF".ToLower()))
         {
            BotData.CSRF = AllCookies["CSRF".ToLower()];
         }
         if (AllCookies.ContainsKey("SID".ToLower()))
         {
            BotData.SID = AllCookies["SID".ToLower()];
         }
         if (AllCookies.ContainsKey("XSRF-TOKEN".ToLower()))
         {
            BotData.XSRF = AllCookies["XSRF-TOKEN".ToLower()];
         }
      }

      private void ServerStartupLoaded_Event()
      {
         Thread.Sleep(500);
         ResponseHandler.WorldsLoaded += ResponseHandler_WorldsLoaded;
         string loginJS = resMgr.GetString("preloadLoginWorld");
         string loginCity = "false";
         if (UserData.LastWorld == null)
            loginCity = "true";

         loginJS = loginJS
            .Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()])
            .Replace("###USERNAME###", UserData.Username)
            .Replace("###PASSWORD###", UserData.Password)
            .Replace("##server##", UserData.WorldServer)
            .Replace("##t##", loginCity)
            .Replace("##city##", "\"" + UserData.LastWorld + "\"");
         cwb.ExecuteScriptAsync(loginJS);
      }

      private void ResponseHandler_WorldsLoaded(object sender)
      {
         if (ListClass.WorldList.Count > 0)
         {
            UserData.PlayableWorlds = ListClass.WorldList.ConvertAll(new Converter<Tuple<string, string, WorldState>, string>(WorldToPlayable));
            WorldSelection ws = new WorldSelection(ListClass.WorldList);
            ws.WorldDataEntered += Ws_WorldDataEntered;
            ws.ShowDialog();
         }
      }

      private void Ws_WorldDataEntered(Form ws, string key, string value)
      {
         UserData.LastWorld = key;
         string loginJS = resMgr.GetString("preloadLoginWorld");
         loginJS = loginJS
            .Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()])
               .Replace("###USERNAME###", UserData.Username)
               .Replace("###PASSWORD###", UserData.Password)
               .Replace("##server##", UserData.WorldServer)
               .Replace("##city##", "\"" + UserData.LastWorld + "\"")
            .Replace("##t##", "false");
         CustomResourceRequestHandler.ForgeHXFoundEvent += ForgeHXFound_Event;
         CustomResourceRequestHandler.UidFoundEvent += UidFound_Event;
         cwb.ExecuteScriptAsync(loginJS);
         ws.Close();
      }

      private void UidFound_Event(string uid, string wid)
      {
         if (UIDLoaded) return;
         Log("[DEBUG] UID loaded");
         UIDLoaded = true;
         CurrentState = 1;
         BotData.UID = uid;
         BotData.WID = wid;
         UserData.SettingsSaved += UserData_SettingsSaved;
         UserData.SaveSettings();
         if (SECRET_LOADED)
         {
            LoadWorlds();
         }
      }

      private void UserData_SettingsSaved(Settings e)
      {
         Log("[DEBUG] UserData saved");
      }

      private void ForgeHXFound_Event(string forgehx, string filename)
      {
         if (ForgeHXLoaded) return;
         Log("[DEBUG] Checking ForgeHX File");
         ForgeHXLoaded = true;
         BotData.ForgeHX = filename;
         ForgeHX_FilePath = Path.Combine(ProgramPath, filename);
         if (!Directory.Exists(ProgramPath)) Directory.CreateDirectory(ProgramPath);
         FileInfo fi = new FileInfo(ForgeHX_FilePath);
         Log("[DEBUG] Checking if ForgeHX exists");
         if (!fi.Exists || fi.Length <= 0)
         {
            Log("[DEBUG] Loading new ForgeHX");
            using (var client = new WebClient())
            {
               Uri uri = new Uri(forgehx.Replace("'", ""));
               client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0");
               client.DownloadProgressChanged += Client_DownloadProgressChanged;
               client.DownloadFileCompleted += Client_DownloadFileCompleted;
               BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = "Downloading " + filename));
               BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 0));
               client.DownloadFileAsync(uri, ForgeHX_FilePath);
            }
         }
         else
         {
            Log("[DEBUG] Reading ForgeHX");
            var content = File.ReadAllText(ForgeHX_FilePath);
            var regExSecret = new Regex("\\.VERSION_SECRET=\"([a-zA-Z0-9_\\-\\+\\/==]+)\";", RegexOptions.IgnoreCase);
            var regExVersion = new Regex("\\.VERSION_MAJOR_MINOR=\"([0-9+.0-9+.0-9+]+)\";", RegexOptions.IgnoreCase);
            var VersionMatch = regExVersion.Match(content);
            var SecretMatch = regExSecret.Match(content);
            if (VersionMatch.Success)
            {
               SettingData.Version = VersionMatch.Groups[1].Value;
               Log("[DEBUG] Version found: " + SettingData.Version);
            }
            if (SecretMatch.Success)
            {
               SettingData.Version_Secret = SecretMatch.Groups[1].Value;
               Log("[DEBUG] Version Secret found: " + SettingData.Version_Secret);
               SECRET_LOADED = true;
               if (UIDLoaded)
               {
                  LoadWorlds();
               }
            }
         }
      }

      private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
      {
         Log("[DEBUG] done Downloading ForgeHX");
         var content = File.ReadAllText(ForgeHX_FilePath);
         var regExSecret = new Regex("\\.VERSION_SECRET=\"([a-zA-Z0-9_\\-\\+\\/==]+)\";", RegexOptions.IgnoreCase);
         var regExVersion = new Regex("\\.VERSION_MAJOR_MINOR=\"([0-9+.0-9+.0-9+]+)\";", RegexOptions.IgnoreCase);
         var VersionMatch = regExVersion.Match(content);
         var SecretMatch = regExSecret.Match(content);
         if (VersionMatch.Success)
         {
            SettingData.Version = VersionMatch.Groups[1].Value;
            Log("[DEBUG] Version found: " + SettingData.Version);
         }
         if (SecretMatch.Success)
         {
            SettingData.Version_Secret = SecretMatch.Groups[1].Value;
            Log("[DEBUG] Version Secret found: " + SettingData.Version_Secret);
            SECRET_LOADED = true;
            if (UIDLoaded)
            {
               LoadWorlds();
            }
         }
         BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 0));
         BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = "IDLE"));
      }

      private void tsbLogin_Click(object sender, EventArgs e)
      {
         if (!blockedLogin)
         {
            Log("[DEBUG] Doing Login");
            blockedLogin = true;
            Thread.Sleep(500);
            CustomResourceRequestHandler.ServerStartpageLoadedEvent += ServerStartupLoaded_Event;
            string loginJS = resMgr.GetString("preloadLoginWorld");
            loginJS = loginJS
               .Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()])
               .Replace("###USERNAME###", UserData.Username)
               .Replace("###PASSWORD###", UserData.Password)
               .Replace("##server##", UserData.WorldServer)
               .Replace("##t##", "false")
            .Replace("##city##", UserData.LastWorld);
            CustomResourceRequestHandler.ForgeHXFoundEvent += ForgeHXFound_Event;
            CustomResourceRequestHandler.UidFoundEvent += UidFound_Event;
            cwb.ExecuteScriptAsync(loginJS);
         }
      }

      private void Client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
      {
         BeginInvoke(new MethodInvoker(() => tspbProgress.Value = e.ProgressPercentage));
      }

      private void tsbSettings_Click(object sender, EventArgs e)
      {
         reloadData();
      }

      private void Main_FormClosed(object sender, FormClosedEventArgs e)
      {
         Log("[DEBUG] Closing App");
         Cef.Shutdown();
      }
      private void LoadWorlds()
      {
         ReqBuilder.User_Key = BotData.UID;
         ReqBuilder.VersionSecret = SettingData.Version_Secret;
         ReqBuilder.Version = SettingData.Version;
         ReqBuilder.WorldID = BotData.WID;
         string script = ReqBuilder.GetRequestScript(RequestType.GetAllWorlds, "[]");
         cwb.ExecuteScriptAsync(script);
         reloadData();
      }
      public void reloadData()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetFriends, "[]");
         cwb.ExecuteScriptAsync(script);
         script = ReqBuilder.GetRequestScript(RequestType.GetClanMember, "[]");
         cwb.ExecuteScriptAsync(script);
         script = ReqBuilder.GetRequestScript(RequestType.GetNeighbor, "[]");
         cwb.ExecuteScriptAsync(script);
      }
   }
   public static class CookieHandler
   {
      public static ICookieManager CookieManager { private set; get; }
      public static Dictionary<string, string> _allCookies = new Dictionary<string, string>();
      public static void GetCookies()
      {
         CookieManager = Cef.GetGlobalCookieManager();
         var visitor = new CookieMonster(allCookies =>
         {
            //BeginInvoke(new MethodInvoker(() => lbCookies.Items.Clear()));
            foreach (var item in allCookies)
            {
               if (item.Item1 == null) continue;
               if (item.Item2 == null) continue;
               if (_allCookies.ContainsKey(item.Item1.ToLower()))
               {
                  if (_allCookies[item.Item1.ToLower()] != item.Item2)
                     _allCookies[item.Item1.ToLower()] = item.Item2;
               }
               else
                  _allCookies.Add(item.Item1.ToLower(), item.Item2);
            }
            OnCookiesLoaded(new CookieLoadedEventArgs() { AllCookies = _allCookies });
         });
         CookieManager.VisitAllCookies(visitor);
      }

      public static void OnCookiesLoaded(CookieLoadedEventArgs e)
      {
         CookiesLoaded?.Invoke("CookieHandler", e);
      }

      public static event CookieLoadedEventHandler CookiesLoaded;
   }
   public delegate void CookieLoadedEventHandler(object sender, CookieLoadedEventArgs e);
   class CookieMonster : ICookieVisitor
   {
      readonly List<Tuple<string, string>> cookies = new List<Tuple<string, string>>();
      readonly Action<IEnumerable<Tuple<string, string>>> useAllCookies;

      public CookieMonster(Action<IEnumerable<Tuple<string, string>>> useAllCookies)
      {
         this.useAllCookies = useAllCookies;
      }

      public void Dispose()
      {

      }

      public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
      {
         cookies.Add(new Tuple<string, string>(cookie.Name, cookie.Value));

         if (count == total - 1)
            useAllCookies(cookies);

         return true;
      }
   }
   public class CustomMenu : IContextMenuHandler
   {
      public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
      {
         return;
      }

      public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
      {
         return false;
      }

      public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
      {
         return;
      }

      public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
      {
         return false;
      }
   }
   public class CookieLoadedEventArgs : EventArgs
   {
      public Dictionary<string, string> AllCookies { get; set; }
   }
}
