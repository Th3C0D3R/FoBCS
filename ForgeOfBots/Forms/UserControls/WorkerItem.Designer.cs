
namespace ForgeOfBots.Forms.UserControls
{
   partial class WorkerItem
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
         this.mpbProgress = new MetroFramework.Controls.MetroProgressBar();
         this.lblTitle = new System.Windows.Forms.Label();
         this.lblTextBeforeCounter = new System.Windows.Forms.Label();
         this.lblTextCounter = new System.Windows.Forms.Label();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.mpbProgress, 0, 2);
         this.tableLayoutPanel1.Controls.Add(this.lblTitle, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.lblTextBeforeCounter, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.lblTextCounter, 1, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 4;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.69072F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.69073F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.25389F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.364665F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(375, 122);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // mpbProgress
         // 
         this.tableLayoutPanel1.SetColumnSpan(this.mpbProgress, 2);
         this.mpbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mpbProgress.Location = new System.Drawing.Point(3, 87);
         this.mpbProgress.Name = "mpbProgress";
         this.mpbProgress.Size = new System.Drawing.Size(369, 22);
         this.mpbProgress.TabIndex = 0;
         // 
         // lblTitle
         // 
         this.tableLayoutPanel1.SetColumnSpan(this.lblTitle, 2);
         this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblTitle.Location = new System.Drawing.Point(3, 0);
         this.lblTitle.Name = "lblTitle";
         this.lblTitle.Size = new System.Drawing.Size(369, 42);
         this.lblTitle.TabIndex = 1;
         this.lblTitle.Text = "label1";
         this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblTextBeforeCounter
         // 
         this.lblTextBeforeCounter.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblTextBeforeCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblTextBeforeCounter.Location = new System.Drawing.Point(3, 42);
         this.lblTextBeforeCounter.Name = "lblTextBeforeCounter";
         this.lblTextBeforeCounter.Size = new System.Drawing.Size(181, 42);
         this.lblTextBeforeCounter.TabIndex = 2;
         this.lblTextBeforeCounter.Text = "label1";
         this.lblTextBeforeCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblTextCounter
         // 
         this.lblTextCounter.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblTextCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblTextCounter.Location = new System.Drawing.Point(190, 42);
         this.lblTextCounter.Name = "lblTextCounter";
         this.lblTextCounter.Size = new System.Drawing.Size(182, 42);
         this.lblTextCounter.TabIndex = 3;
         this.lblTextCounter.Text = "label1";
         this.lblTextCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // WorkerItem
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "WorkerItem";
         this.Size = new System.Drawing.Size(375, 122);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private MetroFramework.Controls.MetroProgressBar mpbProgress;
      private System.Windows.Forms.Label lblTitle;
      private System.Windows.Forms.Label lblTextBeforeCounter;
      private System.Windows.Forms.Label lblTextCounter;
   }
}
