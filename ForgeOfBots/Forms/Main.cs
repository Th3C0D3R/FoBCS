using CefSharp;
using CefSharp.OffScreen;
//using CefSharp.WinForms;
using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.DataHandler;
using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static CefSharp.LogSeverity;
using static ForgeOfBots.Utils.Helper;
using static System.Windows.Forms.ListViewItem;
using WebClient = System.Net.WebClient;
using Settings = ForgeOfBots.Utils.Settings;
using WorldSelection = ForgeOfBots.Forms.WorldSelection;
using CUpdate = ForgeOfBots.DataHandler.Update;

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
      public static SettingData SettingData = new SettingData();
      public static Settings UserData;
      UserDataInput usi = null;
      public static Browser Browser = null;
      public static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      public static string ProgramPath = Path.Combine(AppDataPath, Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().ManifestModule.Name));
      public static Stopwatch RunningTime = new Stopwatch();
      public int CurrentState = 0;
      public string ForgeHX_FilePath = "";
      public bool LoginLoaded = false, ForgeHXLoaded = false, UIDLoaded = false, SECRET_LOADED = false;

      private bool blockedLogin = false;

      public Main()
      {
         InitializeComponent();
         RunningTime.Start();
         bwTimerUpdate.RunWorkerAsync();
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
         Invoker.SetProperty(pnlLoading, () => pnlLoading.Visible, true);
         ResponseHandler.EverythingImportantLoaded += ResponseHandler_EverythingImportantLoaded;
         Updater = new CUpdate(cwb, ReqBuilder);
      }
      private void ResponseHandler_EverythingImportantLoaded(object sender)
      {
         LoadGUI();
         Invoker.SetProperty(pnlLoading, () => pnlLoading.Visible, false);
      }
      private void ContinueExecution()
      {
         cwb = new ChromiumWebBrowser("https://" + UserData.WorldServer + ".forgeofempires.com/")
         {
            RequestHandler = new CustomRequestHandler(),
            MenuHandler = new CustomMenu(),
            //Dock = DockStyle.Fill,
         };
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
         cwb.FrameLoadEnd += Cwb_FrameLoadEnd;
         //Browser.Controls.Add(cwb);
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
            if (!blockedLogin)
               Invoker.SetProperty(lblPleaseLogin, () => lblPleaseLogin.Text, "Please Login now...");
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
         //pnlLoading.Visible = true;
         if (!blockedLogin)
         {
            Invoker.SetProperty(lblPleaseLogin, () => lblPleaseLogin.Text, "Please wait (again)!\nLoggin in...");
            //cwb.ShowDevTools();
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
      private void Main_Load(object sender, EventArgs e)
      {
         foreach (ColumnHeader column in listView1.Columns)
         {
            column.Width = (listView1.Width - 10) / listView1.Columns.Count;
         }
      }
      private void Main_FormClosed(object sender, FormClosedEventArgs e)
      {
         Log("[DEBUG] Closing App");
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
         Updater.UpdatePlayerLists();
         Updater.UpdateStartUp();
         CUpdate.UpdateFinished += CUpdate_UpdateFinished;
      }
      private void CUpdate_UpdateFinished(RequestType type)
      {
         switch (type)
         {
            case RequestType.Startup:
               UpdateOverView();
               break;
            case RequestType.Motivate:
               break;
            case RequestType.CollectIncident:
               break;
            case RequestType.VisitTavern:
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
      private void OnFriendRemoved(object sender)
      {
         Updater.UpdateFirends();
      }
      private void UpdateOverView()
      {
         #region "Overview"
         Invoker.SetProperty(lblCurValue, () => lblCurValue.Text, ListClass.WorldList.Find(x => x.Item3 == WorldState.current).Item2);
         Invoker.SetProperty(lblPlayerValue, () => lblPlayerValue.Text, UserData.Username);
         Invoker.SetProperty(lblSuppliesValue, () => lblSuppliesValue.Text, ListClass.Resources.supplies.ToString("N0"));
         Invoker.SetProperty(lblMoneyValue, () => lblMoneyValue.Text, ListClass.Resources.money.ToString("N0"));
         Invoker.SetProperty(lblDiaValue, () => lblDiaValue.Text, ListClass.Resources.premium.ToString("N0"));
         Invoker.SetProperty(lblMedsValue, () => lblMedsValue.Text, ListClass.Resources.medals.ToString("N0"));
         if(ListClass.ResourceDefinitions.Count > 0)
         {
            Invoker.SetProperty(lblMoney, () => lblMoney.Text, ListClass.ResourceDefinitions.Find(x => x.id == "money").name);
            Invoker.SetProperty(lblSupplies, () => lblSupplies.Text, ListClass.ResourceDefinitions.Find(x => x.id == "supplies").name);
            Invoker.SetProperty(lblMeds, () => lblMeds.Text, ListClass.ResourceDefinitions.Find(x => x.id == "medals").name);
            Invoker.SetProperty(lblDiamonds, () => lblDiamonds.Text, ListClass.ResourceDefinitions.Find(x => x.id == "premium").name);
         }
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
               Invoker.CallMethode(listView1, () => listView1.Items.Add(lvi));
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
         Invoker.SetProperty(lblFriends, () => lblFriends.Text, "Friends");
         Invoker.SetProperty(lblClanMember, () => lblClanMember.Text, "Clanmemebrs");
         Invoker.SetProperty(lblNeighbor, () => lblNeighbor.Text, "Neighbors");
         Invoker.SetProperty(lblFriendsCount, () => lblFriendsCount.Text, $"{friendMotivate.Count}/{ListClass.FriendList.Count}");
         Invoker.SetProperty(lblClanMemberCount, () => lblClanMemberCount.Text, $"{clanMotivate.Count}/{ListClass.ClanMemberList.Count}");
         Invoker.SetProperty(lblNeighborCount, () => lblNeighborCount.Text, $"{neighborlist.Count}/{ListClass.NeighborList.Count}");
         Invoker.SetProperty(lblInactiveFriends, () => lblInactiveFriends.Text, "Inactive Friends");

         var inactiveFriends = ListClass.FriendList.FindAll(f => (f.is_active == false && f.is_self == false));
         int row = 0;
         Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Clear());
         Invoker.SetProperty(tlpInactiveFriends, () => tlpInactiveFriends.RowCount, inactiveFriends.Count);
         foreach (var item in inactiveFriends)
         {
            int col = 0;
            Label lblName = new Label();
            lblName.Dock = DockStyle.Top;
            lblName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15, System.Drawing.FontStyle.Regular);
            lblName.Text = item.name;
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Add(lblName));
            col += 1;
            Label lblScore = new Label();
            lblScore.Dock = DockStyle.Top;
            lblScore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 15, System.Drawing.FontStyle.Regular);
            lblScore.Text = item.score.ToString("N0");
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Add(lblScore));
            col += 1;
            Button btnRemove = new Button();
            btnRemove.Tag = item.player_id;
            btnRemove.Text = "Remove";
            btnRemove.Dock = DockStyle.Top;
            btnRemove.AutoSize = true;
            btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 15, System.Drawing.FontStyle.Regular);
            btnRemove.Click += RemoveFriend;
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.Controls.Add(btnRemove));
            row += 1;
            Invoker.CallMethode(tlpInactiveFriends, () => tlpInactiveFriends.RowStyles.Add(new RowStyle(SizeType.AutoSize)));
         }

         #endregion
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
