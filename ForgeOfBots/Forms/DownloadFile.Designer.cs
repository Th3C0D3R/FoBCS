
namespace ForgeOfBots.Forms
{
   partial class DownloadFile
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadFile));
         this.mpbDownloadProgress = new MetroFramework.Controls.MetroProgressBar();
         this.lblDownloadFile = new System.Windows.Forms.Label();
         this.lblProgress = new System.Windows.Forms.Label();
         this.lblProgressValue = new System.Windows.Forms.Label();
         this.lblFilenameValue = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // mpbDownloadProgress
         // 
         this.mpbDownloadProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.mpbDownloadProgress.HideProgressText = false;
         this.mpbDownloadProgress.Location = new System.Drawing.Point(9, 62);
         this.mpbDownloadProgress.Margin = new System.Windows.Forms.Padding(0);
         this.mpbDownloadProgress.Name = "mpbDownloadProgress";
         this.mpbDownloadProgress.Size = new System.Drawing.Size(372, 23);
         this.mpbDownloadProgress.Step = 1;
         this.mpbDownloadProgress.TabIndex = 0;
         this.mpbDownloadProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblDownloadFile
         // 
         this.lblDownloadFile.AutoSize = true;
         this.lblDownloadFile.Location = new System.Drawing.Point(12, 9);
         this.lblDownloadFile.Name = "lblDownloadFile";
         this.lblDownloadFile.Size = new System.Drawing.Size(122, 13);
         this.lblDownloadFile.TabIndex = 1;
         this.lblDownloadFile.Tag = "GUI.Download.Filename";
         this.lblDownloadFile.Text = "GUI.Download.Filename";
         // 
         // lblProgress
         // 
         this.lblProgress.AutoSize = true;
         this.lblProgress.Location = new System.Drawing.Point(12, 31);
         this.lblProgress.Name = "lblProgress";
         this.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.lblProgress.Size = new System.Drawing.Size(121, 13);
         this.lblProgress.TabIndex = 2;
         this.lblProgress.Tag = "GUI.Download.Progress";
         this.lblProgress.Text = "GUI.Download.Progress";
         // 
         // lblProgressValue
         // 
         this.lblProgressValue.Location = new System.Drawing.Point(155, 31);
         this.lblProgressValue.Name = "lblProgressValue";
         this.lblProgressValue.Size = new System.Drawing.Size(223, 13);
         this.lblProgressValue.TabIndex = 3;
         this.lblProgressValue.Tag = "";
         this.lblProgressValue.Text = "Progress";
         this.lblProgressValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // lblFilenameValue
         // 
         this.lblFilenameValue.Location = new System.Drawing.Point(155, 9);
         this.lblFilenameValue.Name = "lblFilenameValue";
         this.lblFilenameValue.Size = new System.Drawing.Size(226, 13);
         this.lblFilenameValue.TabIndex = 4;
         this.lblFilenameValue.Tag = "";
         this.lblFilenameValue.Text = "Filename";
         this.lblFilenameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // DownloadFile
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(393, 94);
         this.Controls.Add(this.lblFilenameValue);
         this.Controls.Add(this.lblProgressValue);
         this.Controls.Add(this.lblProgress);
         this.Controls.Add(this.lblDownloadFile);
         this.Controls.Add(this.mpbDownloadProgress);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "DownloadFile";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Tag = "GUI.Download.Header";
         this.Text = "GUI.Download.Header";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private MetroFramework.Controls.MetroProgressBar mpbDownloadProgress;
      private System.Windows.Forms.Label lblDownloadFile;
      private System.Windows.Forms.Label lblProgress;
      private System.Windows.Forms.Label lblProgressValue;
      private System.Windows.Forms.Label lblFilenameValue;
   }
}