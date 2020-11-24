using CefSharp;
#if RELEASE || DEBUG
using CefSharp.OffScreen;
#elif DEBUGFORM
using CefSharp.WinForms;
#endif
using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.DataHandler;
using ForgeOfBots.Forms;
using ForgeOfBots.Forms.UserControls;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using MetroFramework.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
//using Windows.UI.Notifications;
using AntiDebugging;

using static CefSharp.LogSeverity;
using static ForgeOfBots.Utils.Helper;
using static System.Windows.Forms.ListViewItem;

using CUpdate = ForgeOfBots.DataHandler.Update;
using Settings = ForgeOfBots.Utils.Settings;
using WebClient = System.Net.WebClient;
using WorldSelection = ForgeOfBots.Forms.WorldSelection;
using ForgeOfBots.Utils.Premium;
using UCPremiumLibrary;

namespace ForgeOfBots
{
   public partial class Main : Form
   {
      public static ChromiumWebBrowser cwb = null;
      public static ResourceManager resMgr = new ResourceManager("ForgeOfBots.Properties.Resources", Assembly.GetExecutingAssembly());
      public static Dictionary<string, string> AllCookies = new Dictionary<string, string>();
      public static RequestBuilder ReqBuilder = new RequestBuilder();
      public static CUpdate Updater;
      public static BotData BotData = new BotData();
      public static Log LogWnd;
      public static SettingData SettingData = new SettingData();
      public static Settings UserData;
      public static Assembly PremAssembly = null;
      public static Version Version = Assembly.GetExecutingAssembly().GetName().Version;
      public static object PremInstance = null;
      private UserDataInput usi = null;
      public static Browser Browser = null;
      public static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      public static string ProgramPath = Path.Combine(AppDataPath, Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().ManifestModule.Name));
      public static Stopwatch RunningTime = new Stopwatch();
      public int CurrentState = 0;
      public string ForgeHX_FilePath = "";
      public bool LoginLoaded = false, ForgeHXLoaded = false, UIDLoaded = false, SECRET_LOADED = false;
      private bool blockedLogin = false;
      private bool blockExpireBox = false;
      public static bool DEBUGMODE = false;
      static readonly object _locker = new object();

      private int xcounter = 0;

      public Main(string[] args)
      {
#if RELEASE
         AntiDebug.HideOsThreads();
         if (AntiDebug.CheckDebuggerManagedPresent() ||
            AntiDebug.CheckDebuggerUnmanagedPresent() ||
            AntiDebug.CheckDebugPort() ||
            AntiDebug.CheckKernelDebugInformation() ||
            AntiDebug.CheckRemoteDebugger())
         {
            System.Timers.Timer stimer = new System.Timers.Timer();
            stimer.Enabled = true;
            stimer.Interval = 1000;
            stimer.Elapsed += Stimer_Elapsed;
            stimer.Start();
         }
#endif
         if (args != null)
         {
            if (args.Length > 0)
               if (args[0].StartsWith("/"))
                  if (args[0].Substring(1).ToLower().Equals("debug"))
                     DEBUGMODE = true;
         }
         if (Settings.SettingsExists())
         {
            UserData = Settings.ReadSettings();
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
         }
         else
         {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en");
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en");
         }
         InitializeComponent();
         Application.ApplicationExit += handleExit;
         RunningTime.Start();
         bwTimerUpdate.RunWorkerAsync();
         Log("[DEBUG] Starting Bot", lbOutputWindow);
         Browser = new Browser(this);
         Browser.Show();
         CefSettings settings = null;
         BrowserSettings browserSettings = null;

         if (!Settings.SettingsExists())
         {
            Log("[DEBUG] No Userdata found, creating new one!", lbOutputWindow);
            UserData = new Settings();
            settings = new CefSettings
            {
               LogSeverity = Verbose,
               CachePath = Path.GetFullPath("cache"),
               PersistSessionCookies = false,
               UserAgent = UserData.CustomUserAgent,
               WindowlessRenderingEnabled = true,

            };
            Cef.Initialize(settings);
            browserSettings = new BrowserSettings
            {
               DefaultEncoding = "UTF-8",
               Javascript = CefState.Enabled,
               JavascriptAccessClipboard = CefState.Enabled,
               LocalStorage = CefState.Enabled,
               WebSecurity = CefState.Disabled,
            };
#if RELEASE || DEBUG
            Browser.Hide();
#endif
            usi = new UserDataInput(ListClass.ServerList);
            usi.UserDataEntered += Usi_UserDataEntered;
            DialogResult dlgRes = usi.ShowDialog();
            if (dlgRes == DialogResult.Cancel) Environment.Exit(0);
            Invoker.SetProperty(pnlLoading, () => pnlLoading.Visible, true);
            ResponseHandler.EverythingImportantLoaded += ResponseHandler_EverythingImportantLoaded;
            Updater = new CUpdate(cwb, ReqBuilder);
            Text = Tag.ToString() + $"{Version.Major}.{Version.Minor} | by TH3C0D3R";
            LogWnd = new Log(new Point(Location.X + Size.Width, Location.Y));
            ToggleGUI(false);
            pnlLoading.BringToFront();
            return;
         }
         else
         {
            settings = new CefSettings
            {
               LogSeverity = Verbose,
               CachePath = Path.GetFullPath("cache"),
               PersistSessionCookies = false,
               UserAgent = UserData.CustomUserAgent,
               WindowlessRenderingEnabled = true,

            };
            Cef.Initialize(settings);

            browserSettings = new BrowserSettings
            {
               DefaultEncoding = "UTF-8",
               Javascript = CefState.Enabled,
               JavascriptAccessClipboard = CefState.Enabled,
               LocalStorage = CefState.Enabled,
               WebSecurity = CefState.Disabled,
            };
            Log("[DEBUG] Browser Settings and Cef Settings loaded", lbOutputWindow);

            ContinueExecution();
#if RELEASE || DEBUG
            Browser.Hide();
#endif
            Invoker.SetProperty(pnlLoading, () => pnlLoading.Visible, true);
            ResponseHandler.EverythingImportantLoaded += ResponseHandler_EverythingImportantLoaded;
            Updater = new CUpdate(cwb, ReqBuilder);
            Text = Tag.ToString() + $"{Version.Major}.{Version.Minor} | by TH3C0D3R";
            LogWnd = new Log(new Point(Location.X + Size.Width, Location.Y));
            ToggleGUI(false);
            pnlLoading.BringToFront();
         }
      }

      readonly int counter = 10;
      private void Stimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         for (int i = 0; i < counter; i++)
         {
            Thread.Sleep(1000);
         }
         AntiDebug.DetachFromDebuggerProcess();
         System.Timers.Timer t = (System.Timers.Timer)sender;
         t.Enabled = false;
         Application.Exit();
      }
      private void ResponseHandler_EverythingImportantLoaded(object sender)
      {
         LoadGUI();
         ToggleGUI(true, true);
         Invoker.SetProperty(pnlLoading, () => pnlLoading.Visible, false);
      }
      private void ContinueExecution()
      {
         cwb = new ChromiumWebBrowser("https://" + UserData.WorldServer + "0.forgeofempires.com/")
         {
            RequestHandler = new CustomRequestHandler(),
            MenuHandler = new CustomMenu(),
#if DEBUGFORM
            Dock = DockStyle.Fill,
#endif
         };
         cwb.FrameLoadEnd += Cwb_FrameLoadEnd;
         ResponseHandler.browser = cwb;
         cwb.JavascriptObjectRepository.ResolveObject += (sender, e) =>
         {
            var repo = e.ObjectRepository;
            if (e.ObjectName == "jsInterface")
            {
               BindingOptions bindingOptions = BindingOptions.DefaultBinder;
               repo.Register("jsInterface", new jsMapInterface() { onHookEvent = ResponseHandler.HookEventHandler }, isAsync: true, options: bindingOptions);
            }
         };
#if DEBUGFORM
      Browser.Controls.Add(cwb);
#endif
      }
      private void Usi_UserDataEntered(string username, string password, string server)
      {
         Log("[DEBUG] Userdata loaded", lbOutputWindow);
         UserData.Username = username;
         UserData.Password = password;
         UserData.WorldServer = server;
         UserData.SaveSettings();
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
            if (!blockedLogin)
            {
               Log("[DEBUG] All Cookies loaded", lbOutputWindow);
               if (!UserData.AutoLogin)
               {
                  Invoker.SetProperty(lblPleaseLogin, () => lblPleaseLogin.Text, strings.PleaseLoginNow);
                  Invoker.SetProperty(pictureBox1, () => pictureBox1.Visible, false);
                  ToggleGUI(true, true);
               }
               else
                  tsbLogin_Click(this, null);
               if (!string.IsNullOrWhiteSpace(UserData.SerialKey))
               {
                  BackgroundWorker bwPremium = new BackgroundWorker();
                  bwPremium.DoWork += CheckPremium;
                  bwPremium.RunWorkerCompleted += CheckPremiumComplete;
                  bwPremium.RunWorkerAsync();
               }
            }
            CustomResourceRequestHandler.ServerStartpageLoadedEvent += ServerStartupLoaded_Event;
         }
      }
      private void CheckPremiumComplete(object sender, RunWorkerCompletedEventArgs e)
      {
         if (e.Result is bool)
         {
            bool.TryParse(e.Result.ToString(), out bool clearControls);
            if (!clearControls)
            {
               Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Clear());
            }
         }
      }
      private void CheckPremium(object sender, DoWorkEventArgs e)
      {
         object ret = TcpConnection.SendAuthData(UserData.SerialKey, FingerPrint.Value(), false);
         if (ret is Result)
         {
            Enum.TryParse(ret.ToString(), out Result result);
            if (result == Result.Success)
            {
               Invoker.SetProperty(this, () => Text, Tag.ToString() + $"{Version.Major}.{Version.Minor} (PREMIUM) | by TH3C0D3R");
               object retList = ExecuteMethod(PremAssembly, "EntryPoint", "AddPremiumControl", null);
               if (retList is List<UCPremium> list)
               {
                  Invoker.SetProperty(this, () => Text, Tag.ToString() + $"{Version.Major}.{Version.Minor} (PREMIUM) | by TH3C0D3R");
                  e.Result = true;
               }
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(this, () => Text, Tag.ToString() + $"{Version.Major}.{Version.Minor} (EXPIRED) | by TH3C0D3R");
               if (!blockExpireBox)
               {
                  DialogResult dlgRes = MessageBox.Show(Owner, "Your Subscription is expired.\n\nPlease purchase a new Subscribtion and acitvate it\nPress 'Yes' to open the Store\n\nelse you are continue using the Free-Version\nPress 'No' for the Free Version", "SUBSCRIPTION EXPIRED", MessageBoxButtons.YesNo);
                  if (dlgRes == DialogResult.Yes)
                  {
                     Process.Start("https://th3c0d3r.selly.store/");
                     e.Result = false;
                  }
                  else if (dlgRes == DialogResult.No)
                  {
                     UserData.SerialKey = "";
                     UserData.SaveSettings();
                     e.Result = false;
                  }
               }
               blockExpireBox = true;
            }
            else
            {
               Invoker.SetProperty(this, () => Text, Tag.ToString() + $"{Version.Major}.{Version.Minor} | by TH3C0D3R");
               MessageBox.Show(Owner, "There was a problem using this Serial-Key with your Device!!\n\nEither you activated the Serial-Key on a other Device\nor the Serial-Key is not valid!", "FAILED TO ACTIVATE");
            }
         }
      }
      private void ServerStartupLoaded_Event(object sender, EventArgs e)
      {
         Thread.Sleep(500);
         ResponseHandler.WorldsLoaded += ResponseHandler_WorldsLoaded;
         string loginJS = resMgr.GetString("preloadLoginWorld");
         string loginCity = "false";
         if (UserData.LastWorld == null || string.IsNullOrWhiteSpace(UserData.LastWorld))
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
      private void UidFound_Event(object sender, TwoStringArgs args)
      {
         if (UIDLoaded) return;
         Log("[DEBUG] UID loaded", lbOutputWindow);
         UIDLoaded = true;
         CurrentState = 1;
         BotData.UID = args.s1;
         BotData.WID = args.s2;
         UserData.SettingsSaved += UserData_SettingsSaved;
         UserData.SaveSettings();
         if (SECRET_LOADED)
         {
            LoadWorlds();
         }
      }
      private void UserData_SettingsSaved(object sender, OneTArgs<Settings> args)
      {
         Log("[DEBUG] UserData saved", lbOutputWindow);
      }
      private void ForgeHXFound_Event(object sender, TwoStringArgs args)
      {
         if (ForgeHXLoaded) return;
         Log("[DEBUG] Checking ForgeHX File", lbOutputWindow);
         ForgeHXLoaded = true;
         BotData.ForgeHX = args.s2;
         ForgeHX_FilePath = Path.Combine(ProgramPath, args.s2);
         if (!Directory.Exists(ProgramPath)) Directory.CreateDirectory(ProgramPath);
         FileInfo fi = new FileInfo(ForgeHX_FilePath);
         Log("[DEBUG] Checking if ForgeHX exists", lbOutputWindow);
         if (!fi.Exists || fi.Length <= 0)
         {
            Log("[DEBUG] Loading new ForgeHX", lbOutputWindow);
            using (var client = new WebClient())
            {
               Uri uri = new Uri(args.s1.Replace("'", ""));
               client.Headers.Add("User-Agent", UserData.CustomUserAgent);
               client.DownloadProgressChanged += Client_DownloadProgressChanged;
               client.DownloadFileCompleted += Client_DownloadFileCompleted;
               BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = "Downloading " + args.s2));
               BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 0));
               client.DownloadFileAsync(uri, ForgeHX_FilePath);
            }
         }
         else
         {
            Log("[DEBUG] Reading ForgeHX", lbOutputWindow);
            var content = File.ReadAllText(ForgeHX_FilePath);
            var startIndex = content.IndexOf(".BUILD_NUMBER=\"");
            var endIndex = content.IndexOf(".TILE_SPEC_NAME_CONTEMPORARY_BUSHES=\"");
            content = content.Substring(startIndex, endIndex - startIndex);
            content = content.Replace("\n", "").Replace("\r", "");
            var regExSecret = new Regex("\\.VERSION_SECRET=\"([a-zA-Z0-9_\\-\\+\\/==]+)\";", RegexOptions.IgnoreCase);
            var regExVersion = new Regex("\\.VERSION_MAJOR_MINOR=\"([0-9+.0-9+.0-9+]+)\";", RegexOptions.IgnoreCase);
            var VersionMatch = regExVersion.Match(content);
            var SecretMatch = regExSecret.Match(content);
            if (VersionMatch.Success)
            {
               SettingData.Version = VersionMatch.Groups[1].Value;
               Log("[DEBUG] Version found: " + SettingData.Version, lbOutputWindow);
            }
            if (SecretMatch.Success)
            {
               SettingData.Version_Secret = SecretMatch.Groups[1].Value;
               Log("[DEBUG] Version Secret found: " + SettingData.Version_Secret, lbOutputWindow);
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
         Log("[DEBUG] done Downloading ForgeHX", lbOutputWindow);
         var content = File.ReadAllText(ForgeHX_FilePath);
         var startIndex = content.IndexOf(".BUILD_NUMBER=\"");
         var endIndex = content.IndexOf(".TILE_SPEC_NAME_CONTEMPORARY_BUSHES=\"");
         content = content.Substring(startIndex, endIndex - startIndex);
         content = content.Replace("\n", "").Replace("\r", "");
         var regExSecret = new Regex("\\.VERSION_SECRET=\"([a-zA-Z0-9_\\-\\+\\/==]+)\";", RegexOptions.IgnoreCase);
         var regExVersion = new Regex("\\.VERSION_MAJOR_MINOR=\"([0-9+.0-9+.0-9+]+)\";", RegexOptions.IgnoreCase);
         var VersionMatch = regExVersion.Match(content);
         var SecretMatch = regExSecret.Match(content);
         if (VersionMatch.Success)
         {
            SettingData.Version = VersionMatch.Groups[1].Value;
            Log("[DEBUG] Version found: " + SettingData.Version, lbOutputWindow);
         }
         if (SecretMatch.Success)
         {
            SettingData.Version_Secret = SecretMatch.Groups[1].Value;
            Log("[DEBUG] Version Secret found: " + SettingData.Version_Secret, lbOutputWindow);
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
            Invoker.SetProperty(pictureBox1, () => pictureBox1.Visible, true);
            ToggleGUI(false, true);
            Invoker.SetProperty(lblPleaseLogin, () => lblPleaseLogin.Text, $"{strings.PleaseWaitAgain}\n{strings.LoggingIn}");
            //cwb.ShowDevTools();
            Log("[DEBUG] Doing Login", lbOutputWindow);
            blockedLogin = true;
            Thread.Sleep(500);
            string loginJS = resMgr.GetString("preloadLoginWorld");
            string loginCity = "false";
            string cityID = "";
            if (UserData.LastWorld == null || string.IsNullOrWhiteSpace(UserData.LastWorld))
               loginCity = "true";
            else
               cityID = UserData.LastWorld;

            loginJS = loginJS
               .Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()])
               .Replace("###USERNAME###", UserData.Username)
               .Replace("###PASSWORD###", UserData.Password)
               .Replace("##server##", UserData.WorldServer)
             .Replace("##t##", loginCity)
             .Replace("##city##", "\"" + cityID + "\"");
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
         lvGoods.Items.Clear();
         reloadData();
      }
      private void Main_FormClosed(object sender, FormClosedEventArgs e)
      {
         Log("[DEBUG] Closing App", lbOutputWindow);
         RunningTime.Stop();
         Cef.Shutdown();
      }
      private void bwTimerUpdate_DoWork(object sender, DoWorkEventArgs e)
      {
         while (true)
         {
            Invoker.SetProperty(lblRunSinceValue, () => lblRunSinceValue.Text, RunningTime.Elapsed.ToString("h'h 'm'm 's's'"));
            Thread.Sleep(999);
         }
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
         CUpdate.UpdateFinished += CUpdate_UpdateFinished;
         Updater.UpdatePlayerLists();
         Updater.UpdateStartUp();
      }
      private void CUpdate_UpdateFinished(RequestType type)
      {
         switch (type)
         {
            case RequestType.Startup:
               UpdateOverView();
               UpdateTavern();
               UpdateHiddenRewardsView();
               break;
            case RequestType.Motivate:
               break;
            case RequestType.CollectIncident:
               UpdateHiddenRewardsView();
               break;
            case RequestType.VisitTavern:
               UpdateTavern();
               break;
            case RequestType.GetClanMember:
            case RequestType.GetFriends:
            case RequestType.GetNeighbor:
               UpdateOtherPlayers();
               break;
            case RequestType.GetEntities:
               break;
            case RequestType.GetLGs:
               break;
            case RequestType.LogService:
               break;
            case RequestType.CollectProduction:
               break;
            case RequestType.QueryProduction:
               break;
            case RequestType.CancelProduction:
               break;
            case RequestType.CollectTavern:
               break;
            case RequestType.GetOwnTavern:
               UpdateTavern();
               break;
            case RequestType.RemovePlayer:
               break;
            case RequestType.GetAllWorlds:
               break;
            default:
               break;
         }
      }
      public void LoadGUI()
      {
         UpdateOverView();
         UpdateOtherPlayers();
         UpdateTavern();
         UpdateProductionView();
         UpdateGoodProductionView();
         UpdateHiddenRewardsView();
         UpdateBotView();
      }
      public void UpdateBotView()
      {
         Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Clear());
         UCPremium ProdBot = new UCPremium();
         ProdBot.ToggleFlipped += ToggleProtBot;
         ProdBot.Type = BotType.ProdBot;
         ProdBot.Dock = DockStyle.Top;
         ProdBot.Checked = UserData.ProductionBot;
         ProdBot.LabelName = strings.ProductionBot;
         Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Add(ProdBot));
         if (string.IsNullOrWhiteSpace(UserData.SerialKey)) return;
         object ret = TcpConnection.SendAuthData(UserData.SerialKey, FingerPrint.Value(), false);
         if (ret is Result)
         {
            Enum.TryParse(ret.ToString(), out Result result);
            if (result == Result.Success)
            {
               Text = Tag.ToString() + $"{Version.Major}.{Version.Minor} (PREMIUM) | by TH3C0D3R";
               object retList = ExecuteMethod(PremAssembly, "EntryPoint", "AddPremiumControl", null);
               if (retList is List<UCPremium> list)
               {
                  foreach (UCPremium item in list)
                  {
                     UCPremium newItem = new UCPremium
                     {
                        Dock = DockStyle.Top
                     };
                     switch (item.Type)
                     {
                        case BotType.TavernBot:
                           newItem.ToggleFlipped += ToggleTavernBot;
                           newItem.LabelName = strings.TavernBot;
                           newItem.Type = BotType.TavernBot;
                           newItem.Checked = UserData.TavernBot;
                           Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Add(newItem));
                           break;
                        case BotType.IncidentBot:
                           newItem.ToggleFlipped += ToggleIncidentBot;
                           newItem.LabelName = strings.IncidentBot;
                           newItem.Type = BotType.IncidentBot;
                           newItem.Checked = UserData.IncidentBot;
                           Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Add(newItem));
                           break;
                        case BotType.PolivateBot:
                           newItem.ToggleFlipped += TogglePolivateBot;
                           newItem.LabelName = strings.PolivateBot;
                           newItem.Type = BotType.PolivateBot;
                           newItem.Checked = UserData.MoppelBot;
                           Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Add(newItem));
                           break;
                        case BotType.RQBot:
                           newItem.ToggleFlipped += ToggleRQBot;
                           newItem.LabelName = strings.RQBot;
                           newItem.Type = BotType.RQBot;
                           newItem.Checked = UserData.RQBot;
                           Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Add(newItem));
                           break;
                        case BotType.ProdBot:
                           break;
                        default:
                           break;
                     }
                  }
               }
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(this, () => Text, Tag.ToString() + $"{Version.Major}.{Version.Minor} (EXPIRED) | by TH3C0D3R");
               if (!blockExpireBox)
                  MessageBox.Show(Owner, "Your Subscription is expired.\n\nPlease purchase a new Subscribtion and acitvate it, else you are continue using the Free-Version", "SUBSCRIPTION EXPIRED");
               blockExpireBox = true;
            }
            else
            {
               Text = Tag.ToString() + $"{Version.Major}.{Version.Minor} | by TH3C0D3R";
               MessageBox.Show(Owner, "There was a problem using this Serial-Key with your Device!!\n\nEither you activated the Serial-Key on a other Device\nor the Serial-Key is not valid!", "FAILED TO ACTIVATE");
            }
         }
      }
      private void UpdateHiddenRewardsView()
      {
         if (ListClass.HiddenRewards.Count == 0) return;
         if (pnlIncident.InvokeRequired)
         {
            Invoker.CallMethode(pnlIncident, () => pnlIncident.Controls.Clear());
         }
         else
         {
            pnlIncident.Controls.Clear();
         }
         foreach (HiddenReward item in ListClass.HiddenRewards)
         {
            if (!item.isVisible) continue;
            if (!UserData.ShowBigRoads && item.position.context == "cityRoadBig") continue;
            IncidentListItem iliIncident = new IncidentListItem
            {
               ILocation = item.position.context,
               IRarity = item.rarity,
               Dock = DockStyle.Top
            };
            if (pnlIncident.InvokeRequired)
            {
               Invoker.CallMethode(pnlIncident, () => pnlIncident.Controls.Add(iliIncident));
            }
            else
            {
               pnlIncident.Controls.Add(iliIncident);
            }
         }
         if (pnlIncident.InvokeRequired)
         {
            Invoker.CallMethode(pnlIncident, () => pnlIncident.Invalidate());
         }
         else
         {
            pnlIncident.Invalidate();
         }
      }
      private void UpdateGoodProductionView()
      {
         Invoker.CallMethode(pnlGoodProductionList, () => pnlGoodProductionList.Controls.Clear());
         if (ListClass.GoodProductionList.Count > 0)
         {
            List<KeyValuePair<string, List<EntityEx>>> groupedList = new List<KeyValuePair<string, List<EntityEx>>>();
            if (UserData.GroupedView)
            {
               groupedList = GetGroupedList(ListClass.GoodProductionList);
               foreach (KeyValuePair<string, List<EntityEx>> item in groupedList)
               {
                  ProdListItem pli = new ProdListItem();
                  if (item.Value.First().state["__class__"].ToString() == "IdleState")
                  {
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {strings.ProductionIdle}", strings.ProductionIdle);
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProducingState")
                  {
                     var product = item.Value.First().state["current_product"]["product"]["resources"].Children().First().Children().First();
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {product} ({(int.Parse(product.ToString())) * item.Value.Count})", "");
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_in"].ToString())).First().state["next_state_transition_in"].ToString();
                     string greatestEnd = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_at"].ToString())).First().state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {strings.ProductionFinishedState}", strings.ProductionFinishedState);
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.AddEntities(item.Value.Select(i => i.id).ToList().ToArray());
                  pli.ProductionDone += Pli_ProductionDone;
                  pli.ProductionIdle += Pli_ProductionIdle;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlGoodProductionList, () => pnlGoodProductionList.Controls.Add(pli));
               }
            }
            else
            {
               foreach (EntityEx item in ListClass.GoodProductionList)
               {
                  ProdListItem pli = new ProdListItem();
                  if (item.state["__class__"].ToString() == "IdleState")
                  {
                     pli.FillControl($"{item.name}", $"{strings.ProductionIdle}", strings.ProductionIdle);
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.state["__class__"].ToString() == "ProducingState")
                  {
                     pli.FillControl($"{item.name}", $"{strings.ProducingState}", "");
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.state["next_state_transition_in"].ToString();
                     string greatestEnd = item.state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     pli.FillControl($"{item.name}", $"{strings.ProductionFinishedState}", strings.ProductionFinishedState);
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.AddEntities(item.id);
                  pli.ProductionDone += Pli_ProductionDone;
                  pli.ProductionIdle += Pli_ProductionIdle;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlGoodProductionList, () => pnlGoodProductionList.Controls.Add(pli));
               }
            }
         }
         Invoker.CallMethode(pnlGoodProductionList, () => pnlGoodProductionList.Invalidate());
      }
      private void Pli_ProductionIdle(object sender, dynamic data = null)
      {
         if (data == null) return;
         if (!UserData.ProductionBot) return;
         lock (_locker)
         {
            if (DEBUGMODE) Log($"[{DateTime.Now}] Production Idle Event", lbOutputWindow);
            Debug.WriteLine($"[{DateTime.Now}] Production Idle Event");
            mbtQuery_Click(this, null);
            xcounter += 1;
         }
      }
      private void UpdateProductionView()
      {
         Invoker.CallMethode(pnlProductionList, () => pnlProductionList.Controls.Clear());
         if (ListClass.ProductionList.Count > 0)
         {
            List<KeyValuePair<string, List<EntityEx>>> groupedList = new List<KeyValuePair<string, List<EntityEx>>>();
            if (UserData.GroupedView)
            {
               groupedList = GetGroupedList(ListClass.ProductionList);
               if (DEBUGMODE) Log($"[{DateTime.Now}] Groupe Update", lbOutputWindow);
               Debug.WriteLine($"[{DateTime.Now}] Groupe Update");
               foreach (KeyValuePair<string, List<EntityEx>> item in groupedList)
               {
                  ProdListItem pli = new ProdListItem();
                  if (item.Value.First().state["__class__"].ToString() == "IdleState")
                  {
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {strings.ProductionIdle}", strings.ProductionIdle);
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProducingState")
                  {
                     var product = item.Value.First().state["current_product"]["product"]["resources"].Children().First().Children().First();
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {product} ({(int.Parse(product.ToString())) * item.Value.Count})", "");
                     //pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {strings.ProducingState}", strings.ProducingState);
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_in"].ToString())).First().state["next_state_transition_in"].ToString();
                     string greatestEnd = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_at"].ToString())).First().state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {strings.ProductionFinishedState}", strings.ProductionFinishedState);
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.AddEntities(item.Value.Select(i => i.id).ToList().ToArray());
                  pli.ProductionDone += Pli_ProductionDone;
                  pli.ProductionIdle += Pli_ProductionIdle;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlProductionList, () => pnlProductionList.Controls.Add(pli));
               }
            }
            else
            {
               if (DEBUGMODE) Log($"[{DateTime.Now}] Single Update", lbOutputWindow);
               Debug.WriteLine($"[{DateTime.Now}] Single Update");
               foreach (EntityEx item in ListClass.ProductionList)
               {
                  ProdListItem pli = new ProdListItem();
                  if (item.state["__class__"].ToString() == "IdleState")
                  {
                     pli.FillControl($"{item.name}", $"{strings.ProductionIdle}", strings.ProductionIdle);
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.state["__class__"].ToString() == "ProducingState")
                  {
                     pli.FillControl($"{item.name}", $"{strings.ProducingState}", strings.ProducingState);
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.state["next_state_transition_in"].ToString();
                     string greatestEnd = item.state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     pli.FillControl($"{item.name}", $"{strings.ProductionFinishedState}", strings.ProductionFinishedState);
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.AddEntities(item.id);
                  pli.ProductionDone += Pli_ProductionDone;
                  pli.ProductionIdle += Pli_ProductionIdle;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlProductionList, () => pnlProductionList.Controls.Add(pli));
               }
            }
         }
         Invoker.CallMethode(pnlProductionList, () => pnlProductionList.Invalidate());
      }
      private void Pli_ProductionDone(object sender, dynamic data = null)
      {
         if (data == null) return;
         if (!UserData.ProductionBot) return;
         if (DEBUGMODE) Log($"[{DateTime.Now}] Production Done Event", lbOutputWindow);
         Debug.WriteLine($"[{DateTime.Now}] Production Done Event");
         if (((object)data).GetType() == typeof(List<int>))
         {
            List<int> ids = (List<int>)data;
            string script = ReqBuilder.GetRequestScript(RequestType.CollectProduction, ids.ToArray());
            cwb.ExecuteScriptAsync(script);
            ResponseHandler.ProdCollected += ProdCollected;
         }
      }
      private void ProdCollected(object sender, dynamic data = null)
      {
         if (!UserData.ProductionBot)
         {
            ListClass.CollectedIDs.Clear();
            return;
         }
         if (DEBUGMODE) Log($"[{DateTime.Now}] Production Collected Event", lbOutputWindow);
         Debug.WriteLine($"[{DateTime.Now}] Production Collected Event");
         mbtQuery_Click(this, null);
         xcounter += 1;
      }
      private void RemoveFriend(object senderObj, EventArgs e)
      {
         Button sender = ((Button)senderObj);
         if (sender.Tag != null)
         {
            string playerid = sender.Tag.ToString();
            string script = ReqBuilder.GetRequestScript(RequestType.RemovePlayer, playerid);
            cwb.ExecuteScriptAsync(script);
            ResponseHandler.FriendRemoved += OnFriendRemoved;
         }
      }
      private void OnFriendRemoved(object sender, dynamic data = null)
      {
         Updater.UpdateFirends();
      }
      private void UpdateOverView()
      {
         #region "Overview"
         Invoker.SetProperty(lblCurValue, () => lblCurValue.Text, ListClass.WorldList.Find(x => x.Item3 == WorldState.current).Item2);
         Invoker.SetProperty(lblPlayerValue, () => lblPlayerValue.Text, UserData.Username);
         if (ListClass.Resources.Count > 0)
         {
            Invoker.SetProperty(lblSuppliesValue, () => lblSuppliesValue.Text, ((int)ListClass.Resources["responseData"].First?.First?["supplies"]?.ToObject(typeof(int))).ToString("N0"));
            Invoker.SetProperty(lblMoneyValue, () => lblMoneyValue.Text, ((int)ListClass.Resources["responseData"].First?.First?["money"]?.ToObject(typeof(int))).ToString("N0"));
            Invoker.SetProperty(lblDiaValue, () => lblDiaValue.Text, ((int)ListClass.Resources["responseData"].First?.First?["premium"]?.ToObject(typeof(int))).ToString("N0"));
            Invoker.SetProperty(lblMedsValue, () => lblMedsValue.Text, ((int)ListClass.Resources["responseData"].First?.First?["medals"]?.ToObject(typeof(int))).ToString("N0"));
            Invoker.SetProperty(lblFPValue, () => lblFPValue.Text, ((int)ListClass.Resources["responseData"].First?.First?["strategy_points"]?.ToObject(typeof(int))).ToString("N0"));
         }

         if (ListClass.ResourceDefinitions.Count > 0)
         {
            Invoker.SetProperty(lblMoney, () => lblMoney.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "money")["name"].ToString());
            Invoker.SetProperty(lblSupplies, () => lblSupplies.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "supplies")["name"].ToString());
            Invoker.SetProperty(lblMeds, () => lblMeds.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "medals")["name"].ToString());
            Invoker.SetProperty(lblDiamonds, () => lblDiamonds.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "premium")["name"].ToString());
            Invoker.SetProperty(lblFP, () => lblFP.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "strategy_points")["name"].ToString());
         }
         Invoker.CallMethode(lvGoods, () => lvGoods.Items.Clear());
         foreach (KeyValuePair<string, List<Good>> item in ListClass.GoodsDict)
         {
            if (item.Value.Find(x => x.value > 0) != null)
            {
               ListViewItem lvi = new ListViewItem(item.Key);
               foreach (Good good in item.Value)
               {
                  ListViewSubItem lvSi = new ListViewSubItem(lvi, $"{good.name} ({good.value})");
                  lvi.SubItems.Add(lvSi);
               }
               Invoker.CallMethode(lvGoods, () => lvGoods.Items.Add(lvi));
            }
         }
         #endregion
      }
      private void UpdateOtherPlayers()
      {
         #region "other Players"
         var friendMotivate = ListClass.FriendList.FindAll(f => (f.next_interaction_in == 0));
         var clanMotivate = ListClass.ClanMemberList.FindAll(f => (f.next_interaction_in == 0));
         var neighborlist = ListClass.NeighborList.FindAll(f => (f.next_interaction_in == 0));
         Invoker.SetProperty(lblFriends, () => lblFriends.Text, strings.Friends);
         Invoker.SetProperty(lblClanMember, () => lblClanMember.Text, strings.Clanmembers);
         Invoker.SetProperty(lblNeighbor, () => lblNeighbor.Text, strings.Neighbors);
         Invoker.SetProperty(lblFriendsCount, () => lblFriendsCount.Text, $"{friendMotivate.Count}/{ListClass.FriendList.Count}");
         Invoker.SetProperty(lblClanMemberCount, () => lblClanMemberCount.Text, $"{clanMotivate.Count}/{ListClass.ClanMemberList.Count}");
         Invoker.SetProperty(lblNeighborCount, () => lblNeighborCount.Text, $"{neighborlist.Count}/{ListClass.NeighborList.Count}");
         Invoker.SetProperty(lblInactiveFriends, () => lblInactiveFriends.Text, strings.InactiveFriends);

         var inactiveFriends = ListClass.FriendList.FindAll(f => (f.is_active == false && f.is_self == false));
         int row = 0;
         Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Clear());
         Invoker.SetProperty(tlpInactiveFriends, () => tlpInactiveFriends.RowCount, inactiveFriends.Count);
         foreach (var item in inactiveFriends)
         {
            int col = 0;
            Label lblName = new Label
            {
               Dock = DockStyle.Top,
               TextAlign = ContentAlignment.TopCenter,
               Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular),
               Text = item.name
            };
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Add(lblName));
            col += 1;
            Label lblScore = new Label
            {
               Dock = DockStyle.Top,
               TextAlign = ContentAlignment.TopCenter,
               Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular),
               Text = item.score.ToString("N0")
            };
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Add(lblScore));
            col += 1;
            Button btnRemove = new Button
            {
               Tag = item.player_id,
               Text = strings.Remove,
               Dock = DockStyle.Top,
               AutoSize = true,
               Font = new System.Drawing.Font("Microsoft Sans Serif", 15, FontStyle.Regular)
            };
            btnRemove.Click += RemoveFriend;
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Add(btnRemove));
            row += 1;
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.RowStyles.Add(new RowStyle(SizeType.AutoSize)));
         }

         #endregion
      }
      private void ToggleGUI(bool newState, bool useInvoker = false)
      {
         if (useInvoker)
         {
            Invoker.SetProperty(toolStrip1, () => toolStrip1.Enabled, newState);
         }
         else
         {
            toolStrip1.Enabled = newState;
            detachToolStripMenuItem.Enabled = newState;
         }
      }
      private void tsmiVisitAll_Click(object sender, EventArgs e)
      {
         VisitAllTavern();
      }
      private void tsmiDoAll_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.All);
         VisitAllTavern();
      }
      private void UpdateTavern()
      {
         #region "Tavern"
         if (ListClass.Resources.Count > 0)
         {
            Invoker.SetProperty(lblTavernSilverValue, () => lblTavernSilverValue.Text, ((int)ListClass.Resources["responseData"].First?.First?["tavern_silver"]?.ToObject(typeof(int))).ToString("N0"));
         }
         Invoker.SetProperty(lblTavernstate, () => lblTavernstate.Text, strings.TavernState);
         Invoker.SetProperty(lblVisitable, () => lblVisitable.Text, strings.Visitable);
         Invoker.SetProperty(btnCollect, () => btnCollect.Text, strings.CollectTavern);
         Invoker.SetProperty(lblTavernstateValue, () => lblTavernstateValue.Text, ListClass.OwnTavern.responseData[2].ToString() + "/" + ListClass.OwnTavern.responseData[1].ToString());
         if (ListClass.ResourceDefinitions.Count > 0)
         {
            Invoker.SetProperty(lblTavernSilver, () => lblTavernSilver.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "tavern_silver")["name"].ToString());
         }

         var visitable = ListClass.FriendTaverns.FindAll(f => (f.sittingPlayerCount < f.unlockedChairCount && f.state == null));
         Invoker.SetProperty(lblVisitableValue, () => lblVisitableValue.Text, visitable.Count.ToString());
         Invoker.SetProperty(lblCurSitting, () => lblCurSitting.Text, strings.CurrentlySittingPlayers);
         if (ListClass.OwnTavernData.view != null)
         {
            var ownTavern = ListClass.OwnTavernData.view.visitors.ToList();
            int row = 0;
            Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Clear());
            Invoker.SetProperty(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.RowCount, ownTavern.Count);
            foreach (var item in ownTavern)
            {
               int col = 0;
               Label lblName = new Label
               {
                  Dock = DockStyle.Top,
                  TextAlign = ContentAlignment.TopCenter,
                  Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular),
                  Text = item.name
               };
               Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(lblName));
               col += 1;
               Label lblScore = new Label
               {
                  Dock = DockStyle.Top,
                  TextAlign = ContentAlignment.TopCenter,
                  Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular),
                  Text = item.player_id.ToString()
               };
               Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(lblScore));
               col += 1;

               if (visitable.Exists(f => f.ownerId == item.player_id))
               {
                  Button btnSitDown = new Button
                  {
                     Tag = item.player_id,
                     Text = strings.SitDown,
                     Dock = DockStyle.Top,
                     AutoSize = true,
                     Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular)
                  };
                  btnSitDown.Click += SitAtTavern;
                  Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(btnSitDown));
               }
               else
               {
                  Label lnlSitState = new Label
                  {
                     Dock = DockStyle.Top,
                     TextAlign = ContentAlignment.TopCenter,
                     Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular),
                     Text = strings.CanNotSitDown
                  };
                  Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(lnlSitState));
               }
               row += 1;
               Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.RowStyles.Add(new RowStyle(SizeType.AutoSize)));
            }
         }
         #endregion
      }
      private void Motivate(E_Motivate player_type)
      {
         TwoTArgs<RequestType, E_Motivate> param = new TwoTArgs<RequestType, E_Motivate> { RequestType = RequestType.Motivate, argument2 = player_type };
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuter_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync(param);
      }
      private void VisitAllTavern()
      {
         //string script = ReqBuilder.GetRequestScript(RequestType.VisitTavern, "[]");
         TwoTArgs<RequestType, object> param = new TwoTArgs<RequestType, object> { RequestType = RequestType.VisitTavern, argument2 = null };
         bwScriptExecuter.RunWorkerAsync(param);
      }
      private void bwScriptExecuter_DoWork(object sender, DoWorkEventArgs e)
      {
         dynamic param;
         try
         {
            param = (TwoTArgs<RequestType, dynamic>)e.Argument;
         }
         catch (Exception)
         {
            param = (TwoTArgs<RequestType, E_Motivate>)e.Argument;
         }
         int steps = 0;
         switch (param.RequestType)
         {
            case RequestType.Motivate:
               E_Motivate e_Motivate = (E_Motivate)Enum.Parse(typeof(E_Motivate), param.argument2.ToString());
               List<Player> list = new List<Player>();
               Player last = null;
               switch (e_Motivate)
               {
                  case E_Motivate.Clan:
                     var clanMotivate = ListClass.ClanMemberList.FindAll(f => (f.next_interaction_in == 0));
                     list.AddRange(clanMotivate);
                     break;
                  case E_Motivate.Neighbor:
                     var neighborlist = ListClass.NeighborList.FindAll(f => (f.next_interaction_in == 0));
                     list.AddRange(neighborlist);
                     break;
                  case E_Motivate.Friend:
                     var friendMotivate = ListClass.FriendList.FindAll(f => (f.next_interaction_in == 0));
                     list.AddRange(friendMotivate);
                     break;
                  case E_Motivate.All:
                     list.AddRange(ListClass.Motivateable);
                     break;
                  default:
                     break;
               }
               steps = (int)Math.Round((double)(100 / list.Count), MidpointRounding.AwayFromZero);
               last = list[0];
               foreach (Player item in list)
               {
                  string script = ReqBuilder.GetRequestScript(param.RequestType, item.player_id);
                  cwb.ExecuteScriptAsync(script);
                  Random r = new Random();
                  int rInt = r.Next(750, 1500);
                  while (!ListClass.doneMotivate.ContainsKey(last.player_id))
                  {
                     Thread.Sleep(1);
                  }
                  if (tspbProgress.Value >= 100)
                     BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 100));
                  BeginInvoke(new MethodInvoker(() => tspbProgress.Value += steps));
                  BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = $"{strings.Motivating} {ListClass.doneMotivate.Count} / {list.Count}"));
                  last = item;
                  Thread.Sleep(rInt);
               }
               ListClass.doneMotivate.Clear();
               BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 0));
               BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = strings.MotivationDone));
               break;
            case RequestType.VisitTavern:
               FriendTavernState FTSlast = null;
               ListClass.FriendTaverns = ListClass.FriendTaverns.Where((f) => f.sittingPlayerCount < f.unlockedChairCount && f.state == null).ToList();
               if (ListClass.FriendList.Count == 0) return;
               steps = (int)Math.Round((double)(100 / ListClass.FriendTaverns.Count), MidpointRounding.AwayFromZero);
               foreach (FriendTavernState item in ListClass.FriendTaverns)
               {
                  FTSlast = item;
                  string script = ReqBuilder.GetRequestScript(param.RequestType, item.ownerId);
                  cwb.ExecuteScriptAsync(script);
                  Random r = new Random();
                  int rInt = r.Next(750, 1500);
                  while (!ListClass.doneTavern.ContainsKey(FTSlast.ownerId))
                  {
                     Thread.Sleep(1);
                  }
                  if (tspbProgress.Value >= 100)
                     BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 100));
                  BeginInvoke(new MethodInvoker(() => tspbProgress.Value += steps));
                  BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = $"{strings.SittingAtTavern} {ListClass.doneTavern.Count} / {ListClass.FriendTaverns.Count}"));
                  Thread.Sleep(rInt);
               }
               ListClass.doneTavern.Clear();
               BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 0));
               BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = strings.TavernDone));
               ResponseHandler_TaverSitted(this);
               ResponseHandler.TaverSitted += ResponseHandler_TaverSitted;
               break;
            default:
               break;
         }
      }
      private void btnCollect_Click(object sender, EventArgs e)
      {
         string script = ReqBuilder.GetRequestScript(RequestType.CollectTavern, "");
         ResponseHandler.TavernCollected += ResponseHandler_TavernCollected;
         cwb.ExecuteScriptAsync(script);
      }
      private void ResponseHandler_TavernCollected(object sender, dynamic data = null)
      {
         reloadData();
      }
      private void aidFriendsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.Friend);
      }
      private void aidClanToolStripMenuItem_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.Clan);
      }
      private void aidNeiToolStripMenuItem_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.Neighbor);
      }
      private void tsmiLog_Click(object sender, EventArgs e)
      {
         if (Application.OpenForms["Log"] != null)
            LogWnd.Close();
         else
         {
            if (!LogWnd.Created)
               LogWnd = new Log(new Point(Location.X + Size.Width, Location.Y));
            LogWnd.Show();
            LogWnd.FillLog(MSGHistory);
            LogWnd.mainForm = this;
         }
      }
      private void Main_LocationChanged(object sender, EventArgs e)
      {
         if (LogWnd != null)
            if (LogWnd.Created && LogWnd.AttachState)
               LogWnd.UpdateLocation(new Point(Location.X + Size.Width, Location.Y));
      }
      private void detachToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (LogWnd != null)
         {
            LogWnd.SwitchAttachState(!LogWnd.AttachState);
            detachToolStripMenuItem.Text = LogWnd.AttachState ? strings.DetachMain : strings.AttachMain;
         }
      }
      private void tsddSettings_Click(object sender, EventArgs e)
      {
         Forms.Settings frmSettings = new Forms.Settings
         {
            mainForm = this
         };
         frmSettings.GroupedChanged += FrmSettings_GroupedChanged;
         frmSettings.ShowBigRoadsChanged += FrmSettings_ShowBigRoadsChanged;
         frmSettings.Show();
      }
      private void FrmSettings_ShowBigRoadsChanged(object sender, dynamic data = null)
      {
         UpdateHiddenRewardsView();
      }
      private void FrmSettings_GroupedChanged(object sender, dynamic data = null)
      {
         UpdateGoodProductionView();
         UpdateProductionView();
      }
      private void mbtQuery_Click(object sender, EventArgs e)
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.QueryProduction };
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuterOneArg_DoWork;
         bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync(param);
      }
      private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         if (DEBUGMODE) Log($"[{DateTime.Now}] Production Query Complete", lbOutputWindow);
         Debug.WriteLine($"[{DateTime.Now}] Production Query Complete");
         lock (_locker)
         {
            UpdateProductionView();
            UpdateGoodProductionView();
         }
      }
      private void mbtCollect_Click(object sender, EventArgs e)
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CollectProduction };
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuterOneArg_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync(param);
      }
      private void mbtCancel_Click(object sender, EventArgs e)
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CancelProduction };
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuterOneArg_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync(param);
      }
      private void mbtIncidents_Click(object sender, EventArgs e)
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CollectIncident };
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuterOneArg_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync(param);
      }
      private void bwScriptExecuterOneArg_DoWork(object sender, DoWorkEventArgs e)
      {
         OneTArgs<RequestType> param = (OneTArgs<RequestType>)e.Argument;
         int[] ids;
         string script = "";
         switch (param.t1)
         {
            case RequestType.CollectProduction:
               ids = ListClass.GoodProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() != "idlestate" && x.state["__class__"].ToString().ToLower() != "producingstate"; }).ToList().Select(y => y.id).ToArray();
               ids = ids.Concat(ListClass.ProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() != "idlestate" && x.state["__class__"].ToString().ToLower() != "producingstate"; }).ToList().Select(y => y.id).ToArray()).ToArray();
               script = ReqBuilder.GetRequestScript(param.t1, ids);
               cwb.ExecuteScriptAsync(script);
               ResponseHandler.ProdCollected += ProdCollected;
               break;
            case RequestType.QueryProduction:
               ListClass.AddedToQuery.Clear();
               if (ListClass.CollectedIDs.Count > 0)
               {
                  ListClass.AddedToQuery.AddRange(ListClass.CollectedIDs);
                  int[] Query = ListClass.AddedToQuery.ToArray();
                  foreach (int id in Query)
                  {
                     if (UserData.GoodProductionOption.id < UserData.ProductionOption.id && UserData.ProductionOption.id > 4)
                     {
                        bool hasID = ListClass.GoodProductionList.Find(ex => ex.id == id) == null;
                        if (hasID)
                           script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.GoodProductionOption.id });
                        else
                           script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.ProductionOption.id });
                     }
                     else
                        script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.ProductionOption.id });
                     cwb.ExecuteScriptAsync(script);
                     Thread.Sleep(100);
                  }
               }
               else
               {
                  ids = ListClass.GoodProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() == "idlestate"; }).ToList().Select(y => y.id).ToArray();
                  ListClass.AddedToQuery.AddRange(ids);
                  foreach (int id in ids)
                  {
                     script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.GoodProductionOption.id });
                     cwb.ExecuteScriptAsync(script);
                     Thread.Sleep(100);
                  }
                  ids = ListClass.ProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() == "idlestate"; }).ToList().Select(y => y.id).ToArray();
                  ListClass.AddedToQuery.AddRange(ids);
                  foreach (int id in ids)
                  {
                     script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.ProductionOption.id });
                     cwb.ExecuteScriptAsync(script);
                     Thread.Sleep(100);
                  }
               }
               ResponseHandler.ProdStarted += ResponseHandler_ProdStarted;
               break;
            case RequestType.CancelProduction:
               ids = ListClass.GoodProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() != "idlestate"; }).ToList().Select(y => y.id).ToArray();
               foreach (int id in ids)
               {
                  script = ReqBuilder.GetRequestScript(param.t1, id);
                  cwb.ExecuteScriptAsync(script);
                  Thread.Sleep(100);
               }
               ids = ListClass.ProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() != "idlestate"; }).ToList().Select(y => y.id).ToArray();
               foreach (int id in ids)
               {
                  script = ReqBuilder.GetRequestScript(param.t1, id);
                  cwb.ExecuteScriptAsync(script);
                  Thread.Sleep(100);
               }
               break;
            case RequestType.CollectIncident:
               ResponseHandler.IncidentCollected += ResponseHandler_IncidentCollected;
               foreach (HiddenReward item in ListClass.HiddenRewards)
               {
                  if (!item.isVisible) continue;
                  if (!UserData.ShowBigRoads && item.position.context == "cityRoadBig") continue;
                  script = ReqBuilder.GetRequestScript(param.t1, item.hiddenRewardId);
                  cwb.ExecuteScriptAsync(script);
                  Thread.Sleep(100);
               }
               break;
            default:
               break;
         }
      }
      private void ResponseHandler_IncidentCollected(object sender, dynamic data = null)
      {
         string reward = data.ToString();
         reloadData();
         LoadGUI();
         if (tsslLogValue.GetCurrentParent().InvokeRequired)
         {
            tsslLogValue.InvokeAction(t =>
            {
               t.Text = $"{strings.IncidentCollected} - {strings.Reward}: {reward}";
            });
            Log($"{strings.IncidentCollected} - {strings.Reward}: {reward}", lbOutputWindow);
         }
         else
         {
            tsslLogValue.Text = $"{strings.IncidentCollected} - {strings.Reward}: {reward}";
            Log($"{strings.IncidentCollected} - {strings.Reward}: {reward}", lbOutputWindow);
         }
         //NotificationHelper.ShowNotify("Incident collected",$"Reward: {reward}",NotificationHelper.NotifyDuration.Long,Activated);
      }
      private void toolStripButton1_Click(object sender, EventArgs e)
      {
         object ret = TcpConnection.SendAuthData("IAVvAuYHY0JxjgnBarwue1JPfvvD6drnK9UVYRFAmW0D0M9D5t1AXD7R1GPP8VL5JgnjLSmJlwHIBFYTvsHNmqQGCDeSB0BRL54UjZ1nGsZk5zQvr6ePN8ScPghIxUHvv06FLEtV1dG4RqKNxslEVvENmbEC2szwkoyF2KkmNJYt1YLjVUVFsCZ9vtct9qdInTTm1FVmH81MXUVOPfqhXuDiOzUIr0ngpPFhEirQ9s7uKMlaYOdd95u3QvYBuM5R", FingerPrint.Value(), true);
         _ = ret is bool;
      }
      private void ResponseHandler_ProdStarted(object sender, dynamic data = null)
      {
         if (DEBUGMODE) Log($"[{DateTime.Now}] ProdStarted Handler", lbOutputWindow);
         Debug.WriteLine($"[{DateTime.Now}] ProdStarted Handler");
         lock (_locker)
         {
            reloadData();
            LoadGUI();
         }
      }
      private void tsmiAbout_Click(object sender, EventArgs e)
      {
         About frmAbout = new About();
         frmAbout.ShowDialog();
      }
      private void ToggleProtBot(object sender, dynamic data = null)
      {
         if (data is int)
         {
            if (data == -1) return;
            UserData.ProductionBot = ((UCPremium)sender).Checked;
            UserData.SaveSettings();
            UpdateGoodProductionView();
            UpdateProductionView();
         }
      }
      public static void ToggleTavernBot(object sender, dynamic e = null)
      {
         if (e is int && e == -1) return;
         UserData.TavernBot = ((UCPremium)sender).Checked;
      }
      public static void ToggleIncidentBot(object sender, dynamic e = null)
      {
         if (e is int && e == -1) return;
         UserData.IncidentBot = ((UCPremium)sender).Checked;
      }
      public static void TogglePolivateBot(object sender, dynamic e = null)
      {
         if (e is int && e == -1) return;
         UserData.MoppelBot = ((UCPremium)sender).Checked;
      }
      public static void ToggleRQBot(object sender, dynamic e = null)
      {
         if (e is int && e == -1) return;
         UserData.RQBot = ((UCPremium)sender).Checked;
      }
      private void TcMenu_SelectedIndexChanged(object sender, EventArgs e)
      {
         //if(tcMenu.SelectedIndex == 0)
         //{
         //   lvGoods.Items.Clear();
         //   reloadData();
         //}
      }
      private void SitAtTavern(object senderObj, EventArgs e)
      {
         Button sender = ((Button)senderObj);
         if (sender.Tag != null)
         {
            string playerid = sender.Tag.ToString();
            string script = ReqBuilder.GetRequestScript(RequestType.VisitTavern, playerid);
            ResponseHandler.TaverSitted += ResponseHandler_TaverSitted;
            cwb.ExecuteScriptAsync(script);
         }
      }
      private void ResponseHandler_TaverSitted(object sender, dynamic data = null)
      {
         reloadData();
      }
      //private void Activated(ToastNotification sender, object data)
      //{
      //   try
      //   {
      //      BringToFront();
      //      tcMenu.SelectTab(1);
      //   }
      //   catch (Exception)
      //   {
      //      Invoker.CallMethode(this, () => BringToFront());
      //      Invoker.CallMethode(tcMenu, () => tcMenu.SelectTab(1));
      //   } 
      //}
      private void handleExit(object sender, EventArgs e)
      {
         UserData.SaveSettings();
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
         _CookiesLoaded?.Invoke("CookieHandler", e);
      }

      private static CookieLoadedEventHandler _CookiesLoaded;
      public static event CookieLoadedEventHandler CookiesLoaded
      {
         add
         {
            if (_CookiesLoaded == null || !_CookiesLoaded.GetInvocationList().ToList().Contains(value))
               _CookiesLoaded += value;
         }
         remove
         {
            _CookiesLoaded -= value;
         }
      }

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
