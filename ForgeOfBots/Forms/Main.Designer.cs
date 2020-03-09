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
         this.sStripStatus.SuspendLayout();
         this.toolStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // sStripStatus
         // 
         this.sStripStatus.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.sStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslProgressState,
            this.tspbProgress});
         this.sStripStatus.Location = new System.Drawing.Point(0, 1050);
         this.sStripStatus.Name = "sStripStatus";
         this.sStripStatus.Size = new System.Drawing.Size(1489, 32);
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
         this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
         this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLogin,
            this.toolStripSeparator1,
            this.tsbSettings});
         this.toolStrip1.Location = new System.Drawing.Point(0, 0);
         this.toolStrip1.Name = "toolStrip1";
         this.toolStrip1.Size = new System.Drawing.Size(1489, 34);
         this.toolStrip1.TabIndex = 3;
         this.toolStrip1.Text = "toolStrip1";
         // 
         // tsbLogin
         // 
         this.tsbLogin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
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
         this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
         // 
         // tsbSettings
         // 
         this.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
         this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
         this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.tsbSettings.Name = "tsbSettings";
         this.tsbSettings.Size = new System.Drawing.Size(80, 29);
         this.tsbSettings.Text = "Settings";
         this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
         // 
         // Main
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1489, 1082);
         this.Controls.Add(this.toolStrip1);
         this.Controls.Add(this.sStripStatus);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "Main";
         this.Text = "Form1";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
         this.sStripStatus.ResumeLayout(false);
         this.sStripStatus.PerformLayout();
         this.toolStrip1.ResumeLayout(false);
         this.toolStrip1.PerformLayout();
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
    }
}

