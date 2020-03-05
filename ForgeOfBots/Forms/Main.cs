using CefSharp;
//using CefSharp.OffScreen;
using CefSharp.WinForms;
using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.DataHandler;
using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses;
using Newtonsoft.Json.Linq;
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
      public BotData BotData = new BotData();
      public SettingData SettingData = new SettingData();
      public Browser Browser = null;

      public int CurrentState = 0;
      public string ForgeHX_FilePath = "";
      public bool LoginLoaded = false, ForgeHXLoaded = false, UIDLoaded = false;

      private bool blockedLogin = false;

      public Main()
      {
         InitializeComponent();

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

         cwb = new ChromiumWebBrowser("https://de.forgeofempires.com/")
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
         //cwb.JavascriptObjectRepository.Register("jsInterface", jsInterface, true, BindingOptions.DefaultBinder);
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
         //Browser.Hide();
      }

      private void Cwb_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
      {
         cwb.ShowDevTools();
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
         if (!blockedLogin)
         {
            Log(lbLog, "[DEBUG] Doing Login");
            blockedLogin = true;
            Thread.Sleep(500);
            string loginJS = resMgr.GetString("preloadLoginWorld");
            loginJS = loginJS.Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()]).Replace("###USERNAME###", "JohnnyWalker").Replace("###PASSWORD###", "Carlo1509!?");
            CustomResourceRequestHandler.ServerStartpageLoadedEvent += ServerStartupLoaded_Event;
            cwb.ExecuteScriptAsync(loginJS);
         }
      }

      private void ServerStartupLoaded_Event()
      {
         Thread.Sleep(500);
         string loginJS = resMgr.GetString("preloadLoginWorld");
         loginJS = loginJS.Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()]).Replace("###USERNAME###", "JohnnyWalker").Replace("###PASSWORD###", "Carlo1509!?");
         CustomResourceRequestHandler.ForgeHXFoundEvent += ForgeHXFound_Event;
         CustomResourceRequestHandler.UidFoundEvent += UidFound_Event;
         cwb.ExecuteScriptAsync(loginJS);
      }

      private void UidFound_Event(string uid,string wid)
      {
         if (UIDLoaded) return;
         Log(lbLog, "[DEBUG] UID loaded");
         UIDLoaded = true;
         CurrentState = 1;
         BotData.UID = uid;
         BotData.WID = wid;
      }

      private void ForgeHXFound_Event(string forgehx, string filename)
      {
         if (ForgeHXLoaded) return;
         Log(lbLog, "[DEBUG] Checking ForgeHX File");
         ForgeHXLoaded = true;
         BotData.ForgeHX = filename;
         var AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
         var ProgramPath = Path.Combine(AppDataPath, Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().ManifestModule.Name));
         ForgeHX_FilePath = Path.Combine(ProgramPath, filename);
         if (!Directory.Exists(ProgramPath)) Directory.CreateDirectory(ProgramPath);
         FileInfo fi = new FileInfo(ForgeHX_FilePath);
         Log(lbLog, "[DEBUG] Checking if ForgeHX exists");
         if (!fi.Exists || fi.Length <= 0)
         {
            Log(lbLog, "[DEBUG] Loading new ForgeHX");
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
            Log(lbLog, "[DEBUG] Reading ForgeHX");
            var content = File.ReadAllText(ForgeHX_FilePath);
            var regExSecret = new Regex("\\.VERSION_SECRET=\"([a-zA-Z0-9_\\-\\+\\/==]+)\";", RegexOptions.IgnoreCase);
            var regExVersion = new Regex("\\.VERSION_MAJOR_MINOR=\"([0-9+.0-9+.0-9+]+)\";", RegexOptions.IgnoreCase);
            var VersionMatch = regExVersion.Match(content);
            var SecretMatch = regExSecret.Match(content);
            if (VersionMatch.Success)
            {
               SettingData.Version = VersionMatch.Groups[1].Value;
               Log(lbLog, "[DEBUG] Version found: " + SettingData.Version);
            }
            if (SecretMatch.Success)
            {
               SettingData.Version_Secret = SecretMatch.Groups[1].Value;
               Log(lbLog, "[DEBUG] Version Secret found: " + SettingData.Version_Secret);
            }
         }
      }

      private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
      {
         Log(lbLog, "[DEBUG] done Downloading ForgeHX");
         var content = File.ReadAllText(ForgeHX_FilePath);
         var regExSecret = new Regex("\\.VERSION_SECRET=\"([a-zA-Z0-9_\\-\\+\\/==]+)\";", RegexOptions.IgnoreCase);
         var regExVersion = new Regex("\\.VERSION_MAJOR_MINOR=\"([0-9+.0-9+.0-9+]+)\";", RegexOptions.IgnoreCase);
         var VersionMatch = regExVersion.Match(content);
         var SecretMatch = regExSecret.Match(content);
         if (VersionMatch.Success)
         {
            SettingData.Version = VersionMatch.Groups[1].Value;
            Log(lbLog, "[DEBUG] Version found: " + SettingData.Version);
         }
         if (SecretMatch.Success)
         {
            SettingData.Version_Secret = SecretMatch.Groups[1].Value;
            Log(lbLog, "[DEBUG] Version Secret found: " + SettingData.Version_Secret);
         }
         BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 0));
         BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = "IDLE"));
      }

      private void button1_Click(object sender, EventArgs e)
      {
         RequestBuilder.User_Key = BotData.UID;
         RequestBuilder.VersionSecret = SettingData.Version_Secret;
         RequestBuilder.Version = SettingData.Version;
         RequestBuilder.WorldID = BotData.WID;
         RequestBuilder Startup = new RequestBuilder();
         var script = Startup.GetRequestScript(RequestType.Startup, "[]");
         cwb.ExecuteScriptAsync(script);
      }

      private void Client_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
      {
         BeginInvoke(new MethodInvoker(() => tspbProgress.Value = e.ProgressPercentage));
      }

      private void Main_FormClosed(object sender, FormClosedEventArgs e)
      {
         Log(lbLog, "[DEBUG] Closing App");
         Cef.Shutdown();
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
