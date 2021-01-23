using ForgeOfBots.DataHandler;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.LanguageFiles;
using ForgeOfBots.Utils;
using ForgeOfBots.Utils.Premium;
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
using static ForgeOfBots.Utils.StaticData;
using AllWorlds = ForgeOfBots.GameClasses.ResponseClasses.WorldSelection;
using CUpdate = ForgeOfBots.DataHandler.Update;
using WebClient = System.Net.WebClient;

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

         FirewallHelper.OpenFirewallPort(4444,"WebDrive");
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
               i18n.Initialize(UserData.Language.Code);
            }
            else
            {
               logger.Info($"changing language to default 'en'");
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en");
               i18n.Initialize("en");
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
               i18n.Initialize(UserData.Language.Code);
            }
            else
            {
               logger.Info($"changing language to default 'en'");
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en");
               i18n.Initialize("en");
            }
            Controls.Clear();
            logger.Info($"check available languages");
            Task.Factory.StartNew(CheckLanguages).Wait();
            InitializeComponent();
            FillText();
            logger.Info($"check for updates");
            CheckForUpdate();
            if (HasLastCrash)
            {

               logger.Info($"request send last crash");
               if (UserData.AllowSendCrashLog == UserConfirmation.Send)
                  CrashHelper.WaitForUserConfirmation(false);
               else if (UserData.AllowSendCrashLog == UserConfirmation.AlwaysSend)
                  CrashHelper.WaitForUserConfirmation(true);
               else if (UserData.AllowSendCrashLog == UserConfirmation.DontSend)
                  Crashes.SetEnabledAsync(false).Wait();
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
         usi.Close();
         ContinueExecution();
      }
      private void ContinueExecution()
      {
         ChromeOptions co = new ChromeOptions()
         {
            AcceptInsecureCertificates = true
         };
         co.AddArguments("--headless",
            "--disable-extensions",
            "--disable-breakpad",
            "--disable-component-update",
            "--disable-hang-monitor",
            "--disable-logging",
            "--disable-print-preview",
            "--disable-metrics-reporting",
            "--disable-dev-tools",
            "--ssl-version-min=tl",
            "--no-sandbox",
            "--remote-debugging-port=9222",
            "--disable-dev-shm-usage",
            "--window-position=-32000,-32000",
            "--disable-metrics");
         co.AddArgument($"user-agent={UserData.CustomUserAgent}");
         var chromeDriverService = ChromeDriverService.CreateDefaultService();
         chromeDriverService.HideCommandPromptWindow = true; 
         //driver = new RemoteWebDriver(new Uri("http://134.255.216.102:4444/"), co.ToCapabilities(),TimeSpan.FromSeconds(60));
         driver = new ChromeDriver(chromeDriverService, co);
         driver.Navigate().GoToUrl($"https://{UserData.WorldServer}0.forgeofempires.com/");
         driver.Manage().Window.Minimize();
         cookieJar = driver.Manage().Cookies;
         jsExecutor = (IJavaScriptExecutor)driver;
         StaticData.BotData.CID = cookieJar.AllCookies.HasCookie("CID").Item2;
         StaticData.BotData.CSRF = cookieJar.AllCookies.HasCookie("CSRF").Item2;
         StaticData.BotData.SID = cookieJar.AllCookies.HasCookie("SID").Item2;
         StaticData.BotData.XSRF = cookieJar.AllCookies.HasCookie("XSRF-TOKEN").Item2;

         GoodImageExtractor.GetGoodImages(UserData.WorldServer);

         if (!string.IsNullOrWhiteSpace(UserData.SerialKey))
         {
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
         if (UserData.LastWorld == null || string.IsNullOrWhiteSpace(UserData.LastWorld))
            loginCity = "true";
         else
            cityID = UserData.LastWorld;
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
             .Replace("##city##", "\"" + UserData.LastWorld + "\"");
            var x = jsExecutor.ExecuteAsyncScript(loginJS);
            var ret = (string)x;
            driver.Navigate().GoToUrl(ret);
            GetUIDAndForgeHX(driver.PageSource);
         }
         else if (loginCity == "true")
         {
            loginJS = resMgr.GetString("preloadLoginWorld");
            loginJS = loginJS
               .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
               .Replace("###USERNAME###", UserData.Username)
               .Replace("###PASSWORD###", UserData.Password)
               .Replace("##server##", UserData.WorldServer)
               .Replace("##t##", loginCity)
               .Replace("##city##", "\"" + UserData.LastWorld + "\"");
            var ret = (string)jsExecutor.ExecuteAsyncScript(loginJS);

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
            ws.ShowDialog();
         }
         else
         {
            loginJS = resMgr.GetString("preloadLoginWorld");
            loginJS = loginJS
                  .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
                  .Replace("###USERNAME###", UserData.Username)
                  .Replace("###PASSWORD###", UserData.Password)
                  .Replace("##server##", UserData.WorldServer)
               .Replace("##city##", "\"" + UserData.LastWorld + "\"")
               .Replace("##t##", "false");
            var ret = (string)jsExecutor.ExecuteAsyncScript(loginJS);
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
         UserData.LastWorld = key;
         string loginJS = resMgr.GetString("preloadLoginWorld");
         loginJS = loginJS
            .Replace("###XSRF-TOKEN###", StaticData.BotData.XSRF)
               .Replace("###USERNAME###", UserData.Username)
               .Replace("###PASSWORD###", UserData.Password)
               .Replace("##server##", UserData.WorldServer)
               .Replace("##city##", "\"" + UserData.LastWorld + "\"")
            .Replace("##t##", "false");
         var ret = (string)jsExecutor.ExecuteAsyncScript(loginJS);
         that.Close();
         GetUIDAndForgeHX(driver.PageSource);
      }
      private void GetUIDAndForgeHX(string source)
      {
         var regExUserID = new Regex(@"https:\/\/(\w{1,2}\d{1,2})\.forgeofempires\.com\/game\/json\?h=(.+)'", RegexOptions.IgnoreCase);
         var regExForgeHX = new Regex(@"https:\/\/foe\w{1,4}\.innogamescdn\.com\/\/cache\/ForgeHX(.+.js)'", RegexOptions.IgnoreCase);
         var FHXMatch = regExForgeHX.Match(source);
         var UIDMatch = regExUserID.Match(source);
         if (UIDMatch.Success)
         {
            StaticData.BotData.UID = UIDMatch.Groups[2].Value;
            StaticData.BotData.WID = UIDMatch.Groups[1].Value;
         }
         if (FHXMatch.Success)
         {
            ForgeHX.ForgeHXURL = FHXMatch.Value;
            ForgeHX.FileName = "ForgeHX" + FHXMatch.Groups[1].Value;
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
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({strings.Premium}) | by TH3C0D3R");
               object retList = ExecuteMethod(PremAssembly, "EntryPoint", "AddPremiumControl", null);
               if (retList is List<UCPremium> list)
               {
                  Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({strings.Premium}) | by TH3C0D3R");
                  e.Result = true;
               }
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(mlVersion, () => mlVersion.Text, mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({strings.Expired}) | by TH3C0D3R");
               if (!blockExpireBox)
               {
                  DialogResult dlgRes = MessageBox.Show(Owner, $"{strings.SubscriptionExpired}", $"{strings.SubExpiredTitle}", MessageBoxButtons.YesNo);
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
               MessageBox.Show(Owner, $"{strings.LicenceNotValid}", $"{strings.FailedToActivate}");
            }
         }
      }
      private void ForgeHX_ForgeHXDownloaded(object sender, EventArgs e)
      {
         LoadWorlds();
      }
      private void LoadWorlds()
      {
         ReqBuilder.User_Key = StaticData.BotData.UID;
         ReqBuilder.VersionSecret = StaticData.SettingData.Version_Secret;
         ReqBuilder.Version = StaticData.SettingData.Version;
         ReqBuilder.WorldID = StaticData.BotData.WID;
         string script = ReqBuilder.GetRequestScript(RequestType.GetAllWorlds, "[]");
         var ret = (string)jsExecutor.ExecuteAsyncScript(script);
         Root<AllWorlds> ws = JsonConvert.DeserializeObject<Root<AllWorlds>>(ret);
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
         Updater.UpdatePlayerLists();
         Updater.UpdateStartUp();
         Updater.UpdateOwnTavern();
         UpdateGUI();
      }
      private void UpdateGUI()
      {
         UpdateDashbord();
         UpdateSocial();
         UpdateTavern();
         //UpdateMessageCenter();
         //UpdateChat();
         //UpdateArmy();
         //UpdateProduction();
         //UpdateCity();
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
            Invoker.SetProperty(lblUserData, () => lblUserData.Text, $"{UserData.Username} {ListClass.WorldList.Find(w => w.Item1 == UserData.LastWorld).Item2}({UserData.LastWorld})");
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
            Invoker.SetProperty(lblMoney, () => lblMoney.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "money")["name"].ToString());
            Invoker.SetProperty(lblSupplies, () => lblSupplies.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "supplies")["name"].ToString());
            Invoker.SetProperty(lblMeds, () => lblMeds.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "medals")["name"].ToString());
            Invoker.SetProperty(lblDiamonds, () => lblDiamonds.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "premium")["name"].ToString());
            Invoker.SetProperty(lblFP, () => lblFP.Text, ListClass.ResourceDefinitions["responseData"].First(x => x["id"].ToString() == "strategy_points")["name"].ToString());
         }
      }
      private void UpdateSocial()
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
#if DEBUG
         Close();
#else
         Environment.Exit(0);
#endif
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
            usi.UserDataEntered += Usi_UserDataEntered;
            DialogResult dlgRes = usi.ShowDialog();
            if (dlgRes == DialogResult.Cancel) Environment.Exit(0);
            mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
            return;
         }
         else
         {
            ContinueExecution();
            mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
         }
#if DEBUG
         var x = new Dictionary<string, string>();
         string startUp = $"{Identifier.GetInfo(_WCS, _WCS_Model)}-{Identifier.GetInfo(_WCS, _WCS_SystemType)} ({Identifier.GetInfo(_WOS, _WOS_Caption)})";
         x.Add("Startup", startUp);
         Analytics.TrackEvent("Startup", x);
#elif RELEASE
        var startEvent = new Dictionary<string, string>();
        string startUp = $"{Identifier.GetInfo(_WCS, _WCS_Model)}-{Identifier.GetInfo(_WCS, _WCS_SystemType)} ({Identifier.GetInfo(_WOS, _WOS_Caption)})";
        startEvent.Add("Startup", startUp);
        Analytics.TrackEvent("Startup", startEvent);

        var userPremiumEvent = new Dictionary<string, string>();
        string userPremium = $"{UserData.Username}({UserData.LastWorld}) {UserData.SerialKey}";
        userPremiumEvent.Add(UserData.Username, userPremium);
        Analytics.TrackEvent("UserPremium", userPremiumEvent);
#endif
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
      private void FillText()
      {
         tpDashbord.Text = i18n.getString(tpDashbord.Tag.ToString());
         tpSocial.Text = i18n.getString(tpSocial.Tag.ToString());
         tpMessageCenter.Text = i18n.getString(tpMessageCenter.Tag.ToString());
         tpChat.Text = i18n.getString(tpChat.Tag.ToString());
         tpArmy.Text = i18n.getString(tpArmy.Tag.ToString());
         tpProduction.Text = i18n.getString(tpProduction.Tag.ToString());
         tpCity.Text = i18n.getString(tpCity.Tag.ToString());
         tpSniper.Text = i18n.getString(tpSniper.Tag.ToString());
         tpTavern.Text = i18n.getString(tpTavern.Tag.ToString());
         tpSettings.Text = i18n.getString(tpSettings.Tag.ToString());
         gbLog.Text = i18n.getString(gbLog.Tag.ToString());
         gbStatistic.Text = i18n.getString(gbStatistic.Tag.ToString());
         gbGoods.Text = i18n.getString(gbGoods.Tag.ToString());
         mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
      }
   }
}
