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
         this.tlpItem = new System.Windows.Forms.TableLayoutPanel();
         this.lblLocation = new MetroFramework.Controls.MetroLabel();
         this.lblRarity = new MetroFramework.Controls.MetroLabel();
         this.tlpItem.SuspendLayout();
         this.SuspendLayout();
         // 
         // tlpItem
         // 
         this.tlpItem.ColumnCount = 2;
         this.tlpItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tlpItem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tlpItem.Controls.Add(this.lblLocation, 0, 0);
         this.tlpItem.Controls.Add(this.lblRarity, 1, 0);
         this.tlpItem.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tlpItem.Location = new System.Drawing.Point(0, 0);
         this.tlpItem.Name = "tlpItem";
         this.tlpItem.RowCount = 1;
         this.tlpItem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tlpItem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tlpItem.Size = new System.Drawing.Size(211, 26);
         this.tlpItem.TabIndex = 0;
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
         this.Controls.Add(this.tlpItem);
         this.Name = "IncidentListItem";
         this.Size = new System.Drawing.Size(211, 26);
         this.tlpItem.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion
      private MetroFramework.Controls.MetroLabel lblRarity;
      public MetroFramework.Controls.MetroLabel lblLocation;
      public System.Windows.Forms.TableLayoutPanel tlpItem;
   }
}
