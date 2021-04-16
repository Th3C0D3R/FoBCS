using ForgeOfBots.DataHandler;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using ForgeOfBots.Utils.Premium;
using MetroFramework.Controls;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UCPremiumLibrary;
using static ForgeOfBots.Utils.Helper;
using static ForgeOfBots.Utils.Extensions;
using static ForgeOfBots.Utils.StaticData;
using AllWorlds = ForgeOfBots.GameClasses.ResponseClasses.WorldSelection;
using CUpdate = ForgeOfBots.DataHandler.Update;
using WebClient = System.Net.WebClient;
using GEXWaves = ForgeOfBots.GameClasses.GEX.GetEncounter.Armywave;
using GBGWaves = ForgeOfBots.GameClasses.GBG.ArmyInfo.Data;
using GBGProvince = ForgeOfBots.GameClasses.GBG.Get.Province;
using ForgeOfBots.Forms.UserControls;
using Newtonsoft.Json.Linq;
using System.Collections;
using static System.Windows.Forms.CheckedListBox;
using ForgeOfBots.GameClasses.GBG.Get;

namespace ForgeOfBots.Forms
{
   public partial class frmMain : Form
   {
      public const int WM_NCLBUTTONDOWN = 0xA1;
      public const int HT_CAPTION = 0x2;
      [DllImport("user32.dll")]
      public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
      [DllImport("user32.dll")]
      public static extern bool ReleaseCapture();

      private EventHandler _FormLoaded;
      public event EventHandler FormLoaded
      {
         add
         {
            if (_FormLoaded == null || !_FormLoaded.GetInvocationList().ToList().Contains(value))
               _FormLoaded += value;
         }
         remove
         {
            _FormLoaded -= value;
         }
      }

      private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
      private bool blockExpireBox = false;
      readonly BackgroundWorkerEX bw = new BackgroundWorkerEX();
      static bool isLoading = false;
      static bool restart = false;
      static bool isFirstRun = true;
      private object _lock = new object();
      Loading LoadingFrm = null;
      private int CBSectorIndex = 0;
      private int FightCounter = 0;
      ucBattle ucCurrentPool = null;

      public frmMain() { }
      public frmMain(string[] args)
      {
         logger.Info($">>> frmMain");
         if (args != null)
         {
            if (args.Length > 0)
               if (args[0].StartsWith("/"))
                  if (args[0].Substring(1).ToLower().Equals("debug"))
                     DEBUGMODE = true;
         }
#if DEBUG
         DEBUGMODE = true;
#endif
         logger.Info($"Debugmode {(DEBUGMODE ? "activated" : "deactivated")}");
         Application.ThreadException += Application_ThreadException;
         FirewallHelper.OpenFirewallPort(4444, "WebDrive");
         MainInstance = this;
         if (!i18n.initialized)
         {
            logger.Info($"Check if settings exists");
            if (Utils.Settings.SettingsExists())
            {
               logger.Info($"settings exists");
               logger.Info($"changing language");
               UserData = Utils.Settings.ReadSettings();
               if (UserData == null) return;
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               i18n.Initialize(UserData.Language.Code, this);
            }
            else
            {
               logger.Info($"changing language to default 'en'");
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en");
               i18n.Initialize("en", this);
            }
         }
         Init();
         logger.Info($"<<< frmMain");
      }

      private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
      {
         logger.Info(e.Exception, $"CRASHED");
         //TelegramNotify.Send(i18n.getString("GUI.Telegram.Crash", new List<KeyValuePair<string, string>>()
         //      {
         //          new KeyValuePair<string, string>("##TimeStamp##",$"{DateTime.Now}")
         //      }.ToArray()));
      }

      private void Loading_FormClosed(object sender, FormClosedEventArgs e)
      {
         Environment.Exit(0);
      }
      public void Init()
      {
         logger.Info($">>> Init()");
         if (!IsChromeInstalled())
         {
            logger.Info($"chrome not installed");
            MessageBox.Show("!! PLEASE INSTALL GOOGLE CHROME TO USE THIS BOT !!", "ERROR STARTING BOT", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Process.Start("https://www.google.com/intl/de_de/chrome/");
            Environment.Exit(0);
            return;
         }
         List<string> stack = new List<string>();
         try
         {
            logger.Info($"Check if settings exists");
            stack.Add($"Check if settings exists");
            if (Utils.Settings.SettingsExists())
            {
               logger.Info($"settings exists");
               logger.Info($"changing language");
               UserData = Utils.Settings.ReadSettings();
               if (UserData == null) return;
               if (UserData.BotVersion.Major < StaticData.Version.Major || UserData.BotVersion.Minor < StaticData.Version.Minor)
               {
                  logger.Info($"old settingsversion found");
                  DialogResult dlgRes = MessageBox.Show(i18n.getString("GUI.MessageBox.OldSettingsVersion.Text"), i18n.getString("GUI.MessageBox.OldSettingsVersion.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                  if (dlgRes == DialogResult.Yes)
                  {
                     if (ListClass.BackgroundWorkers.Count > 0)
                     {
                        foreach (BackgroundWorkerEX backgroundWorker in ListClass.BackgroundWorkers)
                        {
                           backgroundWorker.CancelAsync();
                           while (backgroundWorker.IsBusy) { Application.DoEvents(); }
                        }
                        ListClass.BackgroundWorkers.Clear();
                     }
                     UserData.Delete();
                     Process.Start(Application.ExecutablePath);
                     Environment.Exit(0);
                     return;
                  }
                  else
                  {
                     UserData.BotVersion = StaticData.Version;
                     UserData.SaveSettings();
                  }
               }
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
               i18n.Initialize(UserData.Language.Code, this);
            }
            else
            {
               logger.Info($"changing language to default 'en'");
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en");
               i18n.Initialize("en", this);
            }
            Controls.Clear();
            logger.Info($"check available languages");
            Task.Factory.StartNew(CheckLanguages).Wait();

            InitializeComponent();
            FillArmyControl();
            i18n.TranslateForm();
            i18n.TranslateCMS(cmsMainMenu);
            logger.Info($"check for updates");
            if (HasLastCrash)
            {
               if (UserData != null)
               {
                  logger.Info($"request send last crash");
                  if (UserData.AllowSendCrashLog == UserConfirmation.Send)
                     CrashHelper.WaitForUserConfirmation(false);
                  else if (UserData.AllowSendCrashLog == UserConfirmation.AlwaysSend)
                     CrashHelper.WaitForUserConfirmation(true);
                  else if (UserData.AllowSendCrashLog == UserConfirmation.DontSend)
                     Crashes.SetEnabledAsync(false).Wait();
               }
               else
               {
                  CrashHelper.WaitForUserConfirmation(true);
               }
            }
            Application.ApplicationExit += Application_ApplicationExit;
         }
         catch (Exception ex)
         {
            NLog.LogManager.Flush();
            var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
            var properties = new Dictionary<string, string> { { "Main", ex.Message } };
            Crashes.TrackError(ex, properties, attachments);
         }
         logger.Info($"<<< Init()");
      }

      private void FillArmyControl()
      {
         if (tlpArmy.Controls.Contains(ucCurrentPool)) return;
         ucCurrentPool = new ucBattle();
         ucCurrentPool.Dock = DockStyle.Fill;
         ucCurrentPool.BorderStyle = BorderStyle.FixedSingle;
         ucCurrentPool.SubmitArmy += new CustomEvent(UcCurrentPool_SubmitArmy);
         ucCurrentPool.Name = "ucCurrentPool";
         ucCurrentPool.imgList = null;
         tlpArmy.Controls.Add(ucCurrentPool, 0, 1);
      }

      private void Usi_UserDataEntered(string username, string password, string server)
      {
         logger.Info($">>> Usi_UserDataEntered");
         Log("[DEBUG] Userdata loaded", lbOutputWindow);
         UserData.Username = username;
         UserData.Password = password;
         UserData.WorldServer = server;
         UserData.SaveSettings();
         logger.Info($"username: {username} | worldserver: {server}");
         usi.Close();
         ContinueExecution();
         logger.Info($"<<< Usi_UserDataEntered");
      }
      private void ContinueExecution()
      {
         logger.Info($">>> ContinueExecution");
         ChromeOptions co = new ChromeOptions()
         {
            AcceptInsecureCertificates = true
         };
         co.AddArguments(
            //"--headless",
            "--disable-extensions",
            "--disable-breakpad",
            "--disable-hang-monitor",
            "--disable-metrics-reporting",
            "--ssl-version-min=tl",
            "--no-sandbox",
         "--disable-dev-shm-usage",
            "--disable-metrics"
            );
         if (UserData.HideBrowser)
            co.AddArgument("--window-position=-32000,-32000");
         co.AddArgument($"user-agent={UserData.CustomUserAgent}");
         co.SetLoggingPreference("performance", LogLevel.All);
         logger.Info($"creating chromeDriverService");
         var chromeDriverService = ChromeDriverService.CreateDefaultService();
         chromeDriverService.HideCommandPromptWindow = true;
         logger.Info($"creating (Remote-)Driver");
         //driver = new RemoteWebDriver(new Uri("http://134.255.216.102:4444/"), co.ToCapabilities(),TimeSpan.FromSeconds(60));
         try
         {
            driver = new ChromeDriver(chromeDriverService, co);
            logger.Info($"driver created");
            driver.Manage().Window.Minimize();
         }
         catch (Exception ex)
         {
            MessageBox.Show($"{ex.Message}\n{ex.StackTrace}");
         }
         logger.Info($"navigating to 'https://{UserData.WorldServer}0.forgeofempires.com/'");
         driver.Navigate().GoToUrl($"https://{UserData.WorldServer}0.forgeofempires.com/");
         logger.Info($"navigated");
         cookieJar = driver.Manage().Cookies;
         jsExecutor = (IJavaScriptExecutor)driver;
         StaticData.BotData.CID = cookieJar.AllCookies.HasCookie("CID").Item2;
         StaticData.BotData.CSRF = cookieJar.AllCookies.HasCookie("CSRF").Item2;
         StaticData.BotData.SID = cookieJar.AllCookies.HasCookie("SID").Item2;
         StaticData.BotData.XSRF = cookieJar.AllCookies.HasCookie("XSRF-TOKEN").Item2;

         logger.Info($"Process Portraits");
         ProcessPortraits();
         logger.Info($"ExtractImages");
         ImageExtractor ie = new ImageExtractor();
         ie.GetGoodImages(UserData.WorldServer);
         ie.GetUnitImages(UserData.WorldServer, new Utils.Size(50, 50));

         if (!string.IsNullOrWhiteSpace(UserData.SerialKey))
         {
            logger.Info($"Init Premium if key exists");
            BackgroundWorkerEX bwPremium = new BackgroundWorkerEX();
            bwPremium.DoWork += CheckPremium;
            bwPremium.RunWorkerCompleted += CheckPremiumComplete;
            bwPremium.WorkerSupportsCancellation = true;
            bwPremium.RunWorkerCompleted += workerComplete;
            bwPremium.RunWorkerAsync();
            ListClass.BackgroundWorkers.Add(bwPremium);
         }

         string loginJS = resMgr.GetString("preloadLoginWorld");
         string loginCity = "false";
         string cityID = "";
         logger.Info($"Login Routine: {(UserData.LastWorld == null || string.IsNullOrWhiteSpace(UserData.LastWorld) ? "login need City" : UserData.LastWorld.Split('|')[0])}");
         if (UserData.LastWorld == null || string.IsNullOrWhiteSpace(UserData.LastWorld))
            loginCity = "true";
         else
            cityID = UserData.LastWorld.Split('|')[0];
         if (UserData.AutoLogin)
         {
            loginJS = resMgr.GetString("preloadLoginWorld");
            Log("[DEBUG] Doing Login", lbOutputWindow);
            loginJS = loginJS
               .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
               .Replace("###USERNAME###", UserData.Username)
               .Replace("###PASSWORD###", UserData.Password)
               .Replace("##server##", UserData.WorldServer)
               .Replace("##UserAgent##", UserData.CustomUserAgent)
             .Replace("##t##", "false")
             .Replace("##city##", "\"" + UserData.LastWorld.Split('|')[0] + "\"");
            var x = jsExecutor.ExecuteAsyncScript(loginJS);
            int breakCounter = 0;
            while (x is null)
            {
               if (breakCounter >= 6)
               {
                  MessageBox.Show("Failed to login...\n\nrestarting after dialog is closed", "Failed to Login");
                  UserData.SaveSettings();
                  RunningTime.Stop();
                  driver.Quit();
                  Process.Start(Application.ExecutablePath);
                  Environment.Exit(0);
               }
               x = jsExecutor.ExecuteAsyncScript(loginJS);
               breakCounter += 1;
            }
            var ret = (string)x;
            driver.Navigate().GoToUrl(ret);
            GetUIDAndForgeHX(driver.PageSource);
         }
         else if (loginCity == "true")
         {
            logger.Info($"login and requesting City");
            loginJS = resMgr.GetString("preloadLoginWorld");
            loginJS = loginJS
               .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
               .Replace("###USERNAME###", UserData.Username)
               .Replace("###PASSWORD###", UserData.Password)
               .Replace("##server##", UserData.WorldServer)
               .Replace("##t##", loginCity)
               .Replace("##city##", "\"" + UserData.LastWorld.Split('|')[0] + "\"");
            var ret = (string)jsExecutor.ExecuteAsyncScript(loginJS);
            logger.Info($"CityRequestScript returned: " + ret != null ? ret.Substring(0, 30) + " (first 30 Characters)" : "null");
            WorldData wd = JsonConvert.DeserializeObject<WorldData>(ret);
            ListClass.AllWorlds = wd.worlds;
            foreach (KeyValuePair<string, int> world in wd.player_worlds)
            {
               string worldname = "";
               foreach (World aworld in wd.worlds)
               {
                  if (aworld.id == world.Key)
                  {
                     worldname = aworld.name;
                     break;
                  }
               }
               ListClass.WorldList.Add(new Tuple<string, string, WorldState>(world.Key, worldname, WorldState.active));
            }
            UserData.PlayableWorlds = ListClass.WorldList.Where(wl => wl.Item3 == WorldState.active || wl.Item3 == WorldState.current).ToList().ConvertAll(new Converter<Tuple<string, string, WorldState>, string>(WorldToPlayable));
            WorldSelection ws = new WorldSelection(ListClass.WorldList);
            ws.WorldDataEntered += Ws_WorldDataEntered;
            logger.Info($"show WorldSelection Dialog");
            ws.ShowDialog();
         }
         else
         {
            logger.Info($"login in directly");
            loginJS = resMgr.GetString("preloadLoginWorld");
            loginJS = loginJS
                  .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
                  .Replace("###USERNAME###", UserData.Username)
                  .Replace("###PASSWORD###", UserData.Password)
                  .Replace("##server##", UserData.WorldServer)
               .Replace("##city##", "\"" + UserData.LastWorld.Split('|')[0] + "\"")
               .Replace("##t##", "false");
            logger.Info($"loginscript executing... {UserData.WorldServer} | {UserData.LastWorld.Split('|')[0]}");
            var ret = (string)jsExecutor.ExecuteAsyncScript(loginJS);
            logger.Info($"loginscript returned: {(ret.Length > 30 ? ret.Substring(0, 30) : ret)}");
            GetUIDAndForgeHX(driver.PageSource);
         }

         logger.Info($"<<< ContinueExecution");
      }
      private void CheckLanguages()
      {
         logger.Info($">>> CheckLanguages");
         if (!CheckForInternetConnection()) return;
         string contents = "";
         using (var wc = new WebClient())
            contents = wc.DownloadString("https://raw.githubusercontent.com/Th3C0D3R/FoBCS/master/ForgeOfBots/languages.json");
         try
         {
            LanguageList languages = JsonConvert.DeserializeObject<LanguageList>(contents);
            foreach (Language item in languages.Languages)
            {
               ListClass.AvailableLanguages.Languages.Add(item);
            }
         }
         catch (Exception ex)
         {
            NLog.LogManager.Flush();
            var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
            var properties = new Dictionary<string, string> { { "CheckLanguages", ex.Message } };
            Crashes.TrackError(ex, properties, attachments);
         }
         logger.Info($"<<< CheckLanguages");
      }
      private void Application_ApplicationExit(object sender, EventArgs e)
      {
         logger.Info($">>> Application_ApplicationExit");
         //TelegramNotify.Send($"Session ended {UserData.Username} { RunningTime.Elapsed:h'h 'm'm 's's'}");
         UserData.SaveSettings();
         RunningTime.Stop();
         driver.Quit();
         logger.Info($"<<< Application_ApplicationExit");
         if (restart)
         {
            restart = false;
            Process.Start("Updater.exe", $"restart{(DEBUGMODE ? " /debug" : "")}");
            Process.GetCurrentProcess().Kill();
         }
      }
      private void workerComplete(object sender, RunWorkerCompletedEventArgs e)
      {
         logger.Info($">>> workerComplete");
         ListClass.BackgroundWorkers.Remove((BackgroundWorkerEX)sender);
         if (((BackgroundWorkerEX)sender).param != null)
         {
            dynamic param = ((BackgroundWorkerEX)sender).param;
            try
            {
               if (param.argument2.GetType() == typeof(bool))
               {
                  mbSearch.Invoke((MethodInvoker)delegate
                  {
                     mbSearch.Enabled = false;
                     mbSearch.Text = i18n.getString("GUI.Sniper.SnipBotActive");
                  });
                  mbCancel.Invoke((MethodInvoker)delegate
                  {
                     mbCancel.Enabled = false;
                  });
               }
            }
            catch (Exception)
            { }
         }
         logger.Info($"<<< workerComplete");
      }
      private void Ws_WorldDataEntered(Form that, string key, string value)
      {
         logger.Info($">>> Ws_WorldDataEntered");
         UserData.LastWorld = $"{key}|{value}";
         logger.Info($"World Selected: {UserData.LastWorld.Split('|')[0]}");
         UserData.AutoLogin = true;
         UserData.SaveSettings();
         string loginJS = resMgr.GetString("preloadLoginWorld");
         Log("[DEBUG] Doing Login", lbOutputWindow);
         loginJS = loginJS
            .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
            .Replace("###USERNAME###", UserData.Username)
            .Replace("###PASSWORD###", UserData.Password)
            .Replace("##server##", UserData.WorldServer)
          .Replace("##t##", "false")
          .Replace("##city##", "\"" + UserData.LastWorld.Split('|')[0] + "\"");
         logger.Info($"Login executing... server: {UserData.WorldServer} | city: {UserData.LastWorld.Split('|')[0]}");
         var x = jsExecutor.ExecuteAsyncScript(loginJS);
         if (!(x is string @string))
            logger.Info($"loginscript returned not a string (URL): {x}");
         else
         {
            logger.Info($"loginscript returned: {@string}");
            var ret = @string;
            driver.Navigate().GoToUrl(ret);
            GetUIDAndForgeHX(driver.PageSource);
         }
         that.Close();
         logger.Info($"<<< Ws_WorldDataEntered");
      }
      private void GetUIDAndForgeHX(string source)
      {
         logger.Info($">>> GetUIDAndForgeHX");
         var regExUserID = new Regex(@"https:\/\/(\w{1,2}\d{1,2})\.forgeofempires\.com\/game\/json\?h=(.+)'", RegexOptions.IgnoreCase);
         var regExWSSecret = new Regex(@"'socket_token': '(.+)',", RegexOptions.IgnoreCase);
         var regExForgeHX = new Regex(@"https:\/\/foe\w{1,4}\.innogamescdn\.com\/\/cache\/ForgeHX(.+.js)'", RegexOptions.IgnoreCase);
         var FHXMatch = regExForgeHX.Match(source);
         var UIDMatch = regExUserID.Match(source);
         var WSSecretMatch = regExWSSecret.Match(source);
         if (UIDMatch.Success)
         {
            StaticData.BotData.UID = UIDMatch.Groups[2].Value;
            StaticData.BotData.WID = UIDMatch.Groups[1].Value;
            logger.Info($"UID Found: {StaticData.BotData.UID} | WID Found: {StaticData.BotData.WID}");
         }
         if (WSSecretMatch.Success)
         {
            string wsurl = "wss://" + StaticData.BotData.WID + ".forgeofempires.com/socket/";
            StaticData.BotData.WSecret = WSSecretMatch.Groups[1].Value;
            StaticData.BotData.WSUrl = wsurl;
            logger.Info($"WSSecret found: {StaticData.BotData.WSecret}");
         }
         if (FHXMatch.Success)
         {
            ForgeHX.ForgeHXURL = FHXMatch.Value;
            ForgeHX.FileName = "ForgeHX" + FHXMatch.Groups[1].Value;
            logger.Info($"ForgeHX-URL: {ForgeHX.ForgeHXURL}");
            ForgeHX.ForgeHXDownloaded += ForgeHX_ForgeHXDownloaded;
            ForgeHX.DownloadForge();
         }
         logger.Info($"<<< GetUIDAndForgeHX");
      }
      private void CheckPremiumComplete(object sender, RunWorkerCompletedEventArgs e)
      {
         logger.Info($">>> CheckPremiumComplete");
         if (e.Result is bool)
         {
            bool.TryParse(e.Result.ToString(), out bool clearControls);
            if (!clearControls)
            {
               //Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Clear());
            }
         }
         logger.Info($"<<< CheckPremiumComplete");
      }
      private void CheckPremium(object sender, DoWorkEventArgs e)
      {
         logger.Info($">>> CheckPremium");
         object ret = TcpConnection.SendAuthData(UserData.SerialKey, FingerPrint.Value(), false);
         if (ret is Result)
         {
            Enum.TryParse(ret.ToString(), out Result result);
            if (result == Result.Success)
            {
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} ({i18n.getString("Premium")}) | by TH3C0D3R");
               //object retList = ExecuteMethod(PremAssembly, "EntryPoint", "AddPremiumControl", null);
               //if (retList is List<UCPremium> list)
               //{
               //   var userPremiumEvent = new Dictionary<string, string>();
               //   string userPremium = $"{UserData.Username} ({UserData.LastWorld.Split('|')[0]}) {UserData.SerialKey}";
               //   userPremiumEvent.Add(UserData.Username, userPremium);
               //   Analytics.TrackEvent("UserHasPremium", userPremiumEvent);
               //   Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} ({i18n.getString("Premium")}) | by TH3C0D3R");
               e.Result = true;
               //}
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} ({i18n.getString("Expired")}) | by TH3C0D3R");
               if (!blockExpireBox)
               {
                  DialogResult dlgRes = MessageBox.Show(Owner, $"{i18n.getString("SubscriptionExpired")}", $"{i18n.getString("SubExpiredTitle")}", MessageBoxButtons.YesNo);
                  var userPremiumEvent = new Dictionary<string, string>();
                  string userPremium = $"{UserData.Username} ({UserData.LastWorld.Split('|')[0]}) {UserData.SerialKey}";
                  userPremiumEvent.Add(UserData.Username, userPremium);
                  Analytics.TrackEvent("UserPremiumExpired", userPremiumEvent);
                  if (dlgRes == DialogResult.Yes)
                  {
                     Process.Start("https://th3c0d3r.xyz/shop");
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
               var userPremiumEvent = new Dictionary<string, string>();
               string userPremium = $"{UserData.Username} ({UserData.LastWorld.Split('|')[0]}) {UserData.SerialKey}";
               userPremiumEvent.Add(UserData.Username, userPremium);
               Analytics.TrackEvent("UserPremiumFailed", userPremiumEvent);
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} | by TH3C0D3R");
               MessageBox.Show(Owner, $"{i18n.getString("LicenceNotValid")}", $"{i18n.getString("FailedToActivate")}");
            }
         }
         logger.Info($"<<< CheckPremium");
      }
      private void ForgeHX_ForgeHXDownloaded(object sender, EventArgs e)
      {
         logger.Info($">>> ForgeHX_ForgeHXDownloaded");
         LoadWorlds();
         if (isLoading && LoadingFrm != null)
         {
            isLoading = false;
            LoadingFrm.Close();
         }
         ForgeHX.DownloadFile = null;
         LoadingFrm = null;
         logger.Info($"<<< ForgeHX_ForgeHXDownloaded");
      }
      private void LoadWorlds()
      {
         logger.Info($">>> LoadWorlds");
         ReqBuilder.User_Key = StaticData.BotData.UID;
         ReqBuilder.VersionSecret = StaticData.SettingData.Version_Secret;
         ReqBuilder.Version = StaticData.SettingData.Version;
         ReqBuilder.UserAgent = StaticData.UserData.CustomUserAgent;
         ReqBuilder.WorldID = StaticData.BotData.WID;
         string script = ReqBuilder.GetRequestScript(RequestType.GetAllWorlds, "[]");
         var ret = (string)jsExecutor.ExecuteAsyncScript(script);
         Root<AllWorlds> ws = JsonConvert.DeserializeObject<Root<AllWorlds>>(ret);
         ListClass.WorldList.Clear();
         foreach (AllWorlds item in ws.responseData)
         {
            if (!ListClass.WorldList.HasCityID(item.id))
               ListClass.WorldList.Add(new Tuple<string, string, WorldState>(item.id, item.name, (WorldState)Enum.Parse(typeof(WorldState), item.status)));
            else
               ListClass.WorldList = ListClass.WorldList.ChangeTuple(item.id, item.name, (WorldState)Enum.Parse(typeof(WorldState), item.status));
         }
         UserData.PlayableWorlds = ListClass.WorldList.Where(wl => wl.Item3 == WorldState.active || wl.Item3 == WorldState.current).ToList().ConvertAll(new Converter<Tuple<string, string, WorldState>, string>(WorldToPlayable));
         UserData.SaveSettings();
#if DEBUG
         wsWorker = new WSWorker(StaticData.BotData.WSUrl)
         {
            Main = this
         };
         ListClass.BackgroundWorkers.Add(wsWorker.worker);
#endif
         ReloadData();
         InitSettingsTab();
         logger.Info($"<<< LoadWorlds");
      }
      public void ReloadData()
      {
         lock (_lock)
         {
            logger.Info($">>> ReloadData");
            Updater.UpdatePlayerLists();
            Updater.UpdateStartUp();
            Updater.UpdateOwnTavern();
            Updater.UpdateInventory();
            Updater.UpdateContribution();
            Updater.UpdateBoost();
            logger.Info($"<<< ReloadData");
         }
         UpdateGUI();
      }
      private void UpdateGUI()
      {
         logger.Info($">>> UpdateGUI");
         UpdateDashbord();
         UpdateSocial();
         UpdateTavern();
         UpdateProductionView();
         UpdateGoodProductionView();
         UpdateHiddenRewardsView();
         UpdateSnip();
         UpdateArmy();
         UpdateBoost();
         UpdateBattle();
         //UpdateMessageCenter();
         //UpdateChat();
         if (isFirstRun)
         {
            Visible = true;
            if (Program.LoadingFrm != null)
               Program.LoadingFrm.Close();

            isFirstRun = false;
            if (UserData.SnipBot && !isFirstRun && !tSniper.Enabled)
            {
               System.Timers.Timer temp = new System.Timers.Timer();
               temp.Elapsed += Temp_Elapsed;
               temp.Interval = 1000 * 5;
               temp.AutoReset = false;
               temp.Start();

               tSniper.Interval = 1000 * 60 * UserData.IntervalSnip;
               tSniper.Start();
            }
         }
         logger.Info($"<<< UpdateGUI");
      }
      private void Temp_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         TSniper_Tick(null, null);
      }
      private void UpdateDashbord()
      {
         logger.Info($">>> UpdateDashbord");
         if (GoodImageList != null)
         {
            try
            {
               Invoker.CallMethode(lvGoods, () => lvGoods.Items.Clear());
               Invoker.SetProperty(lvGoods, () => lvGoods.LargeImageList, GoodImageList);
               string lastEra = "";
               ListViewGroup group = null;
               ResearchEra noAge = ListClass.Eras.Find(e => e.era == "NoAge");
               foreach (KeyValuePair<string, List<Good>> item in ListClass.GoodsDict.Where(kvGood => kvGood.Key != noAge.name))
               {
                  if (item.Value.TrueForAll((b) => b.value == 0)) continue;
                  if (lastEra != item.Key)
                  {
                     group = new ListViewGroup(item.Key, HorizontalAlignment.Left);
                  }
                  foreach (Good good in item.Value)
                  {
                     ListViewItem lvi = new ListViewItem($"{good.name} ({good.value})", good.good_id)
                     {
                        Group = group
                     };
                     Invoker.CallMethode(lvGoods, () => lvGoods.Items.Add(lvi));
                     if (group != null && group.Header != lastEra)
                     {
                        Invoker.CallMethode(lvGoods, () => lvGoods.Groups.Add(group));
                        lastEra = item.Key;
                     }
                  }
               }
            }
            catch (Exception)
            { }
         }
         if (!string.IsNullOrEmpty(UserData.Username))
         {
            Invoker.SetProperty(lblUserData, () => lblUserData.Text, $"{UserData.Username} {ListClass.WorldList.Find(w => w.Item1 == UserData.LastWorld.Split('|')[0]).Item2}({UserData.LastWorld.Split('|')[0]})");
         }
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
            Invoker.SetProperty(lblMoney, () => lblMoney.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "money")["name"].ToString() + ":");
            Invoker.SetProperty(lblSupplies, () => lblSupplies.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "supplies")["name"].ToString() + ":");
            Invoker.SetProperty(lblMeds, () => lblMeds.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "medals")["name"].ToString() + ":");
            Invoker.SetProperty(lblDiamonds, () => lblDiamonds.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "premium")["name"].ToString() + ":");
            Invoker.SetProperty(lblFP, () => lblFP.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "strategy_points")["name"].ToString() + ":");
         }
         if (ListClass.Inventory.responseData.Count() > 0)
         {
            Items[] FPPackItems = ListClass.Inventory.responseData.ToList().FindAll(i => i.itemAssetName.EndsWith("forgepoints")).ToArray();
            if (FPPackItems.Count() > 0)
            {
               int FPinStock = 0;
               foreach (Items fpPack in FPPackItems)
               {
                  if (fpPack.itemAssetName.StartsWith("small"))
                     FPinStock += fpPack.inStock * 2;
                  else if (fpPack.itemAssetName.StartsWith("medium"))
                     FPinStock += fpPack.inStock * 5;
                  else if (fpPack.itemAssetName.StartsWith("large"))
                     FPinStock += fpPack.inStock * 10;
               }
               Invoker.SetProperty(lblFPStockValue, () => lblFPStockValue.Text, FPinStock.ToString("N0"));
            }
         }
         logger.Info($"<<< UpdateDashbord");
      }
      private void UpdateSocial()
      {
         logger.Info($">>> UpdateSocial");
         #region "other Players"
         ListClass.AllPlayers.Clear();
         ListClass.AllPlayers.AddRange(ListClass.FriendList);
         ListClass.AllPlayers.AddRange(ListClass.ClanMemberList);
         ListClass.AllPlayers.AddRange(ListClass.NeighborList);
         FillAutoComplete();
         var friendMotivate = ListClass.FriendList.FindAll(f => (f.next_interaction_in == 0));
         var clanMotivate = ListClass.ClanMemberList.FindAll(f => (f.next_interaction_in == 0));
         var neighborlist = ListClass.NeighborList.FindAll(f => (f.next_interaction_in == 0));
         Invoker.SetProperty(lblFriends, () => lblFriends.Text, i18n.getString("Friends"));
         Invoker.SetProperty(lblClanMember, () => lblClanMember.Text, i18n.getString("Clanmembers"));
         Invoker.SetProperty(lblNeighbor, () => lblNeighbor.Text, i18n.getString("Neighbors"));
         Invoker.SetProperty(lblFriendsCount, () => lblFriendsCount.Text, $"{friendMotivate.Count}/{ListClass.FriendList.Count}");
         Invoker.SetProperty(lblClanMemberCount, () => lblClanMemberCount.Text, $"{clanMotivate.Count}/{ListClass.ClanMemberList.Count}");
         Invoker.SetProperty(lblNeighborCount, () => lblNeighborCount.Text, $"{neighborlist.Count}/{ListClass.NeighborList.Count}");
         Invoker.SetProperty(lblPlayerLists, () => lblPlayerLists.Text, i18n.getString("PlayerLists"));

         if (!UserData.IgnoredPlayers.ContainsKey(UserData.LastWorld.Split('|')[0]))
            UserData.IgnoredPlayers.Add(UserData.LastWorld.Split('|')[0], new List<int>());

         UpdatePlayerLists();

         logger.Info($"<<< UpdateSocial");
         #endregion
      }
      private void UpdateTavern()
      {
         logger.Info($">>> UpdateSocial");
         #region "Tavern"
         if (ListClass.Resources.Count > 0)
         {
            Invoker.SetProperty(lblTavernSilverValue, () => lblTavernSilverValue.Text, ((int)ListClass.Resources["responseData"].First?.First?["tavern_silver"]?.ToObject(typeof(int))).ToString("N0"));
         }
         Invoker.SetProperty(lblTavernstate, () => lblTavernstate.Text, i18n.getString("TavernState"));
         Invoker.SetProperty(lblVisitable, () => lblVisitable.Text, i18n.getString("Visitable"));
         Invoker.SetProperty(btnCollect, () => btnCollect.Text, i18n.getString("CollectTavern"));
         if (ListClass.OwnTavern.responseData != null)
            Invoker.SetProperty(lblTavernstateValue, () => lblTavernstateValue.Text, ListClass.OwnTavern.responseData[2].ToString() + "/" + ListClass.OwnTavern.responseData[1].ToString());
         else
            Invoker.SetProperty(lblTavernstateValue, () => lblTavernstateValue.Text, "0/0");
         if (ListClass.ResourceDefinitions.Count > 0)
         {
            Invoker.SetProperty(lblTavernSilver, () => lblTavernSilver.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "tavern_silver")["name"].ToString());
         }

         var visitable = ListClass.FriendTaverns.FindAll(f => (f.sittingPlayerCount < f.unlockedChairCount && f.state == null));
         Invoker.SetProperty(lblVisitableValue, () => lblVisitableValue.Text, visitable.Count.ToString());
         Invoker.SetProperty(lblCurSitting, () => lblCurSitting.Text, i18n.getString("CurrentlySittingPlayers"));
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
                  TextAlign = ContentAlignment.BottomCenter,
                  Font = new Font("Microsoft Sans Serif", 15, FontStyle.Regular),
                  ImageList = Portraits,
                  ImageAlign = ContentAlignment.TopCenter,
                  ImageKey = item.avatar,
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
                     Text = i18n.getString("SitDown"),
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
                     Text = i18n.getString("CanNotSitDown")
                  };
                  Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(lnlSitState));
               }
               row += 1;
               Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.RowStyles.Add(new RowStyle(SizeType.AutoSize)));
            }
         }
         #endregion
         logger.Info($"<<< UpdateSocial");
      }
      private void RemoveFriend(object senderObj, EventArgs e)
      {
         logger.Info($">>> RemoveFriend");
         Button sender = ((Button)senderObj);
         if (sender.Tag != null)
         {
            string playerid = sender.Tag.ToString();
            string script = ReqBuilder.GetRequestScript(RequestType.RemovePlayer, playerid);
            jsExecutor.ExecuteAsyncScript(script);
            Updater.UpdateFirends();
            UpdateSocial();
         }
         logger.Info($"<<< RemoveFriend");
      }
      private void SitAtTavern(object senderObj, EventArgs e)
      {
         Button sender = ((Button)senderObj);
         if (sender.Tag != null)
         {
            string playerid = sender.Tag.ToString();
            string script = ReqBuilder.GetRequestScript(RequestType.VisitTavern, playerid);
            jsExecutor.ExecuteAsyncScript(script);
            ReloadData();
         }
      }
      private void btnCollect_Click(object sender, EventArgs e)
      {
         logger.Info($">>> btnCollect_Click");
         string script = ReqBuilder.GetRequestScript(RequestType.CollectTavern, "");
         var msg = (string)jsExecutor.ExecuteAsyncScript(script);
         if (msg == "{}") return;
         CollectResult cr = JsonConvert.DeserializeObject<CollectResult>(msg);
         if (cr.responseData.__class__.ToLower() == "success")
            ReloadData();
         logger.Info($"<<< btnCollect_Click");
      }
      private void MetroPanel1_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
         }
      }
      private void PbCLose_Click(object sender, EventArgs e)
      {
         logger.Info($">>> PbCLose_Click {(sender != null ? sender.ToString() : "null")}");
         if (ListClass.BackgroundWorkers.Count > 0)
         {
            foreach (BackgroundWorkerEX backgroundWorker in ListClass.BackgroundWorkers)
            {
               backgroundWorker.CancelAsync();
               while (backgroundWorker.IsBusy) { Application.DoEvents(); }
            }
            ListClass.BackgroundWorkers.Clear();
         }
         logger.Info($"<<< PbCLose_Click");
         Close();
      }
      private void Pbminimize_Click(object sender, EventArgs e)
      {
         WindowState = FormWindowState.Minimized;
      }
      private void PbFull_Click(object sender, EventArgs e)
      {
         WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
      }
      private void FrmMain_Load(object sender, EventArgs e)
      {
         logger.Info($">>> FrmMain_Load");
         RunningTime.Start();
         bwUptime.RunWorkerAsync();
         logger.Info($"Starting Bot");
         Log("[DEBUG] Starting Bot", lbOutputWindow);
         Updater = new CUpdate(ReqBuilder);
         i18n.TranslateHelp(tvHelp);
         if (!Utils.Settings.SettingsExists() ||
            string.IsNullOrWhiteSpace(UserData.Username) ||
            string.IsNullOrWhiteSpace(UserData.Password) ||
            string.IsNullOrWhiteSpace(UserData.LastWorld) ||
            string.IsNullOrWhiteSpace(UserData.WorldServer) ||
            UserData.PlayableWorlds.Count <= 0)
         {
            logger.Info($"empty or none userdata, creating new one");
            Log("[DEBUG] empty or none userdata, creating new one!", lbOutputWindow);
            if (!Utils.Settings.SettingsExists())
               UserData = new Utils.Settings();
            usi = new UserDataInput(ListClass.ServerList)
            {
               TopMost = true
            };
            usi.UserDataEntered += Usi_UserDataEntered;
            DialogResult dlgRes = usi.ShowDialog();
            if (dlgRes == DialogResult.Cancel) Environment.Exit(0);
            logger.Info($"userdata entered");
            mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} | by TH3C0D3R";
            return;
         }
         else
         {
            ContinueExecution();
            mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} | by TH3C0D3R";
         }
         var startEvent = new Dictionary<string, string>();
         string startUp = $"{Identifier.GetInfo(_WCS, _WCS_Model)}-{Identifier.GetInfo(_WCS, _WCS_SystemType)} ({Identifier.GetInfo(_WOS, _WOS_Caption)}) ({UserData.Username})";
         startEvent.Add("Startup", startUp);
         Analytics.TrackEvent("Startup", startEvent);
         TelegramNotify.Init();
         logger.Info($"<<< FrmMain_Load");
      }
      private void FrmMain_Shown(object sender, EventArgs e)
      {
         if (isFirstRun)
         {
            Visible = false;
         }
      }
      private void BwUptime_DoWork(object sender, DoWorkEventArgs e)
      {
         while (!bwUptime.CancellationPending)
         {
            Invoker.SetProperty(lblRunning, () => lblRunning.Text, $"{i18n.getString("GUI.Header.Running")} {RunningTime.Elapsed:h'h 'm'm 's's'}");
            Thread.Sleep(999);
         }
      }
      private void CmsMainMenu_Opening(object sender, CancelEventArgs e)
      {
         foreach (ToolStripItem item in cmsMainMenu.Items)
         {
            item.Enabled = item.Visible = false;
         }

         tsmiTestFunctions.Visible = true;
         tsmiTestFunctions.Enabled = true;
#if RELEASE
         tsmiTestFunctions.Visible = false;
         tsmiTestFunctions.Enabled = false;
#endif
         toolStripSeparator1.Visible = false;

         if (tabControl1.SelectedTab.Tag.ToString() == "GUI.Social.Header")
         {
            tsmiCollectTavern.Enabled = tsmiCollectTavern.Visible = false;
            tsmiCollectIncidents.Enabled = tsmiCollectIncidents.Visible = false;
            tsmiVisitTavern.Enabled = tsmiVisitTavern.Visible = false;
            tsmiCancelProduction.Enabled = tsmiCancelProduction.Visible = false;
            tsmiCollectProduction.Enabled = tsmiCollectProduction.Visible = false;
            tsmiStartProduction.Enabled = tsmiStartProduction.Visible = false;

            tsmiMoppleClan.Enabled = tsmiMoppleClan.Visible = true;
            tsmiMoppleFriends.Enabled = tsmiMoppleFriends.Visible = true;
            tsmiMoppleNeighbor.Enabled = tsmiMoppleNeighbor.Visible = true;
            tsmiVisitMopple.Enabled = tsmiVisitMopple.Visible = true;

            toolStripSeparator1.Visible = true;
         }
         else if (tabControl1.SelectedTab.Tag.ToString() == "GUI.Tavern.Header")
         {
            tsmiMoppleClan.Enabled = tsmiMoppleClan.Visible = false;
            tsmiMoppleFriends.Enabled = tsmiMoppleFriends.Visible = false;
            tsmiMoppleNeighbor.Enabled = tsmiMoppleNeighbor.Visible = false;
            tsmiCollectIncidents.Enabled = tsmiCollectIncidents.Visible = false;
            tsmiCancelProduction.Enabled = tsmiCancelProduction.Visible = false;
            tsmiCollectProduction.Enabled = tsmiCollectProduction.Visible = false;
            tsmiStartProduction.Enabled = tsmiStartProduction.Visible = false;

            tsmiCollectTavern.Enabled = tsmiCollectTavern.Visible = true;
            tsmiVisitTavern.Enabled = tsmiVisitTavern.Visible = true;
            tsmiVisitMopple.Enabled = tsmiVisitMopple.Visible = true;

            toolStripSeparator1.Visible = true;
         }
         else if (tabControl1.SelectedTab.Tag.ToString() == "GUI.City.Header")
         {
            tsmiMoppleClan.Enabled = tsmiMoppleClan.Visible = false;
            tsmiMoppleFriends.Enabled = tsmiMoppleFriends.Visible = false;
            tsmiMoppleNeighbor.Enabled = tsmiMoppleNeighbor.Visible = false;
            tsmiCancelProduction.Enabled = tsmiCancelProduction.Visible = false;
            tsmiCollectProduction.Enabled = tsmiCollectProduction.Visible = false;
            tsmiStartProduction.Enabled = tsmiStartProduction.Visible = false;
            tsmiCollectTavern.Enabled = tsmiCollectTavern.Visible = false;
            tsmiVisitTavern.Enabled = tsmiVisitTavern.Visible = false;
            tsmiVisitMopple.Enabled = tsmiVisitMopple.Visible = false;

            tsmiCollectIncidents.Enabled = tsmiCollectIncidents.Visible = true;

            toolStripSeparator1.Visible = true;
         }
         else if (tabControl1.SelectedTab.Tag.ToString() == "GUI.Production.Header")
         {
            tsmiMoppleClan.Enabled = tsmiMoppleClan.Visible = false;
            tsmiMoppleFriends.Enabled = tsmiMoppleFriends.Visible = false;
            tsmiMoppleNeighbor.Enabled = tsmiMoppleNeighbor.Visible = false;
            tsmiCancelProduction.Enabled = tsmiCancelProduction.Visible = false;
            tsmiCollectProduction.Enabled = tsmiCollectProduction.Visible = false;
            tsmiStartProduction.Enabled = tsmiStartProduction.Visible = false;
            tsmiCollectTavern.Enabled = tsmiCollectTavern.Visible = false;
            tsmiVisitTavern.Enabled = tsmiVisitTavern.Visible = false;
            tsmiVisitMopple.Enabled = tsmiVisitMopple.Visible = false;
            tsmiCollectIncidents.Enabled = tsmiCollectIncidents.Visible = false;

            tsmiCancelProduction.Enabled = tsmiCancelProduction.Visible = true;
            tsmiCollectProduction.Enabled = tsmiCollectProduction.Visible = true;
            tsmiStartProduction.Enabled = tsmiStartProduction.Visible = true;

            toolStripSeparator1.Visible = true;
         }
         tsmiReloadDataToolStripMenuItem.Enabled = tsmiReloadDataToolStripMenuItem.Visible = true;
      }
      private void TsmiReloadDataToolStripMenuItem_Click(object sender, EventArgs e)
      {
         tsmiReloadDataToolStripMenuItem.Enabled = false;
         ReloadData();
         tsmiReloadDataToolStripMenuItem.Enabled = true;
      }

      #region "Incident"
      private void UpdateHiddenRewardsView()
      {
         logger.Info($">>> UpdateHiddenRewardsView");
         if (pnlIncident.InvokeRequired)
         {
            Invoker.CallMethode(pnlIncident, () => pnlIncident.Controls.Clear());
         }
         else
         {
            pnlIncident.Controls.Clear();
         }
         if (ListClass.HiddenRewards.Count == 0) return;
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
         logger.Info($"<<< UpdateHiddenRewardsView");
      }
      private void TsmiCollectIncidentsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CollectIncident };
         BackgroundWorkerEX bw = new BackgroundWorkerEX();
         bw.DoWork += bwScriptExecuterOneArg_DoWork;
         bw.param = param;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerCompleted += workerComplete;
         bw.RunWorkerAsync(param);
         ListClass.BackgroundWorkers.Add(bw);
      }
      #endregion

      #region "Production"
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
               foreach (KeyValuePair<string, List<EntityEx>> item in groupedList)
               {
                  ProdListItem pli = new ProdListItem();
                  if (item.Value.First().state["__class__"].ToString() == "IdleState")
                  {
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {i18n.getString("ProductionIdle")}", i18n.getString("ProductionIdle"));
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProducingState")
                  {
                     var productName = item.Value.First().state["current_product"]["product"]["resources"].ToList().First().ToObject<JProperty>().Name;
                     productName = ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == productName)["name"].ToString();
                     var productValue = item.Value.First().state["current_product"]["product"]["resources"].ToList().First().First;
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {productName} ({(int.Parse(productValue.ToString())) * item.Value.Count})", "");
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_in"].ToString())).First().state["next_state_transition_in"].ToString();
                     string greatestEnd = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_at"].ToString())).First().state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     var productName = item.Value.First().state["current_product"]["product"]["resources"].ToList().First().ToObject<JProperty>().Name;
                     productName = ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == productName)["name"].ToString();
                     var productValue = item.Value.First().state["current_product"]["product"]["resources"].ToList().First().First;
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {productName} ({(int.Parse(productValue.ToString())) * item.Value.Count})", i18n.getString("ProductionFinishedState"));
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.ContextMenuStrip = cmsMainMenu;
                  pli.isGoodBuilding = false;
                  pli.AddEntities(item.Value.Select(i => i.id).ToList().ToArray());
                  pli.UpdateGUIEvent += UpdateProdGUI;
                  pli.jsExecutor = jsExecutor;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlProductionList, () => pnlProductionList.Controls.Add(pli));
                  ListClass.ProductionItems.Add(pli);
               }
            }
            else
            {
               if (DEBUGMODE) Log($"[{DateTime.Now}] Single Update", lbOutputWindow);
               foreach (EntityEx item in ListClass.ProductionList)
               {
                  ProdListItem pli = new ProdListItem();
                  if (item.state["__class__"].ToString() == "IdleState")
                  {
                     pli.FillControl($"{item.name}", $"{i18n.getString("ProductionIdle")}", i18n.getString("ProductionIdle"));
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.state["__class__"].ToString() == "ProducingState")
                  {
                     pli.FillControl($"{item.name}", $"{i18n.getString("ProducingState")}", i18n.getString("ProducingState"));
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.state["next_state_transition_in"].ToString();
                     string greatestEnd = item.state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     pli.FillControl($"{item.name}", $"{i18n.getString("ProductionFinishedState")}", i18n.getString("ProductionFinishedState"));
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.ContextMenuStrip = cmsMainMenu;
                  pli.isGoodBuilding = false;
                  pli.AddEntities(item.id);
                  pli.UpdateGUIEvent += UpdateProdGUI;
                  pli.jsExecutor = jsExecutor;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlProductionList, () => pnlProductionList.Controls.Add(pli));
                  ListClass.ProductionItems.Add(pli);
               }
            }
         }
         Invoker.CallMethode(pnlProductionList, () => pnlProductionList.Invalidate());

         if (ListClass.FinishedProductions.Count > 0)
         {
            Invoker.SetProperty(lblSumFinProdValue, () => lblSumFinProdValue.Text, ListClass.FinishedProductions.Count.ToString());
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
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {i18n.getString("ProductionIdle")}", i18n.getString("ProductionIdle"));
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProducingState")
                  {
                     var productName = item.Value.First().state["current_product"]["product"]["resources"].ToList().First().ToObject<JProperty>().Name;
                     productName = ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == productName)["name"].ToString();
                     var productValue = item.Value.First().state["current_product"]["product"]["resources"].ToList().First().First;
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {productName} ({(int.Parse(productValue.ToString())) * item.Value.Count})", "");
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_in"].ToString())).First().state["next_state_transition_in"].ToString();
                     string greatestEnd = item.Value.OrderByDescending(e => double.Parse(e.state["next_state_transition_at"].ToString())).First().state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.Value.First().state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     pli.FillControl($"{item.Value.Count}x {item.Value.First().name}", $"{item.Value.Count}x {i18n.getString("ProductionFinishedState")}", i18n.getString("ProductionFinishedState"));
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.ContextMenuStrip = cmsMainMenu;
                  pli.isGoodBuilding = true;
                  pli.AddEntities(item.Value.Select(i => i.id).ToList().ToArray());
                  pli.UpdateGUIEvent += UpdateProdGUI;
                  pli.jsExecutor = jsExecutor;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlGoodProductionList, () => pnlGoodProductionList.Controls.Add(pli));
                  ListClass.ProductionItems.Add(pli);
               }
            }
            else
            {
               foreach (EntityEx item in ListClass.GoodProductionList)
               {
                  ProdListItem pli = new ProdListItem();
                  if (item.state["__class__"].ToString() == "IdleState")
                  {
                     pli.FillControl($"{item.name}", $"{i18n.getString("ProductionIdle")}", i18n.getString("ProductionIdle"));
                     pli.ProductionState = ProductionState.Idle;
                  }
                  else if (item.state["__class__"].ToString() == "ProducingState")
                  {
                     pli.FillControl($"{item.name}", $"{i18n.getString("ProducingState")}", "");
                     pli.ProductionState = ProductionState.Producing;
                     string greatestDur = item.state["next_state_transition_in"].ToString();
                     string greatestEnd = item.state["next_state_transition_at"].ToString();
                     pli.AddTime(greatestDur, greatestEnd);
                  }
                  else if (item.state["__class__"].ToString() == "ProductionFinishedState")
                  {
                     pli.FillControl($"{item.name}", $"{i18n.getString("ProductionFinishedState")}", i18n.getString("ProductionFinishedState"));
                     pli.ProductionState = ProductionState.Finished;
                  }
                  pli.Dock = DockStyle.Top;
                  pli.ContextMenuStrip = cmsMainMenu;
                  pli.isGoodBuilding = true;
                  pli.AddEntities(item.id);
                  pli.UpdateGUIEvent += UpdateProdGUI;
                  pli.jsExecutor = jsExecutor;
                  pli.StartProductionGUI();
                  Invoker.CallMethode(pnlGoodProductionList, () => pnlGoodProductionList.Controls.Add(pli));
                  ListClass.ProductionItems.Add(pli);
               }
            }
         }
         Invoker.CallMethode(pnlGoodProductionList, () => pnlGoodProductionList.Invalidate());
      }
      private void UpdateProdGUI(object sender, dynamic data)
      {
         if (sender is ProdListItem item)
         {
            Updater.UpdateEntities();
            if (DEBUGMODE) Log($"[{DateTime.Now}] Done STATE: {item.ProductionState}", lbOutputWindow);
            update(item);
         }

      }

      private void update(ProdListItem sender)
      {
         if (!UserData.ProductionBot) return;
         if (sender is ProdListItem ProdItem)
         {
            switch (ProdItem.ProductionState)
            {
               case ProductionState.Idle:
                  ProdItem.StartProduction();
                  break;
               case ProductionState.Producing:
                  break;
               case ProductionState.Finished:
                  ProdItem.CollectProduction();
                  break;
               default:
                  break;
            }
            UpdateProductionView();
            UpdateGoodProductionView();
         }
      }
      private void StartProduction()
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.QueryProduction };
         bwScriptExecuterOneArg_DoWork(this, new DoWorkEventArgs(param));
      }
      private void CollectProduction()
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CollectProduction };
         bwScriptExecuterOneArg_DoWork(this, new DoWorkEventArgs(param));
      }
      private void CancelProduction()
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CancelProduction };
         bwScriptExecuterOneArg_DoWork(this, new DoWorkEventArgs(param));
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
               string ret = (string)jsExecutor.ExecuteAsyncScript(script);
               try
               {
                  JToken ColRes = JsonConvert.DeserializeObject<JToken>(ret);
                  if (ColRes["responseData"]?["updatedEntities"]?.ToList().Count > 0
                     && ColRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "IdleState")
                  {
                     foreach (var item in ColRes["responseData"]?["updatedEntities"].ToList())
                     {
                        int exIndex = ListClass.ProductionList.FindIndex(x => x.id == int.Parse(item["id"].ToString()));
                        if (exIndex >= 0)
                        {
                           EntityEx old = ListClass.ProductionList[exIndex];
                           ListClass.ProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(item.ToString());
                           ListClass.ProductionList[exIndex].name = old.name;
                           ListClass.ProductionList[exIndex].type = old.type;
                           ListClass.ProductionList[exIndex].available_products = old.available_products;
                        }
                        else
                        {
                           exIndex = ListClass.GoodProductionList.FindIndex(g => g.id == int.Parse(item["id"].ToString()));
                           if (exIndex >= 0)
                           {
                              EntityEx old = ListClass.GoodProductionList[exIndex];
                              ListClass.GoodProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(item.ToString());
                              ListClass.GoodProductionList[exIndex].name = old.name;
                              ListClass.GoodProductionList[exIndex].type = old.type;
                              ListClass.GoodProductionList[exIndex].available_products = old.available_products;
                           }
                        }
                     }
                  }
               }
               catch (Exception ex)
               {
                  NLog.LogManager.Flush();
                  var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
                  var properties = new Dictionary<string, string> { { "CollectProduction", ret } };
                  Crashes.TrackError(ex, properties, attachments);
               }
               Updater.UpdateResources();
               UpdateProductionView();
               UpdateGoodProductionView();
               UpdateDashbord();
               //if (UserData.ProductionBotNotification)
               //TelegramNotify.Send($"{i18n.getString("GUI.Telegram.ProdFinished")}{(UserData.ProductionBot ? $" {i18n.getString("GUI.Telegram.ProdStarted")}" : "")}");
               break;
            case RequestType.QueryProduction:
               List<string> retQuery = new List<string>();
               ids = ListClass.GoodProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() == "idlestate"; }).ToList().Select(y => y.id).ToArray();
               ListClass.AddedToQuery.AddRange(ids);
               foreach (int id in ids)
               {
                  script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.GoodProductionOption.id });
                  retQuery.Add((string)jsExecutor.ExecuteAsyncScript(script));
                  Thread.Sleep(100);
               }
               ids = ListClass.ProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() == "idlestate"; }).ToList().Select(y => y.id).ToArray();
               ListClass.AddedToQuery.AddRange(ids);
               foreach (int id in ids)
               {
                  script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.ProductionOption.id });
                  retQuery.Add((string)jsExecutor.ExecuteAsyncScript(script));
                  Thread.Sleep(100);
               }
               foreach (string item in retQuery)
               {
                  JToken QueryRes = JsonConvert.DeserializeObject<JToken>(item);
                  try
                  {
                     if (QueryRes["responseData"]?["updatedEntities"]?.ToList().Count > 0
                        && QueryRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "ProducingState")
                     {

                        int id = int.Parse(QueryRes["responseData"]?["updatedEntities"]?[0]?["id"].ToString());
                        if (ListClass.AddedToQuery.Contains(id))
                        {
                           ListClass.doneQuery.Add(id);
                           int exIndex = ListClass.ProductionList.FindIndex(g => g.id == id);
                           if (exIndex >= 0)
                           {
                              EntityEx old = ListClass.ProductionList[exIndex];
                              ListClass.ProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(QueryRes["responseData"]?["updatedEntities"][0].ToString());
                              ListClass.ProductionList[exIndex].name = old.name;
                              ListClass.ProductionList[exIndex].type = old.type;
                              ListClass.ProductionList[exIndex].available_products = old.available_products;
                           }
                           else
                           {
                              exIndex = ListClass.GoodProductionList.FindIndex(x => x.id == int.Parse(QueryRes["responseData"]?["updatedEntities"][0]?["id"].ToString()));
                              if (exIndex >= 0)
                              {
                                 EntityEx old = ListClass.GoodProductionList[exIndex];
                                 ListClass.GoodProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(QueryRes["responseData"]?["updatedEntities"][0].ToString());
                                 ListClass.GoodProductionList[exIndex].name = old.name;
                                 ListClass.GoodProductionList[exIndex].type = old.type;
                                 ListClass.GoodProductionList[exIndex].available_products = old.available_products;
                              }
                           }
                        }
                        var a = ListClass.doneQuery.All(ListClass.AddedToQuery.Contains) && ListClass.AddedToQuery.Count == ListClass.doneQuery.Count;
                        if (a)
                        {
                           ListClass.doneQuery.Clear();
                           ListClass.AddedToQuery.Clear();
                           if (DEBUGMODE) Helper.Log($"[{DateTime.Now}] doneQuery Count = {ListClass.doneQuery.Count}\n[{DateTime.Now}] AddedToQuery Count = {ListClass.AddedToQuery.Count}");
                        }
                     }
                  }
                  catch (Exception ex)
                  {
                     NLog.LogManager.Flush();
                     var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
                     var properties = new Dictionary<string, string> { { "QueryProduction", string.Join("\n", retQuery.ToArray()) } };
                     Crashes.TrackError(ex, properties, attachments);
                  }
               }
               Updater.UpdateResources();
               UpdateProductionView();
               UpdateGoodProductionView();
               UpdateDashbord();
               break;
            case RequestType.CancelProduction:
               List<string> retCancel = new List<string>();
               ids = ListClass.GoodProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() != "idlestate"; }).ToList().Select(y => y.id).ToArray();
               foreach (int id in ids)
               {
                  script = ReqBuilder.GetRequestScript(param.t1, id);
                  retCancel.Add((string)jsExecutor.ExecuteAsyncScript(script));
                  Thread.Sleep(100);
               }
               ids = ListClass.ProductionList.Where((x) => { return x.state["__class__"].ToString().ToLower() != "idlestate"; }).ToList().Select(y => y.id).ToArray();
               foreach (int id in ids)
               {
                  script = ReqBuilder.GetRequestScript(param.t1, id);
                  retCancel.Add((string)jsExecutor.ExecuteAsyncScript(script));
                  Thread.Sleep(100);
               }
               Updater.UpdateResources();
               UpdateProductionView();
               UpdateGoodProductionView();
               UpdateDashbord();
               break;
            case RequestType.CollectIncident:
               UserData.LastIncidentTime = DateTime.Now;
               UserData.SaveSettings();
               foreach (HiddenReward item in ListClass.HiddenRewards)
               {
                  if (!item.isVisible) continue;
                  if (!UserData.ShowBigRoads && item.position.context == "cityRoadBig") continue;
                  script = ReqBuilder.GetRequestScript(param.t1, item.hiddenRewardId);
                  string retIncident = (string)jsExecutor.ExecuteAsyncScript(script);
                  string[] ciResponse = retIncident.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
                  bool successed = false;
                  dynamic Reward = null;
                  foreach (string res in ciResponse)
                  {
                     var methode = res.Substring(0, res.IndexOf("{"));
                     var body = res.Substring(res.IndexOf("{"));
                     dynamic ColIncRes = JsonConvert.DeserializeObject(body);
                     if (ColIncRes["requestClass"].ToString() == "HiddenRewardService")
                     {
                        if (ColIncRes["responseData"]["__class__"].ToString() == "Success")
                           successed = true;
                        if (Reward != null)
                        {
                           Log($"{i18n.getString("IncidentCollected")} - {i18n.getString("Reward")}: {Reward.ToString()}", lbOutputWindow, lbIncidentBox);
                        }
                     }
                     else if (ColIncRes["requestClass"].ToString() == "RewardService")
                     {
                        Reward = ColIncRes["responseData"][0][0]["name"];
                        if (successed)
                        {

                           Log($"{i18n.getString("IncidentCollected")} - {i18n.getString("Reward")}: {Reward.ToString()}", lbOutputWindow, lbIncidentBox);
                        }
                     }
                  }
                  Thread.Sleep(333);
               }
               script = ReqBuilder.GetRequestScript(RequestType.GetIncidents, "");
               ret = (string)jsExecutor.ExecuteAsyncScript(script);
               HiddenRewardRoot newHiddenRewards = JsonConvert.DeserializeObject<HiddenRewardRoot>(ret);
               foreach (HiddenReward item in newHiddenRewards.responseData.hiddenRewards)
               {
                  DateTime endTime = UnixTimeStampToDateTime(item.expireTime);
                  DateTime startTime = UnixTimeStampToDateTime(item.startTime);
                  bool vis = (endTime > DateTime.Now) && (startTime < DateTime.Now);
                  item.isVisible = vis;
               }
               ListClass.HiddenRewards = newHiddenRewards.responseData.hiddenRewards.ToList();
               UpdateHiddenRewardsView();
               Updater.UpdateResources();
               UpdateDashbord();
               UpdateHiddenRewardsView();
               break;
            case RequestType.CollectOtherProductions:
               Updater.UpdateEntities(true);
               ids = ListClass.FinishedProductions.Where((x) => { return x.state["__class__"].ToString().ToLower() != "idlestate" && x.state["__class__"].ToString().ToLower() != "producingstate"; }).ToList().Select(y => y.id).ToArray();
               script = ReqBuilder.GetRequestScript(RequestType.CollectProduction, ids);
               _ = (string)jsExecutor.ExecuteAsyncScript(script);
               Updater.UpdateEntities();
               UpdateProductionView();
               UpdateGoodProductionView();
               break;
            default:
               break;
         }
      }

      private void BtnShowList_Click(object sender, EventArgs e)
      {
         if (Application.OpenForms["FinProdForm"] != null) Application.OpenForms["FinProdForm"].Close();
         FinProdForm fpf = new FinProdForm();

         var items = ListClass.FinishedProductions.Select(ex =>
         {
            var productName = "";
            var productValue = "";
            var exprods = "";
            int prio = 999;
            Priority type = Priority.NoMatter;
            if (ex.state["current_product"]["product"] == null)
            {
               if (ex.state["current_product"]["goods"] == null)
               {
                  if (!string.Equals(ex.state["current_product"]["name"].ToString(), "penal_unit"))
                  {
                     MessageBox.Show("!!!! PLEASE REPORT THE TEXT INSIDE YOU CLIPBOARD TO THE DEV (GITHUB/DISCORD) (COPY/PASTE) !!!!", "PLEASE REPORT!!");
                     Clipboard.SetText(ex.state.ToString());
                  }
                  else
                  {
                     productName = i18n.getString("GUI.Production.AlcaUnit");
                     productValue = ex.state["current_production"]["amount"].ToString();
                     exprods = $"{productValue} {productName}";
                  }
               }
               else
               {
                  foreach (var i in ex.state["current_product"]["goods"].ToList())
                  {
                     productName = i["good_id"].ToString();
                     productName = ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == productName)["name"].ToString();
                     productValue = i["value"].ToString();
                     exprods += $" {productValue} {productName},";
                  }
                  exprods = exprods.TrimEnd(',');
               }
            }
            else
            {
               foreach (var i in ex.state["current_product"]["product"]["resources"].ToList())
               {
                  productName = i.ToObject<JProperty>().Name;
                  productName = ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == productName)["name"].ToString();
                  productValue = i.First.ToString();
                  exprods += $" {productValue} {productName},";
               }
               exprods = exprods.TrimEnd(',');
            }
            switch (ex.cityentity_id)
            {
               case "X_OceanicFuture_Landmark3":
                  prio = 1;
                  break;
               default:
                  break;
            }
            return new EntityProd() { entity = ex, Name = ex.name, Value = exprods, Priority = prio, Type = type };
         }).ToList();
         items = items.OrderBy(o => o.Priority).ToList();
         fpf.AddItem(items.ToArray());
         fpf.CollectAll += CollectAll;
         fpf.CollectSelected += CollectSelected;
         fpf.Show();
      }
      private void CollectSelected(object sender, dynamic data = null)
      {
         List<EntityProd> entities = new List<EntityProd>();
         foreach (var item in (CheckedItemCollection)data)
         {
            entities.Add((EntityProd)item);
         }
         entities = entities.OrderBy(o => o.Priority).ToList();
         TwoTArgs<RequestType, List<EntityProd>> param = new TwoTArgs<RequestType, List<EntityProd>> { RequestType = RequestType.CollectOtherProductions, argument2 = entities };
         bwScriptExecuter_DoWork(this, new DoWorkEventArgs(param));
         if (Application.OpenForms["FinProdForm"] != null) Application.OpenForms["FinProdForm"].Close();
      }
      private void CollectAll(object sender, dynamic data = null)
      {
         List<EntityProd> entities = new List<EntityProd>();
         foreach (var item in (ObjectCollection)data)
         {
            entities.Add((EntityProd)item);
         }
         entities = entities.OrderBy(o => o.Priority).ToList();
         TwoTArgs<RequestType, List<EntityProd>> param = new TwoTArgs<RequestType, List<EntityProd>> { RequestType = RequestType.CollectOtherProductions, argument2 = entities };
         bwScriptExecuter_DoWork(this, new DoWorkEventArgs(param));
         Invoker.SetProperty(lblSumFinProdValue, () => lblSumFinProdValue.Text, "0");
         if (Application.OpenForms["FinProdForm"] != null) Application.OpenForms["FinProdForm"].Close();
      }

      private void TsmiStartProduction_Click(object sender, EventArgs e)
      {
         StartProduction();
      }
      private void TsmiCollectProduction_Click(object sender, EventArgs e)
      {
         CollectProduction();
      }
      private void TsmiCancelProduction_Click(object sender, EventArgs e)
      {
         CancelProduction();
      }
      private void TsmiCollectOtherProductionToolStripMenuItem_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region "Settings"
      private void InitSettingsTab()
      {
         Control.ControlCollection mrb5s = mpProdCycle.Controls;
         foreach (Control control in mrb5s)
         {
            try
            {
               MetroRadioButton mrb = (MetroRadioButton)control;
               if (int.Parse(mrb.Tag.ToString()) == UserData.ProductionOption.time)
               {
                  mrb.Checked = true;
                  break;
               }
            }
            catch (Exception)
            {
            }
         }
         Control.ControlCollection mrbG4h = mpGoodCycle.Controls;
         foreach (Control control in mrbG4h)
         {
            try
            {
               MetroRadioButton mrb = (MetroRadioButton)control;
               if (int.Parse(mrb.Tag.ToString()) == UserData.GoodProductionOption.time)
               {
                  mrb.Checked = true;
                  break;
               }
            }
            catch (Exception)
            {
            }
         }
         mtView.Checked = UserData.GroupedView;
         mtIncident.Checked = UserData.IncidentBot;
         mtMoppel.Checked = UserData.MoppelBot;
         mtRQBot.Checked = UserData.RQBot;
         mtProduction.Checked = UserData.ProductionBot;
         mtTavern.Checked = UserData.TavernBot;
         mtBigRoads.Checked = UserData.ShowBigRoads;
         mtbCustomUserAgent.Text = UserData.CustomUserAgent;
         mtbSerialKey.Text = UserData.SerialKey;
         mtAutoLogin.Checked = UserData.AutoLogin;
         mtToggleBrowser.Checked = UserData.HideBrowser;
         mtDarkMode.Checked = UserData.DarkMode;
         mtbTelegramUsername.Text = UserData.TelegramUserName;
         nudMinProfit.Value = UserData.MinProfit;
         nudMinProfitPerc.Value = UserData.MinProfitPercentage;
         nudMaxInvest.Value = UserData.MaxInvestment;
         mcbAutoInvest.Checked = UserData.AutoInvest;
         mcbBlueGalaxiePrio.SelectedItem = UserData.BGPriority;
         mtSnipBot.Checked = UserData.SnipBot;
         mtBuyAttempts.Checked = UserData.BuyGEXAttempts;
         nudSnipInterval.Value = UserData.IntervalSnip;
         mcbNotifySnip.Checked = UserData.SnipBotNotification;
         mcbNotifyProd.Checked = UserData.ProductionBotNotification;
         mcbFriends.Checked = UserData.SelectedSnipTarget.HasFlag(SnipTarget.friends);
         mcbGuild.Checked = UserData.SelectedSnipTarget.HasFlag(SnipTarget.members);
         mcbNeighbor.Checked = UserData.SelectedSnipTarget.HasFlag(SnipTarget.neighbors);
         mcbCitySelection.Items.Clear();
         if (UserData.PlayableWorlds == null || UserData.PlayableWorlds.Count == 0)
         {
            mcbCitySelection.Enabled = false;
            mbSaveReload.Enabled = false;
         }
         else
         {
            for (int i = 0; i < UserData.PlayableWorlds.Count; i++)
            {
               string item = UserData.PlayableWorlds[i];
               if (StaticData.Version.IsVersion("2.1", true))
                  mcbCitySelection.Items.Add(new PlayAbleWorldItem() { WorldID = item.Split('|')[0], WorldName = item.Split('|')[1] });
               else
               {
                  if (item.Split('|').Length > 1)
                     mcbCitySelection.Items.Add(new PlayAbleWorldItem() { WorldID = item.Split('|')[0], WorldName = item.Split('|')[1] });
                  else
                  {
                     World itemWorld = ListClass.AllWorlds.Find(w => w.id == item);
                     if (itemWorld.id == item)
                     {
                        UserData.PlayableWorlds[i] = $"{item}|{itemWorld.name}";
                        mcbCitySelection.Items.Add(new PlayAbleWorldItem() { WorldID = item, WorldName = itemWorld.name });
                     }
                  }
               }
               UserData.SaveSettings();
            }
            mcbCitySelection.DisplayMember = "WorldName";
            mcbCitySelection.AutoCompleteSource = AutoCompleteSource.ListItems;
            mcbCitySelection.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            mcbCitySelection.SelectedIndex = UserData.PlayableWorlds.FindIndex(e => e.Split('|')[0] == UserData.LastWorld.Split('|')[0]);
         }
         foreach (Language item in ListClass.AvailableLanguages.Languages)
         {
            LanguageItem languageItem = new LanguageItem()
            {
               Code = item.code,
               Description = item.language,
               Language = item.index
            };
            mcbLanguage.Items.Add(languageItem);
         }
         mcbLanguage.DisplayMember = "Description";
         mcbLanguage.AutoCompleteSource = AutoCompleteSource.ListItems;
         mcbLanguage.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
         mcbLanguage.SelectedIndex = UserData.Language.Language;
         FillIgnorePlayer();
         FillAutoComplete();

         if (bw.IsBusy)
         {
            bw.CancelAsync();
            while (bw.IsBusy) Application.DoEvents();
         }
         bw.DoWork += Bw_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync();
         ListClass.BackgroundWorkers.Add(bw);
      }
      private void FillIgnorePlayer()
      {
         pnlIgnore.Controls.Clear();
         if (UserData.IgnoredPlayers.Count > 0)
         {
            if (ListClass.AllPlayers.Count > 0)
            {
               foreach (int item in UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]])
               {
                  Player p = ListClass.AllPlayers.Find(a => a.player_id == item);
                  ucIgnorePlayer ucIP = new ucIgnorePlayer
                  {
                     Player = p
                  };
                  ucIP.Remove += RemoveIgnoredPlayer;
                  pnlIgnore.Controls.Add(ucIP);
               }
            }
         }
      }
      private void FillPriorityList()
      {
         var enumList = Enum.GetNames(typeof(Priority));
         foreach (var item in enumList)
         {
            Invoker.CallMethode(mcbBlueGalaxiePrio, () => mcbBlueGalaxiePrio.Items.Add(i18n.getString($"GUI.Settings.Production.{item}")));
         }
      }
      private void Bw_DoWork(object sender, DoWorkEventArgs e)
      {
         while (!bw.CancellationPending)
         {
            foreach (MetroFramework.MetroColorStyle foo in Enum.GetValues(typeof(MetroFramework.MetroColorStyle)))
            {
               if (foo == MetroFramework.MetroColorStyle.White ||
                  foo == MetroFramework.MetroColorStyle.Black ||
                  foo == MetroFramework.MetroColorStyle.Default ||
                  foo == MetroFramework.MetroColorStyle.Silver) continue;
               Invoker.SetProperty(mtcSettings, () => mtcSettings.Style, foo);
               Thread.Sleep(150);
            }
         }
      }
      private void mrb5_CheckedChanged(object sender, EventArgs e)
      {
         MetroRadioButton mrb = (MetroRadioButton)sender;
         if (!mrb.Checked) return;
         int time = int.Parse(mrb.Tag.ToString());
         UserData.ProductionOption = GetProductionOption(time);
         UserData.SaveSettings();
      }
      private void mrbG4h_CheckedChanged(object sender, EventArgs e)
      {
         MetroRadioButton mrb = (MetroRadioButton)sender;
         if (!mrb.Checked) return;
         int time = int.Parse(mrb.Tag.ToString());
         UserData.GoodProductionOption = GetGoodProductionOption(time);
         UserData.SaveSettings();
      }
      private void mtView_CheckedChanged(object sender, EventArgs e)
      {
         UserData.GroupedView = mtView.Checked;
         UserData.SaveSettings();
      }
      private void mtProduction_CheckedChanged(object sender, EventArgs e)
      {
         UserData.ProductionBot = mtProduction.Checked;
         UserData.SaveSettings();
         UpdateGoodProductionView();
         UpdateProductionView();
      }
      private void mtTavern_CheckedChanged(object sender, EventArgs e)
      {
         UserData.TavernBot = mtTavern.Checked;
         //UpdateBotView();
         UserData.SaveSettings();
      }
      private void mtMoppel_CheckedChanged(object sender, EventArgs e)
      {
         UserData.MoppelBot = mtMoppel.Checked;
         //UpdateBotView();
         UserData.SaveSettings();
      }
      private void mtIncident_CheckedChanged(object sender, EventArgs e)
      {
         UserData.IncidentBot = mtIncident.Checked;
         //UpdateBotView();
         UserData.SaveSettings();
      }
      private void mtRQBot_CheckedChanged(object sender, EventArgs e)
      {
         UserData.RQBot = mtRQBot.Checked;
         //UpdateBotView();
         UserData.SaveSettings();
      }
      private void metroToggle1_CheckedChanged(object sender, EventArgs e)
      {
         UserData.ShowBigRoads = mtBigRoads.Checked;
         UserData.SaveSettings();
      }
      private void mtbSave_Click(object sender, EventArgs e)
      {
         UserData.CustomUserAgent = mtbCustomUserAgent.Text;
         UserData.TelegramUserName = mtbTelegramUsername.Text;
         UserData.Language = (LanguageItem)mcbLanguage.SelectedItem;
         UserData.SaveSettings();
      }
      private void mcbLanguage_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (UserData.Language.Language != ((LanguageItem)mcbLanguage.SelectedItem).Language)
         {
            lblRestartNeeded.Visible = true;
            lblRestartNeeded.CustomForeColor = true;
            lblRestartNeeded.ForeColor = Color.Red;
         }
         else
            lblRestartNeeded.Visible = false;
      }
      private void mbSaveReload_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(i18n.getString("GUI.Loading.Changing"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
         if (ListClass.BackgroundWorkers.Count > 0)
         {
            foreach (BackgroundWorkerEX backgroundWorker in ListClass.BackgroundWorkers)
            {
               backgroundWorker.CancelAsync();
               while (backgroundWorker.IsBusy) { Application.DoEvents(); }
            }
            ListClass.BackgroundWorkers.Clear();
         }
         UserData.LastWorld = $"{((PlayAbleWorldItem)mcbCitySelection.SelectedItem).WorldID}|{((PlayAbleWorldItem)mcbCitySelection.SelectedItem).WorldName}";
         UserData.SaveSettings();
         restart = true;
         PbCLose_Click(null, null);
      }
      private void McbNotifyProd_CheckedChanged(object sender, EventArgs e)
      {
         //return;
         //UserData.ProductionBotNotification = mcbNotifyProd.Checked;
         //UserData.SaveSettings();
         //if (UserData.TelegramUserName.IsEmpty() && mcbNotifyProd.Checked)
         //{
         //   MessageBox.Show(i18n.getString("GUI.MessageBox.NoTelegramNameText"), i18n.getString("GUI.MessageBox.NoTelegramNameTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
         //}
      }
      private void McbNotifySnip_CheckedChanged(object sender, EventArgs e)
      {
         //return;
         //UserData.SnipBotNotification = mcbNotifySnip.Checked;
         //UserData.SaveSettings();
         //if (UserData.TelegramUserName.IsEmpty() && mcbNotifySnip.Checked)
         //{
         //   MessageBox.Show(i18n.getString("GUI.MessageBox.NoTelegramNameText"), i18n.getString("GUI.MessageBox.NoTelegramNameTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
         //}
      }
      private void mbDeleteData_Click(object sender, EventArgs e)
      {
         UserData.Delete();
         Process.Start(Application.ExecutablePath);
         Environment.Exit(0);
      }
      private void mbCheckSerial_Click(object sender, EventArgs e)
      {
         object ret = TcpConnection.SendAuthData(mtbSerialKey.Text, FingerPrint.Value(), false);
         if (ret is Result)
         {
            Enum.TryParse(ret.ToString(), out Result result);
            if (result == Result.Success)
            {

               mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} ({i18n.getString("Premium")}) | by TH3C0D3R";
               UserData.SerialKey = mtbSerialKey.Text;
               UserData.SaveSettings();
               object retList = ExecuteMethod(PremAssembly, "EntryPoint", "AddPremiumControl", null);
               if (retList is List<UCPremium> list)
               {
                  //tlpPremium.Controls.AddRange(list.ToArray());
                  //tlpPremium.Invalidate(true);
               }
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} ({i18n.getString("Expired")}) | by TH3C0D3R");
               if (!blockExpireBox)
                  MessageBox.Show(Owner, $"{i18n.getString("SubscriptionExpired")}", $"{i18n.getString("SubExpiredTitle")}");
               blockExpireBox = true;
            }
            else
            {
               mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} | by TH3C0D3R";
               MessageBox.Show(Owner, $"{i18n.getString("LicenceNotValid")}", $"{i18n.getString("FailedToActivate")}");
            }
         }
      }
      private void MtAutoLogin_CheckedChanged(object sender, EventArgs e)
      {
         UserData.AutoLogin = mtAutoLogin.Checked;
         UserData.SaveSettings();
      }
      private void MtToggleBrowser_CheckedChanged(object sender, EventArgs e)
      {
         lblRestartNeeded.Visible = true;
         lblRestartNeeded.CustomForeColor = true;
         lblRestartNeeded.ForeColor = Color.Red;

         UserData.HideBrowser = mtToggleBrowser.Checked;
         UserData.SaveSettings();
      }
      private void MtbIntervalIncident_TextChanged(object sender, EventArgs e)
      {
         if (int.TryParse(mtbIntervalIncident.Text, out int interval))
            UserData.IntervalIncidentBot = interval;
         else
            UserData.IntervalIncidentBot = 1;
         UserData.SaveSettings();
      }
      private void mcbNeighbor_CheckedChanged(object sender, EventArgs e)
      {
         SnipTargetChanged((MetroCheckBox)sender, e);
      }
      private void mcbFriends_CheckedChanged(object sender, EventArgs e)
      {
         SnipTargetChanged((MetroCheckBox)sender, e);
      }
      private void MtBuyAttempts_CheckedChanged(object sender, EventArgs e)
      {
         UserData.BuyGEXAttempts = mtBuyAttempts.Checked;
         UserData.SaveSettings();
      }
      private void mcbGuild_CheckedChanged(object sender, EventArgs e)
      {
         SnipTargetChanged((MetroCheckBox)sender, e);
      }
      private void SnipTargetChanged(MetroCheckBox sender, EventArgs e)
      {
         SnipTarget set = (SnipTarget)Enum.Parse(typeof(SnipTarget), sender.Tag.ToString());
         if (sender.Checked)
         {
            UserData.SelectedSnipTarget |= set;
         }
         else
         {
            if (UserData.SelectedSnipTarget.HasFlag(set))
            {
               UserData.SelectedSnipTarget &= ~set;
            }
         }
         UserData.SaveSettings();
         FillAutoComplete();
      }
      private void nudMinProfit_ValueChanged(object sender, EventArgs e)
      {
         UserData.MinProfit = (int)nudMinProfit.Value;
         UserData.SaveSettings();
      }
      private void NudMinProfitPerc_ValueChanged(object sender, EventArgs e)
      {
         UserData.MinProfitPercentage = (int)nudMinProfitPerc.Value;
         UserData.SaveSettings();
      }
      private void NudMaxInvest_ValueChanged(object sender, EventArgs e)
      {
         UserData.MaxInvestment = (int)nudMaxInvest.Value;
         UserData.SaveSettings();
      }
      private void McbAutoInvest_CheckedChanged(object sender, EventArgs e)
      {
         if (UserData.ShowWarning)
            MessageBox.Show(i18n.getString("GUI.Settings.Production.AutoSnip.AutoInvest.WarningText"), i18n.getString("GUI.Settings.Production.AutoSnip.AutoInvest.WarningTitle"));
         UserData.ShowWarning = false;
         UserData.AutoInvest = mcbAutoInvest.Checked;
         UserData.SaveSettings();
      }
      private void McbBlueGalaxiePrio_SelectedIndexChanged(object sender, EventArgs e)
      {
         UserData.BGPriority = (Priority)mcbBlueGalaxiePrio.SelectedItem;
      }
      private void mtSnipBot_CheckedChanged(object sender, EventArgs e)
      {
         UserData.SnipBot = mtSnipBot.Checked;
         UserData.SaveSettings();
         if (UserData.SnipBot && !isFirstRun)
         {
            if (tSniper.Enabled)
               tSniper.Stop();
            tSniper.Interval = 1000 * 60 * UserData.IntervalSnip;
            tSniper.Start();
            TSniper_Tick(null, null);
         }
         else
         {
            if (tSniper.Enabled)
               tSniper.Stop();
            mbCancel.Invoke((MethodInvoker)delegate
            {
               mbCancel.Enabled = false;
            });
            mbSearch.Invoke((MethodInvoker)delegate
            {
               mbSearch.Enabled = true;
               mbSearch.Text = i18n.getString(mbSearch.Tag.ToString());
            });
         }
      }
      private void nudSnipInterval_ValueChanged(object sender, EventArgs e)
      {
         UserData.IntervalSnip = (int)nudSnipInterval.Value;
         UserData.SaveSettings();
         if (UserData.SnipBot && !isFirstRun)
         {
            if (tSniper.Enabled)
               tSniper.Stop();
            tSniper.Interval = 1000 * 60 * UserData.IntervalSnip;
            tSniper.Start();
            TSniper_Tick(null, null);
         }
         else
         {
            tSniper.Stop();
         }
      }
      private void FillAutoComplete()
      {
         if (txbPlayer.InvokeRequired)
         {
            txbPlayer.Invoke((MethodInvoker)delegate
             {
                txbPlayer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txbPlayer.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                acsc.AddRange(UsernameList.Get);
                txbPlayer.AutoCompleteCustomSource = acsc;
             });
         }
         else
         {
            txbPlayer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txbPlayer.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            acsc.AddRange(UsernameList.Get);
            txbPlayer.AutoCompleteCustomSource = acsc;
         }
      }
      private void TxbPlayer_KeyUp(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == System.Windows.Forms.Keys.Enter)
         {
            if (!string.IsNullOrWhiteSpace(txbPlayer.Text))
            {
               string text = txbPlayer.Text;
               text = text.Split('(', ')')[1];
               if (int.TryParse(text, out int playerID))
               {
                  Player player = ListClass.AllPlayers.Find(p => p.player_id == playerID);
                  if (txbPlayer.Text.Split('(')[0].TrimEnd(' ').Equals(player.name.TrimEnd(' ')))
                  {
                     if (UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Contains(player.player_id.Value)) return;
                     UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Add(player.player_id.Value);
                     ucIgnorePlayer ucIP = new ucIgnorePlayer
                     {
                        Player = player
                     };
                     ucIP.Remove += RemoveIgnoredPlayer;
                     pnlIgnore.Controls.Add(ucIP);
                     UserData.SaveSettings();
                     UpdatePlayerLists();
                     txbPlayer.Text = "";
                  }
                  else
                  {
                     Log($"{txbPlayer.Text} {i18n.getString("PlayerNotFound")}", lbOutputWindow);
                  }
               }
               else
               {
                  Log($"{txbPlayer.Text} {i18n.getString("PlayerNotFound")}", lbOutputWindow);
               }
            }
         }
      }
      private void RemoveIgnoredPlayer(object sender, dynamic data = null)
      {
         try
         {
            Player p = (Player)data;
            if (p != null)
            {
               UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Remove(p.player_id.Value);
               pnlIgnore.Controls.Remove(sender as ucIgnorePlayer);
               UserData.SaveSettings();
               UpdatePlayerLists();
            }
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.ToString());
         }
      }
      private void TxbPlayer_Enter(object sender, EventArgs e)
      {
         if (txbPlayer.Text.Equals(i18n.getString("GUI.Settings.Production.Ignore")))
         {
            txbPlayer.Text = "";
         }
         else
         {
            if (string.IsNullOrWhiteSpace(txbPlayer.Text))
               txbPlayer.Text = i18n.getString("GUI.Settings.Production.Ignore");
         }
      }
      private void TSniper_Tick(object sender, EventArgs e)
      {
         if (!UserData.SnipBot) return;
         MbSearch_Click(true, null);
      }
      #endregion

      #region "Snip"
      public void UpdateSnip()
      {
         logger.Info($">>> UpdateSnip");
         Invoker.CallMethode(pnlContributions, () => pnlContributions.Controls.Clear());
         if (ListClass.Contributions.Count > 0)
         {
            int Total = 0;
            foreach (LGContribution item in ListClass.Contributions)
            {
               Label conItem = new Label();
               if (item.reward != null)
               {
                  ListClass.ArcBonus = (ListClass.ArcBonus == 0 ? 1 : ListClass.ArcBonus);
                  if (ListClass.ArcBonus >= 2) ListClass.ArcBonus = (ListClass.ArcBonus / 100) + 1;
                  int reward = (int)Math.Round((double)item.reward.strategy_point_amount * ListClass.ArcBonus);
                  int outcome = reward - item.forge_points;
                  Total += outcome;
                  conItem.Text = $"{item.player.name} ({item.name}) => ({(outcome > 0 ? "+" : "") + outcome})";
               }
               else
               {
                  conItem.Text = $"{item.player.name} ({item.name}) => (-{item.forge_points})";
                  Total -= item.forge_points;
               }
               conItem.Dock = DockStyle.Top;
               conItem.AutoSize = false;
               conItem.TextAlign = ContentAlignment.MiddleCenter;
               Invoker.CallMethode(pnlContributions, () => pnlContributions.Controls.Add(conItem));
            }
            Label conTotal = new Label
            {
               Text = i18n.getString("GUI.Sniper.Contributions.Total") + " => " + Total.ToString("N0"),
               Dock = DockStyle.Top,
               AutoSize = false,
               TextAlign = ContentAlignment.MiddleCenter,
            };
            Invoker.CallMethode(pnlContributions, () => pnlContributions.Controls.Add(conTotal));
         }
         if (UserData.SnipBot)
         {
            mbSearch.Invoke((MethodInvoker)delegate
            {
               mbSearch.Enabled = false;
               mbSearch.Text = i18n.getString("GUI.Sniper.SnipBotActive");
            });
         }
         else
         {
            mbSearch.Invoke((MethodInvoker)delegate
            {
               mbSearch.Enabled = true;
               mbSearch.Text = i18n.getString("GUI.Sniper.Search");
            });
         }
         logger.Info($"<<< UpdateSnip");
      }
      private void MbSearch_Click(object sender, EventArgs e)
      {
         if (sender.GetType() != typeof(bool))
         {
            WorkerItem wi = new WorkerItem()
            {
               Title = i18n.getString("GUI.Sniper.BWTitle"),
               BeforeCountText = i18n.getString("GUI.Sniper.BWBeforeCountText"),
               CountText = "Count",
               ID = LGSnipWorkerID
            };
            if (Application.OpenForms["WorkerList"] == null)
            {
               StaticData.WorkerList = new WorkerList();
               StaticData.WorkerList.Show();
               if (!IsOnScreen(StaticData.WorkerList))
                  StaticData.WorkerList.DesktopLocation = new Point(Location.X, Location.Y);
            }
            StaticData.WorkerList.BringToFront();
            StaticData.WorkerList.AddWorkerItem(wi);
         }

         TwoTArgs<RequestType, object> param = new TwoTArgs<RequestType, object> { RequestType = RequestType.GetLGs, argument2 = sender };
         BackgroundWorkerEX bw = new BackgroundWorkerEX();
         bw.DoWork += bwScriptExecuter_DoWork;
         bw.param = param;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerCompleted += workerComplete;
         bw.RunWorkerAsync(param);
         mbCancel.Invoke((MethodInvoker)delegate
         {
            mbCancel.Enabled = true;
         });
         mbSearch.Invoke((MethodInvoker)delegate
         {
            mbSearch.Enabled = false;
         });
         ListClass.BackgroundWorkers.Add(bw);
      }
      private void MbSnip_Click(object sender, EventArgs e)
      {
         mbCancel.Enabled = false;
         mbSearch.Enabled = false;
         Random r = new Random();
         foreach (Control c in mpSnipItem.Controls)
         {
            LGSnipItem lsi = (LGSnipItem)c;
            if (sender is bool)
            {
               string script = ReqBuilder.GetRequestScript(RequestType.contributeForgePoints, new int[] { lsi.LGSnip.entity_id, lsi.LGSnip.player.player_id.Value, lsi.LGSnip.level, lsi.LGSnip.Invest, 0 });
               _ = (string)jsExecutor.ExecuteAsyncScript(script);
               logger.Info($"[Snip] Sniped: {lsi.LGSnip.player.name}'s {lsi.LGSnip.name} for {lsi.LGSnip.Invest} with {lsi.Profit} Profit");
               Log($"[{DateTime.Now}] [{i18n.getString("GUI.Log.Snip")}] {lsi.LGSnip.player.name} {lsi.LGSnip.name}: {lsi.LGSnip.Invest}(+{lsi.Profit})", lbOutputWindow);
               lsi.mcbSnip.Enabled = false;
               lsi.mcbSnip.Checked = false;
               lsi.mcbSnip.Text = i18n.getString("GUI.Sniper.SnipDone");
               //if (UserData.SnipBotNotification)
               //   TelegramNotify.Send(i18n.getString("GUI.Telegram.Snip", new List<KeyValuePair<string, string>>()
               //{
               //    new KeyValuePair<string, string>("##GB##",$"{lsi.LGSnip.name}"),
               //    new KeyValuePair<string, string>("##Invest##",$"{lsi.LGSnip.Invest}"),
               //    new KeyValuePair<string, string>("##Profit##",$"{lsi.Profit}"),
               //    new KeyValuePair<string, string>("##Player##",$"{lsi.LGSnip.player.name}"),
               //    new KeyValuePair<string, string>("##Social##",$"{lsi.LGSnip.player.GetIdentifier()}"),
               //}.ToArray()));
               int rInt = r.Next(250, 1500);
               Thread.Sleep(rInt);
               Application.DoEvents();
            }
            else if (lsi.mcbSnip.Checked)
            {
               string script = ReqBuilder.GetRequestScript(RequestType.contributeForgePoints, new int[] { lsi.LGSnip.entity_id, lsi.LGSnip.player.player_id.Value, lsi.LGSnip.level, lsi.LGSnip.Invest, 0 });
               _ = (string)jsExecutor.ExecuteAsyncScript(script);
               logger.Info($"[Snip] Sniped: {lsi.LGSnip.player.name}'s {lsi.LGSnip.name} for {lsi.LGSnip.Invest} with {lsi.Profit} Profit");
               Log($"[{DateTime.Now}] [{i18n.getString("GUI.Log.Snip")}] {lsi.LGSnip.player.name} {lsi.LGSnip.name}: {lsi.LGSnip.Invest}(+{lsi.Profit})", lbOutputWindow);
               lsi.mcbSnip.Enabled = false;
               lsi.mcbSnip.Checked = false;
               lsi.mcbSnip.Text = i18n.getString("GUI.Sniper.SnipDone");
               //if (UserData.SnipBotNotification)
               //   TelegramNotify.Send(i18n.getString("GUI.Telegram.Snip", new List<KeyValuePair<string, string>>()
               //{
               //    new KeyValuePair<string, string>("##GB##",$"{lsi.LGSnip.name}"),
               //    new KeyValuePair<string, string>("##Invest##",$"{lsi.LGSnip.Invest}"),
               //    new KeyValuePair<string, string>("##Profit##",$"{lsi.Profit}"),
               //    new KeyValuePair<string, string>("##Player##",$"{lsi.LGSnip.player.name}"),
               //    new KeyValuePair<string, string>("##Social##",$"{lsi.LGSnip.player.GetIdentifier()}"),
               //}.ToArray()));
               Application.DoEvents();
            }
         }
         if (UserData.SnipBot)
            UserData.LastSnipTime = DateTime.Now;
         UserData.SaveSettings();
         mbCancel.Invoke((MethodInvoker)delegate
         {
            mbCancel.Enabled = false;
            mbCancel.Text = i18n.getString(mbCancel.Tag.ToString());
         });
         mbSearch.Invoke((MethodInvoker)delegate
         {
            mbSearch.Enabled = true;
            mbSearch.Text = i18n.getString(mbSearch.Tag.ToString());
         });
         Updater.UpdateContribution();
         Updater.UpdateInventory();
         UpdateDashbord();
         UpdateSnip();
      }
      private void MbCancel_Click(object sender, EventArgs e)
      {
         if (ListClass.BackgroundWorkers.Count > 0)
         {
            for (int i = 0; i < ListClass.BackgroundWorkers.Count; i++)
            {
               BackgroundWorkerEX bw = ListClass.BackgroundWorkers[i];
               if (bw.param == null) continue;
               dynamic param;
               try
               {
                  param = (TwoTArgs<RequestType, dynamic>)bw.param;
               }
               catch (Exception)
               {
                  param = (TwoTArgs<RequestType, E_Motivate>)bw.param;
               }
               if (bw.WorkerSupportsCancellation && param.RequestType == RequestType.GetLGs && bw.IsBusy)
               {
                  bw.CancelAsync();
                  mbCancel.Enabled = false;
                  mbSearch.Enabled = true;
                  (bool, bool) returnValLG = StaticData.WorkerList.RemoveWorkerByID(LGSnipWorkerID);
                  if (returnValLG.Item2)
                  {
                     if (InvokeRequired)
                     {
                        StaticData.WorkerList.Invoke((MethodInvoker)delegate
                        {
                           StaticData.WorkerList.Close();
                        });
                     }
                     else
                        StaticData.WorkerList.Close();
                  }
                  ListClass.BackgroundWorkers.Remove(bw);
               }
            }
         }
      }
      #endregion

      #region "BackgroundWorkerEX"
      private void bwScriptExecuter_DoWork(object sender, DoWorkEventArgs e)
      {
         Random r = new Random();
         int counter = 0;
         dynamic param;
         try
         {
            param = (TwoTArgs<RequestType, List<EntityProd>>)e.Argument;
         }
         catch (Exception)
         {
            try
            {
               param = (TwoTArgs<RequestType, dynamic>)e.Argument;
            }
            catch (Exception)
            {
               param = (TwoTArgs<RequestType, E_Motivate>)e.Argument;
            }
         }
         switch (param.RequestType)
         {
            case RequestType.CollectOtherProductions:
               List<EntityProd> prodList = (List<EntityProd>)param.argument2;
               var ids = prodList.ToList().Select(y => y.entity.id).ToArray();
               var script = ReqBuilder.GetRequestScript(RequestType.CollectProduction, ids);
               _ = (string)jsExecutor.ExecuteAsyncScript(script);
               Updater.UpdateEntities();
               if (ListClass.FinishedProductions.Count > 0)
               {
                  Invoker.SetProperty(lblSumFinProdValue, () => lblSumFinProdValue.Text, ListClass.FinishedProductions.Count.ToString());
               }
               break;
            case RequestType.Motivate:
               E_Motivate e_Motivate = (E_Motivate)Enum.Parse(typeof(E_Motivate), param.argument2.ToString());
               List<Player> list = new List<Player>();
               int ID = PolivateAllWorkerID;
               switch (e_Motivate)
               {
                  case E_Motivate.Clan:
                     var clanMotivate = ListClass.ClanMemberList.FindAll(f => (f.next_interaction_in == 0));
                     list.AddRange(clanMotivate);
                     ID = PolivateMemberWorkerID;
                     break;
                  case E_Motivate.Neighbor:
                     var neighborlist = ListClass.NeighborList.FindAll(f => (f.next_interaction_in == 0));
                     list.AddRange(neighborlist);
                     ID = PolivateNeighborsWorkerID;
                     break;
                  case E_Motivate.Friend:
                     var friendMotivate = ListClass.FriendList.FindAll(f => (f.next_interaction_in == 0));
                     list.AddRange(friendMotivate);
                     ID = PolivateFriendsWorkerID;
                     break;
                  case E_Motivate.All:
                     var clan = ListClass.ClanMemberList.FindAll(f => (f.next_interaction_in == 0));
                     var neighbor = ListClass.NeighborList.FindAll(f => (f.next_interaction_in == 0));
                     var friend = ListClass.FriendList.FindAll(f => (f.next_interaction_in == 0));
                     list.AddRange(neighbor);
                     list.AddRange(friend);
                     list.AddRange(clan);
                     ID = PolivateAllWorkerID;
                     break;
                  default:
                     break;
               }
               StaticData.WorkerList.UpdateWorkerProgressBar(ID, 0, list.Count);
               StaticData.WorkerList.UpdateWorkerLabel(ID, i18n.getString("CountLabel").Replace("##Done##", "0").Replace("##End##", list.Count.ToString()));
               foreach (Player item in list)
               {
                  counter += 1;
                  StaticData.WorkerList.UpdateWorkerProgressBar(ID, counter, list.Count);
                  StaticData.WorkerList.UpdateWorkerLabel(ID, i18n.getString("CountLabel").Replace("##Done##", counter.ToString()).Replace("##End##", list.Count.ToString()));
                  script = ReqBuilder.GetRequestScript(param.RequestType, item.player_id);
                  string retMot = (string)jsExecutor.ExecuteAsyncScript(script);
                  string[] motResponse = retMot.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
                  Polivate motivation = null;
                  object rewardResources = null;
                  foreach (string res in motResponse)
                  {
                     var methode = res.Substring(0, res.IndexOf("{"));
                     var body = res.Substring(res.IndexOf("{"));
                     switch (methode)
                     {
                        case "rewardResources":
                           rewardResources = JsonConvert.DeserializeObject(body);
                           break;
                        case "polivateRandomBuilding":
                           motivation = JsonConvert.DeserializeObject<Polivate>(body);
                           break;
                        default:
                           break;
                     }
                  }
                  if (motivation != null && !retMot.Contains("\"error_code\":202,\"__class__\":\"Error\""))
                  {
                     if (motivation.responseData.action == "polish" || motivation.responseData.action == "motivate" || (motivation.responseData.action == "polivate_failed" && rewardResources != null))
                     {
                        int playerid = 0;
                        if (motivation.responseData.action == "polivate_failed")
                           playerid = item.player_id.Value;
                        else
                           playerid = motivation.responseData.mapEntity.player_id;
                        ListClass.doneMotivate.Add(playerid, (true, rewardResources));
                     }
                     else
                        MessageBox.Show($"{i18n.getString("UnknownAction")} {motivation.responseData.action}");
                  }
                  int nextInt = r.Next(333, 1000);
                  Thread.Sleep(nextInt);
               }
               Dictionary<string, int> results = new Dictionary<string, int>();
               foreach (var item in ListClass.doneMotivate)
               {
                  if (item.Value.Item1)
                  {
                     var jtokenlist = ((JObject)item.Value.Item2)["responseData"]["resources"].ToList();
                     foreach (var jtoken in jtokenlist)
                     {
                        var resource = jtoken.Path.Substring(jtoken.Path.LastIndexOf(".") + 1);
                        string name = ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == resource)["name"].ToString();
                        if (results.ContainsKey(name))
                           results[name] += int.Parse(jtoken.First.ToString());
                        else
                           results.Add(name, int.Parse(jtoken.First.ToString()));
                     }
                  }
               }
               foreach (var resItem in results)
                  Log($"[{DateTime.Now}] {i18n.getString("PolivateResult")} - {resItem.Key}: {resItem.Value}", lbOutputWindow);
               ListClass.doneMotivate.Clear();
               StaticData.WorkerList.UpdateWorkerProgressBar(ID, list.Count, list.Count);
               StaticData.WorkerList.UpdateWorkerLabel(ID, i18n.getString("MotivationDone"));
               UserData.LastPolivateTime = DateTime.Now;
               UserData.SaveSettings();
               (bool, bool) returnValMot = StaticData.WorkerList.RemoveWorkerByID(ID);
               if (returnValMot.Item2)
               {
                  if (InvokeRequired)
                  {
                     StaticData.WorkerList.Invoke((MethodInvoker)delegate
                     {
                        StaticData.WorkerList.Close();
                     });
                  }
                  else
                     StaticData.WorkerList.Close();
               }
               ReloadData();
               break;
            case RequestType.VisitTavern:
               ListClass.FriendTaverns = ListClass.FriendTaverns.Where((f) => f.sittingPlayerCount < f.unlockedChairCount && f.state == null).ToList();
               if (ListClass.FriendTaverns.Count == 0 || ListClass.FriendList.Count == 0) return;
               StaticData.WorkerList.UpdateWorkerProgressBar(TavernVisitWorkerID, 0, ListClass.FriendTaverns.Count);
               StaticData.WorkerList.UpdateWorkerLabel(TavernVisitWorkerID, i18n.getString("CountLabel").Replace("##Done##", "0").Replace("##End##", ListClass.FriendTaverns.Count.ToString()));
               foreach (FriendTavernState item in ListClass.FriendTaverns)
               {
                  script = ReqBuilder.GetRequestScript(param.RequestType, item.ownerId);
                  string retTavern = (string)jsExecutor.ExecuteAsyncScript(script);

                  string[] TavResponse = retTavern.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
                  object rewardTavern = null;
                  object TavernResultSitting = null;
                  TavernResult tavernresult = null;
                  foreach (string res in TavResponse)
                  {
                     var methode = res.Substring(0, res.IndexOf("{"));
                     var body = res.Substring(res.IndexOf("{"));
                     dynamic resItem = JsonConvert.DeserializeObject(body);
                     if (resItem["requestMethod"] == "getOtherTavern")
                     {
                        rewardTavern = resItem["responseData"]["rewardResources"];
                        TavernResultSitting = resItem["responseData"]["state"];
                     }
                     else if (resItem["requestMethod"] == "getOtherTavernState")
                        tavernresult = JsonConvert.DeserializeObject<TavernResult>(body);
                  }
                  if (TavernResultSitting.ToString() == "satDown")
                     if (item.ownerId.ToString() != "" && tavernresult.responseData.ownerId == item.ownerId)
                        ListClass.doneTavern.Add(tavernresult.responseData.ownerId, (true, rewardTavern));
                  if (TavernResultSitting == null)
                     MessageBox.Show($"{i18n.getString("UnknownAction")} {JsonConvert.SerializeObject(TavernResultSitting)}");
                  int nextIntTavern = r.Next(333, 1000);
                  StaticData.WorkerList.UpdateWorkerProgressBar(TavernVisitWorkerID, ListClass.doneTavern.Count, ListClass.FriendTaverns.Count);
                  StaticData.WorkerList.UpdateWorkerLabel(TavernVisitWorkerID, i18n.getString("CountLabel").Replace("##Done##", ListClass.doneTavern.Count.ToString()).Replace("##End##", ListClass.FriendTaverns.Count.ToString()));
                  Thread.Sleep(nextIntTavern);
               }
               Dictionary<string, int> resultsTavern = new Dictionary<string, int>();
               foreach (var item in ListClass.doneTavern)
               {
                  if (item.Value.Item1)
                  {
                     var jtokenlist = ((JObject)item.Value.Item2)["resources"].ToList();
                     foreach (var jtoken in jtokenlist)
                     {
                        var resource = jtoken.Path.Substring(jtoken.Path.LastIndexOf(".") + 1);
                        string name = ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == resource)["name"].ToString();
                        if (resultsTavern.ContainsKey(name))
                           resultsTavern[name] += int.Parse(jtoken.First.ToString());
                        else
                           resultsTavern.Add(name, int.Parse(jtoken.First.ToString()));
                     }
                  }
               }
               foreach (var resItem in resultsTavern)
                  Log($"[{DateTime.Now}] {i18n.getString("TavernResult")} - {resItem.Key}: {resItem.Value}", lbOutputWindow);
               ListClass.doneTavern.Clear();
               StaticData.WorkerList.UpdateWorkerProgressBar(TavernVisitWorkerID, ListClass.doneTavern.Count, ListClass.FriendTaverns.Count);
               StaticData.WorkerList.UpdateWorkerLabel(TavernVisitWorkerID, i18n.getString("TavernDone"));
               (bool, bool) returnVal = StaticData.WorkerList.RemoveWorkerByID(TavernVisitWorkerID);
               if (returnVal.Item2)
               {
                  if (InvokeRequired)
                  {
                     StaticData.WorkerList.Invoke((MethodInvoker)delegate
                     {
                        StaticData.WorkerList.Close();
                     });
                  }
                  else
                     StaticData.WorkerList.Close();
               }
               ReloadData();
               break;
            case RequestType.GetLGs:
               try
               {
                  BackgroundWorkerEX bwEx = (BackgroundWorkerEX)sender;
                  string retSnip = "";
                  ListClass.SnipWithProfit = new List<LGSnip>();
                  ListClass.SnipablePlayers = new List<Player>();
                  if (UserData.SelectedSnipTarget == SnipTarget.none) return;
                  if (HasPremium)
                  {

                  }
                  if (UserData.SelectedSnipTarget.HasFlag(SnipTarget.friends)) ListClass.SnipablePlayers.AddRange(ListClass.FriendList);
                  if (UserData.SelectedSnipTarget.HasFlag(SnipTarget.neighbors)) ListClass.SnipablePlayers.AddRange(ListClass.NeighborList);
                  if (UserData.SelectedSnipTarget.HasFlag(SnipTarget.members)) ListClass.SnipablePlayers.AddRange(ListClass.ClanMemberList);
                  if (UserData.IgnoredPlayers.Values.Any(ip => ip.Count > 0))
                     ListClass.SnipablePlayers = ListClass.SnipablePlayers.Where(p => !UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Contains(p.player_id.Value)).ToList();
                  ListClass.SnipablePlayers = ListClass.SnipablePlayers.Where(p => p.incoming == false && p.isInvitedFriend == false).ToList();
                  ListClass.SnipablePlayers = LG.HasGB(ListClass.SnipablePlayers);
                  if (ListClass.SnipablePlayers.Count == 0) return;
                  if (param.argument2.GetType() != typeof(bool)) StaticData.WorkerList.UpdateWorkerProgressBar(LGSnipWorkerID, 0, ListClass.SnipablePlayers.Count);
                  if (param.argument2.GetType() != typeof(bool)) StaticData.WorkerList.UpdateWorkerLabel(LGSnipWorkerID, i18n.getString("CountLabel").Replace("##Done##", "0").Replace("##End##", ListClass.SnipablePlayers.Count.ToString()));
                  int rInt = 0;
                  foreach (Player item in ListClass.SnipablePlayers)
                  {
                     if (bwEx.CancellationPending) break;
                     counter += 1;
                     if (param.argument2.GetType() != typeof(bool)) StaticData.WorkerList.UpdateWorkerProgressBar(LGSnipWorkerID, counter, ListClass.SnipablePlayers.Count);
                     if (param.argument2.GetType() != typeof(bool)) StaticData.WorkerList.UpdateWorkerLabel(LGSnipWorkerID, $"{i18n.getString("GUI.Sniper.SearchingPlayer")} {item.name}...");
                     script = ReqBuilder.GetRequestScript(RequestType.GetLGs, item.player_id);
                     retSnip = (string)jsExecutor.ExecuteAsyncScript(script);
                     if (retSnip.Length <= 4) break;
                     LGRootObject lgs = JsonConvert.DeserializeObject<LGRootObject>(retSnip);
                     List<LGSnip> LGData = lgs.responseData.ToList();
                     if (LGData.Count == 0) break;
                     ListClass.SnipablePlayers.Find(p => p.player_id == LGData[0].player.player_id).LGs = LGData;
                     foreach (LGSnip lg in item.LGs)
                     {
                        if (bwEx.CancellationPending) break;
                        if (param.argument2.GetType() != typeof(bool)) StaticData.WorkerList.UpdateWorkerProgressBar(LGSnipWorkerID, counter, ListClass.SnipablePlayers.Count);
                        if (param.argument2.GetType() != typeof(bool)) StaticData.WorkerList.UpdateWorkerLabel(LGSnipWorkerID, $"{i18n.getString("GUI.Sniper.SearchingLG").Replace("##Player##", item.name)} {lg.name}...");

                        int UnderScorePos = lg.city_entity_id.IndexOf("_");
                        string AgeString = lg.city_entity_id.Substring(UnderScorePos + 1);
                        UnderScorePos = AgeString.IndexOf("_");
                        AgeString = AgeString.Substring(0, UnderScorePos);
                        if (lg.current_progress == null)
                           lg.current_progress = 0;
                        int P1 = GetP1(AgeString, lg.level);
                        ListClass.ArcBonus = (ListClass.ArcBonus == 0 ? 1 : ListClass.ArcBonus);
                        if (ListClass.ArcBonus >= 2) ListClass.ArcBonus = (ListClass.ArcBonus / 100) + 1;
                        if (lg.rank == null && P1 * ListClass.ArcBonus >= (lg.max_progress - lg.current_progress) / 2)
                        {
                           script = ReqBuilder.GetRequestScript(RequestType.getConstruction, new int[] { lg.entity_id, lg.player.player_id.Value });
                           string retConstruction = (string)jsExecutor.ExecuteAsyncScript(script);
                           string[] snipResponse = retConstruction.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
                           foreach (var res in snipResponse)
                           {
                              var methode = res.Substring(0, res.IndexOf("{"));
                              var body = res.Substring(res.IndexOf("{"));
                              dynamic resItem = JsonConvert.DeserializeObject(body);
                              if (resItem["requestMethod"] == "updateEntity")
                              {
                                 lg.state = new LGState()
                                 {
                                    forge_points_for_level_up = (int?)resItem["responseData"][0]["state"]["forge_points_for_level_up"],
                                    invested_forge_points = (int?)resItem["responseData"][0]["state"]["invested_forge_points"],
                                    paused_state = resItem["responseData"][0]["state"]["paused_state"]
                                 };
                                 lg.maxLevel = (int)resItem["responseData"][0]["max_level"];
                                 if (lg.state != null && lg.state.paused_state != null) continue;
                              }
                              else if (resItem["requestMethod"] == "getConstruction")
                              {
                                 if (lg.state != null && lg.state.paused_state != null) continue;
                                 SnipRoot SnipeRoot = JsonConvert.DeserializeObject<SnipRoot>(body);
                                 SnipResponse SnipResponse = SnipeRoot.responseData;
                                 Ranking[] Rankings = SnipResponse.rankings;

                                 ArrayList hFordern = new ArrayList();
                                 ArrayList hBPMeds = new ArrayList();
                                 ArrayList hSnipen = new ArrayList();
                                 var arc = ListClass.ArcBonus >= 2 ? (ListClass.ArcBonus / 100) + 1 : ListClass.ArcBonus;
                                 var ForderArc = 1.90f;
                                 int EigenPos = 0, EigenBetrag = 0;
                                 for (int i = 0; i < Rankings.Length; i++)
                                 {
                                    if (Rankings[i].player != null && Rankings[i].player.player_id == int.Parse(ListClass.UserData["player_id"].ToString()))
                                    {
                                       EigenPos = i;
                                       EigenBetrag = Rankings[i].forge_points >= 0 ? Rankings[i].forge_points : 0;
                                       break;
                                    }
                                 }
                                 string[] ForderStates = new string[6];
                                 string[] SnipeStates = new string[6];
                                 int[] FPNettoRewards = new int[6];
                                 int[] FPRewards = new int[6];
                                 int[] BPRewards = new int[6];
                                 int[] MedalRewards = new int[6];
                                 int[] ForderFPRewards = new int[6];
                                 int[] ForderRankCosts = new int[6];
                                 int[] SnipeRankCosts = new int[6];
                                 int[] Einzahlungen = new int[6];
                                 int BestGewinn = -999999;
                                 int RankProfit = -1;

                                 for (int i = 0; i < Rankings.Length; i++)
                                 {
                                    Ranking rang = Rankings[i];
                                    int Rank = -1, CurrentFP = 0, TotalFP = 0, RestFP = 0;
                                    bool IsSelf = false;
                                    if (rang.rank < 0) continue;
                                    else Rank = rang.rank - 1;
                                    if (rang.reward == null) continue;
                                    ForderStates[Rank] = "";
                                    SnipeStates[Rank] = "";
                                    FPNettoRewards[Rank] = 0;
                                    FPRewards[Rank] = 0;
                                    BPRewards[Rank] = 0;
                                    MedalRewards[Rank] = 0;
                                    ForderFPRewards[Rank] = 0;
                                    ForderRankCosts[Rank] = -1;
                                    SnipeRankCosts[Rank] = -1;
                                    Einzahlungen[Rank] = 0;

                                    if (rang.reward.strategy_point_amount >= 0)
                                       FPNettoRewards[Rank] = rang.reward.strategy_point_amount;
                                    if (rang.reward.blueprints >= 0)
                                       BPRewards[Rank] = rang.reward.blueprints;
                                    if (rang.reward.resources.medals >= 0)
                                       MedalRewards[Rank] = rang.reward.resources.medals;
                                    FPRewards[Rank] = (int)Math.Round((double)FPNettoRewards[Rank] * arc, MidpointRounding.AwayFromZero);
                                    BPRewards[Rank] = (int)Math.Round((double)BPRewards[Rank] * arc, MidpointRounding.AwayFromZero);
                                    MedalRewards[Rank] = (int)Math.Round((double)MedalRewards[Rank] * arc, MidpointRounding.AwayFromZero);
                                    ForderFPRewards[Rank] = (int)Math.Round((double)FPNettoRewards[Rank] * ForderArc, MidpointRounding.AwayFromZero);

                                    if (rang.player != null && Rankings[i].player.player_id == int.Parse(ListClass.UserData["player_id"].ToString()))
                                       IsSelf = true;

                                    if (rang.forge_points >= 0)
                                       Einzahlungen[Rank] = rang.forge_points;
                                    if (lg.state != null)
                                    {
                                       if (lg.state.invested_forge_points == null) lg.state.invested_forge_points = 0;
                                       CurrentFP = (lg.state.invested_forge_points.Value >= 0 ? lg.state.invested_forge_points.Value : 0) - EigenBetrag;
                                       TotalFP = lg.state.forge_points_for_level_up.Value;
                                       RestFP = TotalFP - CurrentFP;
                                    }
                                    if (!IsSelf)
                                    {
                                       SnipeRankCosts[Rank] = (int)Math.Round((double)(Einzahlungen[Rank] + RestFP) / 2, MidpointRounding.AwayFromZero);
                                       ForderRankCosts[Rank] = Math.Max(ForderFPRewards[Rank], SnipeRankCosts[Rank]);
                                       ForderRankCosts[Rank] = Math.Min(ForderRankCosts[Rank], RestFP);
                                       bool exitLoop = false;
                                       if (SnipeRankCosts[Rank] <= Einzahlungen[Rank])
                                       {
                                          ForderRankCosts[Rank] = 0;
                                          ForderStates[Rank] = "NotPossible";
                                          exitLoop = true;
                                       }
                                       else
                                       {
                                          if (ForderRankCosts[Rank] == RestFP)
                                             ForderStates[Rank] = "LevelWarning";
                                          else if (ForderRankCosts[Rank] <= ForderFPRewards[Rank])
                                             ForderStates[Rank] = "Profit";
                                          else
                                             ForderStates[Rank] = "NegativeProfit";
                                       }
                                       if (SnipeRankCosts[Rank] <= Einzahlungen[Rank])
                                       {
                                          SnipeRankCosts[Rank] = 0;
                                          SnipeStates[Rank] = "NotPossible";
                                          exitLoop = true;
                                       }
                                       else
                                       {
                                          if (SnipeRankCosts[Rank] == RestFP)
                                             SnipeStates[Rank] = "LevelWarning";
                                          else if (FPRewards[Rank] <= SnipeRankCosts[Rank])
                                             SnipeStates[Rank] = "NegativeProfit";
                                          else
                                          {
                                             SnipeStates[Rank] = "Profit";
                                             var CurrentGewinn = FPRewards[Rank] - SnipeRankCosts[Rank];
                                             if (CurrentGewinn > BestGewinn)
                                             {
                                                if (SnipeStates[Rank] == "Profit")
                                                {
                                                   BestGewinn = CurrentGewinn;
                                                   RankProfit = Rank;
                                                   exitLoop = true;
                                                }
                                             }
                                          }
                                       }
                                       if (exitLoop)
                                          continue;
                                    }
                                 }
                                 if (SnipeStates.Contains("Profit"))
                                 {
                                    if (ListClass.SnipWithProfit.Find(g => g.entity_id == lg.entity_id && g.player.player_id == lg.player.player_id) != null) continue;
                                    if (lg.level == lg.maxLevel) continue;
                                    if (BestGewinn < UserData.MinProfit) continue;
                                    int SnipCost = FPRewards[RankProfit] - BestGewinn;
                                    float Win = (float)(FPRewards[RankProfit] - SnipCost) / SnipCost * 100;
                                    if ((int)Win < UserData.MinProfitPercentage) continue;
                                    if (SnipCost > UserData.MaxInvestment) continue;
                                    lg.KurzString = $"{(int)Win} %";
                                    lg.GewinnString = $"{BestGewinn}";
                                    lg.Invest = SnipCost;
                                    ListClass.SnipWithProfit.Add(lg);
                                 }
                                 if (ListClass.AllLGs.Find(k => k.entity_id == lg.entity_id && k.player.player_id == lg.player.player_id) != null) continue;
                                 ListClass.AllLGs.Add(lg);
                              }
                           }
                           rInt = r.Next(250, 1500);
                           Thread.Sleep(rInt);
                        }
                     }
                     rInt = r.Next(250, 1500);
                     Thread.Sleep(rInt);
                  }
                  Invoker.CallMethode(mpSnipItem, () => mpSnipItem.Controls.Clear());
                  foreach (LGSnip item in ListClass.SnipWithProfit)
                  {
                     Player player = ListClass.AllPlayers.Find(p => p.player_id == item.player.player_id);
                     string PlayerIdentifier = "Neighbor";
                     if (player.is_friend) PlayerIdentifier = "Friend";
                     if (player.is_guild_member) PlayerIdentifier = "Member";
                     LGSnipItem lsi = new LGSnipItem()
                     {
                        LG = $"{item.player.name} ({i18n.getString($"GUI.Sniper.PlayerIndentifier.{PlayerIdentifier}")}) -> {item.name} ({item.level})",
                        LGSnip = item,
                        Profit = $"{item.Invest} (+{item.GewinnString} / +{item.KurzString})"
                     };
                     lsi.mcbSnip.Text = i18n.getString(lsi.mcbSnip.Tag.ToString());
                     lsi.Dock = DockStyle.Top;
                     Invoker.CallMethode(mpSnipItem, () => mpSnipItem.Controls.Add(lsi));
                     Invoker.SetProperty(mbSnip, () => mbSnip.Enabled, true);
                  }
                  Invoker.CallMethode(mpSnipItem, () => mpSnipItem.Update());
                  if (param.argument2.GetType() != typeof(bool))
                  {
                     (bool, bool) returnValLG = StaticData.WorkerList.RemoveWorkerByID(LGSnipWorkerID);
                     if (returnValLG.Item2)
                     {
                        if (InvokeRequired)
                        {
                           if (StaticData.WorkerList.IsHandleCreated)
                              StaticData.WorkerList.Invoke((MethodInvoker)delegate
                              {
                                 StaticData.WorkerList.Close();
                              });
                        }
                        else
                           StaticData.WorkerList.Close();
                     }

                     mbCancel.Invoke((MethodInvoker)delegate
                     {
                        mbCancel.Enabled = false;
                     });
                     mbSearch.Invoke((MethodInvoker)delegate
                     {
                        mbSearch.Enabled = true;
                        mbSearch.Text = i18n.getString(mbSearch.Tag.ToString());
                     });
                  }
                  else
                  {
                     if (UserData.AutoInvest)
                     {
                        MbSnip_Click(UserData.AutoInvest, null);
                        break;
                     }
                     else
                     {
                        mbSearch.Invoke((MethodInvoker)delegate
                        {
                           mbSearch.Enabled = false;
                           mbSearch.Text = i18n.getString("GUI.Sniper.SnipBotActive");
                        });
                        break;
                     }
                  }

               }
               catch (Exception ex)
               {
                  logger.Info(ex,$"!Snip-Modul catched an exception!");
               }
               break;
            default:
               break;
         }
      }
      #endregion

      #region "Polivate/Tavern"
      public void Motivate(E_Motivate player_type)
      {
         WorkerItem wi = new WorkerItem()
         {
            Title = i18n.getString("PolivateTitle"),
            BeforeCountText = i18n.getString("Status"),
            CountText = i18n.getString("CountLabel"),
            ID = player_type == E_Motivate.Clan ? PolivateMemberWorkerID : player_type == E_Motivate.Friend ? PolivateFriendsWorkerID : player_type == E_Motivate.Neighbor ? PolivateNeighborsWorkerID : PolivateAllWorkerID
         };
         if (Application.OpenForms["WorkerList"] == null)
         {
            StaticData.WorkerList = new WorkerList();
            StaticData.WorkerList.Show();
            if (!IsOnScreen(StaticData.WorkerList))
               StaticData.WorkerList.DesktopLocation = new Point(Location.X, Location.Y);
         }
         else
            StaticData.WorkerList.BringToFront();
         StaticData.WorkerList.AddWorkerItem(wi);


         TwoTArgs<RequestType, E_Motivate> param = new TwoTArgs<RequestType, E_Motivate> { RequestType = RequestType.Motivate, argument2 = player_type };
         BackgroundWorkerEX bw = new BackgroundWorkerEX();
         bw.DoWork += bwScriptExecuter_DoWork;
         bw.param = param;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync(param);
         ListClass.BackgroundWorkers.Add(bw);
      }
      private void VisitAllTavern()
      {
         WorkerItem wi = new WorkerItem()
         {
            Title = i18n.getString("VisitAllTitle"),
            BeforeCountText = i18n.getString("Status"),
            CountText = i18n.getString("CountLabel"),
            ID = TavernVisitWorkerID
         };
         if (Application.OpenForms["WorkerList"] == null)
         {
            StaticData.WorkerList = new WorkerList();
            StaticData.WorkerList.Show();
            if (!IsOnScreen(StaticData.WorkerList))
               StaticData.WorkerList.DesktopLocation = new Point(Location.X, Location.Y);
         }
         else
            StaticData.WorkerList.BringToFront();
         StaticData.WorkerList.AddWorkerItem(wi);

         TwoTArgs<RequestType, object> param = new TwoTArgs<RequestType, object> { RequestType = RequestType.VisitTavern, argument2 = null };
         BackgroundWorkerEX bw = new BackgroundWorkerEX();
         bw.DoWork += bwScriptExecuter_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.param = param;
         bw.RunWorkerCompleted += workerComplete;
         bw.RunWorkerAsync(param);
         ListClass.BackgroundWorkers.Add(bw);
      }
      private void TsmiMoppleFriends_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.Friend);
      }
      private void TsmiMoppleClan_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.Clan);
      }
      private void TsmiMoppleNeighbor_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.Neighbor);
      }
      private void TsmiVisitMopple_Click(object sender, EventArgs e)
      {
         Motivate(E_Motivate.All);
         VisitAllTavern();
      }
      private void TsmiVisitTavern_Click(object sender, EventArgs e)
      {
         VisitAllTavern();
      }
      private void TsmiCollectTavern_Click(object sender, EventArgs e)
      {
         btnCollect_Click(null, null);
      }
      #endregion

      #region "Help"
      private void TvHelp_AfterSelect(object sender, TreeViewEventArgs e)
      {
         if (e.Node.Tag != null && !string.IsNullOrWhiteSpace(e.Node.Tag.ToString()))
         {
            mlHelpText.Text = e.Node.Tag.ToString();
         }
      }
      #endregion

      #region "Army"
      public void UpdateArmy()
      {
         logger.Info($">>> UpdateArmy");
         if (!UserData.ArmySelection.ContainsKey(UserData.LastWorld.Split('|')[0]))
            UserData.ArmySelection.Add(UserData.LastWorld.Split('|')[0], new List<string>());
         if (UnitImageLise != null)
         {
            try
            {
               Updater.UpdateArmy();
               Invoker.CallMethode(lvArmy, () => lvArmy.Items.Clear());
               Invoker.CallMethode(lvArmy, () => lvArmy.Groups.Clear());
               Invoker.SetProperty(lvArmy, () => lvArmy.LargeImageList, UnitImageLise);
               ListViewGroup group = null;
               string lastEra = "";
               var list = ListClass.UnitListEraSorted.TakeSome(1, 3);
               foreach (KeyValuePair<string, List<Unit>> item in list)
               {
                  if (lastEra != item.Key)
                  {
                     group = new ListViewGroup(item.Key, HorizontalAlignment.Left);
                  }
                  foreach (Unit unit in item.Value)
                  {
                     ListViewItem lvi = new ListViewItem($"{unit.name} ({unit.count})", $"armyuniticons_50x50_{unit.unit[0].unitTypeId}")
                     {
                        Group = group
                     };
                     Invoker.CallMethode(lvArmy, () => lvArmy.Items.Add(lvi));
                     if (group != null && group.Header != lastEra)
                     {
                        Invoker.CallMethode(lvArmy, () => lvArmy.Groups.Add(group));
                        lastEra = item.Key;
                     }
                  }
               }
            }
            catch (Exception)
            { }
         }
         FillArmyPool();
         logger.Info($"<<< UpdateArmy");
      }
      public void FillArmyPool()
      {
         DebugWatch.Start("Fill");
         ListViewGroup group = null;
         string lastEra = "";
         var list = ListClass.UnitListEraSorted.TakeSome(1, 3);
         Invoker.CallMethode(ucCurrentPool.lvArmy, () => ucCurrentPool.lvArmy.Items.Clear());
         Invoker.CallMethode(ucCurrentPool.lvSelectedArmy, () => ucCurrentPool.lvSelectedArmy.Items.Clear());
         ucCurrentPool.FillSelectedArmy(UserData.ArmySelection[UserData.LastWorld.Split('|')[0]]);
         foreach (KeyValuePair<string, List<Unit>> item in list)
         {
            if (lastEra != item.Key)
            {
               group = new ListViewGroup(item.Key, HorizontalAlignment.Left);
            }
            foreach (Unit unit in item.Value)
            {
               ListViewItem lvi = new ListViewItem($"{unit.name} ({unit.count})", $"armyuniticons_50x50_{unit.unit[0].unitTypeId}")
               {
                  Group = group,
                  Tag = unit
               };
               Invoker.CallMethode(ucCurrentPool.lvArmy, () => ucCurrentPool.lvArmy.Items.Add(lvi));
               if (group != null && group.Header != lastEra)
               {
                  Invoker.CallMethode(ucCurrentPool.lvArmy, () => ucCurrentPool.lvArmy.Groups.Add(group));
                  lastEra = item.Key;
               }
            }
         }
         foreach (var unitType in ucCurrentPool.SelectedArmyTypes)
         {
            Unit unit = ListClass.UnitList.Find(u => u.unit[0].unitTypeId == unitType);
            Invoker.CallMethode(ucCurrentPool.lvSelectedArmy, () => ucCurrentPool.lvSelectedArmy.Items.Add(unit));
         }
         DebugWatch.Stop();
      }
      private void UcCurrentPool_SubmitArmy(object sender, object data)
      {
         List<string> list = (List<string>)data;
         UserData.ArmySelection[UserData.LastWorld.Split('|')[0]] = list;
         Updater.UpdateAttackPool();
         UpdateArmy();
      }
      #endregion

      #region "Social"
      private void LvFriends_ItemChecked(object sender, ItemCheckedEventArgs e)
      {
         Player p = (Player)e.Item.Tag;
         if (e.Item.Checked)
         {
            if (UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Contains(p.player_id.Value)) return;
            UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Add(p.player_id.Value);
            UserData.SaveSettings();
            FillIgnorePlayer();
         }
         else
         {
            if (!UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Contains(p.player_id.Value)) return;
            UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Remove(p.player_id.Value);
            UserData.SaveSettings();
            FillIgnorePlayer();
         }
      }
      private void UpdatePlayerLists()
      {
         Invoker.CallMethode(lvFriends, () => lvFriends.Items.Clear());
         foreach (var friend in ListClass.FriendList)
         {
            ListViewItem lvi = new ListViewItem()
            {
               Text = $"{friend.name} {(friend.clan != null ? "(" + friend.clan.name + ") " : "")}{(friend.is_active == false && friend.is_self == false ? "(" + i18n.getString("GUI.Social.Inactive").ToLower() + ")" : "")}",
               Tag = friend,
               Checked = UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Contains(friend.player_id.Value)
            };
            Invoker.CallMethode(lvFriends, () => lvFriends.Items.Add(lvi));
         }
         Invoker.CallMethode(lvMember, () => lvMember.Items.Clear());
         foreach (var member in ListClass.ClanMemberList)
         {
            ListViewItem lvi = new ListViewItem()
            {
               Text = $"{member.name} {(member.is_active == false && member.is_self == false ? "(" + i18n.getString("GUI.Social.Inactive").ToLower() + ")" : "")}",
               Tag = member,
               Checked = UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Contains(member.player_id.Value)
            };
            Invoker.CallMethode(lvMember, () => lvMember.Items.Add(lvi));
         }
         Invoker.CallMethode(lvNeighbor, () => lvNeighbor.Items.Clear());
         foreach (var neighbor in ListClass.NeighborList)
         {
            ListViewItem lvi = new ListViewItem()
            {
               Text = $"{neighbor.name} {(neighbor.clan != null ? "(" + neighbor.clan.name + ") " : "")} {(neighbor.is_active == false && neighbor.is_self == false ? "(" + i18n.getString("GUI.Social.Inactive").ToLower() + ")" : "")}",
               Tag = neighbor,
               Checked = UserData.IgnoredPlayers[UserData.LastWorld.Split('|')[0]].Contains(neighbor.player_id.Value)
            };
            Invoker.CallMethode(lvNeighbor, () => lvNeighbor.Items.Add(lvi));
         }
      }
      #endregion

      #region "Messages"
      public void UpdateMessages()
      {
         try
         {
            Updater.UpdateMessages("social");
            Updater.UpdateMessages("guild");
            Invoker.CallMethode(lvMessages, () => lvMessages.Items.Clear());
            Invoker.SetProperty(lvMessages, () => lvMessages.LargeImageList, UnitImageLise);
            ListViewGroup group = null;
            string lastEra = "";
            foreach (KeyValuePair<string, List<Unit>> item in ListClass.UnitListEraSorted)
            {
               if (lastEra != item.Key)
               {
                  group = new ListViewGroup(item.Key, HorizontalAlignment.Left);
               }
               foreach (Unit unit in item.Value)
               {
                  ListViewItem lvi = new ListViewItem($"{unit.name} ({unit.count})", $"armyuniticons_50x50_{unit.unit[0].unitTypeId}")
                  {
                     Group = group
                  };
                  Invoker.CallMethode(lvArmy, () => lvArmy.Items.Add(lvi));
                  if (group != null && group.Header != lastEra)
                  {
                     Invoker.CallMethode(lvArmy, () => lvArmy.Groups.Add(group));
                     lastEra = item.Key;
                  }
               }
            }
         }
         catch (Exception)
         { }
      }
      #endregion

      #region "Battle"
      public void UpdateBoost()
      {
         Invoker.SetProperty(lblAttackBoost, () => lblAttackBoost.Text, $"{AttackBoost[0]}{lblAttackBoost.Tag} / {AttackBoost[1]}{lblAttackBoost.Tag}");
         Invoker.SetProperty(lblDefensBoost, () => lblDefensBoost.Text, $"{DefenseBoost[0]}{lblDefensBoost.Tag} / {DefenseBoost[1]}{lblDefensBoost.Tag}");
      }
      public void UpdateBattle()
      {
         UpdateGBG();
         CbManualFighting_CheckedChanged(null, null);
         UpdateGEX();
      }
      public void UpdateGEX()
      {
         Invoker.CallMethode(lvWave, () => lvWave.Items.Clear());
         UpdateArmy();
         GEXHelper.UpdateGEX();
         if (GEXHelper.GetCurrentState == -1)
         {
            Invoker.SetProperty(lblStage, () => lblStage.Text, $"{i18n.getString("GUI.Battle.GEX.Stage")} {i18n.getString("GUI.Battle.GEX.NoGEX")}");
            Invoker.SetProperty(lvWave, () => lvWave.Visible, false);
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Enabled, false);
            Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Enabled, false);
            Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Text, i18n.getString("GUI.Battle.GEX.NoGEX"));
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Text, i18n.getString("GUI.Battle.GEX.NoGEX"));
            return;
         }
         if (string.Equals(GEXHelper.GEXOverview.state, "inactive"))
         {
            Invoker.SetProperty(lblStage, () => lblStage.Text, $"{i18n.getString("GUI.Battle.GEX.Stage")} {i18n.getString("GUI.Battle.GEX.NoGEX")}");
            Invoker.SetProperty(lvWave, () => lvWave.Visible, false);
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Enabled, false);
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Text, i18n.getString("GUI.Battle.GEX.NoGEX"));
            Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Enabled, false);
            Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Text, i18n.getString("GUI.Battle.GEX.NoGEX"));
            return;
         }
         if (GEXHelper.NeedChangeDiff() && GEXHelper.NextDiffOpen(GEXHelper.GEXOverview.progress.difficulty))
         {
            if(!GEXHelper.ChangeDiff(GEXHelper.GEXOverview.progress.difficulty + 1).Item1)
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString("GUI.GEX.Map.IncidentsFound")}");
               Updater.UpdateStartUp();
               UpdateHiddenRewardsView();
               OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CollectIncident };
               bwScriptExecuterOneArg_DoWork(this, new DoWorkEventArgs(param));
               var ret = GEXHelper.ChangeDiff(GEXHelper.GEXOverview.progress.difficulty + 1);
               if (!ret.Item1)
               {
                  MessageBox.Show("!!!! PLEASE REPORT THE TEXT INSIDE YOU CLIPBOARD TO THE DEV (GITHUB/DISCORD) (COPY/PASTE) !!!!", "PLEASE REPORT!!");
                  Clipboard.SetText(ret.Item2);
               }
            }
         }
         else if (GEXHelper.NeedChangeDiff() && !GEXHelper.NextDiffOpen(GEXHelper.GEXOverview.progress.difficulty))
         {
            Invoker.SetProperty(lblStage, () => lblStage.Text, $"{i18n.getString("GUI.Battle.GEX.Stage")} {i18n.getString("GUI.Battle.GEX.NoOpenDiff")}");
            Invoker.SetProperty(lvWave, () => lvWave.Visible, false);
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Enabled, false);
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Text, i18n.getString("GUI.Battle.GEX.NoOpenDiff"));
            Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Enabled, false);
            Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Text, i18n.getString("GUI.Battle.GEX.NoOpenDiff"));
            return;
         }
         Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Enabled, true);
         Invoker.SetProperty(btnBuyAttempt, () => btnBuyAttempt.Text, i18n.getString("GUI.Battle.GEX.BuyAttempt"));
         Invoker.SetProperty(lvWave, () => lvWave.Visible, true);
         Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Enabled, true);
         ResearchEra noAge = ListClass.Eras.Find(re => re.era == "NoAge");
         GEXWaves[] waves = GEXHelper.Armywaves;
         Invoker.SetProperty(lvWave, () => lvWave.SmallImageList, UnitImageLise);
         Invoker.SetProperty(lvWave, () => lvWave.View, System.Windows.Forms.View.SmallIcon);
         Invoker.SetProperty(lvWave, () => lvWave.Visible, true);
         int GexTries = ListClass.GoodsDict[noAge.name].Find(g => g.good_id == "guild_expedition_attempt").value;
         string GEXTriesText = "";
         if (!GEXHelper.IsCurrentStateChest)
         {
            GEXTriesText = $"{i18n.getString("GUI.Battle.GEX.Tries")} {GexTries}";
         }
         Invoker.SetProperty(lblStage, () => lblStage.Text, $"{i18n.getString("GUI.Battle.GEX.Stage")} {GEXHelper.GetCurrentState} {(GEXHelper.IsCurrentStateChest ? i18n.getString("GUI.Battle.GEX.Chest") : "")} {GEXTriesText}");
         if (waves == null)
         {
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Text, i18n.getString("GUI.Battle.GEX.ChestOpen"));
            Invoker.SetProperty(lvWave, () => lvWave.Visible, false);
         }
         else
         {
            Invoker.SetProperty(btnDoGEXAction, () => btnDoGEXAction.Text, i18n.getString("GUI.Battle.Fight"));
            int currentwave = 1;
            foreach (GEXWaves wave in waves)
            {
               foreach (var unit in wave.units)
               {
                  string unitname = ListClass.UnitTypes.Find(ut => ut.unitTypeId == unit.unitTypeId).name;
                  ListViewItem lvi = new ListViewItem($"{unitname}", $"armyuniticons_50x50_{unit.unitTypeId}");
                  if (currentwave == 1)
                  {
                     ListViewGroup group = new ListViewGroup(i18n.getString("GUI.Battle.UC.Wave1"), HorizontalAlignment.Left);
                     if (lvWave.Groups.Count == 0)
                     {
                        group.Name = "wave1";
                        lvi.Group = group;
                     }
                     else
                     {
                        lvi.Group = lvWave.Groups["wave1"];
                     }
                     Invoker.CallMethode(lvWave, () => lvWave.Items.Add(lvi));
                     if (lvWave.Groups.Count == 0)
                        Invoker.CallMethode(lvWave, () => lvWave.Groups.Add(group));
                  }
                  else if (currentwave == 2)
                  {
                     ListViewGroup group = new ListViewGroup(i18n.getString("GUI.Battle.UC.Wave2"), HorizontalAlignment.Left);
                     if (lvWave.Groups.Count == 1)
                     {
                        group.Name = "wave2";
                        lvi.Group = group;
                     }
                     else
                     {
                        lvi.Group = lvWave.Groups["wave2"];
                     }
                     Invoker.CallMethode(lvWave, () => lvWave.Items.Add(lvi));
                     if (lvWave.Groups.Count == 1)
                        Invoker.CallMethode(lvWave, () => lvWave.Groups.Add(group));
                  }
               }
               currentwave = 2;
            }
         }
      }
      frmArmySelection frmAS = null;
      private void BtnDoGEXAction_Click(object sender, EventArgs e)
      {
         if (GEXHelper.IsCurrentStateChest)
         {
            var ret = GEXHelper.OpenChest(GEXHelper.GetCurrentState);
            lblResult.Text = $"{i18n.getString(lblResult.Tag.ToString())} {ret.name}";
         }
         else
         {
            ResearchEra noAge = ListClass.Eras.Find(re => re.era == "NoAge");
            if (ListClass.GoodsDict[noAge.name].Find(g => g.good_id == "guild_expedition_attempt").value == 0)
            {
               if (UserData.BuyGEXAttempts)
               {
                  if (GEXHelper.BuyNextAttempt())
                  {
                     Updater.UpdateResources();
                     Updater.UpdateInventory();
                     frmAS = new frmArmySelection();
                     frmAS.Name = "frmAS";
                     frmAS.ArmySelection.imgList = UnitImageLise;
                     frmAS.ArmySelection.FillArmyList(ListClass.UnitListEraSorted);
                     frmAS.ArmySelection.FillSelectedArmy(UserData.ArmySelection[UserData.LastWorld.Split('|')[0]]);
                     frmAS.ArmySelection.SubmitArmy += ArmySelection_SubmitArmy;
                     frmAS.ShowDialog();
                  }
               }
               else { return; }
            }
            else
            {
               frmAS = new frmArmySelection();
               frmAS.Name = "frmAS";
               frmAS.ArmySelection.imgList = UnitImageLise;
               frmAS.ArmySelection.FillArmyList(ListClass.UnitListEraSorted);
               frmAS.ArmySelection.FillSelectedArmy(UserData.ArmySelection[UserData.LastWorld.Split('|')[0]]);
               frmAS.ArmySelection.SubmitArmy += ArmySelection_SubmitArmy;
               frmAS.ShowDialog();
            }
         }
         DebugWatch.Start("Update Resource");
         Updater.UpdateResources();
         DebugWatch.Stop();
         DebugWatch.Start("Update Inventory");
         Updater.UpdateInventory();
         DebugWatch.Stop();
         DebugWatch.Start("Update Dashbord");
         UpdateDashbord();
         DebugWatch.Stop();
         DebugWatch.Start("Update GEX");
         UpdateGEX();
         DebugWatch.Stop();
      }
      private void ArmySelection_SubmitArmy(object sender, dynamic data = null)
      {
         List<string> list = (List<string>)data;
         UserData.ArmySelection[UserData.LastWorld.Split('|')[0]] = list;
         Updater.UpdateAttackPool();
         if (frmAS != null)
         {
            if (Application.OpenForms["frmAS"] != null) Application.OpenForms["frmAS"].Close();
         }
         var ret = GEXHelper.StartFight(GEXHelper.GetCurrentState);
         if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
         {
            ret = GEXHelper.StartFight(GEXHelper.GetCurrentState);
            if (ret.battleType.currentWaveId == 1 && ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
            {
               lblResult.Text = $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}";
            }
         }
         else if (ret.state.winnerBit == 1)
         {
            lblResult.Text = $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}";
         }
         else
         {
            lblResult.Text = $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}";
         }
      }
      public void UpdateGBG(bool updateOnly = false, int ProvID = -1)
      {
         Invoker.CallMethode(lvGBGWave, () => lvGBGWave.Items.Clear());
         DebugWatch.Start("Update Army");
         Updater.UpdateArmy();
         DebugWatch.Stop();
         DebugWatch.Start("Update GBG");
         GBGHelper.UpdateGBG();
         DebugWatch.Stop();

         if (!GBGHelper.IsParticipating)
         {
            Invoker.SetProperty(lvGBGWave, () => lvGBGWave.Visible, false);
            Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, false);
            Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Text, i18n.getString("GUI.Battle.GBG.NoGBG"));
            Invoker.SetProperty(cbSector, () => cbSector.Enabled, false);
            Invoker.SetProperty(cbAutoFight, () => cbAutoFight.Enabled, false);
            Invoker.SetProperty(nudCount, () => nudCount.Enabled, false);
            Invoker.SetProperty(cbType, () => cbType.Enabled, false);
            Invoker.SetProperty(nudDelay, () => nudDelay.Enabled, false);
            Invoker.SetProperty(btnReloadGBG, () => btnReloadGBG.Enabled, false);
            Invoker.SetProperty(lblCurrentAttrition, () => lblCurrentAttrition.Text, $"0");
            Invoker.SetProperty(lblCurrentMultiplier, () => lblCurrentMultiplier.Text, $"x0 / +0%");
            return;
         }
         DebugWatch.Start("Get Provinces");
         ListClass.ProvincesGBG = GBGHelper.GetProvinces();
         DebugWatch.Stop();
         Invoker.SetProperty(lvGBGWave, () => lvGBGWave.SmallImageList, UnitImageLise);
         Invoker.SetProperty(lvGBGWave, () => lvGBGWave.View, System.Windows.Forms.View.SmallIcon);
         Invoker.SetProperty(lvGBGWave, () => lvGBGWave.Visible, true);

         Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, true);
         Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Text, i18n.getString("GUI.Battle.GBG.DoFight"));

         int currentAttr = GBGHelper.CurrentBattleground.currentPlayerParticipant.attrition.level;
         Invoker.SetProperty(lblCurrentAttrition, () => lblCurrentAttrition.Text, $"{currentAttr}");

         int bonusNego = GBGHelper.CurrentBattleground.currentPlayerParticipant.attrition.negotiationMultiplier;
         int bonusFight = GBGHelper.CurrentBattleground.currentPlayerParticipant.attrition.defendingArmyBonus;
         Invoker.SetProperty(lblCurrentMultiplier, () => lblCurrentMultiplier.Text, $"x{bonusNego} / +{bonusFight}%");

         if (!updateOnly)
         {
            DebugWatch.Start("For-Loop Sector List");
            Invoker.CallMethode(cbSector, () => cbSector.Items.Clear());
            for (int i = 0; i < ListClass.ProvincesGBG.Count; i++)
            {
               var item = ListClass.ProvincesGBG[i];
               if (!GBGHelper.CanFightProvince(item.id.Value)) continue;
               item = GBGHelper.GetSiegeCount(item);
               ProvinceEX provinceEX = new ProvinceEX()
               {
                  connections = item.connections,
                  OwnGuildID = GBGHelper.CurrentBattleground.currentParticipantId,
                  OwnProgress = item.conquestProgress.ToList().Find(cp => cp.participantId == GBGHelper.CurrentBattleground.currentParticipantId),
                  conquestProgress = item.conquestProgress,
                  id = item.id,
                  isSpawnSpot = item.isSpawnSpot,
                  lockedUntil = item.lockedUntil,
                  name = item.name,
                  ownerId = item.ownerId,
                  SiegeCount = item.SiegeCount,
                  totalBuildingSlots = item.totalBuildingSlots,
                  victoryPoints = item.victoryPoints,
                  __class__ = item.__class__
               };
               Invoker.CallMethode(cbSector, () => { cbSector.Items.Add(provinceEX); });
            }
            DebugWatch.Stop();
         }
         else
         {
            var pEX = ListClass.ProvincesGBG.Find(pgbg => pgbg.id == ProvID);
            int index = -1;
            var items = cbSector.Items;
            foreach (ProvinceEX item in items)
            {
               index += 1;
               if (item.id.Value != ProvID) continue;
               ProvinceEX newItem = item;
               newItem.conquestProgress = ListClass.ProvincesGBG.Find(pgbg => pgbg.id == ProvID).conquestProgress;
               newItem.OwnProgress = pEX.conquestProgress.ToList().Find(cp => cp.participantId == GBGHelper.CurrentBattleground.currentParticipantId);
               Invoker.CallMethode(cbSector, () => cbSector.Items.RemoveAt(index));
               Invoker.CallMethode(cbSector, () => cbSector.Items.Insert(index, newItem));
               break;
            }
         }
         Invoker.CallMethode(cbSector, () => cbSector.Update());
         Invoker.SetProperty(cbSector, () => cbSector.SelectedIndex, CBSectorIndex);

         if (cbType.Items.Count < 3)
         {
            Invoker.CallMethode(cbType, () => cbType.Items.Add(i18n.getString("GUI.GBG.Attrition")));
            Invoker.CallMethode(cbType, () => cbType.Items.Add(i18n.getString("GUI.GBG.Fights")));
            Invoker.CallMethode(cbType, () => cbType.Items.Add(i18n.getString("GUI.GBG.Progress")));
            Invoker.SetProperty(cbType, () => cbType.SelectedIndex, 0);
         }
      }
      private void CbManualFighting_CheckedChanged(object sender, EventArgs e)
      {
         if (cbAutoFight.Checked)
         {
            nudCount.Enabled = true;
            cbType.Enabled = true;
            nudDelay.Enabled = true;
         }
         else
         {
            FightCounter = 0;
            nudCount.Enabled = false;
            cbType.Enabled = false;
            nudDelay.Enabled = false;
         }
      }
      private void CbSector_SelectedIndexChanged(object sender, EventArgs e)
      {
         CBSectorIndex = cbSector.SelectedIndex;
         GBGProvince selProv = (GBGProvince)cbSector.SelectedItem;
         if (!GBGHelper.CanFightProvince(selProv.id.Value))
         {
            Invoker.SetProperty(nudCount, () => nudCount.Enabled, false);
            Invoker.SetProperty(cbType, () => cbType.Enabled, false);
            Invoker.SetProperty(nudDelay, () => nudDelay.Enabled, false);
            Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, false);
            return;
         }
         GBGWaves[] waves = GBGHelper.GetArmyInfo(selProv.id.Value);
         if (waves == null)
         {
            Invoker.SetProperty(nudCount, () => nudCount.Enabled, false);
            Invoker.SetProperty(cbType, () => cbType.Enabled, false);
            Invoker.SetProperty(nudDelay, () => nudDelay.Enabled, false);
            Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, false);
            return;
         }
         if (cbAutoFight.Checked)
         {
            Invoker.SetProperty(nudCount, () => nudCount.Enabled, true);
            Invoker.SetProperty(cbType, () => cbType.Enabled, true);
            Invoker.SetProperty(nudDelay, () => nudDelay.Enabled, true);
         }
         Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, true);
         Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Text, i18n.getString("GUI.Battle.GBG.DoFight"));
         int currentwave = 1;
         Invoker.CallMethode(lvGBGWave, () => lvGBGWave.Items.Clear());
         foreach (var wave in waves)
         {
            foreach (var unit in wave.units)
            {
               string unitname = ListClass.UnitTypes.Find(ut => ut.unitTypeId == unit.unitTypeId).name;
               ListViewItem lvi = new ListViewItem($"{unitname}", $"armyuniticons_50x50_{unit.unitTypeId}");
               if (currentwave == 1)
               {
                  ListViewGroup group = new ListViewGroup(i18n.getString("GUI.Battle.UC.Wave1"), HorizontalAlignment.Left);
                  if (lvGBGWave.Groups.Count == 0)
                  {
                     group.Name = "wave1";
                     lvi.Group = group;
                  }
                  else
                  {
                     lvi.Group = lvGBGWave.Groups["wave1"];
                  }
                  Invoker.CallMethode(lvGBGWave, () => lvGBGWave.Items.Add(lvi));
                  if (lvGBGWave.Groups.Count == 0)
                     Invoker.CallMethode(lvGBGWave, () => lvGBGWave.Groups.Add(group));
               }
               else if (currentwave == 2)
               {
                  ListViewGroup group = new ListViewGroup(i18n.getString("GUI.Battle.UC.Wave2"), HorizontalAlignment.Left);
                  if (lvGBGWave.Groups.Count == 1)
                  {
                     group.Name = "wave2";
                     lvi.Group = group;
                  }
                  else
                  {
                     lvi.Group = lvGBGWave.Groups["wave2"];
                  }
                  Invoker.CallMethode(lvGBGWave, () => lvGBGWave.Items.Add(lvi));
                  if (lvGBGWave.Groups.Count == 1)
                     Invoker.CallMethode(lvGBGWave, () => lvGBGWave.Groups.Add(group));
               }
            }
            currentwave = 2;
         }
      }
      private void BtnReloadGBG_Click(object sender, EventArgs e)
      {
         UpdateGBG();
      }
      private void BtnFightGBG_Click(object sender, EventArgs e)
      {
         if (!cbAutoFight.Checked && UserData.ArmySelection.Count > 0)
         {
            frmAS = new frmArmySelection();
            frmAS.Name = "frmAS";
            frmAS.ArmySelection.imgList = UnitImageLise;
            frmAS.ArmySelection.FillArmyList(ListClass.UnitListEraSorted);
            frmAS.ArmySelection.FillSelectedArmy(UserData.ArmySelection[UserData.LastWorld.Split('|')[0]]);
            frmAS.ArmySelection.SubmitArmy += SubmitGBGArmy;
            frmAS.ShowDialog();
            ProvinceEX prov = (ProvinceEX)cbSector.SelectedItem;
            UpdateGBG();
         }
         else
         {
            int value = (int)nudCount.Value;
            if (cbType.SelectedIndex == 0) //Attrition
            {
               Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, false);
               BackgroundWorkerEX ex = new BackgroundWorkerEX();
               ex.DoWork += Attrition;
               ex.RunWorkerAsync(value);
            }
            else if (cbType.SelectedIndex == 1) //Fights
            {
               Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, false);
               BackgroundWorkerEX ex = new BackgroundWorkerEX();
               ex.DoWork += Fight;
               ex.RunWorkerAsync(value);
            }
            else if (cbType.SelectedIndex == 2) //Progress
            {
               Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, false);
               BackgroundWorkerEX ex = new BackgroundWorkerEX();
               ex.DoWork += Progress;
               ex.RunWorkerAsync(value);
            }
         }
      }

      private void Fight(object sender, DoWorkEventArgs e)
      {
         while (FightCounter < (int)e.Argument)
         {
            UpdateArmy();
            if (!Updater.UpdateAttackPool()) break;
            var prov = (ProvinceEX)cbSector.Invoke(new Func<ProvinceEX>(() => (ProvinceEX)cbSector.SelectedItem));
            var ret = GBGHelper.StartFight(prov.id.Value);
            if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
            {
               ret = GBGHelper.StartFight(prov.id.Value);
               if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
               {
                  Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
               }
               else
               {
                  Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
                  break;
               }
            }
            else if (ret.state.winnerBit == 1)
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
            }
            else
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
               break;
            }
            UpdateGBG(true, prov.id.Value);
            FightCounter += 1;
            Thread.Sleep((int)nudDelay.Value);
         }
         FightCounter = 0;
         Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, true);
      }
      private void Attrition(object sender, DoWorkEventArgs e)
      {
         while (GBGHelper.CurrentBattleground.currentPlayerParticipant.attrition.level < (int)e.Argument)
         {
            UpdateArmy();
            if (!Updater.UpdateAttackPool()) break;
            var prov = (ProvinceEX)cbSector.Invoke(new Func<ProvinceEX>(() => (ProvinceEX)cbSector.SelectedItem));
            var ret = GBGHelper.StartFight(prov.id.Value);
            if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
            {
               ret = GBGHelper.StartFight(prov.id.Value);
               if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
               {
                  Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
               }
               else
               {
                  Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
                  break;
               }
            }
            else if (ret.state.winnerBit == 1)
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
            }
            else
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
               break;
            }
            UpdateGBG(true, prov.id.Value);
            Thread.Sleep((int)nudDelay.Value);
         }
         Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, true);
      }
      private void Progress(object sender, DoWorkEventArgs e)
      {
         var prov = (ProvinceEX)cbSector.Invoke(new Func<ProvinceEX>(() => (ProvinceEX)cbSector.SelectedItem));
         while (prov.OwnProgress.progress < (int)e.Argument && prov.OwnProgress.progress < (prov.OwnProgress.maxProgress - 10))
         {
            UpdateArmy();
            if (!Updater.UpdateAttackPool()) break;
            prov = (ProvinceEX)cbSector.Invoke(new Func<ProvinceEX>(() => (ProvinceEX)cbSector.SelectedItem));
            var ret = GBGHelper.StartFight(prov.id.Value);
            if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
            {
               ret = GBGHelper.StartFight(prov.id.Value);
               if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
               {
                  Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
               }
               else
               {
                  Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
                  break;
               }
            }
            else if (ret.state.winnerBit == 1)
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
            }
            else
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
               break;
            }
            UpdateGBG(true, prov.id.Value);
            Thread.Sleep((int)nudDelay.Value);
         }
         Invoker.SetProperty(btnFightGBG, () => btnFightGBG.Enabled, true);
      }

      private void SubmitGBGArmy(object sender, dynamic data = null)
      {
         List<string> list = (List<string>)data;
         UserData.ArmySelection[UserData.LastWorld.Split('|')[0]] = list;
         Updater.UpdateAttackPool();
         if (frmAS != null)
            if (Application.OpenForms["frmAS"] != null) Application.OpenForms["frmAS"].Close();

         ProvinceEX prov = (ProvinceEX)cbSector.SelectedItem;
         var ret = GBGHelper.StartFight(prov.id.Value);
         if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
         {
            ret = GBGHelper.StartFight(prov.id.Value);
            if (ret.battleType.totalWaves == 2 && ret.state.winnerBit == 1)
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
            }
            else
            {
               Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
            }
         }
         else if (ret.state.winnerBit == 1)
         {
            Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Won")}");
         }
         else
         {
            Invoker.SetProperty(lblResult, () => lblResult.Text, $"{i18n.getString(lblResult.Tag.ToString())} {i18n.getString("GUI.GEX.Battle.Lost")}");
         }
      }
      private void CbType_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (cbType.SelectedIndex == 0)
         {
            Invoker.SetProperty(nudCount, () => nudCount.Value, GBGHelper.CurrentBattleground.currentPlayerParticipant.attrition.level);
         }
         else if (cbType.SelectedIndex == 2)
         {
            var prov = (ProvinceEX)cbSector.Invoke(new Func<ProvinceEX>(() => (ProvinceEX)cbSector.SelectedItem));
            Invoker.SetProperty(nudCount, () => nudCount.Value, prov.OwnProgress.maxProgress);
         }
      }
      private void BtnBuyAttempt_Click(object sender, EventArgs e)
      {
         if (GEXHelper.BuyNextAttempt())
         {
            Updater.UpdateResources();
            Updater.UpdateInventory();
            ResearchEra noAge = ListClass.Eras.Find(re => re.era == "NoAge");
            int GexTries = ListClass.GoodsDict[noAge.name].Find(g => g.good_id == "guild_expedition_attempt").value;
            string GEXTriesText = "";
            if (!GEXHelper.IsCurrentStateChest)
            {
               GEXTriesText = $"{i18n.getString("GUI.Battle.GEX.Tries")} {GexTries}";
            }
            Invoker.SetProperty(lblStage, () => lblStage.Text, $"{i18n.getString("GUI.Battle.GEX.Stage")} {GEXHelper.GetCurrentState} {(GEXHelper.IsCurrentStateChest ? i18n.getString("GUI.Battle.GEX.Chest") : "")} {GEXTriesText}");
         }
      }
      #endregion


      protected override void WndProc(ref Message m)
      {
         const int RESIZE_HANDLE_SIZE = 10;
         switch (m.Msg)
         {
            case 0x0084/*NCHITTEST*/ :
               base.WndProc(ref m);

               if ((int)m.Result == 0x01/*HTCLIENT*/)
               {
                  Point screenPoint = new Point(m.LParam.ToInt32());
                  Point clientPoint = this.PointToClient(screenPoint);
                  if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                  {
                     if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                     else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                        m.Result = (IntPtr)12/*HTTOP*/ ;
                     else
                        m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                  }
                  else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                  {
                     if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)10/*HTLEFT*/ ;
                     else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                        m.Result = (IntPtr)2/*HTCAPTION*/ ;
                     else
                        m.Result = (IntPtr)11/*HTRIGHT*/ ;
                  }
                  else
                  {
                     if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                     else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                        m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                     else
                        m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                  }
               }
               return;
         }
         base.WndProc(ref m);
      }
      protected override CreateParams CreateParams
      {
         get
         {
            CreateParams cp = base.CreateParams;
            cp.Style |= 0x20000;
            return cp;
         }
      }
      public void ShowLoadingForm()
      {
         LoadingFrm = new Loading(false);
         if (LoadingFrm.lblPleaseLogin.InvokeRequired)
         {
            LoadingFrm.lblPleaseLogin.Invoke((MethodInvoker)delegate
            {
               LoadingFrm.lblPleaseLogin.Text = i18n.getString("GUI.Loading.Changing");
               Font tmp = LoadingFrm.lblPleaseLogin.Font;
               LoadingFrm.lblPleaseLogin.Font = new Font(tmp.FontFamily, 16);
            });
         }
         else
         {
            LoadingFrm.lblPleaseLogin.Text = i18n.getString("GUI.Loading.Changing");
            Font tmp = LoadingFrm.lblPleaseLogin.Font;
            LoadingFrm.lblPleaseLogin.Font = new Font(tmp.FontFamily, 16);
         }
         LoadingFrm.Show();
         isLoading = true;
         for (int i = 0; i < 500; i++)
         {
            Thread.Sleep(2);
            Application.DoEvents();
         }
      }





      private void TsmiTestFunctions_Click(object sender, EventArgs e)
      {
         double[][] data = BattleAI.CalcIdentifier();
         File.WriteAllText("training.data", JsonConvert.SerializeObject(data));
         //Updater.UpdateMessages("social");
      }

   }
}
