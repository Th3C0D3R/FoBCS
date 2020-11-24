namespace UCPremiumLibrary
{
   partial class UCPremium
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

      #region Vom Komponenten-Designer generierter Code

      /// <summary> 
      /// Erforderliche Methode für die Designerunterstützung. 
      /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
      /// </summary>
      private void InitializeComponent()
      {
         this.lblName = new MetroFramework.Controls.MetroLabel();
         this.mtToggle = new MetroFramework.Controls.MetroToggle();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // lblName
         // 
         this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblName.Location = new System.Drawing.Point(3, 0);
         this.lblName.Name = "lblName";
         this.lblName.Size = new System.Drawing.Size(87, 22);
         this.lblName.TabIndex = 0;
         this.lblName.Text = "metroLabel1";
         this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // mtToggle
         // 
         this.mtToggle.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mtToggle.Location = new System.Drawing.Point(96, 3);
         this.mtToggle.Name = "mtToggle";
         this.mtToggle.Size = new System.Drawing.Size(88, 16);
         this.mtToggle.TabIndex = 1;
         this.mtToggle.Text = "Aus";
         this.mtToggle.UseVisualStyleBackColor = true;
         this.mtToggle.CheckedChanged += new System.EventHandler(this.mtToggle_CheckedChanged);
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.mtToggle, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(187, 22);
         this.tableLayoutPanel1.TabIndex = 2;
         // 
         // UCPremium
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "UCPremium";
         this.Size = new System.Drawing.Size(187, 22);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private MetroFramework.Controls.MetroLabel lblName;
      private MetroFramework.Controls.MetroToggle mtToggle;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
   }
}
