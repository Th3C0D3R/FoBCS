
namespace ForgeOfBots.Forms.UserControls
{
   partial class ucIgnorePlayer
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
         this.pbRemove = new System.Windows.Forms.PictureBox();
         this.label1 = new System.Windows.Forms.Label();
         this.tableLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbRemove)).BeginInit();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.07595F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.92405F));
         this.tableLayoutPanel1.Controls.Add(this.pbRemove, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(158, 24);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // pbRemove
         // 
         this.pbRemove.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pbRemove.Image = global::ForgeOfBots.Properties.Resources.x_mark;
         this.pbRemove.InitialImage = global::ForgeOfBots.Properties.Resources.x_mark;
         this.pbRemove.Location = new System.Drawing.Point(136, 0);
         this.pbRemove.Margin = new System.Windows.Forms.Padding(0);
         this.pbRemove.Name = "pbRemove";
         this.pbRemove.Size = new System.Drawing.Size(22, 24);
         this.pbRemove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.pbRemove.TabIndex = 0;
         this.pbRemove.TabStop = false;
         this.pbRemove.Click += new System.EventHandler(this.PbRemove_Click);
         // 
         // label1
         // 
         this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.label1.Location = new System.Drawing.Point(3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(130, 24);
         this.label1.TabIndex = 1;
         this.label1.Text = "label1";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ucIgnorePlayer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "ucIgnorePlayer";
         this.Size = new System.Drawing.Size(158, 24);
         this.tableLayoutPanel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pbRemove)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.PictureBox pbRemove;
      private System.Windows.Forms.Label label1;
   }
}
