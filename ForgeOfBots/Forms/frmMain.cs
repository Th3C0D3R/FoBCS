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
using static ForgeOfBots.FoBUpdater.FoBUpdater;
using static ForgeOfBots.Utils.Helper;
using static ForgeOfBots.Utils.Extensions;
using static ForgeOfBots.Utils.StaticData;
using AllWorlds = ForgeOfBots.GameClasses.ResponseClasses.WorldSelection;
using CUpdate = ForgeOfBots.DataHandler.Update;
using WebClient = System.Net.WebClient;
using ForgeOfBots.Forms.UserControls;
using Newtonsoft.Json.Linq;
using System.Collections;

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
      private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
      private bool blockExpireBox = false;
      readonly BackgroundWorker bw = new BackgroundWorker();
      bool Canceled = false;

      public frmMain() { }
      public frmMain(string[] args)
      {
         if (args != null)
         {
            if (args.Length > 0)
               if (args[0].StartsWith("/"))
                  if (args[0].Substring(1).ToLower().Equals("debug"))
                     DEBUGMODE = true;
         }
         logger.Info($"Debugmode {(DEBUGMODE ? "activated" : "deactivated")}");

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
            i18n.TranslateForm();
            i18n.TranslateCMS(cmsMainMenu);
            logger.Info($"check for updates");
            CheckForUpdate();
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
      }
      private void Usi_UserDataEntered(string username, string password, string server)
      {
         Log("[DEBUG] Userdata loaded", lbOutputWindow);
         UserData.Username = username;
         UserData.Password = password;
         UserData.WorldServer = server;
         UserData.SaveSettings();
         logger.Info($"username: {username} | worldserver: {server}");
         usi.Close();
         ContinueExecution();
      }
      private void ContinueExecution()
      {
         logger.Info($">>> ContinueExecution()");
         ChromeOptions co = new ChromeOptions()
         {
            AcceptInsecureCertificates = true
         };
         co.AddArguments(
            //"--headless",
            "--disable-extensions",
            "--disable-breakpad",
            "--disable-hang-monitor",
            "--disable-logging",
            "--disable-metrics-reporting",
            "--ssl-version-min=tl",
            "--no-sandbox",
            "--remote-debugging-port=9222",
         "--disable-dev-shm-usage",
#if RELEASE
         "--window-position=-32000,-32000",
#endif
            "--disable-metrics"
            );
         co.AddArgument($"user-agent={UserData.CustomUserAgent}");

         logger.Info($"creating chromeDriverService");
         var chromeDriverService = ChromeDriverService.CreateDefaultService();
         chromeDriverService.HideCommandPromptWindow = true;
         logger.Info($"creating (Remote-)Driver");
         //driver = new RemoteWebDriver(new Uri("http://134.255.216.102:4444/"), co.ToCapabilities(),TimeSpan.FromSeconds(60));
         driver = new ChromeDriver(chromeDriverService, co);
         logger.Info($"navigating to 'https://{UserData.WorldServer}0.forgeofempires.com/'");
         driver.Navigate().GoToUrl($"https://{UserData.WorldServer}0.forgeofempires.com/");
         driver.Manage().Window.Minimize();
         cookieJar = driver.Manage().Cookies;
         jsExecutor = (IJavaScriptExecutor)driver;
         StaticData.BotData.CID = cookieJar.AllCookies.HasCookie("CID").Item2;
         StaticData.BotData.CSRF = cookieJar.AllCookies.HasCookie("CSRF").Item2;
         StaticData.BotData.SID = cookieJar.AllCookies.HasCookie("SID").Item2;
         StaticData.BotData.XSRF = cookieJar.AllCookies.HasCookie("XSRF-TOKEN").Item2;

         logger.Info($"Process Portraits");
         ProcessPortraits();
         logger.Info($"ExtractGoodImages");
         GoodImageExtractor.GetGoodImages(UserData.WorldServer);

         if (!string.IsNullOrWhiteSpace(UserData.SerialKey))
         {
            logger.Info($"Init Premium if key exists");
            BackgroundWorker bwPremium = new BackgroundWorker();
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
             .Replace("##t##", "false")
             .Replace("##city##", "\"" + UserData.LastWorld.Split('|')[0] + "\"");
            var x = jsExecutor.ExecuteAsyncScript(loginJS);
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
            UserData.PlayableWorlds = ListClass.WorldList.ConvertAll(new Converter<Tuple<string, string, WorldState>, string>(WorldToPlayable));
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
      }
      private void CheckLanguages()
      {
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
      }
      private void Application_ApplicationExit(object sender, EventArgs e)
      {
         //TelegramNotify.Send($"Session ended {UserData.Username} { RunningTime.Elapsed:h'h 'm'm 's's'}");
         UserData.SaveSettings();
         RunningTime.Stop();
         driver.Quit();
      }
      private void workerComplete(object sender, RunWorkerCompletedEventArgs e)
      {
         ListClass.BackgroundWorkers.Remove((BackgroundWorker)sender);
      }
      private void Ws_WorldDataEntered(Form that, string key, string value)
      {
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
         if (!(x is string))
            logger.Info($"loginscript returned not a string (URL): {x.ToString()}");
         else
         {
            logger.Info($"loginscript returned: {(string)x}");
            var ret = (string)x;
            driver.Navigate().GoToUrl(ret);
            GetUIDAndForgeHX(driver.PageSource);
         }
         that.Close();
      }
      private void GetUIDAndForgeHX(string source)
      {
         logger.Info($">>> GetUIDAndForgeHX");
         InitSettingsTab();
         var regExUserID = new Regex(@"https:\/\/(\w{1,2}\d{1,2})\.forgeofempires\.com\/game\/json\?h=(.+)'", RegexOptions.IgnoreCase);
         var regExForgeHX = new Regex(@"https:\/\/foe\w{1,4}\.innogamescdn\.com\/\/cache\/ForgeHX(.+.js)'", RegexOptions.IgnoreCase);
         var FHXMatch = regExForgeHX.Match(source);
         var UIDMatch = regExUserID.Match(source);
         if (UIDMatch.Success)
         {
            StaticData.BotData.UID = UIDMatch.Groups[2].Value;
            StaticData.BotData.WID = UIDMatch.Groups[1].Value;
            logger.Info($"UID Found: {StaticData.BotData.UID} | WID Found: {StaticData.BotData.WID}");
         }
         if (FHXMatch.Success)
         {
            ForgeHX.ForgeHXURL = FHXMatch.Value;
            ForgeHX.FileName = "ForgeHX" + FHXMatch.Groups[1].Value;
            logger.Info($"ForgeHX-URL: {ForgeHX.ForgeHXURL}");
            ForgeHX.ForgeHXDownloaded += ForgeHX_ForgeHXDownloaded;
            ForgeHX.DownloadForge();
         }
      }
      private void CheckPremiumComplete(object sender, RunWorkerCompletedEventArgs e)
      {
         if (e.Result is bool)
         {
            bool.TryParse(e.Result.ToString(), out bool clearControls);
            if (!clearControls)
            {
               //Invoker.CallMethode(tlpPremium, () => tlpPremium.Controls.Clear());
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
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({i18n.getString("Premium")}) | by TH3C0D3R");
               object retList = ExecuteMethod(PremAssembly, "EntryPoint", "AddPremiumControl", null);
               if (retList is List<UCPremium> list)
               {
                  Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({i18n.getString("Premium")}) | by TH3C0D3R");
                  e.Result = true;
               }
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({i18n.getString("Expired")}) | by TH3C0D3R");
               if (!blockExpireBox)
               {
                  DialogResult dlgRes = MessageBox.Show(Owner, $"{i18n.getString("SubscriptionExpired")}", $"{i18n.getString("SubExpiredTitle")}", MessageBoxButtons.YesNo);
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
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R");
               MessageBox.Show(Owner, $"{i18n.getString("LicenceNotValid")}", $"{i18n.getString("FailedToActivate")}");
            }
         }
      }
      private void ForgeHX_ForgeHXDownloaded(object sender, EventArgs e)
      {
         LoadWorlds();
      }
      private void LoadWorlds()
      {
         logger.Info($">>> LoadWorlds()");
         ReqBuilder.User_Key = StaticData.BotData.UID;
         ReqBuilder.VersionSecret = StaticData.SettingData.Version_Secret;
         ReqBuilder.Version = StaticData.SettingData.Version;
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
         ReloadData();
      }
      public void ReloadData()
      {
         logger.Info($">>> ReloadData()");
         Updater.UpdatePlayerLists();
         Updater.UpdateStartUp();
         Updater.UpdateOwnTavern();
         Updater.UpdateInventory();
         UpdateGUI();
      }
      private void UpdateGUI()
      {
         logger.Info($">>> UpdateGUI()");
         UpdateDashbord();
         UpdateSocial();
         UpdateTavern();
         UpdateProductionView();
         UpdateGoodProductionView();
         UpdateHiddenRewardsView();
         //UpdateMessageCenter();
         //UpdateChat();
         //UpdateArmy();
      }
      private void UpdateDashbord()
      {
         if (GoodImageList != null)
         {
            try
            {
               Invoker.CallMethode(lvGoods, () => lvGoods.Items.Clear());
               Invoker.SetProperty(lvGoods, () => lvGoods.LargeImageList, GoodImageList);
               string lastEra = "";
               ListViewGroup group = null;
               foreach (KeyValuePair<string, List<Good>> item in ListClass.GoodsDict)
               {
                  if (item.Value.TrueForAll((b) => b.value == 0)) continue;
                  if (lastEra != item.Key)
                  {
                     group = new ListViewGroup(item.Key, HorizontalAlignment.Left);
                  }
                  foreach (Good good in item.Value)
                  {
                     ListViewItem lvi = new ListViewItem($"{good.name} ({good.value})", good.good_id);
                     lvi.Group = group;
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
         if(ListClass.Inventory.responseData.Count() > 0)
         {
            Items[] FPPackItems = ListClass.Inventory.responseData.ToList().FindAll(i => i.itemAssetName.EndsWith("forgepoints")).ToArray();
            if(FPPackItems.Count() > 0)
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
      }
      private void UpdateSocial()
      {
         #region "other Players"
         var friendMotivate = ListClass.FriendList.FindAll(f => (f.next_interaction_in == 0));
         var clanMotivate = ListClass.ClanMemberList.FindAll(f => (f.next_interaction_in == 0));
         var neighborlist = ListClass.NeighborList.FindAll(f => (f.next_interaction_in == 0));
         Invoker.SetProperty(lblFriends, () => lblFriends.Text, i18n.getString("Friends"));
         Invoker.SetProperty(lblClanMember, () => lblClanMember.Text, i18n.getString("Clanmembers"));
         Invoker.SetProperty(lblNeighbor, () => lblNeighbor.Text, i18n.getString("Neighbors"));
         Invoker.SetProperty(lblFriendsCount, () => lblFriendsCount.Text, $"{friendMotivate.Count}/{ListClass.FriendList.Count}");
         Invoker.SetProperty(lblClanMemberCount, () => lblClanMemberCount.Text, $"{clanMotivate.Count}/{ListClass.ClanMemberList.Count}");
         Invoker.SetProperty(lblNeighborCount, () => lblNeighborCount.Text, $"{neighborlist.Count}/{ListClass.NeighborList.Count}");
         Invoker.SetProperty(lblInactiveFriends, () => lblInactiveFriends.Text, i18n.getString("InactiveFriends"));

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
               TextAlign = ContentAlignment.BottomCenter,
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
               Text = i18n.getString("Remove"),
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
      private void UpdateTavern()
      {
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
      }
      private void RemoveFriend(object senderObj, EventArgs e)
      {
         Button sender = ((Button)senderObj);
         if (sender.Tag != null)
         {
            string playerid = sender.Tag.ToString();
            string script = ReqBuilder.GetRequestScript(RequestType.RemovePlayer, playerid);
            jsExecutor.ExecuteAsyncScript(script);
            Updater.UpdateFirends();
            UpdateSocial();
         }
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
         string script = ReqBuilder.GetRequestScript(RequestType.CollectTavern, "");
         var msg = (string)jsExecutor.ExecuteAsyncScript(script);
         if (msg == "{}") return;
         CollectResult cr = JsonConvert.DeserializeObject<CollectResult>(msg);
         if (cr.responseData.__class__.ToLower() == "success")
            ReloadData();
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
         bw.CancelAsync();
         while (!Canceled)
         {
            Application.DoEvents();
         };
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
            usi = new UserDataInput(ListClass.ServerList);
            usi.TopMost = true;
            usi.UserDataEntered += Usi_UserDataEntered;
            DialogResult dlgRes = usi.ShowDialog();
            if (dlgRes == DialogResult.Cancel) Environment.Exit(0);
            logger.Info($"userdata entered");
            mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
            return;
         }
         else
         {
            ContinueExecution();
            mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
         }
         var startEvent = new Dictionary<string, string>();
         string startUp = $"{Identifier.GetInfo(_WCS, _WCS_Model)}-{Identifier.GetInfo(_WCS, _WCS_SystemType)} ({Identifier.GetInfo(_WOS, _WOS_Caption)})";
         startEvent.Add("Startup", startUp);
         Analytics.TrackEvent("Startup", startEvent);

         var userPremiumEvent = new Dictionary<string, string>();
         string userPremium = $"{UserData.Username}({UserData.LastWorld.Split('|')[0]}) {UserData.SerialKey}";
         userPremiumEvent.Add(UserData.Username, userPremium);
         Analytics.TrackEvent("UserPremium", userPremiumEvent);
      }
      private void FrmMain_Shown(object sender, EventArgs e)
      {
         if (Program.LoadingFrm != null)
            Program.LoadingFrm.Close();
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
         if (tabControl1.SelectedTab.Tag.ToString() == "GUI.Social")
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
         }
         else if (tabControl1.SelectedTab.Tag.ToString() == "GUI.Tavern")
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
         }
         else if (tabControl1.SelectedTab.Tag.ToString() == "GUI.City")
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
         }
         else if (tabControl1.SelectedTab.Tag.ToString() == "GUI.Production")
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
         }
      }


      #region "Incident"
      private void UpdateHiddenRewardsView()
      {
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
      }
      private void TsmiCollectIncidentsToolStripMenuItem_Click(object sender, EventArgs e)
      {
         OneTArgs<RequestType> param = new OneTArgs<RequestType> { t1 = RequestType.CollectIncident };
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuterOneArg_DoWork;
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
      private void Pli_ProductionIdle(object sender, dynamic data)
      {
         ListClass.State = 0;
         string script = ReqBuilder.GetRequestScript(RequestType.GetEntities, "");
         string ret = (string)jsExecutor.ExecuteAsyncScript(script);
         dynamic entities = JsonConvert.DeserializeObject(ret);
         Updater.UpdateBuildings(entities["responseData"]);
         if (DEBUGMODE) Log($"[{DateTime.Now}] Idle STATE: {ListClass.State}", lbOutputWindow);
         update(ListClass.State);
      }
      private void Pli_ProductionDone(object sender, dynamic data)
      {
         ListClass.State = 1;
         string script = ReqBuilder.GetRequestScript(RequestType.GetEntities, "");
         string ret = (string)jsExecutor.ExecuteAsyncScript(script);
         dynamic entities = JsonConvert.DeserializeObject(ret);
         Updater.UpdateBuildings(entities["responseData"]);
         if (DEBUGMODE) Log($"[{DateTime.Now}] Done STATE: {ListClass.State}", lbOutputWindow);
         update(ListClass.State);
      }
      private void ProductionCollected()
      {
         if (!UserData.ProductionBot)
         {
            ListClass.CollectedIDs.Clear();
            string script = ReqBuilder.GetRequestScript(RequestType.GetEntities, "");
            string ret = (string)jsExecutor.ExecuteAsyncScript(script);
            dynamic entities = JsonConvert.DeserializeObject(ret);
            Updater.UpdateBuildings(entities["responseData"]);
            if (DEBUGMODE) Log($"[{DateTime.Now}] UpdateBuildings", lbOutputWindow);
            update(ListClass.State);
            ListClass.State = 2;
            return;
         }
      }
      private void ProductionStarted()
      {
         ListClass.State = 2;
         update(ListClass.State);
      }
      private void update(int sender)
      {
         UpdateProductionView();
         UpdateGoodProductionView();
         if (!UserData.ProductionBot) return;
         if (sender is int state)
         {
            if (DEBUGMODE) Log($"[{DateTime.Now}] STATE: {state}", lbOutputWindow);
            switch (state)
            {
               case 0:
                  if (DEBUGMODE) Log($"[{DateTime.Now}] Production Idle Event", lbOutputWindow);
                  StartProduction();
                  ListClass.State = 2;
                  break;
               case 1:
                  if (DEBUGMODE) Log($"[{DateTime.Now}] Production Done Event", lbOutputWindow);
                  CollectProduction();
                  ListClass.State = 0;
                  break;
               default:
                  break;
            }
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
                     List<int> CollectedIDs = new List<int>();
                     foreach (var item in ColRes["responseData"]?["updatedEntities"].ToList())
                     {
                        CollectedIDs.Add(int.Parse(item["id"].ToString()));
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
                     if (DEBUGMODE) Helper.Log($"[{DateTime.Now}] CollectedIDs Count = {CollectedIDs.Count}");
                     ListClass.CollectedIDs = CollectedIDs;
                  }
               }
               catch (Exception ex)
               {
                  NLog.LogManager.Flush();
                  var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
                  var properties = new Dictionary<string, string> { { "CollectProduction", ret } };
                  Crashes.TrackError(ex, properties, attachments);
               }
               ProductionCollected();
               break;
            case RequestType.QueryProduction:
               List<string> retQuery = new List<string>();
               if (ListClass.CollectedIDs.Count > 0)
               {
                  int[] Query = ListClass.CollectedIDs.ToArray();
                  ListClass.AddedToQuery.AddRange(ListClass.CollectedIDs);
                  foreach (int id in Query)
                  {
                     bool hasID = ListClass.GoodProductionList.Find(ex => ex.id == id) == null;
                     if (hasID)
                        script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.GoodProductionOption.id });
                     else
                        script = ReqBuilder.GetRequestScript(param.t1, new int[] { id, UserData.ProductionOption.id });
                     retQuery.Add((string)jsExecutor.ExecuteAsyncScript(script));
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
               ProductionStarted();
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
               UpdateProductionView();
               UpdateGoodProductionView();
               break;
            case RequestType.CollectIncident:
               UserData.LastIncidentTime = DateTime.Now;
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
                           Log($"{i18n.getString("IncidentCollected")} - {i18n.getString("Reward")}: {Reward.ToString()}", lbOutputWindow);
                        }
                     }
                     else if (ColIncRes["requestClass"].ToString() == "RewardService")
                     {
                        Reward = ColIncRes["responseData"][0][0]["name"];
                        if (successed)
                        {

                           Log($"{i18n.getString("IncidentCollected")} - {i18n.getString("Reward")}: {Reward.ToString()}", lbOutputWindow);
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
               break;
            default:
               break;
         }
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
         mtDarkMode.Checked = UserData.DarkMode;
         nudMinProfit.Value = UserData.MinProfit;
         mcbAutoInvest.Checked = UserData.AutoInvest;
         mtSnipBot.Checked = UserData.SnipBot;
         nudSnipInterval.Value = UserData.IntervalSnip;
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
            foreach (string item in UserData.PlayableWorlds)
            {
               mcbCitySelection.Items.Add(new PlayAbleWorldItem() { WorldID = item.Split('|')[0], WorldName = item.Split('|')[1] });
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
         Canceled = true;
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
         //UpdateBotView();
         UserData.SaveSettings();
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
         UserData.LastWorld = $"{((PlayAbleWorldItem)mcbCitySelection.SelectedItem).WorldID}|{((PlayAbleWorldItem)mcbCitySelection.SelectedItem).WorldName}";
         UserData.SaveSettings();

         string script = ReqBuilder.GetRequestScript(RequestType.switchWorld, UserData.LastWorld.Split('|')[0]);
         jsExecutor.ExecuteAsyncScript(script);
         driver.Navigate().GoToUrl($"https://{UserData.LastWorld.Split('|')[0]}.forgeofempires.com/game/index?");
         cookieJar = driver.Manage().Cookies;
         StaticData.BotData.CID = cookieJar.AllCookies.HasCookie("CID").Item2;
         StaticData.BotData.CSRF = cookieJar.AllCookies.HasCookie("CSRF").Item2;
         StaticData.BotData.SID = cookieJar.AllCookies.HasCookie("SID").Item2;
         StaticData.BotData.XSRF = cookieJar.AllCookies.HasCookie("XSRF-TOKEN").Item2;

         string loginJS = resMgr.GetString("preloadLoginWorld");
         Log("[DEBUG] Doing Login", lbOutputWindow);
         loginJS = loginJS
            .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
            .Replace("###USERNAME###", UserData.Username)
            .Replace("###PASSWORD###", UserData.Password)
            .Replace("##server##", UserData.WorldServer)
          .Replace("##t##", "false")
          .Replace("##city##", "\"" + UserData.LastWorld.Split('|')[0] + "\"");
         var x = jsExecutor.ExecuteAsyncScript(loginJS);
         if (x != null)
         {
            var ret = (string)x;
            driver.Navigate().GoToUrl(ret);
            cookieJar = driver.Manage().Cookies;
            jsExecutor = (IJavaScriptExecutor)driver;
            StaticData.BotData.CID = cookieJar.AllCookies.HasCookie("CID").Item2;
            StaticData.BotData.CSRF = cookieJar.AllCookies.HasCookie("CSRF").Item2;
            StaticData.BotData.SID = cookieJar.AllCookies.HasCookie("SID").Item2;
            StaticData.BotData.XSRF = cookieJar.AllCookies.HasCookie("XSRF-TOKEN").Item2;
            ForgeHX.ForgeHXLoaded = false;
            GetUIDAndForgeHX(driver.PageSource);
         }
         else
         {
            Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
         }
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

               mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({i18n.getString("Premium")}) | by TH3C0D3R";
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
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({i18n.getString("Expired")}) | by TH3C0D3R");
               if (!blockExpireBox)
                  MessageBox.Show(Owner, $"{i18n.getString("SubscriptionExpired")}", $"{i18n.getString("SubExpiredTitle")}");
               blockExpireBox = true;
            }
            else
            {
               mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
               MessageBox.Show(Owner, $"{i18n.getString("LicenceNotValid")}", $"{i18n.getString("FailedToActivate")}");
            }
         }
      }
      private void MtAutoLogin_CheckedChanged(object sender, EventArgs e)
      {
         UserData.AutoLogin = mtAutoLogin.Checked;
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
      }
      private void nudMinProfit_ValueChanged(object sender, EventArgs e)
      {
         UserData.MinProfit = (int)nudMinProfit.Value;
         UserData.SaveSettings();
      }
      private void McbAutoInvest_CheckedChanged(object sender, EventArgs e)
      {
         UserData.AutoInvest = mcbAutoInvest.Checked;
         UserData.SaveSettings();
      }
      private void mtSnipBot_CheckedChanged(object sender, EventArgs e)
      {
         UserData.SnipBot = mtSnipBot.Checked;
         UserData.SaveSettings();
      }
      private void nudSnipInterval_ValueChanged(object sender, EventArgs e)
      {
         UserData.IntervalSnip = (int)nudSnipInterval.Value;
         UserData.SaveSettings();
      }
      #endregion

      #region "Snip"
      private void MbSearch_Click(object sender, EventArgs e)
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
         else
            StaticData.WorkerList.BringToFront();
         StaticData.WorkerList.AddWorkerItem(wi);

         TwoTArgs<RequestType, object> param = new TwoTArgs<RequestType, object> { RequestType = RequestType.GetLGs, argument2 = null };
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuter_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerCompleted += workerComplete;
         bw.RunWorkerAsync(param);
         ListClass.BackgroundWorkers.Add(bw);
      }
      private void MbSnip_Click(object sender, EventArgs e)
      {
         foreach (Control c in mpSnipItem.Controls)
         {
            LGSnipItem lsi = (LGSnipItem)c;
            if (lsi.mcbSnip.Checked)
            {
               string script = ReqBuilder.GetRequestScript(RequestType.contributeForgePoints, new int[] { lsi.LGSnip.entity_id, lsi.LGSnip.player.player_id.Value, lsi.LGSnip.level, lsi.LGSnip.Invest, 0 });
               string ret = (string)jsExecutor.ExecuteAsyncScript(script);
               lsi.mcbSnip.Enabled = false;
               lsi.mcbSnip.Checked = false;
               lsi.mcbSnip.Text = i18n.getString("GUI.Sniper.SnipDone");
               Application.DoEvents();
            }
         }
      }
      #endregion

      #region "BackgroundWorker"
      private void bwScriptExecuter_DoWork(object sender, DoWorkEventArgs e)
      {
         Random r = new Random();
         int counter = 0;
         dynamic param;
         try
         {
            param = (TwoTArgs<RequestType, dynamic>)e.Argument;
         }
         catch (Exception)
         {
            param = (TwoTArgs<RequestType, E_Motivate>)e.Argument;
         }
         switch (param.RequestType)
         {
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
                     list.AddRange(ListClass.Motivateable);
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
                  string script = ReqBuilder.GetRequestScript(param.RequestType, item.player_id);
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
                  string script = ReqBuilder.GetRequestScript(param.RequestType, item.ownerId);
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
               string retSnip = "";
               ListClass.SnipWithProfit = new List<LGSnip>();
               ListClass.SnipablePlayers = new List<Player>();
               if (UserData.SelectedSnipTarget == SnipTarget.none) return;
               if (UserData.SelectedSnipTarget.HasFlag(SnipTarget.friends)) ListClass.SnipablePlayers.AddRange(ListClass.FriendList);
               if (UserData.SelectedSnipTarget.HasFlag(SnipTarget.neighbors)) ListClass.SnipablePlayers.AddRange(ListClass.NeighborList);
               if (UserData.SelectedSnipTarget.HasFlag(SnipTarget.members)) ListClass.SnipablePlayers.AddRange(ListClass.ClanMemberList);
               ListClass.SnipablePlayers = LG.HasGB(ListClass.SnipablePlayers);
               if (ListClass.SnipablePlayers.Count == 0) return;
               StaticData.WorkerList.UpdateWorkerProgressBar(LGSnipWorkerID, 0, ListClass.SnipablePlayers.Count);
               StaticData.WorkerList.UpdateWorkerLabel(LGSnipWorkerID, i18n.getString("CountLabel").Replace("##Done##", "0").Replace("##End##", ListClass.SnipablePlayers.Count.ToString()));
               int rInt = 0;
               foreach (Player item in ListClass.SnipablePlayers)
               {
                  counter += 1;
                  StaticData.WorkerList.UpdateWorkerProgressBar(LGSnipWorkerID, counter, ListClass.SnipablePlayers.Count);
                  StaticData.WorkerList.UpdateWorkerLabel(LGSnipWorkerID, $"{i18n.getString("GUI.Sniper.SearchingPlayer")} {item.name}...");
                  string script = ReqBuilder.GetRequestScript(RequestType.GetLGs, item.player_id);
                  retSnip = (string)jsExecutor.ExecuteAsyncScript(script);
                  if (retSnip.Length <= 4) break;
                  LGRootObject lgs = JsonConvert.DeserializeObject<LGRootObject>(retSnip);
                  List<LGSnip> LGData = lgs.responseData.ToList();
                  if (LGData.Count == 0) break;
                  ListClass.SnipablePlayers.Find(p => p.player_id == LGData[0].player.player_id).LGs = LGData;
                  foreach (LGSnip lg in item.LGs)
                  {
                     StaticData.WorkerList.UpdateWorkerProgressBar(LGSnipWorkerID, counter, ListClass.SnipablePlayers.Count);
                     StaticData.WorkerList.UpdateWorkerLabel(LGSnipWorkerID, $"{i18n.getString("GUI.Sniper.SearchingLG").Replace("##Player##", item.name)} {lg.name}...");

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
                                 invested_forge_points = (int?)resItem["responseData"][0]["state"]["invested_forge_points"]
                              };
                              lg.maxLevel = (int)resItem["responseData"][0]["max_level"];
                           }
                           else if (resItem["requestMethod"] == "getConstruction")
                           {
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

                                 FPRewards[Rank] = (int)Math.Round((double)FPNettoRewards[Rank] * arc);
                                 BPRewards[Rank] = (int)Math.Round((double)BPRewards[Rank] * arc);
                                 MedalRewards[Rank] = (int)Math.Round((double)MedalRewards[Rank] * arc);
                                 ForderFPRewards[Rank] = (int)Math.Round((double)FPNettoRewards[Rank] * ForderArc);

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
                                    SnipeRankCosts[Rank] = (int)Math.Round((double)(Einzahlungen[Rank] + RestFP) / 2);
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
                                 lg.KurzString = $"{(int)((float)FPRewards[RankProfit] / SnipCost * 100)} %";
                                 lg.GewinnString = $"{BestGewinn}";
                                 lg.Invest = SnipCost;
                                 ListClass.SnipWithProfit.Add(lg);
                              }
                              if (ListClass.AllLGs.Find(k => k.entity_id == lg.entity_id && k.player.player_id == lg.player.player_id) != null) continue;
                              ListClass.AllLGs.Add(lg);
                           }
                        }
                        rInt = r.Next(333, 500);
                        Thread.Sleep(rInt);
                     }
                  }
                  rInt = r.Next(333, 500);
                  Thread.Sleep(rInt);
               }
               Invoker.CallMethode(mpSnipItem, () => mpSnipItem.Controls.Clear());
               foreach (LGSnip item in ListClass.SnipWithProfit)
               {
                  LGSnipItem lsi = new LGSnipItem()
                  {
                     LG = $"{item.player.name} -> {item.name} ({item.level})",
                     LGSnip = item,
                     Profit = $"{item.Invest} (+{item.GewinnString})"
                  };
                  lsi.mcbSnip.Text = i18n.getString(lsi.mcbSnip.Tag.ToString());
                  lsi.Dock = DockStyle.Top;
                  Invoker.CallMethode(mpSnipItem, () => mpSnipItem.Controls.Add(lsi));
                  Invoker.SetProperty(mbSnip, () => mbSnip.Enabled, true);
               }
               Invoker.CallMethode(mpSnipItem, () => mpSnipItem.Update());
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
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuter_DoWork;
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
         BackgroundWorker bw = new BackgroundWorker();
         bw.DoWork += bwScriptExecuter_DoWork;
         bw.WorkerSupportsCancellation = true;
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

   }
}
