namespace ForgeOfBots.Forms.UserControls
{
   partial class ProdListItem
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
         this.panel1 = new System.Windows.Forms.Panel();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.lblState = new System.Windows.Forms.Label();
         this.lblProduct = new System.Windows.Forms.Label();
         this.lblBuilding = new System.Windows.Forms.Label();
         this.panel1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.tableLayoutPanel1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(236, 34);
         this.panel1.TabIndex = 0;
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.5F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.5F));
         this.tableLayoutPanel1.Controls.Add(this.lblState, 2, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblProduct, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblBuilding, 0, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(236, 34);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // lblState
         // 
         this.lblState.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblState.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblState.Location = new System.Drawing.Point(173, 0);
         this.lblState.Name = "lblState";
         this.lblState.Size = new System.Drawing.Size(60, 34);
         this.lblState.TabIndex = 4;
         this.lblState.Text = "PLACEHOLDER";
         this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblProduct
         // 
         this.lblProduct.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblProduct.Location = new System.Drawing.Point(109, 0);
         this.lblProduct.Name = "lblProduct";
         this.lblProduct.Size = new System.Drawing.Size(58, 34);
         this.lblProduct.TabIndex = 3;
         this.lblProduct.Text = "PLACEHOLDER";
         this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblBuilding
         // 
         this.lblBuilding.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblBuilding.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblBuilding.Location = new System.Drawing.Point(3, 0);
         this.lblBuilding.Name = "lblBuilding";
         this.lblBuilding.Size = new System.Drawing.Size(100, 34);
         this.lblBuilding.TabIndex = 2;
         this.lblBuilding.Text = "PLACEHOLDER";
         this.lblBuilding.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ProdListItem
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.panel1);
         this.Name = "ProdListItem";
         this.Size = new System.Drawing.Size(236, 34);
         this.panel1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label lblState;
      private System.Windows.Forms.Label lblProduct;
      private System.Windows.Forms.Label lblBuilding;
   }
}
