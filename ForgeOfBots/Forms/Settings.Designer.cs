namespace ForgeOfBots.Forms
{
   partial class Settings
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
         this.mtView = new MetroFramework.Controls.MetroToggle();
         this.mtcSettings = new MetroFramework.Controls.MetroTabControl();
         this.mtpProduction = new MetroFramework.Controls.MetroTabPage();
         this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
         this.mpGoodCycle = new MetroFramework.Controls.MetroPanel();
         this.mrbG2d = new MetroFramework.Controls.MetroRadioButton();
         this.mrbG1d = new MetroFramework.Controls.MetroRadioButton();
         this.mrbG8h = new MetroFramework.Controls.MetroRadioButton();
         this.mrbG4h = new MetroFramework.Controls.MetroRadioButton();
         this.mpProdCycle = new MetroFramework.Controls.MetroPanel();
         this.mrb1d = new MetroFramework.Controls.MetroRadioButton();
         this.mrb8 = new MetroFramework.Controls.MetroRadioButton();
         this.mrb4 = new MetroFramework.Controls.MetroRadioButton();
         this.mrb1 = new MetroFramework.Controls.MetroRadioButton();
         this.mrb15 = new MetroFramework.Controls.MetroRadioButton();
         this.mrb5 = new MetroFramework.Controls.MetroRadioButton();
         this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
         this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
         this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
         this.mtpData = new MetroFramework.Controls.MetroTabPage();
         this.metroLabel11 = new MetroFramework.Controls.MetroLabel();
         this.mbSaveReload = new MetroFramework.Controls.MetroButton();
         this.lblChooseWorld = new MetroFramework.Controls.MetroLabel();
         this.mcbCitySelection = new MetroFramework.Controls.MetroComboBox();
         this.mbDeleteData = new MetroFramework.Controls.MetroButton();
         this.lblDeleteData = new MetroFramework.Controls.MetroLabel();
         this.mtpBots = new MetroFramework.Controls.MetroTabPage();
         this.metroLabel14 = new MetroFramework.Controls.MetroLabel();
         this.mtbIntervalIncident = new MetroFramework.Controls.MetroTextBox();
         this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
         this.mtRQBot = new MetroFramework.Controls.MetroToggle();
         this.metroLabel10 = new MetroFramework.Controls.MetroLabel();
         this.mtIncident = new MetroFramework.Controls.MetroToggle();
         this.mtMoppel = new MetroFramework.Controls.MetroToggle();
         this.mtTavern = new MetroFramework.Controls.MetroToggle();
         this.mtProduction = new MetroFramework.Controls.MetroToggle();
         this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
         this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
         this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
         this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
         this.mtpManually = new MetroFramework.Controls.MetroTabPage();
         this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
         this.mtBigRoads = new MetroFramework.Controls.MetroToggle();
         this.mtpPremium = new MetroFramework.Controls.MetroTabPage();
         this.mbCheckSerial = new MetroFramework.Controls.MetroButton();
         this.lblSerialKey = new MetroFramework.Controls.MetroLabel();
         this.mtbSerialKey = new MetroFramework.Controls.MetroTextBox();
         this.mtpMisc = new MetroFramework.Controls.MetroTabPage();
         this.metroLabel12 = new MetroFramework.Controls.MetroLabel();
         this.mtDarkMode = new MetroFramework.Controls.MetroToggle();
         this.lblAutoLogin = new MetroFramework.Controls.MetroLabel();
         this.mtAutoLogin = new MetroFramework.Controls.MetroToggle();
         this.lblRestartNeeded = new MetroFramework.Controls.MetroLabel();
         this.mtbSave = new MetroFramework.Controls.MetroButton();
         this.mtbCustomUserAgent = new MetroFramework.Controls.MetroTextBox();
         this.lblCustomUserAgent = new MetroFramework.Controls.MetroLabel();
         this.lblLanguage = new MetroFramework.Controls.MetroLabel();
         this.mcbLanguage = new MetroFramework.Controls.MetroComboBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.mcbNeighbor = new MetroFramework.Controls.MetroCheckBox();
         this.mcbFriends = new MetroFramework.Controls.MetroCheckBox();
         this.mcbGuild = new MetroFramework.Controls.MetroCheckBox();
         this.nudMinProfit = new System.Windows.Forms.NumericUpDown();
         this.metroLabel15 = new MetroFramework.Controls.MetroLabel();
         this.lblTarget = new MetroFramework.Controls.MetroLabel();
         this.mtcSettings.SuspendLayout();
         this.mtpProduction.SuspendLayout();
         this.mpGoodCycle.SuspendLayout();
         this.mpProdCycle.SuspendLayout();
         this.mtpData.SuspendLayout();
         this.mtpBots.SuspendLayout();
         this.mtpManually.SuspendLayout();
         this.mtpPremium.SuspendLayout();
         this.mtpMisc.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudMinProfit)).BeginInit();
         this.SuspendLayout();
         // 
         // mtView
         // 
         resources.ApplyResources(this.mtView, "mtView");
         this.mtView.Name = "mtView";
         this.mtView.UseVisualStyleBackColor = false;
         this.mtView.CheckedChanged += new System.EventHandler(this.mtView_CheckedChanged);
         // 
         // mtcSettings
         // 
         this.mtcSettings.Controls.Add(this.mtpProduction);
         this.mtcSettings.Controls.Add(this.mtpData);
         this.mtcSettings.Controls.Add(this.mtpBots);
         this.mtcSettings.Controls.Add(this.mtpManually);
         this.mtcSettings.Controls.Add(this.mtpPremium);
         this.mtcSettings.Controls.Add(this.mtpMisc);
         resources.ApplyResources(this.mtcSettings, "mtcSettings");
         this.mtcSettings.FontWeight = MetroFramework.MetroTabControlWeight.Bold;
         this.mtcSettings.Name = "mtcSettings";
         this.mtcSettings.SelectedIndex = 3;
         this.mtcSettings.Style = MetroFramework.MetroColorStyle.Red;
         this.mtcSettings.UseStyleColors = true;
         // 
         // mtpProduction
         // 
         this.mtpProduction.Controls.Add(this.metroLabel5);
         this.mtpProduction.Controls.Add(this.mpGoodCycle);
         this.mtpProduction.Controls.Add(this.mpProdCycle);
         this.mtpProduction.Controls.Add(this.metroLabel3);
         this.mtpProduction.Controls.Add(this.metroLabel2);
         this.mtpProduction.Controls.Add(this.metroLabel1);
         this.mtpProduction.Controls.Add(this.mtView);
         this.mtpProduction.HorizontalScrollbarBarColor = true;
         this.mtpProduction.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mtpProduction, "mtpProduction");
         this.mtpProduction.Name = "mtpProduction";
         this.mtpProduction.VerticalScrollbarBarColor = true;
         this.mtpProduction.VerticalScrollbarSize = 13;
         // 
         // metroLabel5
         // 
         this.metroLabel5.FontSize = MetroFramework.MetroLabelSize.Small;
         this.metroLabel5.FontWeight = MetroFramework.MetroLabelWeight.Regular;
         resources.ApplyResources(this.metroLabel5, "metroLabel5");
         this.metroLabel5.Name = "metroLabel5";
         // 
         // mpGoodCycle
         // 
         this.mpGoodCycle.Controls.Add(this.mrbG2d);
         this.mpGoodCycle.Controls.Add(this.mrbG1d);
         this.mpGoodCycle.Controls.Add(this.mrbG8h);
         this.mpGoodCycle.Controls.Add(this.mrbG4h);
         this.mpGoodCycle.HorizontalScrollbarBarColor = true;
         this.mpGoodCycle.HorizontalScrollbarHighlightOnWheel = false;
         this.mpGoodCycle.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mpGoodCycle, "mpGoodCycle");
         this.mpGoodCycle.Name = "mpGoodCycle";
         this.mpGoodCycle.VerticalScrollbarBarColor = true;
         this.mpGoodCycle.VerticalScrollbarHighlightOnWheel = false;
         this.mpGoodCycle.VerticalScrollbarSize = 13;
         // 
         // mrbG2d
         // 
         resources.ApplyResources(this.mrbG2d, "mrbG2d");
         this.mrbG2d.Name = "mrbG2d";
         this.mrbG2d.Tag = "2880";
         this.mrbG2d.UseVisualStyleBackColor = true;
         this.mrbG2d.CheckedChanged += new System.EventHandler(this.mrbG4h_CheckedChanged);
         // 
         // mrbG1d
         // 
         resources.ApplyResources(this.mrbG1d, "mrbG1d");
         this.mrbG1d.Name = "mrbG1d";
         this.mrbG1d.Tag = "1440";
         this.mrbG1d.UseVisualStyleBackColor = true;
         this.mrbG1d.CheckedChanged += new System.EventHandler(this.mrbG4h_CheckedChanged);
         // 
         // mrbG8h
         // 
         resources.ApplyResources(this.mrbG8h, "mrbG8h");
         this.mrbG8h.Name = "mrbG8h";
         this.mrbG8h.Tag = "480";
         this.mrbG8h.UseVisualStyleBackColor = true;
         this.mrbG8h.CheckedChanged += new System.EventHandler(this.mrbG4h_CheckedChanged);
         // 
         // mrbG4h
         // 
         resources.ApplyResources(this.mrbG4h, "mrbG4h");
         this.mrbG4h.Checked = true;
         this.mrbG4h.Name = "mrbG4h";
         this.mrbG4h.TabStop = true;
         this.mrbG4h.Tag = "240";
         this.mrbG4h.UseVisualStyleBackColor = true;
         this.mrbG4h.CheckedChanged += new System.EventHandler(this.mrbG4h_CheckedChanged);
         // 
         // mpProdCycle
         // 
         this.mpProdCycle.Controls.Add(this.mrb1d);
         this.mpProdCycle.Controls.Add(this.mrb8);
         this.mpProdCycle.Controls.Add(this.mrb4);
         this.mpProdCycle.Controls.Add(this.mrb1);
         this.mpProdCycle.Controls.Add(this.mrb15);
         this.mpProdCycle.Controls.Add(this.mrb5);
         this.mpProdCycle.HorizontalScrollbarBarColor = true;
         this.mpProdCycle.HorizontalScrollbarHighlightOnWheel = false;
         this.mpProdCycle.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mpProdCycle, "mpProdCycle");
         this.mpProdCycle.Name = "mpProdCycle";
         this.mpProdCycle.VerticalScrollbarBarColor = true;
         this.mpProdCycle.VerticalScrollbarHighlightOnWheel = false;
         this.mpProdCycle.VerticalScrollbarSize = 13;
         // 
         // mrb1d
         // 
         resources.ApplyResources(this.mrb1d, "mrb1d");
         this.mrb1d.Name = "mrb1d";
         this.mrb1d.Tag = "1440";
         this.mrb1d.UseVisualStyleBackColor = true;
         this.mrb1d.CheckedChanged += new System.EventHandler(this.mrb5_CheckedChanged);
         // 
         // mrb8
         // 
         resources.ApplyResources(this.mrb8, "mrb8");
         this.mrb8.Name = "mrb8";
         this.mrb8.Tag = "480";
         this.mrb8.UseVisualStyleBackColor = true;
         this.mrb8.CheckedChanged += new System.EventHandler(this.mrb5_CheckedChanged);
         // 
         // mrb4
         // 
         resources.ApplyResources(this.mrb4, "mrb4");
         this.mrb4.Name = "mrb4";
         this.mrb4.Tag = "240";
         this.mrb4.UseVisualStyleBackColor = false;
         this.mrb4.CheckedChanged += new System.EventHandler(this.mrb5_CheckedChanged);
         // 
         // mrb1
         // 
         resources.ApplyResources(this.mrb1, "mrb1");
         this.mrb1.Name = "mrb1";
         this.mrb1.Tag = "60";
         this.mrb1.UseVisualStyleBackColor = true;
         this.mrb1.CheckedChanged += new System.EventHandler(this.mrb5_CheckedChanged);
         // 
         // mrb15
         // 
         resources.ApplyResources(this.mrb15, "mrb15");
         this.mrb15.Name = "mrb15";
         this.mrb15.Tag = "15";
         this.mrb15.UseVisualStyleBackColor = true;
         this.mrb15.CheckedChanged += new System.EventHandler(this.mrb5_CheckedChanged);
         // 
         // mrb5
         // 
         resources.ApplyResources(this.mrb5, "mrb5");
         this.mrb5.Checked = true;
         this.mrb5.Name = "mrb5";
         this.mrb5.TabStop = true;
         this.mrb5.Tag = "5";
         this.mrb5.UseVisualStyleBackColor = true;
         this.mrb5.CheckedChanged += new System.EventHandler(this.mrb5_CheckedChanged);
         // 
         // metroLabel3
         // 
         resources.ApplyResources(this.metroLabel3, "metroLabel3");
         this.metroLabel3.Name = "metroLabel3";
         // 
         // metroLabel2
         // 
         resources.ApplyResources(this.metroLabel2, "metroLabel2");
         this.metroLabel2.Name = "metroLabel2";
         // 
         // metroLabel1
         // 
         resources.ApplyResources(this.metroLabel1, "metroLabel1");
         this.metroLabel1.Name = "metroLabel1";
         // 
         // mtpData
         // 
         this.mtpData.Controls.Add(this.metroLabel11);
         this.mtpData.Controls.Add(this.mbSaveReload);
         this.mtpData.Controls.Add(this.lblChooseWorld);
         this.mtpData.Controls.Add(this.mcbCitySelection);
         this.mtpData.Controls.Add(this.mbDeleteData);
         this.mtpData.Controls.Add(this.lblDeleteData);
         this.mtpData.HorizontalScrollbarBarColor = true;
         this.mtpData.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mtpData, "mtpData");
         this.mtpData.Name = "mtpData";
         this.mtpData.VerticalScrollbarBarColor = true;
         this.mtpData.VerticalScrollbarSize = 13;
         // 
         // metroLabel11
         // 
         this.metroLabel11.BackColor = System.Drawing.SystemColors.Control;
         this.metroLabel11.CustomForeColor = true;
         this.metroLabel11.FontSize = MetroFramework.MetroLabelSize.Small;
         this.metroLabel11.FontWeight = MetroFramework.MetroLabelWeight.Regular;
         this.metroLabel11.ForeColor = System.Drawing.Color.Red;
         resources.ApplyResources(this.metroLabel11, "metroLabel11");
         this.metroLabel11.Name = "metroLabel11";
         // 
         // mbSaveReload
         // 
         resources.ApplyResources(this.mbSaveReload, "mbSaveReload");
         this.mbSaveReload.Name = "mbSaveReload";
         this.mbSaveReload.Click += new System.EventHandler(this.mbSaveReload_Click);
         // 
         // lblChooseWorld
         // 
         resources.ApplyResources(this.lblChooseWorld, "lblChooseWorld");
         this.lblChooseWorld.Name = "lblChooseWorld";
         // 
         // mcbCitySelection
         // 
         this.mcbCitySelection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
         this.mcbCitySelection.FormattingEnabled = true;
         resources.ApplyResources(this.mcbCitySelection, "mcbCitySelection");
         this.mcbCitySelection.Name = "mcbCitySelection";
         // 
         // mbDeleteData
         // 
         resources.ApplyResources(this.mbDeleteData, "mbDeleteData");
         this.mbDeleteData.Name = "mbDeleteData";
         this.mbDeleteData.Click += new System.EventHandler(this.mbDeleteData_Click);
         // 
         // lblDeleteData
         // 
         resources.ApplyResources(this.lblDeleteData, "lblDeleteData");
         this.lblDeleteData.Name = "lblDeleteData";
         // 
         // mtpBots
         // 
         this.mtpBots.Controls.Add(this.metroLabel14);
         this.mtpBots.Controls.Add(this.mtbIntervalIncident);
         this.mtpBots.Controls.Add(this.metroLabel13);
         this.mtpBots.Controls.Add(this.mtRQBot);
         this.mtpBots.Controls.Add(this.metroLabel10);
         this.mtpBots.Controls.Add(this.mtIncident);
         this.mtpBots.Controls.Add(this.mtMoppel);
         this.mtpBots.Controls.Add(this.mtTavern);
         this.mtpBots.Controls.Add(this.mtProduction);
         this.mtpBots.Controls.Add(this.metroLabel9);
         this.mtpBots.Controls.Add(this.metroLabel8);
         this.mtpBots.Controls.Add(this.metroLabel7);
         this.mtpBots.Controls.Add(this.metroLabel6);
         this.mtpBots.HorizontalScrollbarBarColor = true;
         this.mtpBots.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mtpBots, "mtpBots");
         this.mtpBots.Name = "mtpBots";
         this.mtpBots.VerticalScrollbarBarColor = true;
         this.mtpBots.VerticalScrollbarSize = 13;
         // 
         // metroLabel14
         // 
         resources.ApplyResources(this.metroLabel14, "metroLabel14");
         this.metroLabel14.Name = "metroLabel14";
         // 
         // mtbIntervalIncident
         // 
         resources.ApplyResources(this.mtbIntervalIncident, "mtbIntervalIncident");
         this.mtbIntervalIncident.Name = "mtbIntervalIncident";
         this.mtbIntervalIncident.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
         this.mtbIntervalIncident.TextChanged += new System.EventHandler(this.MtbIntervalIncident_TextChanged);
         // 
         // metroLabel13
         // 
         resources.ApplyResources(this.metroLabel13, "metroLabel13");
         this.metroLabel13.Name = "metroLabel13";
         // 
         // mtRQBot
         // 
         resources.ApplyResources(this.mtRQBot, "mtRQBot");
         this.mtRQBot.Name = "mtRQBot";
         this.mtRQBot.UseVisualStyleBackColor = true;
         this.mtRQBot.CheckedChanged += new System.EventHandler(this.mtRQBot_CheckedChanged);
         // 
         // metroLabel10
         // 
         resources.ApplyResources(this.metroLabel10, "metroLabel10");
         this.metroLabel10.Name = "metroLabel10";
         // 
         // mtIncident
         // 
         resources.ApplyResources(this.mtIncident, "mtIncident");
         this.mtIncident.Name = "mtIncident";
         this.mtIncident.UseVisualStyleBackColor = true;
         this.mtIncident.CheckedChanged += new System.EventHandler(this.mtIncident_CheckedChanged);
         // 
         // mtMoppel
         // 
         resources.ApplyResources(this.mtMoppel, "mtMoppel");
         this.mtMoppel.Name = "mtMoppel";
         this.mtMoppel.UseVisualStyleBackColor = true;
         this.mtMoppel.CheckedChanged += new System.EventHandler(this.mtMoppel_CheckedChanged);
         // 
         // mtTavern
         // 
         resources.ApplyResources(this.mtTavern, "mtTavern");
         this.mtTavern.Name = "mtTavern";
         this.mtTavern.UseVisualStyleBackColor = true;
         this.mtTavern.CheckedChanged += new System.EventHandler(this.mtTavern_CheckedChanged);
         // 
         // mtProduction
         // 
         resources.ApplyResources(this.mtProduction, "mtProduction");
         this.mtProduction.Name = "mtProduction";
         this.mtProduction.UseVisualStyleBackColor = true;
         this.mtProduction.CheckedChanged += new System.EventHandler(this.mtProduction_CheckedChanged);
         // 
         // metroLabel9
         // 
         resources.ApplyResources(this.metroLabel9, "metroLabel9");
         this.metroLabel9.Name = "metroLabel9";
         // 
         // metroLabel8
         // 
         resources.ApplyResources(this.metroLabel8, "metroLabel8");
         this.metroLabel8.Name = "metroLabel8";
         // 
         // metroLabel7
         // 
         resources.ApplyResources(this.metroLabel7, "metroLabel7");
         this.metroLabel7.Name = "metroLabel7";
         // 
         // metroLabel6
         // 
         resources.ApplyResources(this.metroLabel6, "metroLabel6");
         this.metroLabel6.Name = "metroLabel6";
         // 
         // mtpManually
         // 
         this.mtpManually.Controls.Add(this.groupBox1);
         this.mtpManually.Controls.Add(this.metroLabel4);
         this.mtpManually.Controls.Add(this.mtBigRoads);
         this.mtpManually.HorizontalScrollbarBarColor = true;
         this.mtpManually.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mtpManually, "mtpManually");
         this.mtpManually.Name = "mtpManually";
         this.mtpManually.VerticalScrollbarBarColor = true;
         this.mtpManually.VerticalScrollbarSize = 13;
         // 
         // metroLabel4
         // 
         resources.ApplyResources(this.metroLabel4, "metroLabel4");
         this.metroLabel4.Name = "metroLabel4";
         // 
         // mtBigRoads
         // 
         resources.ApplyResources(this.mtBigRoads, "mtBigRoads");
         this.mtBigRoads.Name = "mtBigRoads";
         this.mtBigRoads.UseVisualStyleBackColor = true;
         this.mtBigRoads.CheckedChanged += new System.EventHandler(this.metroToggle1_CheckedChanged);
         // 
         // mtpPremium
         // 
         this.mtpPremium.Controls.Add(this.mbCheckSerial);
         this.mtpPremium.Controls.Add(this.lblSerialKey);
         this.mtpPremium.Controls.Add(this.mtbSerialKey);
         this.mtpPremium.HorizontalScrollbarBarColor = true;
         this.mtpPremium.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mtpPremium, "mtpPremium");
         this.mtpPremium.Name = "mtpPremium";
         this.mtpPremium.VerticalScrollbarBarColor = true;
         this.mtpPremium.VerticalScrollbarSize = 13;
         // 
         // mbCheckSerial
         // 
         resources.ApplyResources(this.mbCheckSerial, "mbCheckSerial");
         this.mbCheckSerial.Name = "mbCheckSerial";
         this.mbCheckSerial.Click += new System.EventHandler(this.mbCheckSerial_Click);
         // 
         // lblSerialKey
         // 
         resources.ApplyResources(this.lblSerialKey, "lblSerialKey");
         this.lblSerialKey.Name = "lblSerialKey";
         // 
         // mtbSerialKey
         // 
         resources.ApplyResources(this.mtbSerialKey, "mtbSerialKey");
         this.mtbSerialKey.Multiline = true;
         this.mtbSerialKey.Name = "mtbSerialKey";
         this.mtbSerialKey.PasswordChar = '*';
         this.mtbSerialKey.PromptText = "Input SerialKey here";
         // 
         // mtpMisc
         // 
         this.mtpMisc.Controls.Add(this.metroLabel12);
         this.mtpMisc.Controls.Add(this.mtDarkMode);
         this.mtpMisc.Controls.Add(this.lblAutoLogin);
         this.mtpMisc.Controls.Add(this.mtAutoLogin);
         this.mtpMisc.Controls.Add(this.lblRestartNeeded);
         this.mtpMisc.Controls.Add(this.mtbSave);
         this.mtpMisc.Controls.Add(this.mtbCustomUserAgent);
         this.mtpMisc.Controls.Add(this.lblCustomUserAgent);
         this.mtpMisc.Controls.Add(this.lblLanguage);
         this.mtpMisc.Controls.Add(this.mcbLanguage);
         this.mtpMisc.HorizontalScrollbarBarColor = true;
         this.mtpMisc.HorizontalScrollbarSize = 12;
         resources.ApplyResources(this.mtpMisc, "mtpMisc");
         this.mtpMisc.Name = "mtpMisc";
         this.mtpMisc.VerticalScrollbarBarColor = true;
         this.mtpMisc.VerticalScrollbarSize = 13;
         // 
         // metroLabel12
         // 
         resources.ApplyResources(this.metroLabel12, "metroLabel12");
         this.metroLabel12.Name = "metroLabel12";
         // 
         // mtDarkMode
         // 
         resources.ApplyResources(this.mtDarkMode, "mtDarkMode");
         this.mtDarkMode.Name = "mtDarkMode";
         this.mtDarkMode.UseVisualStyleBackColor = true;
         this.mtDarkMode.CheckedChanged += new System.EventHandler(this.MtDarkMode_CheckedChanged);
         // 
         // lblAutoLogin
         // 
         resources.ApplyResources(this.lblAutoLogin, "lblAutoLogin");
         this.lblAutoLogin.Name = "lblAutoLogin";
         // 
         // mtAutoLogin
         // 
         resources.ApplyResources(this.mtAutoLogin, "mtAutoLogin");
         this.mtAutoLogin.Name = "mtAutoLogin";
         this.mtAutoLogin.UseVisualStyleBackColor = true;
         this.mtAutoLogin.CheckedChanged += new System.EventHandler(this.MtAutoLogin_CheckedChanged);
         // 
         // lblRestartNeeded
         // 
         this.lblRestartNeeded.CustomForeColor = true;
         this.lblRestartNeeded.FontSize = MetroFramework.MetroLabelSize.Small;
         this.lblRestartNeeded.ForeColor = System.Drawing.Color.Red;
         resources.ApplyResources(this.lblRestartNeeded, "lblRestartNeeded");
         this.lblRestartNeeded.Name = "lblRestartNeeded";
         this.lblRestartNeeded.UseStyleColors = true;
         // 
         // mtbSave
         // 
         resources.ApplyResources(this.mtbSave, "mtbSave");
         this.mtbSave.Name = "mtbSave";
         this.mtbSave.Click += new System.EventHandler(this.mtbSave_Click);
         // 
         // mtbCustomUserAgent
         // 
         resources.ApplyResources(this.mtbCustomUserAgent, "mtbCustomUserAgent");
         this.mtbCustomUserAgent.Name = "mtbCustomUserAgent";
         // 
         // lblCustomUserAgent
         // 
         resources.ApplyResources(this.lblCustomUserAgent, "lblCustomUserAgent");
         this.lblCustomUserAgent.Name = "lblCustomUserAgent";
         // 
         // lblLanguage
         // 
         this.lblLanguage.BackColor = System.Drawing.Color.Black;
         this.lblLanguage.ForeColor = System.Drawing.Color.White;
         resources.ApplyResources(this.lblLanguage, "lblLanguage");
         this.lblLanguage.Name = "lblLanguage";
         // 
         // mcbLanguage
         // 
         this.mcbLanguage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
         this.mcbLanguage.FormattingEnabled = true;
         resources.ApplyResources(this.mcbLanguage, "mcbLanguage");
         this.mcbLanguage.Name = "mcbLanguage";
         this.mcbLanguage.SelectedIndexChanged += new System.EventHandler(this.mcbLanguage_SelectedIndexChanged);
         // 
         // groupBox1
         // 
         this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
         this.groupBox1.Controls.Add(this.lblTarget);
         this.groupBox1.Controls.Add(this.metroLabel15);
         this.groupBox1.Controls.Add(this.nudMinProfit);
         this.groupBox1.Controls.Add(this.mcbGuild);
         this.groupBox1.Controls.Add(this.mcbFriends);
         this.groupBox1.Controls.Add(this.mcbNeighbor);
         resources.ApplyResources(this.groupBox1, "groupBox1");
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.TabStop = false;
         this.groupBox1.Tag = "Settings.Manually.AutoSnip.Options";
         // 
         // mcbNeighbor
         // 
         resources.ApplyResources(this.mcbNeighbor, "mcbNeighbor");
         this.mcbNeighbor.BackColor = System.Drawing.SystemColors.Control;
         this.mcbNeighbor.Checked = true;
         this.mcbNeighbor.CheckState = System.Windows.Forms.CheckState.Checked;
         this.mcbNeighbor.CustomBackground = true;
         this.mcbNeighbor.Name = "mcbNeighbor";
         this.mcbNeighbor.Tag = "4";
         this.mcbNeighbor.UseVisualStyleBackColor = false;
         this.mcbNeighbor.CheckedChanged += new System.EventHandler(this.mcbNeighbor_CheckedChanged);
         // 
         // mcbFriends
         // 
         resources.ApplyResources(this.mcbFriends, "mcbFriends");
         this.mcbFriends.Checked = true;
         this.mcbFriends.CheckState = System.Windows.Forms.CheckState.Checked;
         this.mcbFriends.CustomBackground = true;
         this.mcbFriends.Name = "mcbFriends";
         this.mcbFriends.Tag = "1";
         this.mcbFriends.UseVisualStyleBackColor = true;
         this.mcbFriends.CheckedChanged += new System.EventHandler(this.mcbFriends_CheckedChanged);
         // 
         // mcbGuild
         // 
         resources.ApplyResources(this.mcbGuild, "mcbGuild");
         this.mcbGuild.CustomBackground = true;
         this.mcbGuild.Name = "mcbGuild";
         this.mcbGuild.Tag = "2";
         this.mcbGuild.UseVisualStyleBackColor = true;
         this.mcbGuild.CheckedChanged += new System.EventHandler(this.mcbGuild_CheckedChanged);
         // 
         // nudMinProfit
         // 
         resources.ApplyResources(this.nudMinProfit, "nudMinProfit");
         this.nudMinProfit.Name = "nudMinProfit";
         this.nudMinProfit.ValueChanged += new System.EventHandler(this.nudMinProfit_ValueChanged);
         // 
         // metroLabel15
         // 
         resources.ApplyResources(this.metroLabel15, "metroLabel15");
         this.metroLabel15.BackColor = System.Drawing.SystemColors.Control;
         this.metroLabel15.CustomBackground = true;
         this.metroLabel15.FontSize = MetroFramework.MetroLabelSize.Small;
         this.metroLabel15.FontWeight = MetroFramework.MetroLabelWeight.Regular;
         this.metroLabel15.Name = "metroLabel15";
         this.metroLabel15.Tag = "Settings.Manually.AutoSnip.MinProfit";
         // 
         // lblTarget
         // 
         resources.ApplyResources(this.lblTarget, "lblTarget");
         this.lblTarget.BackColor = System.Drawing.SystemColors.Control;
         this.lblTarget.CustomBackground = true;
         this.lblTarget.FontSize = MetroFramework.MetroLabelSize.Small;
         this.lblTarget.FontWeight = MetroFramework.MetroLabelWeight.Regular;
         this.lblTarget.Name = "lblTarget";
         this.lblTarget.Tag = "Settings.Manually.AutoSnip.Target";
         // 
         // Settings
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.mtcSettings);
         this.Cursor = System.Windows.Forms.Cursors.Default;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "Settings";
         this.ShowIcon = false;
         this.TopMost = true;
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
         this.mtcSettings.ResumeLayout(false);
         this.mtpProduction.ResumeLayout(false);
         this.mpGoodCycle.ResumeLayout(false);
         this.mpGoodCycle.PerformLayout();
         this.mpProdCycle.ResumeLayout(false);
         this.mpProdCycle.PerformLayout();
         this.mtpData.ResumeLayout(false);
         this.mtpBots.ResumeLayout(false);
         this.mtpManually.ResumeLayout(false);
         this.mtpPremium.ResumeLayout(false);
         this.mtpMisc.ResumeLayout(false);
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudMinProfit)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private MetroFramework.Controls.MetroToggle mtView;
      private MetroFramework.Controls.MetroTabControl mtcSettings;
      private MetroFramework.Controls.MetroTabPage mtpProduction;
      private MetroFramework.Controls.MetroLabel metroLabel1;
      private MetroFramework.Controls.MetroPanel mpProdCycle;
      private MetroFramework.Controls.MetroRadioButton mrb1d;
      private MetroFramework.Controls.MetroRadioButton mrb8;
      private MetroFramework.Controls.MetroRadioButton mrb4;
      private MetroFramework.Controls.MetroRadioButton mrb1;
      private MetroFramework.Controls.MetroRadioButton mrb15;
      private MetroFramework.Controls.MetroRadioButton mrb5;
      private MetroFramework.Controls.MetroLabel metroLabel2;
      private MetroFramework.Controls.MetroPanel mpGoodCycle;
      private MetroFramework.Controls.MetroRadioButton mrbG2d;
      private MetroFramework.Controls.MetroRadioButton mrbG1d;
      private MetroFramework.Controls.MetroRadioButton mrbG8h;
      private MetroFramework.Controls.MetroRadioButton mrbG4h;
      private MetroFramework.Controls.MetroLabel metroLabel3;
      private MetroFramework.Controls.MetroTabPage mtpManually;
      private MetroFramework.Controls.MetroLabel metroLabel4;
      private MetroFramework.Controls.MetroToggle mtBigRoads;
      private MetroFramework.Controls.MetroLabel metroLabel5;
      private MetroFramework.Controls.MetroTabPage mtpBots;
      private MetroFramework.Controls.MetroLabel metroLabel9;
      private MetroFramework.Controls.MetroLabel metroLabel8;
      private MetroFramework.Controls.MetroLabel metroLabel7;
      private MetroFramework.Controls.MetroLabel metroLabel6;
      private MetroFramework.Controls.MetroToggle mtIncident;
      private MetroFramework.Controls.MetroToggle mtMoppel;
      private MetroFramework.Controls.MetroToggle mtTavern;
      private MetroFramework.Controls.MetroToggle mtProduction;
      private MetroFramework.Controls.MetroToggle mtRQBot;
      private MetroFramework.Controls.MetroLabel metroLabel10;
      private MetroFramework.Controls.MetroTabPage mtpMisc;
      private MetroFramework.Controls.MetroLabel lblLanguage;
      private MetroFramework.Controls.MetroComboBox mcbLanguage;
      private MetroFramework.Controls.MetroTextBox mtbCustomUserAgent;
      private MetroFramework.Controls.MetroLabel lblCustomUserAgent;
      private MetroFramework.Controls.MetroButton mtbSave;
      private MetroFramework.Controls.MetroLabel lblRestartNeeded;
      private MetroFramework.Controls.MetroTabPage mtpData;
      private MetroFramework.Controls.MetroLabel lblDeleteData;
      private MetroFramework.Controls.MetroLabel lblChooseWorld;
      private MetroFramework.Controls.MetroComboBox mcbCitySelection;
      private MetroFramework.Controls.MetroButton mbDeleteData;
      private MetroFramework.Controls.MetroButton mbSaveReload;
      private MetroFramework.Controls.MetroLabel metroLabel11;
      private MetroFramework.Controls.MetroLabel lblAutoLogin;
      private MetroFramework.Controls.MetroToggle mtAutoLogin;
      private MetroFramework.Controls.MetroTabPage mtpPremium;
      private MetroFramework.Controls.MetroButton mbCheckSerial;
      private MetroFramework.Controls.MetroLabel lblSerialKey;
      private MetroFramework.Controls.MetroTextBox mtbSerialKey;
      private MetroFramework.Controls.MetroLabel metroLabel12;
      private MetroFramework.Controls.MetroToggle mtDarkMode;
      private MetroFramework.Controls.MetroLabel metroLabel14;
      private MetroFramework.Controls.MetroTextBox mtbIntervalIncident;
      private MetroFramework.Controls.MetroLabel metroLabel13;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.NumericUpDown nudMinProfit;
    private MetroFramework.Controls.MetroCheckBox mcbGuild;
    private MetroFramework.Controls.MetroCheckBox mcbFriends;
    private MetroFramework.Controls.MetroCheckBox mcbNeighbor;
    private MetroFramework.Controls.MetroLabel lblTarget;
    private MetroFramework.Controls.MetroLabel metroLabel15;
  }
}