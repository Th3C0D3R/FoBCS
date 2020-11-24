namespace ForgeOfBots.Forms.UserControls
{
   partial class IncidentListItem
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
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.lblLocation = new MetroFramework.Controls.MetroLabel();
         this.lblRarity = new MetroFramework.Controls.MetroLabel();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.lblLocation, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblRarity, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(211, 26);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // lblLocation
         // 
         this.lblLocation.BackColor = System.Drawing.SystemColors.Control;
         this.lblLocation.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblLocation.Location = new System.Drawing.Point(3, 0);
         this.lblLocation.Name = "lblLocation";
         this.lblLocation.Size = new System.Drawing.Size(99, 26);
         this.lblLocation.TabIndex = 0;
         this.lblLocation.Text = "Location";
         this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblRarity
         // 
         this.lblRarity.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblRarity.Location = new System.Drawing.Point(108, 0);
         this.lblRarity.Name = "lblRarity";
         this.lblRarity.Size = new System.Drawing.Size(100, 26);
         this.lblRarity.TabIndex = 1;
         this.lblRarity.Text = "Rarity";
         this.lblRarity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // IncidentListItem
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "IncidentListItem";
         this.Size = new System.Drawing.Size(211, 26);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private MetroFramework.Controls.MetroLabel lblLocation;
      private MetroFramework.Controls.MetroLabel lblRarity;
   }
}
