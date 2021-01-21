
namespace ForgeOfBots.Forms
{
   partial class frmMain
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
         this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
         this.lblRunning = new System.Windows.Forms.Label();
         this.lblUserData = new System.Windows.Forms.Label();
         this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
         this.pbminimize = new System.Windows.Forms.PictureBox();
         this.pbFull = new System.Windows.Forms.PictureBox();
         this.pbCLose = new System.Windows.Forms.PictureBox();
         this.mlVersion = new MetroFramework.Controls.MetroLabel();
         this.mlTitle = new MetroFramework.Controls.MetroLabel();
         this.imgListTab = new System.Windows.Forms.ImageList(this.components);
         this.mpMenu = new MetroFramework.Controls.MetroPanel();
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tpDashbord = new System.Windows.Forms.TabPage();
         this.gbGoods = new System.Windows.Forms.GroupBox();
         this.lvGoods = new System.Windows.Forms.ListView();
         this.gbLog = new System.Windows.Forms.GroupBox();
         this.lbOutputWindow = new System.Windows.Forms.ListBox();
         this.gbStatistic = new System.Windows.Forms.GroupBox();
         this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
         this.lblDiaValue = new System.Windows.Forms.Label();
         this.lblSuppliesValue = new System.Windows.Forms.Label();
         this.lblSupplies = new System.Windows.Forms.Label();
         this.lblMeds = new System.Windows.Forms.Label();
         this.lblMedsValue = new System.Windows.Forms.Label();
         this.lblDiamonds = new System.Windows.Forms.Label();
         this.lblFP = new System.Windows.Forms.Label();
         this.lblFPValue = new System.Windows.Forms.Label();
         this.lblMoney = new System.Windows.Forms.Label();
         this.lblMoneyValue = new System.Windows.Forms.Label();
         this.tpSocial = new System.Windows.Forms.TabPage();
         this.panel5 = new System.Windows.Forms.Panel();
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.lblInactiveFriends = new System.Windows.Forms.Label();
         this.tlpInactiveFriends = new System.Windows.Forms.TableLayoutPanel();
         this.panel4 = new System.Windows.Forms.Panel();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.lblClanMemberCount = new System.Windows.Forms.Label();
         this.lblFriendsCount = new System.Windows.Forms.Label();
         this.lblClanMember = new System.Windows.Forms.Label();
         this.lblFriends = new System.Windows.Forms.Label();
         this.lblNeighbor = new System.Windows.Forms.Label();
         this.lblNeighborCount = new System.Windows.Forms.Label();
         this.tpMessageCenter = new System.Windows.Forms.TabPage();
         this.tpChat = new System.Windows.Forms.TabPage();
         this.tpArmy = new System.Windows.Forms.TabPage();
         this.tpProduction = new System.Windows.Forms.TabPage();
         this.tpCity = new System.Windows.Forms.TabPage();
         this.tpSniper = new System.Windows.Forms.TabPage();
         this.tpTavern = new System.Windows.Forms.TabPage();
         this.tpSettings = new System.Windows.Forms.TabPage();
         this.bwUptime = new System.ComponentModel.BackgroundWorker();
         this.panel6 = new System.Windows.Forms.Panel();
         this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
         this.lblTavernstateValue = new System.Windows.Forms.Label();
         this.lblTavernSilverValue = new System.Windows.Forms.Label();
         this.lblTavernstate = new System.Windows.Forms.Label();
         this.lblTavernSilver = new System.Windows.Forms.Label();
         this.lblVisitable = new System.Windows.Forms.Label();
         this.lblVisitableValue = new System.Windows.Forms.Label();
         this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
         this.btnCollect = new System.Windows.Forms.Button();
         this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
         this.lblCurSitting = new System.Windows.Forms.Label();
         this.tlpCurrentSittingPlayer = new System.Windows.Forms.TableLayoutPanel();
         this.metroPanel1.SuspendLayout();
         this.metroPanel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbminimize)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbFull)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbCLose)).BeginInit();
         this.mpMenu.SuspendLayout();
         this.tabControl1.SuspendLayout();
         this.tpDashbord.SuspendLayout();
         this.gbGoods.SuspendLayout();
         this.gbLog.SuspendLayout();
         this.gbStatistic.SuspendLayout();
         this.tableLayoutPanel8.SuspendLayout();
         this.tpSocial.SuspendLayout();
         this.panel5.SuspendLayout();
         this.tableLayoutPanel3.SuspendLayout();
         this.panel4.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.tpTavern.SuspendLayout();
         this.panel6.SuspendLayout();
         this.tableLayoutPanel4.SuspendLayout();
         this.tableLayoutPanel5.SuspendLayout();
         this.tableLayoutPanel7.SuspendLayout();
         this.SuspendLayout();
         // 
         // metroPanel1
         // 
         resources.ApplyResources(this.metroPanel1, "metroPanel1");
         this.metroPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.metroPanel1.Controls.Add(this.lblRunning);
         this.metroPanel1.Controls.Add(this.lblUserData);
         this.metroPanel1.Controls.Add(this.metroPanel2);
         this.metroPanel1.Controls.Add(this.mlVersion);
         this.metroPanel1.Controls.Add(this.mlTitle);
         this.metroPanel1.CustomBackground = true;
         this.metroPanel1.HorizontalScrollbarBarColor = true;
         this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
         this.metroPanel1.HorizontalScrollbarSize = 10;
         this.metroPanel1.Name = "metroPanel1";
         this.metroPanel1.VerticalScrollbarBarColor = true;
         this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
         this.metroPanel1.VerticalScrollbarSize = 10;
         this.metroPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MetroPanel1_MouseDown);
         // 
         // lblRunning
         // 
         resources.ApplyResources(this.lblRunning, "lblRunning");
         this.lblRunning.ForeColor = System.Drawing.Color.White;
         this.lblRunning.Name = "lblRunning";
         this.lblRunning.Tag = "GUI.Header.Running";
         this.lblRunning.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MetroPanel1_MouseDown);
         // 
         // lblUserData
         // 
         resources.ApplyResources(this.lblUserData, "lblUserData");
         this.lblUserData.ForeColor = System.Drawing.Color.White;
         this.lblUserData.Name = "lblUserData";
         this.lblUserData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MetroPanel1_MouseDown);
         // 
         // metroPanel2
         // 
         resources.ApplyResources(this.metroPanel2, "metroPanel2");
         this.metroPanel2.Controls.Add(this.pbminimize);
         this.metroPanel2.Controls.Add(this.pbFull);
         this.metroPanel2.Controls.Add(this.pbCLose);
         this.metroPanel2.CustomBackground = true;
         this.metroPanel2.ForeColor = System.Drawing.Color.Transparent;
         this.metroPanel2.HorizontalScrollbarBarColor = true;
         this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
         this.metroPanel2.HorizontalScrollbarSize = 10;
         this.metroPanel2.Name = "metroPanel2";
         this.metroPanel2.VerticalScrollbarBarColor = true;
         this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
         this.metroPanel2.VerticalScrollbarSize = 10;
         this.metroPanel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MetroPanel1_MouseDown);
         // 
         // pbminimize
         // 
         resources.ApplyResources(this.pbminimize, "pbminimize");
         this.pbminimize.Image = global::ForgeOfBots.Properties.Resources.minus;
         this.pbminimize.Name = "pbminimize";
         this.pbminimize.TabStop = false;
         this.pbminimize.Click += new System.EventHandler(this.Pbminimize_Click);
         // 
         // pbFull
         // 
         resources.ApplyResources(this.pbFull, "pbFull");
         this.pbFull.Image = global::ForgeOfBots.Properties.Resources.plus;
         this.pbFull.Name = "pbFull";
         this.pbFull.TabStop = false;
         this.pbFull.Click += new System.EventHandler(this.PbFull_Click);
         // 
         // pbCLose
         // 
         resources.ApplyResources(this.pbCLose, "pbCLose");
         this.pbCLose.Image = global::ForgeOfBots.Properties.Resources.error;
         this.pbCLose.Name = "pbCLose";
         this.pbCLose.TabStop = false;
         this.pbCLose.Click += new System.EventHandler(this.PbCLose_Click);
         // 
         // mlVersion
         // 
         this.mlVersion.CustomBackground = true;
         this.mlVersion.CustomForeColor = true;
         this.mlVersion.ForeColor = System.Drawing.Color.White;
         resources.ApplyResources(this.mlVersion, "mlVersion");
         this.mlVersion.Name = "mlVersion";
         this.mlVersion.Tag = "v.";
         this.mlVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MetroPanel1_MouseDown);
         // 
         // mlTitle
         // 
         this.mlTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.mlTitle.CustomBackground = true;
         this.mlTitle.CustomForeColor = true;
         this.mlTitle.FontSize = MetroFramework.MetroLabelSize.Tall;
         this.mlTitle.FontWeight = MetroFramework.MetroLabelWeight.Regular;
         this.mlTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(117)))), ((int)(((byte)(172)))));
         resources.ApplyResources(this.mlTitle, "mlTitle");
         this.mlTitle.Name = "mlTitle";
         this.mlTitle.Tag = "Forge of Bots";
         this.mlTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MetroPanel1_MouseDown);
         // 
         // imgListTab
         // 
         this.imgListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListTab.ImageStream")));
         this.imgListTab.TransparentColor = System.Drawing.Color.Transparent;
         this.imgListTab.Images.SetKeyName(0, "dashbord.png");
         this.imgListTab.Images.SetKeyName(1, "012-email.png");
         this.imgListTab.Images.SetKeyName(2, "360-users.png");
         this.imgListTab.Images.SetKeyName(3, "609803.png");
         this.imgListTab.Images.SetKeyName(4, "3500804.png");
         this.imgListTab.Images.SetKeyName(5, "4004044.png");
         this.imgListTab.Images.SetKeyName(6, "208-settings.png");
         this.imgListTab.Images.SetKeyName(7, "358-paper plane.png");
         this.imgListTab.Images.SetKeyName(8, "profits.png");
         this.imgListTab.Images.SetKeyName(9, "beer.png");
         // 
         // mpMenu
         // 
         resources.ApplyResources(this.mpMenu, "mpMenu");
         this.mpMenu.Controls.Add(this.tabControl1);
         this.mpMenu.HorizontalScrollbarBarColor = true;
         this.mpMenu.HorizontalScrollbarHighlightOnWheel = false;
         this.mpMenu.HorizontalScrollbarSize = 10;
         this.mpMenu.Name = "mpMenu";
         this.mpMenu.VerticalScrollbarBarColor = true;
         this.mpMenu.VerticalScrollbarHighlightOnWheel = false;
         this.mpMenu.VerticalScrollbarSize = 10;
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tpDashbord);
         this.tabControl1.Controls.Add(this.tpSocial);
         this.tabControl1.Controls.Add(this.tpTavern);
         this.tabControl1.Controls.Add(this.tpMessageCenter);
         this.tabControl1.Controls.Add(this.tpChat);
         this.tabControl1.Controls.Add(this.tpArmy);
         this.tabControl1.Controls.Add(this.tpProduction);
         this.tabControl1.Controls.Add(this.tpCity);
         this.tabControl1.Controls.Add(this.tpSniper);
         this.tabControl1.Controls.Add(this.tpSettings);
         resources.ApplyResources(this.tabControl1, "tabControl1");
         this.tabControl1.ImageList = this.imgListTab;
         this.tabControl1.Multiline = true;
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
         // 
         // tpDashbord
         // 
         this.tpDashbord.BackColor = System.Drawing.Color.Transparent;
         this.tpDashbord.Controls.Add(this.gbGoods);
         this.tpDashbord.Controls.Add(this.gbLog);
         this.tpDashbord.Controls.Add(this.gbStatistic);
         resources.ApplyResources(this.tpDashbord, "tpDashbord");
         this.tpDashbord.Name = "tpDashbord";
         this.tpDashbord.Tag = "GUI.Tab.Dashbord";
         // 
         // gbGoods
         // 
         resources.ApplyResources(this.gbGoods, "gbGoods");
         this.gbGoods.Controls.Add(this.lvGoods);
         this.gbGoods.Name = "gbGoods";
         this.gbGoods.TabStop = false;
         this.gbGoods.Tag = "GUI.Tab.Dashbord.Goods";
         // 
         // lvGoods
         // 
         this.lvGoods.BackColor = System.Drawing.SystemColors.Control;
         this.lvGoods.BorderStyle = System.Windows.Forms.BorderStyle.None;
         resources.ApplyResources(this.lvGoods, "lvGoods");
         this.lvGoods.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.lvGoods.HideSelection = false;
         this.lvGoods.MultiSelect = false;
         this.lvGoods.Name = "lvGoods";
         this.lvGoods.UseCompatibleStateImageBehavior = false;
         // 
         // gbLog
         // 
         resources.ApplyResources(this.gbLog, "gbLog");
         this.gbLog.Controls.Add(this.lbOutputWindow);
         this.gbLog.Name = "gbLog";
         this.gbLog.TabStop = false;
         this.gbLog.Tag = "GUI.Tab.Dashbord.Log";
         // 
         // lbOutputWindow
         // 
         resources.ApplyResources(this.lbOutputWindow, "lbOutputWindow");
         this.lbOutputWindow.BackColor = System.Drawing.SystemColors.ControlLight;
         this.lbOutputWindow.FormattingEnabled = true;
         this.lbOutputWindow.Name = "lbOutputWindow";
         this.lbOutputWindow.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         // 
         // gbStatistic
         // 
         resources.ApplyResources(this.gbStatistic, "gbStatistic");
         this.gbStatistic.Controls.Add(this.tableLayoutPanel8);
         this.gbStatistic.Name = "gbStatistic";
         this.gbStatistic.TabStop = false;
         this.gbStatistic.Tag = "GUI.Tab.Dashbord.Statistic";
         // 
         // tableLayoutPanel8
         // 
         resources.ApplyResources(this.tableLayoutPanel8, "tableLayoutPanel8");
         this.tableLayoutPanel8.Controls.Add(this.lblDiaValue, 3, 1);
         this.tableLayoutPanel8.Controls.Add(this.lblSuppliesValue, 1, 0);
         this.tableLayoutPanel8.Controls.Add(this.lblSupplies, 0, 0);
         this.tableLayoutPanel8.Controls.Add(this.lblMeds, 2, 0);
         this.tableLayoutPanel8.Controls.Add(this.lblMedsValue, 3, 0);
         this.tableLayoutPanel8.Controls.Add(this.lblDiamonds, 2, 1);
         this.tableLayoutPanel8.Controls.Add(this.lblFP, 0, 2);
         this.tableLayoutPanel8.Controls.Add(this.lblFPValue, 1, 2);
         this.tableLayoutPanel8.Controls.Add(this.lblMoney, 0, 1);
         this.tableLayoutPanel8.Controls.Add(this.lblMoneyValue, 1, 1);
         this.tableLayoutPanel8.Name = "tableLayoutPanel8";
         // 
         // lblDiaValue
         // 
         resources.ApplyResources(this.lblDiaValue, "lblDiaValue");
         this.lblDiaValue.Name = "lblDiaValue";
         // 
         // lblSuppliesValue
         // 
         resources.ApplyResources(this.lblSuppliesValue, "lblSuppliesValue");
         this.lblSuppliesValue.Name = "lblSuppliesValue";
         // 
         // lblSupplies
         // 
         resources.ApplyResources(this.lblSupplies, "lblSupplies");
         this.lblSupplies.Name = "lblSupplies";
         // 
         // lblMeds
         // 
         resources.ApplyResources(this.lblMeds, "lblMeds");
         this.lblMeds.Name = "lblMeds";
         // 
         // lblMedsValue
         // 
         resources.ApplyResources(this.lblMedsValue, "lblMedsValue");
         this.lblMedsValue.Name = "lblMedsValue";
         // 
         // lblDiamonds
         // 
         resources.ApplyResources(this.lblDiamonds, "lblDiamonds");
         this.lblDiamonds.Name = "lblDiamonds";
         // 
         // lblFP
         // 
         resources.ApplyResources(this.lblFP, "lblFP");
         this.lblFP.Name = "lblFP";
         // 
         // lblFPValue
         // 
         resources.ApplyResources(this.lblFPValue, "lblFPValue");
         this.lblFPValue.Name = "lblFPValue";
         // 
         // lblMoney
         // 
         resources.ApplyResources(this.lblMoney, "lblMoney");
         this.lblMoney.Name = "lblMoney";
         // 
         // lblMoneyValue
         // 
         resources.ApplyResources(this.lblMoneyValue, "lblMoneyValue");
         this.lblMoneyValue.Name = "lblMoneyValue";
         // 
         // tpSocial
         // 
         this.tpSocial.Controls.Add(this.panel5);
         this.tpSocial.Controls.Add(this.panel4);
         resources.ApplyResources(this.tpSocial, "tpSocial");
         this.tpSocial.Name = "tpSocial";
         this.tpSocial.Tag = "GUI.Tab.Social";
         this.tpSocial.UseVisualStyleBackColor = true;
         // 
         // panel5
         // 
         resources.ApplyResources(this.panel5, "panel5");
         this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel5.Controls.Add(this.tableLayoutPanel3);
         this.panel5.Controls.Add(this.tlpInactiveFriends);
         this.panel5.Name = "panel5";
         // 
         // tableLayoutPanel3
         // 
         resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
         this.tableLayoutPanel3.Controls.Add(this.lblInactiveFriends, 0, 0);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         // 
         // lblInactiveFriends
         // 
         resources.ApplyResources(this.lblInactiveFriends, "lblInactiveFriends");
         this.lblInactiveFriends.Name = "lblInactiveFriends";
         // 
         // tlpInactiveFriends
         // 
         resources.ApplyResources(this.tlpInactiveFriends, "tlpInactiveFriends");
         this.tlpInactiveFriends.Name = "tlpInactiveFriends";
         // 
         // panel4
         // 
         resources.ApplyResources(this.panel4, "panel4");
         this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel4.Controls.Add(this.tableLayoutPanel1);
         this.panel4.Name = "panel4";
         // 
         // tableLayoutPanel1
         // 
         resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
         this.tableLayoutPanel1.Controls.Add(this.lblClanMemberCount, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.lblFriendsCount, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.lblClanMember, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblFriends, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblNeighbor, 2, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblNeighborCount, 2, 1);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         // 
         // lblClanMemberCount
         // 
         resources.ApplyResources(this.lblClanMemberCount, "lblClanMemberCount");
         this.lblClanMemberCount.Name = "lblClanMemberCount";
         // 
         // lblFriendsCount
         // 
         resources.ApplyResources(this.lblFriendsCount, "lblFriendsCount");
         this.lblFriendsCount.Name = "lblFriendsCount";
         // 
         // lblClanMember
         // 
         resources.ApplyResources(this.lblClanMember, "lblClanMember");
         this.lblClanMember.Name = "lblClanMember";
         // 
         // lblFriends
         // 
         resources.ApplyResources(this.lblFriends, "lblFriends");
         this.lblFriends.Name = "lblFriends";
         // 
         // lblNeighbor
         // 
         resources.ApplyResources(this.lblNeighbor, "lblNeighbor");
         this.lblNeighbor.Name = "lblNeighbor";
         // 
         // lblNeighborCount
         // 
         resources.ApplyResources(this.lblNeighborCount, "lblNeighborCount");
         this.lblNeighborCount.Name = "lblNeighborCount";
         // 
         // tpMessageCenter
         // 
         resources.ApplyResources(this.tpMessageCenter, "tpMessageCenter");
         this.tpMessageCenter.Name = "tpMessageCenter";
         this.tpMessageCenter.Tag = "GUI.Tab.MessageCenter";
         this.tpMessageCenter.UseVisualStyleBackColor = true;
         // 
         // tpChat
         // 
         resources.ApplyResources(this.tpChat, "tpChat");
         this.tpChat.Name = "tpChat";
         this.tpChat.Tag = "GUI.Tab.Chat";
         this.tpChat.UseVisualStyleBackColor = true;
         // 
         // tpArmy
         // 
         resources.ApplyResources(this.tpArmy, "tpArmy");
         this.tpArmy.Name = "tpArmy";
         this.tpArmy.Tag = "GUI.Tab.Army";
         this.tpArmy.UseVisualStyleBackColor = true;
         // 
         // tpProduction
         // 
         resources.ApplyResources(this.tpProduction, "tpProduction");
         this.tpProduction.Name = "tpProduction";
         this.tpProduction.Tag = "GUI.Tab.Production";
         this.tpProduction.UseVisualStyleBackColor = true;
         // 
         // tpCity
         // 
         resources.ApplyResources(this.tpCity, "tpCity");
         this.tpCity.Name = "tpCity";
         this.tpCity.Tag = "GUI.Tab.City";
         this.tpCity.UseVisualStyleBackColor = true;
         // 
         // tpSniper
         // 
         resources.ApplyResources(this.tpSniper, "tpSniper");
         this.tpSniper.Name = "tpSniper";
         this.tpSniper.Tag = "GUI.Tab.Sniper";
         this.tpSniper.UseVisualStyleBackColor = true;
         // 
         // tpTavern
         // 
         this.tpTavern.Controls.Add(this.tlpCurrentSittingPlayer);
         this.tpTavern.Controls.Add(this.tableLayoutPanel7);
         this.tpTavern.Controls.Add(this.tableLayoutPanel5);
         this.tpTavern.Controls.Add(this.panel6);
         resources.ApplyResources(this.tpTavern, "tpTavern");
         this.tpTavern.Name = "tpTavern";
         this.tpTavern.Tag = "GUI.Tab.Tavern";
         this.tpTavern.UseVisualStyleBackColor = true;
         // 
         // tpSettings
         // 
         resources.ApplyResources(this.tpSettings, "tpSettings");
         this.tpSettings.Name = "tpSettings";
         this.tpSettings.Tag = "GUI.Tab.Settings";
         this.tpSettings.UseVisualStyleBackColor = true;
         // 
         // bwUptime
         // 
         this.bwUptime.WorkerSupportsCancellation = true;
         this.bwUptime.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BwUptime_DoWork);
         // 
         // panel6
         // 
         resources.ApplyResources(this.panel6, "panel6");
         this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel6.Controls.Add(this.tableLayoutPanel4);
         this.panel6.Name = "panel6";
         // 
         // tableLayoutPanel4
         // 
         resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
         this.tableLayoutPanel4.Controls.Add(this.lblTavernstateValue, 1, 1);
         this.tableLayoutPanel4.Controls.Add(this.lblTavernSilverValue, 0, 1);
         this.tableLayoutPanel4.Controls.Add(this.lblTavernstate, 1, 0);
         this.tableLayoutPanel4.Controls.Add(this.lblTavernSilver, 0, 0);
         this.tableLayoutPanel4.Controls.Add(this.lblVisitable, 2, 0);
         this.tableLayoutPanel4.Controls.Add(this.lblVisitableValue, 2, 1);
         this.tableLayoutPanel4.Name = "tableLayoutPanel4";
         // 
         // lblTavernstateValue
         // 
         resources.ApplyResources(this.lblTavernstateValue, "lblTavernstateValue");
         this.lblTavernstateValue.Name = "lblTavernstateValue";
         // 
         // lblTavernSilverValue
         // 
         resources.ApplyResources(this.lblTavernSilverValue, "lblTavernSilverValue");
         this.lblTavernSilverValue.Name = "lblTavernSilverValue";
         // 
         // lblTavernstate
         // 
         resources.ApplyResources(this.lblTavernstate, "lblTavernstate");
         this.lblTavernstate.Name = "lblTavernstate";
         // 
         // lblTavernSilver
         // 
         resources.ApplyResources(this.lblTavernSilver, "lblTavernSilver");
         this.lblTavernSilver.Name = "lblTavernSilver";
         // 
         // lblVisitable
         // 
         resources.ApplyResources(this.lblVisitable, "lblVisitable");
         this.lblVisitable.Name = "lblVisitable";
         // 
         // lblVisitableValue
         // 
         resources.ApplyResources(this.lblVisitableValue, "lblVisitableValue");
         this.lblVisitableValue.Name = "lblVisitableValue";
         // 
         // tableLayoutPanel5
         // 
         resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
         this.tableLayoutPanel5.Controls.Add(this.btnCollect, 2, 0);
         this.tableLayoutPanel5.Name = "tableLayoutPanel5";
         // 
         // btnCollect
         // 
         resources.ApplyResources(this.btnCollect, "btnCollect");
         this.btnCollect.FlatAppearance.BorderColor = System.Drawing.Color.Black;
         this.btnCollect.FlatAppearance.BorderSize = 2;
         this.btnCollect.Name = "btnCollect";
         this.btnCollect.UseVisualStyleBackColor = true;
         this.btnCollect.Click += new System.EventHandler(this.btnCollect_Click);
         // 
         // tableLayoutPanel7
         // 
         resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
         this.tableLayoutPanel7.Controls.Add(this.lblCurSitting, 0, 0);
         this.tableLayoutPanel7.Name = "tableLayoutPanel7";
         // 
         // lblCurSitting
         // 
         resources.ApplyResources(this.lblCurSitting, "lblCurSitting");
         this.lblCurSitting.Name = "lblCurSitting";
         // 
         // tlpCurrentSittingPlayer
         // 
         resources.ApplyResources(this.tlpCurrentSittingPlayer, "tlpCurrentSittingPlayer");
         this.tlpCurrentSittingPlayer.Name = "tlpCurrentSittingPlayer";
         // 
         // frmMain
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.mpMenu);
         this.Controls.Add(this.metroPanel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "frmMain";
         this.Tag = "Forge of Bots v";
         this.Load += new System.EventHandler(this.FrmMain_Load);
         this.Shown += new System.EventHandler(this.FrmMain_Shown);
         this.metroPanel1.ResumeLayout(false);
         this.metroPanel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pbminimize)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbFull)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbCLose)).EndInit();
         this.mpMenu.ResumeLayout(false);
         this.tabControl1.ResumeLayout(false);
         this.tpDashbord.ResumeLayout(false);
         this.gbGoods.ResumeLayout(false);
         this.gbLog.ResumeLayout(false);
         this.gbStatistic.ResumeLayout(false);
         this.tableLayoutPanel8.ResumeLayout(false);
         this.tpSocial.ResumeLayout(false);
         this.panel5.ResumeLayout(false);
         this.tableLayoutPanel3.ResumeLayout(false);
         this.tableLayoutPanel3.PerformLayout();
         this.panel4.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.tpTavern.ResumeLayout(false);
         this.panel6.ResumeLayout(false);
         this.tableLayoutPanel4.ResumeLayout(false);
         this.tableLayoutPanel4.PerformLayout();
         this.tableLayoutPanel5.ResumeLayout(false);
         this.tableLayoutPanel7.ResumeLayout(false);
         this.tableLayoutPanel7.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private MetroFramework.Controls.MetroPanel metroPanel1;
      private MetroFramework.Controls.MetroLabel mlTitle;
      private System.Windows.Forms.PictureBox pbFull;
      private System.Windows.Forms.PictureBox pbminimize;
      private System.Windows.Forms.PictureBox pbCLose;
      private MetroFramework.Controls.MetroLabel mlVersion;
      private System.Windows.Forms.ImageList imgListTab;
      private MetroFramework.Controls.MetroPanel mpMenu;
      private System.Windows.Forms.TabControl tabControl1;
      private System.Windows.Forms.TabPage tpDashbord;
      private System.Windows.Forms.TabPage tpSocial;
      private System.Windows.Forms.TabPage tpMessageCenter;
      private System.Windows.Forms.TabPage tpChat;
      private System.Windows.Forms.TabPage tpArmy;
      private System.Windows.Forms.TabPage tpProduction;
      private System.Windows.Forms.TabPage tpCity;
      private System.Windows.Forms.TabPage tpSettings;
      private System.Windows.Forms.GroupBox gbStatistic;
      private MetroFramework.Controls.MetroPanel metroPanel2;
      private System.Windows.Forms.GroupBox gbLog;
      private System.Windows.Forms.GroupBox gbGoods;
      private System.ComponentModel.BackgroundWorker bwUptime;
      private System.Windows.Forms.ListBox lbOutputWindow;
      private System.Windows.Forms.ListView lvGoods;
      private System.Windows.Forms.TabPage tpSniper;
      private System.Windows.Forms.Label lblRunning;
      private System.Windows.Forms.Label lblUserData;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
      private System.Windows.Forms.Label lblDiaValue;
      private System.Windows.Forms.Label lblSuppliesValue;
      private System.Windows.Forms.Label lblSupplies;
      private System.Windows.Forms.Label lblMeds;
      private System.Windows.Forms.Label lblMedsValue;
      private System.Windows.Forms.Label lblDiamonds;
      private System.Windows.Forms.Label lblFP;
      private System.Windows.Forms.Label lblFPValue;
      private System.Windows.Forms.Label lblMoney;
      private System.Windows.Forms.Label lblMoneyValue;
      private System.Windows.Forms.Panel panel4;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label lblClanMemberCount;
      private System.Windows.Forms.Label lblFriendsCount;
      private System.Windows.Forms.Label lblClanMember;
      private System.Windows.Forms.Label lblFriends;
      private System.Windows.Forms.Label lblNeighbor;
      private System.Windows.Forms.Label lblNeighborCount;
      private System.Windows.Forms.Panel panel5;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
      private System.Windows.Forms.Label lblInactiveFriends;
      private System.Windows.Forms.TableLayoutPanel tlpInactiveFriends;
      private System.Windows.Forms.TabPage tpTavern;
      private System.Windows.Forms.Panel panel6;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
      private System.Windows.Forms.Label lblTavernstateValue;
      private System.Windows.Forms.Label lblTavernSilverValue;
      private System.Windows.Forms.Label lblTavernstate;
      private System.Windows.Forms.Label lblTavernSilver;
      private System.Windows.Forms.Label lblVisitable;
      private System.Windows.Forms.Label lblVisitableValue;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
      private System.Windows.Forms.Button btnCollect;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
      private System.Windows.Forms.Label lblCurSitting;
      private System.Windows.Forms.TableLayoutPanel tlpCurrentSittingPlayer;
   }
}