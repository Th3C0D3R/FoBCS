
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
         this.gbLog = new System.Windows.Forms.GroupBox();
         this.gbStatistic = new System.Windows.Forms.GroupBox();
         this.tpSocial = new System.Windows.Forms.TabPage();
         this.tpMessageCenter = new System.Windows.Forms.TabPage();
         this.tpChat = new System.Windows.Forms.TabPage();
         this.tpArmy = new System.Windows.Forms.TabPage();
         this.tpProduction = new System.Windows.Forms.TabPage();
         this.tpCity = new System.Windows.Forms.TabPage();
         this.tpSettings = new System.Windows.Forms.TabPage();
         this.metroPanel1.SuspendLayout();
         this.metroPanel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbminimize)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbFull)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbCLose)).BeginInit();
         this.mpMenu.SuspendLayout();
         this.tabControl1.SuspendLayout();
         this.tpDashbord.SuspendLayout();
         this.SuspendLayout();
         // 
         // metroPanel1
         // 
         resources.ApplyResources(this.metroPanel1, "metroPanel1");
         this.metroPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
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
         resources.ApplyResources(this.mlVersion, "mlVersion");
         this.mlVersion.CustomBackground = true;
         this.mlVersion.CustomForeColor = true;
         this.mlVersion.ForeColor = System.Drawing.Color.White;
         this.mlVersion.Name = "mlVersion";
         this.mlVersion.Tag = "v.";
         // 
         // mlTitle
         // 
         this.mlTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.mlTitle.CustomBackground = true;
         this.mlTitle.CustomForeColor = true;
         resources.ApplyResources(this.mlTitle, "mlTitle");
         this.mlTitle.FontSize = MetroFramework.MetroLabelSize.Tall;
         this.mlTitle.FontWeight = MetroFramework.MetroLabelWeight.Regular;
         this.mlTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(117)))), ((int)(((byte)(172)))));
         this.mlTitle.Name = "mlTitle";
         this.mlTitle.Tag = "Forge of Bots";
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
         // 
         // mpMenu
         // 
         this.mpMenu.Controls.Add(this.tabControl1);
         resources.ApplyResources(this.mpMenu, "mpMenu");
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
         this.tabControl1.Controls.Add(this.tpMessageCenter);
         this.tabControl1.Controls.Add(this.tpChat);
         this.tabControl1.Controls.Add(this.tpArmy);
         this.tabControl1.Controls.Add(this.tpProduction);
         this.tabControl1.Controls.Add(this.tpCity);
         this.tabControl1.Controls.Add(this.tpSettings);
         resources.ApplyResources(this.tabControl1, "tabControl1");
         this.tabControl1.ImageList = this.imgListTab;
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.SelectedIndex = 0;
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
         this.gbGoods.Name = "gbGoods";
         this.gbGoods.TabStop = false;
         this.gbGoods.Tag = "GUI.Tab.Dashbord.Goods";
         // 
         // gbLog
         // 
         resources.ApplyResources(this.gbLog, "gbLog");
         this.gbLog.Name = "gbLog";
         this.gbLog.TabStop = false;
         this.gbLog.Tag = "GUI.Tab.Dashbord.Log";
         // 
         // gbStatistic
         // 
         resources.ApplyResources(this.gbStatistic, "gbStatistic");
         this.gbStatistic.Name = "gbStatistic";
         this.gbStatistic.TabStop = false;
         this.gbStatistic.Tag = "GUI.Tab.Dashbord.Statistic";
         // 
         // tpSocial
         // 
         resources.ApplyResources(this.tpSocial, "tpSocial");
         this.tpSocial.Name = "tpSocial";
         this.tpSocial.Tag = "GUI.Tab.Social";
         this.tpSocial.UseVisualStyleBackColor = true;
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
         // tpSettings
         // 
         resources.ApplyResources(this.tpSettings, "tpSettings");
         this.tpSettings.Name = "tpSettings";
         this.tpSettings.Tag = "GUI.Tab.Settings";
         this.tpSettings.UseVisualStyleBackColor = true;
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
         this.metroPanel1.ResumeLayout(false);
         this.metroPanel1.PerformLayout();
         this.metroPanel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pbminimize)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbFull)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbCLose)).EndInit();
         this.mpMenu.ResumeLayout(false);
         this.tabControl1.ResumeLayout(false);
         this.tpDashbord.ResumeLayout(false);
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
   }
}