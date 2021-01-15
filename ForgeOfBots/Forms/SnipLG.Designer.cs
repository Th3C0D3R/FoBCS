
using ForgeOfBots.LanguageFiles;

namespace ForgeOfBots.Forms
{
   partial class SnipLG
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnipLG));
         this.flpItems = new System.Windows.Forms.FlowLayoutPanel();
         this.SuspendLayout();
         // 
         // flpItems
         // 
         resources.ApplyResources(this.flpItems, "flpItems");
         this.flpItems.Name = "flpItems";
         // 
         // SnipLG
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.flpItems);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "SnipLG";
         this.ResumeLayout(false);

      }

      #endregion

      public System.Windows.Forms.FlowLayoutPanel flpItems;
   }
}