
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
         this.flpItems.Dock = System.Windows.Forms.DockStyle.Fill;
         this.flpItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
         this.flpItems.Location = new System.Drawing.Point(0, 0);
         this.flpItems.Name = "flpItems";
         this.flpItems.Size = new System.Drawing.Size(488, 489);
         this.flpItems.TabIndex = 0;
         // 
         // WorkerList
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(488, 489);
         this.Controls.Add(this.flpItems);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "WorkerList";
         this.Text = "WorkerList";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.FlowLayoutPanel flpItems;
   }
}