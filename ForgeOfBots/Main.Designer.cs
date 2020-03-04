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
         this.lbCookies = new System.Windows.Forms.ListBox();
         this.sStripStatus = new System.Windows.Forms.StatusStrip();
         this.tsslProgressState = new System.Windows.Forms.ToolStripStatusLabel();
         this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
         this.sStripStatus.SuspendLayout();
         this.SuspendLayout();
         // 
         // lbCookies
         // 
         this.lbCookies.FormattingEnabled = true;
         this.lbCookies.ItemHeight = 20;
         this.lbCookies.Location = new System.Drawing.Point(12, 12);
         this.lbCookies.Name = "lbCookies";
         this.lbCookies.Size = new System.Drawing.Size(621, 384);
         this.lbCookies.TabIndex = 0;
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
         // Main
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1489, 1082);
         this.Controls.Add(this.sStripStatus);
         this.Controls.Add(this.lbCookies);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "Main";
         this.Text = "Form1";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
         this.sStripStatus.ResumeLayout(false);
         this.sStripStatus.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

        #endregion

        private System.Windows.Forms.ListBox lbCookies;
        private System.Windows.Forms.StatusStrip sStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslProgressState;
        private System.Windows.Forms.ToolStripProgressBar tspbProgress;
    }
}

