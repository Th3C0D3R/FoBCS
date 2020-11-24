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
         this.panel1 = new System.Windows.Forms.Panel();
         this.btnDeAttach = new System.Windows.Forms.Button();
         this.panel2 = new System.Windows.Forms.Panel();
         this.panel3 = new System.Windows.Forms.Panel();
         this.panel1.SuspendLayout();
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
         // panel1
         // 
         this.panel1.Controls.Add(this.btnDeAttach);
         resources.ApplyResources(this.panel1, "panel1");
         this.panel1.Name = "panel1";
         // 
         // btnDeAttach
         // 
         resources.ApplyResources(this.btnDeAttach, "btnDeAttach");
         this.btnDeAttach.Name = "btnDeAttach";
         this.btnDeAttach.UseVisualStyleBackColor = true;
         this.btnDeAttach.Click += new System.EventHandler(this.btnDeAttach_Click);
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.lbOutput);
         resources.ApplyResources(this.panel2, "panel2");
         this.panel2.Name = "panel2";
         // 
         // panel3
         // 
         resources.ApplyResources(this.panel3, "panel3");
         this.panel3.Name = "panel3";
         // 
         // Log
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel3);
         this.Controls.Add(this.panel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "Log";
         this.ShowIcon = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
         this.LocationChanged += new System.EventHandler(this.Log_LocationChanged);
         this.panel1.ResumeLayout(false);
         this.panel2.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ListBox lbOutput;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button btnDeAttach;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Panel panel3;
   }
}