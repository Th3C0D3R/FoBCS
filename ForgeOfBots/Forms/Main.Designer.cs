namespace ForgeOfBots
{
   partial class Main
   {
      /// <summary>
      /// Erforderliche Designervariable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Verwendete Ressourcen bereinigen.
      /// </summary>
      /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Vom Windows Form-Designer generierter Code

      /// <summary>
      /// Erforderliche Methode für die Designerunterstützung.
      /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
         this.sStripStatus = new System.Windows.Forms.StatusStrip();
         this.tsslProgressState = new System.Windows.Forms.ToolStripStatusLabel();
         this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
         this.toolStrip1 = new System.Windows.Forms.ToolStrip();
         this.tsbLogin = new System.Windows.Forms.ToolStripButton();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.tsbSettings = new System.Windows.Forms.ToolStripButton();
         this.tcMenu = new System.Windows.Forms.TabControl();
         this.tpOverview = new System.Windows.Forms.TabPage();
         this.panel3 = new System.Windows.Forms.Panel();
         this.lblCur = new System.Windows.Forms.Label();
         this.lblRunSince = new System.Windows.Forms.Label();
         this.lblCurValue = new System.Windows.Forms.Label();
         this.lblPlayerValue = new System.Windows.Forms.Label();
         this.lblPlayer = new System.Windows.Forms.Label();
         this.lblRunSinceValue = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.lblDiamonds = new System.Windows.Forms.Label();
         this.lblDiaValue = new System.Windows.Forms.Label();
         this.lblMoneyValue = new System.Windows.Forms.Label();
         this.lblMedsValue = new System.Windows.Forms.Label();
         this.lblMoney = new System.Windows.Forms.Label();
         this.lblSupplies = new System.Windows.Forms.Label();
         this.lblSuppliesValue = new System.Windows.Forms.Label();
         this.lblMeds = new System.Windows.Forms.Label();
         this.panel2 = new System.Windows.Forms.Panel();
         this.listView1 = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.tpOthers = new System.Windows.Forms.TabPage();
         this.tpTavern = new System.Windows.Forms.TabPage();
         this.tpBots = new System.Windows.Forms.TabPage();
         this.tpProduction = new System.Windows.Forms.TabPage();
         this.tpManually = new System.Windows.Forms.TabPage();
         this.bwTimerUpdate = new System.ComponentModel.BackgroundWorker();
         this.pnlLoading = new System.Windows.Forms.Panel();
         this.lblPleaseLogin = new System.Windows.Forms.Label();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.panel4 = new System.Windows.Forms.Panel();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.lblNeighbor = new System.Windows.Forms.Label();
         this.lblNeighborCount = new System.Windows.Forms.Label();
         this.lblFriends = new System.Windows.Forms.Label();
         this.lblClanMember = new System.Windows.Forms.Label();
         this.lblFriendsCount = new System.Windows.Forms.Label();
         this.lblClanMemberCount = new System.Windows.Forms.Label();
         this.panel5 = new System.Windows.Forms.Panel();
         this.tlpInactiveFriends = new System.Windows.Forms.TableLayoutPanel();
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.lblInactiveFriends = new System.Windows.Forms.Label();
         this.sStripStatus.SuspendLayout();
         this.toolStrip1.SuspendLayout();
         this.tcMenu.SuspendLayout();
         this.tpOverview.SuspendLayout();
         this.panel3.SuspendLayout();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.tpOthers.SuspendLayout();
         this.pnlLoading.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         this.panel4.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.panel5.SuspendLayout();
         this.tableLayoutPanel3.SuspendLayout();
         this.SuspendLayout();
         // 
         // sStripStatus
         // 
         this.sStripStatus.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.sStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslProgressState,
            this.tspbProgress});
         this.sStripStatus.Location = new System.Drawing.Point(0, 1136);
         this.sStripStatus.Name = "sStripStatus";
         this.sStripStatus.Size = new System.Drawing.Size(1464, 32);
         this.sStripStatus.TabIndex = 1;
         this.sStripStatus.Text = "statusStrip1";
         // 
         // tsslProgressState
         // 
         this.tsslProgressState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
         this.tsslProgressState.Name = "tsslProgressState";
         this.tsslProgressState.Size = new System.Drawing.Size(50, 25);
         this.tsslProgressState.Text = "IDLE";
         // 
         // tspbProgress
         // 
         this.tspbProgress.Name = "tspbProgress";
         this.tspbProgress.Size = new System.Drawing.Size(200, 24);
         this.tspbProgress.Step = 1;
         this.tspbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
         // 
         // toolStrip1
         // 
         this.toolStrip1.BackColor = System.Drawing.Color.Black;
         this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLogin,
            this.toolStripSeparator1,
            this.tsbSettings});
         this.toolStrip1.Location = new System.Drawing.Point(0, 0);
         this.toolStrip1.Name = "toolStrip1";
         this.toolStrip1.Size = new System.Drawing.Size(1464, 34);
         this.toolStrip1.TabIndex = 3;
         this.toolStrip1.Text = "toolStrip1";
         // 
         // tsbLogin
         // 
         this.tsbLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
         this.tsbLogin.ForeColor = System.Drawing.Color.White;
         this.tsbLogin.Image = ((System.Drawing.Image)(resources.GetObject("tsbLogin.Image")));
         this.tsbLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.tsbLogin.Name = "tsbLogin";
         this.tsbLogin.Size = new System.Drawing.Size(60, 29);
         this.tsbLogin.Tag = "login";
         this.tsbLogin.Text = "Login";
         this.tsbLogin.Click += new System.EventHandler(this.tsbLogin_Click);
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
         // 
         // tsbSettings
         // 
         this.tsbSettings.BackColor = System.Drawing.Color.Black;
         this.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
         this.tsbSettings.ForeColor = System.Drawing.Color.White;
         this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
         this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.tsbSettings.Name = "tsbSettings";
         this.tsbSettings.Size = new System.Drawing.Size(80, 33);
         this.tsbSettings.Text = "Settings";
         this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
         // 
         // tcMenu
         // 
         this.tcMenu.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
         this.tcMenu.Controls.Add(this.tpOverview);
         this.tcMenu.Controls.Add(this.tpOthers);
         this.tcMenu.Controls.Add(this.tpTavern);
         this.tcMenu.Controls.Add(this.tpBots);
         this.tcMenu.Controls.Add(this.tpProduction);
         this.tcMenu.Controls.Add(this.tpManually);
         this.tcMenu.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tcMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.tcMenu.Location = new System.Drawing.Point(0, 34);
         this.tcMenu.Name = "tcMenu";
         this.tcMenu.SelectedIndex = 0;
         this.tcMenu.Size = new System.Drawing.Size(1464, 1102);
         this.tcMenu.TabIndex = 4;
         // 
         // tpOverview
         // 
         this.tpOverview.Controls.Add(this.panel3);
         this.tpOverview.Controls.Add(this.panel1);
         this.tpOverview.Controls.Add(this.panel2);
         this.tpOverview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.tpOverview.Location = new System.Drawing.Point(4, 58);
         this.tpOverview.Name = "tpOverview";
         this.tpOverview.Padding = new System.Windows.Forms.Padding(3);
         this.tpOverview.Size = new System.Drawing.Size(1453, 1099);
         this.tpOverview.TabIndex = 0;
         this.tpOverview.Text = "Overview";
         this.tpOverview.UseVisualStyleBackColor = true;
         // 
         // panel3
         // 
         this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel3.Controls.Add(this.lblCur);
         this.panel3.Controls.Add(this.lblRunSince);
         this.panel3.Controls.Add(this.lblCurValue);
         this.panel3.Controls.Add(this.lblPlayerValue);
         this.panel3.Controls.Add(this.lblPlayer);
         this.panel3.Controls.Add(this.lblRunSinceValue);
         this.panel3.Location = new System.Drawing.Point(6, 6);
         this.panel3.Name = "panel3";
         this.panel3.Size = new System.Drawing.Size(1429, 306);
         this.panel3.TabIndex = 6;
         // 
         // lblCur
         // 
         this.lblCur.AutoSize = true;
         this.lblCur.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblCur.Location = new System.Drawing.Point(12, 15);
         this.lblCur.Name = "lblCur";
         this.lblCur.Size = new System.Drawing.Size(293, 46);
         this.lblCur.TabIndex = 0;
         this.lblCur.Text = "Current World:";
         // 
         // lblRunSince
         // 
         this.lblRunSince.AutoSize = true;
         this.lblRunSince.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblRunSince.Location = new System.Drawing.Point(12, 223);
         this.lblRunSince.Name = "lblRunSince";
         this.lblRunSince.Size = new System.Drawing.Size(381, 46);
         this.lblRunSince.TabIndex = 5;
         this.lblRunSince.Text = "Running Bot Since:";
         // 
         // lblCurValue
         // 
         this.lblCurValue.AutoSize = true;
         this.lblCurValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblCurValue.Location = new System.Drawing.Point(1011, 15);
         this.lblCurValue.Name = "lblCurValue";
         this.lblCurValue.Size = new System.Drawing.Size(212, 46);
         this.lblCurValue.TabIndex = 3;
         this.lblCurValue.Text = "asdafsgdh";
         // 
         // lblPlayerValue
         // 
         this.lblPlayerValue.AutoSize = true;
         this.lblPlayerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblPlayerValue.Location = new System.Drawing.Point(1011, 112);
         this.lblPlayerValue.Name = "lblPlayerValue";
         this.lblPlayerValue.Size = new System.Drawing.Size(146, 46);
         this.lblPlayerValue.TabIndex = 1;
         this.lblPlayerValue.Text = "Quinrir";
         // 
         // lblPlayer
         // 
         this.lblPlayer.AutoSize = true;
         this.lblPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblPlayer.Location = new System.Drawing.Point(12, 112);
         this.lblPlayer.Name = "lblPlayer";
         this.lblPlayer.Size = new System.Drawing.Size(151, 46);
         this.lblPlayer.TabIndex = 2;
         this.lblPlayer.Text = "Player:";
         // 
         // lblRunSinceValue
         // 
         this.lblRunSinceValue.AutoSize = true;
         this.lblRunSinceValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblRunSinceValue.Location = new System.Drawing.Point(1011, 223);
         this.lblRunSinceValue.Name = "lblRunSinceValue";
         this.lblRunSinceValue.Size = new System.Drawing.Size(146, 46);
         this.lblRunSinceValue.TabIndex = 4;
         this.lblRunSinceValue.Text = "Quinrir";
         // 
         // panel1
         // 
         this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel1.Controls.Add(this.lblDiamonds);
         this.panel1.Controls.Add(this.lblDiaValue);
         this.panel1.Controls.Add(this.lblSupplies);
         this.panel1.Controls.Add(this.lblMoneyValue);
         this.panel1.Controls.Add(this.lblMedsValue);
         this.panel1.Controls.Add(this.lblMoney);
         this.panel1.Controls.Add(this.lblSuppliesValue);
         this.panel1.Controls.Add(this.lblMeds);
         this.panel1.Location = new System.Drawing.Point(6, 318);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(1429, 191);
         this.panel1.TabIndex = 0;
         // 
         // lblDiamonds
         // 
         this.lblDiamonds.AutoSize = true;
         this.lblDiamonds.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblDiamonds.Location = new System.Drawing.Point(794, 30);
         this.lblDiamonds.Name = "lblDiamonds";
         this.lblDiamonds.Size = new System.Drawing.Size(136, 29);
         this.lblDiamonds.TabIndex = 8;
         this.lblDiamonds.Text = "Diamonds:";
         // 
         // lblDiaValue
         // 
         this.lblDiaValue.AutoSize = true;
         this.lblDiaValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblDiaValue.Location = new System.Drawing.Point(1101, 30);
         this.lblDiaValue.Name = "lblDiaValue";
         this.lblDiaValue.Size = new System.Drawing.Size(133, 29);
         this.lblDiaValue.TabIndex = 11;
         this.lblDiaValue.Text = "asdafsgdh";
         // 
         // lblMoneyValue
         // 
         this.lblMoneyValue.AutoSize = true;
         this.lblMoneyValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblMoneyValue.Location = new System.Drawing.Point(231, 127);
         this.lblMoneyValue.Name = "lblMoneyValue";
         this.lblMoneyValue.Size = new System.Drawing.Size(93, 29);
         this.lblMoneyValue.TabIndex = 5;
         this.lblMoneyValue.Text = "Quinrir";
         // 
         // lblMedsValue
         // 
         this.lblMedsValue.AutoSize = true;
         this.lblMedsValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblMedsValue.Location = new System.Drawing.Point(1101, 127);
         this.lblMedsValue.Name = "lblMedsValue";
         this.lblMedsValue.Size = new System.Drawing.Size(93, 29);
         this.lblMedsValue.TabIndex = 9;
         this.lblMedsValue.Text = "Quinrir";
         // 
         // lblMoney
         // 
         this.lblMoney.AutoSize = true;
         this.lblMoney.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblMoney.Location = new System.Drawing.Point(20, 127);
         this.lblMoney.Name = "lblMoney";
         this.lblMoney.Size = new System.Drawing.Size(97, 29);
         this.lblMoney.TabIndex = 6;
         this.lblMoney.Text = "Money:";
         // 
         // lblSupplies
         // 
         this.lblSupplies.AutoSize = true;
         this.lblSupplies.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblSupplies.Location = new System.Drawing.Point(20, 30);
         this.lblSupplies.Name = "lblSupplies";
         this.lblSupplies.Size = new System.Drawing.Size(121, 29);
         this.lblSupplies.TabIndex = 4;
         this.lblSupplies.Text = "Supplies:";
         // 
         // lblSuppliesValue
         // 
         this.lblSuppliesValue.AutoSize = true;
         this.lblSuppliesValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblSuppliesValue.Location = new System.Drawing.Point(231, 30);
         this.lblSuppliesValue.Name = "lblSuppliesValue";
         this.lblSuppliesValue.Size = new System.Drawing.Size(133, 29);
         this.lblSuppliesValue.TabIndex = 7;
         this.lblSuppliesValue.Text = "asdafsgdh";
         // 
         // lblMeds
         // 
         this.lblMeds.AutoSize = true;
         this.lblMeds.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblMeds.Location = new System.Drawing.Point(794, 127);
         this.lblMeds.Name = "lblMeds";
         this.lblMeds.Size = new System.Drawing.Size(102, 29);
         this.lblMeds.TabIndex = 10;
         this.lblMeds.Text = "Medals:";
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.listView1);
         this.panel2.Location = new System.Drawing.Point(8, 515);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(1427, 573);
         this.panel2.TabIndex = 1;
         // 
         // listView1
         // 
         this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
         this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.listView1.HideSelection = false;
         this.listView1.LabelWrap = false;
         this.listView1.Location = new System.Drawing.Point(0, 0);
         this.listView1.Name = "listView1";
         this.listView1.ShowGroups = false;
         this.listView1.Size = new System.Drawing.Size(1427, 573);
         this.listView1.TabIndex = 0;
         this.listView1.UseCompatibleStateImageBehavior = false;
         this.listView1.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader1
         // 
         this.columnHeader1.Text = "Era";
         this.columnHeader1.Width = 160;
         // 
         // columnHeader2
         // 
         this.columnHeader2.Text = "";
         this.columnHeader2.Width = 92;
         // 
         // columnHeader3
         // 
         this.columnHeader3.Text = "";
         // 
         // columnHeader4
         // 
         this.columnHeader4.Text = "";
         this.columnHeader4.Width = 79;
         // 
         // columnHeader5
         // 
         this.columnHeader5.Text = "";
         // 
         // columnHeader6
         // 
         this.columnHeader6.Text = "";
         // 
         // tpOthers
         // 
         this.tpOthers.Controls.Add(this.panel5);
         this.tpOthers.Controls.Add(this.panel4);
         this.tpOthers.Location = new System.Drawing.Point(4, 58);
         this.tpOthers.Name = "tpOthers";
         this.tpOthers.Padding = new System.Windows.Forms.Padding(3);
         this.tpOthers.Size = new System.Drawing.Size(1456, 1040);
         this.tpOthers.TabIndex = 1;
         this.tpOthers.Text = "Other Players";
         this.tpOthers.UseVisualStyleBackColor = true;
         // 
         // tpTavern
         // 
         this.tpTavern.Location = new System.Drawing.Point(4, 58);
         this.tpTavern.Name = "tpTavern";
         this.tpTavern.Size = new System.Drawing.Size(1453, 1099);
         this.tpTavern.TabIndex = 2;
         this.tpTavern.Text = "Tavern";
         this.tpTavern.UseVisualStyleBackColor = true;
         // 
         // tpBots
         // 
         this.tpBots.Location = new System.Drawing.Point(4, 58);
         this.tpBots.Name = "tpBots";
         this.tpBots.Size = new System.Drawing.Size(1453, 1099);
         this.tpBots.TabIndex = 3;
         this.tpBots.Text = "Bots";
         this.tpBots.UseVisualStyleBackColor = true;
         // 
         // tpProduction
         // 
         this.tpProduction.Location = new System.Drawing.Point(4, 58);
         this.tpProduction.Name = "tpProduction";
         this.tpProduction.Size = new System.Drawing.Size(1453, 1099);
         this.tpProduction.TabIndex = 4;
         this.tpProduction.Text = "Production";
         this.tpProduction.UseVisualStyleBackColor = true;
         // 
         // tpManually
         // 
         this.tpManually.Location = new System.Drawing.Point(4, 58);
         this.tpManually.Name = "tpManually";
         this.tpManually.Size = new System.Drawing.Size(1453, 1099);
         this.tpManually.TabIndex = 5;
         this.tpManually.Text = "Manually";
         this.tpManually.UseVisualStyleBackColor = true;
         // 
         // bwTimerUpdate
         // 
         this.bwTimerUpdate.WorkerSupportsCancellation = true;
         this.bwTimerUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwTimerUpdate_DoWork);
         // 
         // pnlLoading
         // 
         this.pnlLoading.Controls.Add(this.lblPleaseLogin);
         this.pnlLoading.Controls.Add(this.pictureBox1);
         this.pnlLoading.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pnlLoading.Location = new System.Drawing.Point(0, 34);
         this.pnlLoading.Name = "pnlLoading";
         this.pnlLoading.Size = new System.Drawing.Size(1464, 1102);
         this.pnlLoading.TabIndex = 12;
         this.pnlLoading.Visible = false;
         // 
         // lblPleaseLogin
         // 
         this.lblPleaseLogin.AutoSize = true;
         this.lblPleaseLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblPleaseLogin.Location = new System.Drawing.Point(516, 298);
         this.lblPleaseLogin.Name = "lblPleaseLogin";
         this.lblPleaseLogin.Size = new System.Drawing.Size(399, 69);
         this.lblPleaseLogin.TabIndex = 1;
         this.lblPleaseLogin.Text = "Please wait...";
         // 
         // pictureBox1
         // 
         this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
         this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
         this.pictureBox1.Location = new System.Drawing.Point(595, 437);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(200, 200);
         this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         this.pictureBox1.TabIndex = 0;
         this.pictureBox1.TabStop = false;
         // 
         // panel4
         // 
         this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel4.Controls.Add(this.tableLayoutPanel1);
         this.panel4.Location = new System.Drawing.Point(6, 6);
         this.panel4.Name = "panel4";
         this.panel4.Size = new System.Drawing.Size(1441, 255);
         this.panel4.TabIndex = 0;
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.Controls.Add(this.lblClanMemberCount, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.lblFriendsCount, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.lblClanMember, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblFriends, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblNeighbor, 2, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblNeighborCount, 2, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(1439, 253);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // lblNeighbor
         // 
         this.lblNeighbor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblNeighbor.AutoSize = true;
         this.lblNeighbor.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblNeighbor.Location = new System.Drawing.Point(961, 0);
         this.lblNeighbor.Name = "lblNeighbor";
         this.lblNeighbor.Size = new System.Drawing.Size(475, 126);
         this.lblNeighbor.TabIndex = 2;
         this.lblNeighbor.Text = "label3";
         this.lblNeighbor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblNeighborCount
         // 
         this.lblNeighborCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblNeighborCount.AutoSize = true;
         this.lblNeighborCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblNeighborCount.Location = new System.Drawing.Point(961, 126);
         this.lblNeighborCount.Name = "lblNeighborCount";
         this.lblNeighborCount.Size = new System.Drawing.Size(475, 127);
         this.lblNeighborCount.TabIndex = 5;
         this.lblNeighborCount.Text = "label6";
         this.lblNeighborCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblFriends
         // 
         this.lblFriends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblFriends.AutoSize = true;
         this.lblFriends.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblFriends.Location = new System.Drawing.Point(3, 0);
         this.lblFriends.Name = "lblFriends";
         this.lblFriends.Size = new System.Drawing.Size(473, 126);
         this.lblFriends.TabIndex = 6;
         this.lblFriends.Text = "label1";
         this.lblFriends.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblClanMember
         // 
         this.lblClanMember.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblClanMember.AutoSize = true;
         this.lblClanMember.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblClanMember.Location = new System.Drawing.Point(482, 0);
         this.lblClanMember.Name = "lblClanMember";
         this.lblClanMember.Size = new System.Drawing.Size(473, 126);
         this.lblClanMember.TabIndex = 7;
         this.lblClanMember.Text = "label2";
         this.lblClanMember.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblFriendsCount
         // 
         this.lblFriendsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblFriendsCount.AutoSize = true;
         this.lblFriendsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblFriendsCount.Location = new System.Drawing.Point(3, 126);
         this.lblFriendsCount.Name = "lblFriendsCount";
         this.lblFriendsCount.Size = new System.Drawing.Size(473, 127);
         this.lblFriendsCount.TabIndex = 8;
         this.lblFriendsCount.Text = "label4";
         this.lblFriendsCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblClanMemberCount
         // 
         this.lblClanMemberCount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblClanMemberCount.AutoSize = true;
         this.lblClanMemberCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblClanMemberCount.Location = new System.Drawing.Point(482, 126);
         this.lblClanMemberCount.Name = "lblClanMemberCount";
         this.lblClanMemberCount.Size = new System.Drawing.Size(473, 127);
         this.lblClanMemberCount.TabIndex = 9;
         this.lblClanMemberCount.Text = "label5";
         this.lblClanMemberCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // panel5
         // 
         this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel5.Controls.Add(this.tableLayoutPanel3);
         this.panel5.Controls.Add(this.tlpInactiveFriends);
         this.panel5.Location = new System.Drawing.Point(8, 267);
         this.panel5.Name = "panel5";
         this.panel5.Size = new System.Drawing.Size(1439, 767);
         this.panel5.TabIndex = 1;
         // 
         // tlpInactiveFriends
         // 
         this.tlpInactiveFriends.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.tlpInactiveFriends.AutoScroll = true;
         this.tlpInactiveFriends.ColumnCount = 3;
         this.tlpInactiveFriends.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tlpInactiveFriends.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tlpInactiveFriends.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tlpInactiveFriends.Location = new System.Drawing.Point(3, 109);
         this.tlpInactiveFriends.Name = "tlpInactiveFriends";
         this.tlpInactiveFriends.RowCount = 1;
         this.tlpInactiveFriends.RowStyles.Add(new System.Windows.Forms.RowStyle());
         this.tlpInactiveFriends.Size = new System.Drawing.Size(1431, 653);
         this.tlpInactiveFriends.TabIndex = 0;
         // 
         // tableLayoutPanel3
         // 
         this.tableLayoutPanel3.ColumnCount = 1;
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel3.Controls.Add(this.lblInactiveFriends, 0, 0);
         this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         this.tableLayoutPanel3.RowCount = 1;
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel3.Size = new System.Drawing.Size(1431, 100);
         this.tableLayoutPanel3.TabIndex = 1;
         // 
         // lblInactiveFriends
         // 
         this.lblInactiveFriends.AutoSize = true;
         this.lblInactiveFriends.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblInactiveFriends.Location = new System.Drawing.Point(3, 0);
         this.lblInactiveFriends.Name = "lblInactiveFriends";
         this.lblInactiveFriends.Size = new System.Drawing.Size(1425, 100);
         this.lblInactiveFriends.TabIndex = 0;
         this.lblInactiveFriends.Text = "label1";
         this.lblInactiveFriends.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Main
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1464, 1168);
         this.Controls.Add(this.pnlLoading);
         this.Controls.Add(this.tcMenu);
         this.Controls.Add(this.toolStrip1);
         this.Controls.Add(this.sStripStatus);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "Main";
         this.Text = "Form1";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
         this.Load += new System.EventHandler(this.Main_Load);
         this.sStripStatus.ResumeLayout(false);
         this.sStripStatus.PerformLayout();
         this.toolStrip1.ResumeLayout(false);
         this.toolStrip1.PerformLayout();
         this.tcMenu.ResumeLayout(false);
         this.tpOverview.ResumeLayout(false);
         this.panel3.ResumeLayout(false);
         this.panel3.PerformLayout();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.panel2.ResumeLayout(false);
         this.tpOthers.ResumeLayout(false);
         this.pnlLoading.ResumeLayout(false);
         this.pnlLoading.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         this.panel4.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.panel5.ResumeLayout(false);
         this.tableLayoutPanel3.ResumeLayout(false);
         this.tableLayoutPanel3.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

        #endregion
        private System.Windows.Forms.StatusStrip sStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslProgressState;
        private System.Windows.Forms.ToolStripProgressBar tspbProgress;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbLogin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbSettings;
        private System.Windows.Forms.TabControl tcMenu;
        private System.Windows.Forms.TabPage tpOverview;
        private System.Windows.Forms.TabPage tpOthers;
        private System.Windows.Forms.TabPage tpTavern;
        private System.Windows.Forms.Label lblCur;
        private System.Windows.Forms.TabPage tpBots;
        private System.Windows.Forms.TabPage tpProduction;
        private System.Windows.Forms.TabPage tpManually;
        private System.Windows.Forms.Label lblPlayerValue;
        private System.Windows.Forms.Label lblCurValue;
        private System.Windows.Forms.Label lblPlayer;
        private System.Windows.Forms.Label lblRunSince;
        private System.Windows.Forms.Label lblRunSinceValue;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label lblDiamonds;
        private System.Windows.Forms.Label lblMedsValue;
        private System.Windows.Forms.Label lblMeds;
        private System.Windows.Forms.Label lblDiaValue;
        private System.Windows.Forms.Label lblSupplies;
        private System.Windows.Forms.Label lblMoneyValue;
        private System.Windows.Forms.Label lblMoney;
        private System.Windows.Forms.Label lblSuppliesValue;
        private System.ComponentModel.BackgroundWorker bwTimerUpdate;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblPleaseLogin;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblNeighbor;
        private System.Windows.Forms.Label lblNeighborCount;
        private System.Windows.Forms.Label lblClanMemberCount;
        private System.Windows.Forms.Label lblFriendsCount;
        private System.Windows.Forms.Label lblClanMember;
        private System.Windows.Forms.Label lblFriends;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TableLayoutPanel tlpInactiveFriends;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblInactiveFriends;
    }
}

