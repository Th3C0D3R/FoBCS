
namespace ForgeOfBots.Forms.UserControls
{
   partial class LGSnipItem
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
         this.lblProfit = new System.Windows.Forms.Label();
         this.lblLG = new System.Windows.Forms.Label();
         this.btnSnip = new System.Windows.Forms.Button();
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
         this.panel1.Size = new System.Drawing.Size(422, 37);
         this.panel1.TabIndex = 1;
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.5F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.5F));
         this.tableLayoutPanel1.Controls.Add(this.lblProfit, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblLG, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.btnSnip, 2, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(422, 37);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // lblProfit
         // 
         this.lblProfit.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblProfit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblProfit.Location = new System.Drawing.Point(192, 0);
         this.lblProfit.Name = "lblProfit";
         this.lblProfit.Size = new System.Drawing.Size(110, 37);
         this.lblProfit.TabIndex = 3;
         this.lblProfit.Text = "PLACEHOLDER";
         this.lblProfit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblLG
         // 
         this.lblLG.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblLG.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblLG.Location = new System.Drawing.Point(3, 0);
         this.lblLG.Name = "lblLG";
         this.lblLG.Size = new System.Drawing.Size(183, 37);
         this.lblLG.TabIndex = 2;
         this.lblLG.Text = "PLACEHOLDER";
         this.lblLG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // btnSnip
         // 
         this.btnSnip.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnSnip.Location = new System.Drawing.Point(308, 3);
         this.btnSnip.Name = "btnSnip";
         this.btnSnip.Size = new System.Drawing.Size(111, 31);
         this.btnSnip.TabIndex = 4;
         this.btnSnip.Text = "Snip";
         this.btnSnip.UseVisualStyleBackColor = true;
         this.btnSnip.Click += new System.EventHandler(this.BtnSnip_Click);
         // 
         // LGSnipItem
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.panel1);
         this.Name = "LGSnipItem";
         this.Size = new System.Drawing.Size(422, 37);
         this.panel1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label lblProfit;
      private System.Windows.Forms.Label lblLG;
      private System.Windows.Forms.Button btnSnip;
   }
}
