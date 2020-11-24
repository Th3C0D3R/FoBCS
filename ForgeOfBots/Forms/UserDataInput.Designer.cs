namespace ForgeOfBots.Forms
{
   partial class UserDataInput
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserDataInput));
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.txbUsername = new System.Windows.Forms.TextBox();
         this.txbPassword = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.cbServer = new System.Windows.Forms.ComboBox();
         this.button1 = new System.Windows.Forms.Button();
         this.label6 = new System.Windows.Forms.Label();
         this.mbExit = new MetroFramework.Controls.MetroButton();
         this.SuspendLayout();
         // 
         // label1
         // 
         resources.ApplyResources(this.label1, "label1");
         this.label1.ForeColor = System.Drawing.Color.Red;
         this.label1.Name = "label1";
         // 
         // label2
         // 
         resources.ApplyResources(this.label2, "label2");
         this.label2.Name = "label2";
         this.label2.Tag = "Forge of Bots v";
         // 
         // label3
         // 
         resources.ApplyResources(this.label3, "label3");
         this.label3.Name = "label3";
         // 
         // txbUsername
         // 
         resources.ApplyResources(this.txbUsername, "txbUsername");
         this.txbUsername.Name = "txbUsername";
         // 
         // txbPassword
         // 
         resources.ApplyResources(this.txbPassword, "txbPassword");
         this.txbPassword.Name = "txbPassword";
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
         // cbServer
         // 
         this.cbServer.FormattingEnabled = true;
         resources.ApplyResources(this.cbServer, "cbServer");
         this.cbServer.Name = "cbServer";
         // 
         // button1
         // 
         this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
         resources.ApplyResources(this.button1, "button1");
         this.button1.Name = "button1";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // label6
         // 
         resources.ApplyResources(this.label6, "label6");
         this.label6.Name = "label6";
         // 
         // mbExit
         // 
         this.mbExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         resources.ApplyResources(this.mbExit, "mbExit");
         this.mbExit.Name = "mbExit";
         // 
         // UserDataInput
         // 
         resources.ApplyResources(this, "$this");
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ControlBox = false;
         this.Controls.Add(this.mbExit);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.cbServer);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.txbPassword);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.txbUsername);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "UserDataInput";
         this.ShowIcon = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.ResumeLayout(false);
         this.PerformLayout();

      }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbUsername;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbServer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
      private MetroFramework.Controls.MetroButton mbExit;
   }
}