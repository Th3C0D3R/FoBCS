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
         this.lbLog = new System.Windows.Forms.ListBox();
         this.sStripStatus = new System.Windows.Forms.StatusStrip();
         this.tsslProgressState = new System.Windows.Forms.ToolStripStatusLabel();
         this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
         this.button1 = new System.Windows.Forms.Button();
         this.sStripStatus.SuspendLayout();
         this.SuspendLayout();
         // 
         // lbLog
         // 
         this.lbLog.FormattingEnabled = true;
         this.lbLog.ItemHeight = 20;
         this.lbLog.Location = new System.Drawing.Point(12, 12);
         this.lbLog.Name = "lbLog";
         this.lbLog.Size = new System.Drawing.Size(1454, 984);
         this.lbLog.TabIndex = 0;
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
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(12, 1005);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(143, 42);
         this.button1.TabIndex = 2;
         this.button1.Text = "button1";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // Main
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1489, 1082);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.sStripStatus);
         this.Controls.Add(this.lbLog);
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

        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.StatusStrip sStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslProgressState;
        private System.Windows.Forms.ToolStripProgressBar tspbProgress;
        private System.Windows.Forms.Button button1;
    }
}

