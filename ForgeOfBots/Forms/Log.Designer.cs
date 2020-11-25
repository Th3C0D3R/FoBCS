namespace ForgeOfBots.Forms
{
   partial class Log
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Log));
         this.lbOutput = new System.Windows.Forms.ListBox();
         this.panel2 = new System.Windows.Forms.Panel();
         this.panel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // lbOutput
         // 
         resources.ApplyResources(this.lbOutput, "lbOutput");
         this.lbOutput.FormattingEnabled = true;
         this.lbOutput.Name = "lbOutput";
         this.lbOutput.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.lbOutput);
         resources.ApplyResources(this.panel2, "panel2");
         this.panel2.Name = "panel2";
         // 
         // Log
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.panel2);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "Log";
         this.ShowIcon = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
         this.panel2.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ListBox lbOutput;
      private System.Windows.Forms.Panel panel2;
   }
}