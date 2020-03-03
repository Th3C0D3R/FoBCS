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
         // Main
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1489, 1082);
         this.Controls.Add(this.lbCookies);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "Main";
         this.Text = "Form1";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
         this.ResumeLayout(false);

      }

        #endregion

        private System.Windows.Forms.ListBox lbCookies;
    }
}

