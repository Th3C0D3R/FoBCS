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
using System.Linq;

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
                CachePath = Path.GetFullPath("cache"),
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
                Invoker.SetProperty(pnlLoading, () => pnlLoading.Visible, true);
                ResponseHandler.EverythingImportantLoaded += ResponseHandler_EverythingImportantLoaded;
                Updater = new CUpdate(cwb, ReqBuilder);
                Name += Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return;
            }
            ContinueExecution();
            Browser.Hide();
            Invoker.SetProperty(pnlLoading, () => pnlLoading.Visible, true);
            ResponseHandler.EverythingImportantLoaded += ResponseHandler_EverythingImportantLoaded;
            Updater = new CUpdate(cwb, ReqBuilder);
            Name += Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
                {
                    Invoker.SetProperty(lblPleaseLogin, () => lblPleaseLogin.Text, "Please Login now...");
                    Invoker.SetProperty(pictureBox1, () => pictureBox1.Visible, false);
                }
            }
        }
        private void ServerStartupLoaded_Event(object sender, EventArgs e)
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
        private void UidFound_Event(object sender, TwoStringArgs args)
        {
            if (UIDLoaded) return;
            Log("[DEBUG] UID loaded");
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
            Log("[DEBUG] UserData saved");
        }
        private void ForgeHXFound_Event(object sender, TwoStringArgs args)
        {
            if (ForgeHXLoaded) return;
            Log("[DEBUG] Checking ForgeHX File");
            ForgeHXLoaded = true;
            BotData.ForgeHX = args.s2;
            ForgeHX_FilePath = Path.Combine(ProgramPath, args.s2);
            if (!Directory.Exists(ProgramPath)) Directory.CreateDirectory(ProgramPath);
            FileInfo fi = new FileInfo(ForgeHX_FilePath);
            Log("[DEBUG] Checking if ForgeHX exists");
            if (!fi.Exists || fi.Length <= 0)
            {
                Log("[DEBUG] Loading new ForgeHX");
                using (var client = new WebClient())
                {
                    Uri uri = new Uri(args.s1.Replace("'", ""));
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0");
                    client.DownloadProgressChanged += Client_DownloadProgressChanged;
                    client.DownloadFileCompleted += Client_DownloadFileCompleted;
                    BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = "Downloading " + args.s2));
                    BeginInvoke(new MethodInvoker(() => tspbProgress.Value = 0));
                    client.DownloadFileAsync(uri, ForgeHX_FilePath);
                }
            }
            else
            {
                Log("[DEBUG] Reading ForgeHX");
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
                Invoker.SetProperty(pictureBox1, () => pictureBox1.Visible, true);
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
                    break;
                case RequestType.Motivate:
                    break;
                case RequestType.CollectIncident:
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
            Invoker.SetProperty(lblSuppliesValue, () => lblSuppliesValue.Text, ListClass.Resources.supplies.ToString("N0"));
            Invoker.SetProperty(lblMoneyValue, () => lblMoneyValue.Text, ListClass.Resources.money.ToString("N0"));
            Invoker.SetProperty(lblDiaValue, () => lblDiaValue.Text, ListClass.Resources.premium.ToString("N0"));
            Invoker.SetProperty(lblMedsValue, () => lblMedsValue.Text, ListClass.Resources.medals.ToString("N0"));
            if (ListClass.ResourceDefinitions.Count > 0)
            {
                Invoker.SetProperty(lblMoney, () => lblMoney.Text, ListClass.ResourceDefinitions.Find(x => x.id == "money").name);
                Invoker.SetProperty(lblSupplies, () => lblSupplies.Text, ListClass.ResourceDefinitions.Find(x => x.id == "supplies").name);
                Invoker.SetProperty(lblMeds, () => lblMeds.Text, ListClass.ResourceDefinitions.Find(x => x.id == "medals").name);
                Invoker.SetProperty(lblDiamonds, () => lblDiamonds.Text, ListClass.ResourceDefinitions.Find(x => x.id == "premium").name);
            }
            Invoker.CallMethode(listView1, () => listView1.Clear());
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
            Invoker.SetProperty(lblTavernSilverValue, () => lblTavernSilverValue.Text, ListClass.Resources.tavern_silver.ToString("N0"));
            Invoker.SetProperty(lblTavernstate, () => lblTavernstate.Text, "Tavernstate");
            Invoker.SetProperty(lblVisitable, () => lblVisitable.Text, "Visitable");
            Invoker.SetProperty(btnCollect, () => btnCollect.Text, "Collect Tavern");
            Invoker.SetProperty(lblTavernstateValue, () => lblTavernstateValue.Text, ListClass.OwnTavern.responseData[2].ToString() + "/" + ListClass.OwnTavern.responseData[1].ToString());
            if (ListClass.ResourceDefinitions.Count > 0)
            {
                Invoker.SetProperty(lblTavernSilver, () => lblTavernSilver.Text, ListClass.ResourceDefinitions.Find(x => x.id == "tavern_silver").name);
            }

            var visitable = ListClass.FriendTaverns.FindAll(f => (f.sittingPlayerCount < f.unlockedChairCount && f.state != "alreadyVisited" && f.state != "isSitting"));
            Invoker.SetProperty(lblVisitableValue, () => lblVisitableValue.Text, visitable.Count.ToString());
            Invoker.SetProperty(lblCurSitting, () => lblCurSitting.Text, "Currently Sitting Players: ");
            if (ListClass.OwnTavernData.view != null)
            {
                var ownTavern = ListClass.OwnTavernData.view.visitors.ToList();
                int row = 0;
                Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Clear());
                Invoker.SetProperty(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.RowCount, ownTavern.Count);
                foreach (var item in ownTavern)
                {
                    int col = 0;
                    Label lblName = new Label();
                    lblName.Dock = DockStyle.Top;
                    lblName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                    lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15, System.Drawing.FontStyle.Regular);
                    lblName.Text = item.name;
                    Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(lblName));
                    col += 1;
                    Label lblScore = new Label();
                    lblScore.Dock = DockStyle.Top;
                    lblScore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                    lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 15, System.Drawing.FontStyle.Regular);
                    lblScore.Text = item.player_id.ToString();
                    Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(lblScore));
                    col += 1;

                    if (visitable.Exists(f => f.ownerId == item.player_id))
                    {
                        Button btnSitDown = new Button();
                        btnSitDown.Tag = item.player_id;
                        btnSitDown.Text = "Sit down";
                        btnSitDown.Dock = DockStyle.Top;
                        btnSitDown.AutoSize = true;
                        btnSitDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 15, System.Drawing.FontStyle.Regular);
                        btnSitDown.Click += SitAtTavern;
                        Invoker.CallMethode(tlpCurrentSittingPlayer, () => tlpCurrentSittingPlayer.Controls.Add(btnSitDown));
                    }
                    else
                    {
                        Label lnlSitState = new Label();
                        lnlSitState.Dock = DockStyle.Top;
                        lnlSitState.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                        lnlSitState.Font = new System.Drawing.Font("Microsoft Sans Serif", 15, System.Drawing.FontStyle.Regular);
                        lnlSitState.Text = "CAN NOT SIT DOWN";
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
            //string script = ReqBuilder.GetRequestScript(RequestType.Motivate, "[]");
            TwoTArgs<RequestType, E_Motivate> param = new TwoTArgs<RequestType, E_Motivate> { RequestType = RequestType.Motivate, argument2 = player_type };
            bwScriptExecuter.RunWorkerAsync(param);
        }

        private void VisitAllTavern()
        {
            //string script = ReqBuilder.GetRequestScript(RequestType.VisitTavern, "[]");
            TwoTArgs<RequestType, object> param = new TwoTArgs<RequestType, object> { RequestType = RequestType.VisitTavern, argument2 = null };
            bwScriptExecuter.RunWorkerAsync(param);
        }
        private void bwScriptExecuter_DoWork(object sender, DoWorkEventArgs e)
        {
            TwoTArgs<RequestType, object> param = (TwoTArgs<RequestType, object>)e.Argument;
            switch (param.RequestType)
            {
                case RequestType.Motivate:
                    E_Motivate e_Motivate = (E_Motivate)Enum.Parse(typeof(E_Motivate), param.argument2.ToString());
                    List<Player> list = new List<Player>();
                    Player last = null;
                    int steps = 100 / list.Count;
                    BeginInvoke(new MethodInvoker(() => tspbProgress.Value += steps));
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
                    foreach (Player item in list)
                    {
                        last = item;
                        string script = ReqBuilder.GetRequestScript(param.RequestType, item.player_id);
                        cwb.ExecuteScriptAsync(script);
                        Random r = new Random();
                        int rInt = r.Next(750, 1500);
                        while (!ListClass.doneMotivate.ContainsKey(last.player_id))
                        {
                            Thread.Sleep(1);
                        }
                        BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = $"Motivating {ListClass.doneMotivate.Count} / {list.Count}"));
                        Thread.Sleep(rInt);
                    }
                    ListClass.doneMotivate.Clear();
                    break;
                case RequestType.CollectIncident:
                    break;
                case RequestType.VisitTavern:
                    //return (undefined !== friend.taverninfo && undefined === friend.taverninfo["state"] && friend.taverninfo["sittingPlayerCount"] < friend.taverninfo["unlockedChairCount"])
                    FriendTavernState FTSlast = null;
                    ResponseHandler.TaverSitted -= ResponseHandler_TaverSitted;
                    ListClass.FriendTaverns = (List<FriendTavernState>)ListClass.FriendTaverns.Where((f) => f.sittingPlayerCount < f.unlockedChairCount);
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
                        BeginInvoke(new MethodInvoker(() => tsslProgressState.Text = $"Sitting at Tavern {ListClass.doneTavern.Count} / {ListClass.FriendTaverns.Count}"));
                        Thread.Sleep(rInt);
                    }
                    ResponseHandler.TaverSitted += ResponseHandler_TaverSitted;
                    break;
                case RequestType.CollectProduction:
                    break;
                case RequestType.QueryProduction:
                    break;
                case RequestType.CancelProduction:
                    break;
                default:
                    break;
            }
        }
        private void bwScriptExecuter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void btnCollect_Click(object sender, EventArgs e)
        {
            string script = ReqBuilder.GetRequestScript(RequestType.CollectTavern, "");
            ResponseHandler.TavernCollected += ResponseHandler_TavernCollected; ;
            cwb.ExecuteScriptAsync(script);
        }

        private void ResponseHandler_TavernCollected(object sender, dynamic data = null)
        { 
            reloadData();
        }

        private void bwScriptExecuter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

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
