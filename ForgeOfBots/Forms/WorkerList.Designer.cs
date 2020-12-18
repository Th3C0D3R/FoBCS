
using ForgeOfBots.LanguageFiles;

namespace ForgeOfBots.Forms
{
   partial class WorkerList
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerList));
         this.flpItems = new System.Windows.Forms.FlowLayoutPanel();
         this.SuspendLayout();
         // 
         // flpItems
         // 
         resources.ApplyResources(this.flpItems, "flpItems");
         this.flpItems.Name = "flpItems";
         // 
         // WorkerList
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.flpItems);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "WorkerList";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.FlowLayoutPanel flpItems;
   }
}