namespace ForgeOfBots.Forms
{
   partial class About
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
         this.lblTitle = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.lblVersion = new System.Windows.Forms.Label();
         this.lblInternalVersion = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // lblTitle
         // 
         resources.ApplyResources(this.lblTitle, "lblTitle");
         this.lblTitle.Name = "lblTitle";
         // 
         // label3
         // 
         resources.ApplyResources(this.label3, "label3");
         this.label3.Name = "label3";
         // 
         // label4
         // 
         resources.ApplyResources(this.label4, "label4");
         this.label4.Name = "label4";
         // 
         // label5
         // 
         resources.ApplyResources(this.label5, "label5");
         this.label5.Name = "label5";
         // 
         // lblVersion
         // 
         resources.ApplyResources(this.lblVersion, "lblVersion");
         this.lblVersion.Name = "lblVersion";
         // 
         // lblInternalVersion
         // 
         resources.ApplyResources(this.lblInternalVersion, "lblInternalVersion");
         this.lblInternalVersion.Name = "lblInternalVersion";
         // 
         // About
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.lblInternalVersion);
         this.Controls.Add(this.lblVersion);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.lblTitle);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "About";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Label lblTitle;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label lblVersion;
      private System.Windows.Forms.Label lblInternalVersion;
   }
}