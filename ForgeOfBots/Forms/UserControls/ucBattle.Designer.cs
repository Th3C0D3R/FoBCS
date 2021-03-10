
namespace ForgeOfBots.Forms.UserControls
{
   partial class ucBattle
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
         this.components = new System.ComponentModel.Container();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.btnFight = new System.Windows.Forms.Button();
         this.pnlWave1 = new System.Windows.Forms.Panel();
         this.lblWave = new System.Windows.Forms.Label();
         this.lvWave = new System.Windows.Forms.ListView();
         this.imgList = new System.Windows.Forms.ImageList(this.components);
         this.pnlContent = new System.Windows.Forms.Panel();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.lblStageType = new System.Windows.Forms.Label();
         this.lblProgress = new System.Windows.Forms.Label();
         this.lblChest = new System.Windows.Forms.Label();
         this.pnlOwnArmy = new System.Windows.Forms.Panel();
         this.label1 = new System.Windows.Forms.Label();
         this.lvOwnArmy = new System.Windows.Forms.ListView();
         this.tableLayoutPanel1.SuspendLayout();
         this.pnlWave1.SuspendLayout();
         this.pnlContent.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.pnlOwnArmy.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.13718F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.86282F));
         this.tableLayoutPanel1.Controls.Add(this.btnFight, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.pnlWave1, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.pnlContent, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.pnlOwnArmy, 0, 3);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 4;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(430, 245);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // btnFight
         // 
         this.btnFight.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnFight.Location = new System.Drawing.Point(331, 44);
         this.btnFight.Margin = new System.Windows.Forms.Padding(0);
         this.btnFight.Name = "btnFight";
         this.btnFight.Size = new System.Drawing.Size(99, 66);
         this.btnFight.TabIndex = 0;
         this.btnFight.Tag = "GUI.Battle.Fight";
         this.btnFight.Text = "GUI.Battle.Fight";
         this.btnFight.UseVisualStyleBackColor = true;
         this.btnFight.Click += new System.EventHandler(this.BtnFight_Click);
         // 
         // pnlWave1
         // 
         this.pnlWave1.Controls.Add(this.lblWave);
         this.pnlWave1.Controls.Add(this.lvWave);
         this.pnlWave1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pnlWave1.Location = new System.Drawing.Point(0, 44);
         this.pnlWave1.Margin = new System.Windows.Forms.Padding(0);
         this.pnlWave1.Name = "pnlWave1";
         this.tableLayoutPanel1.SetRowSpan(this.pnlWave1, 2);
         this.pnlWave1.Size = new System.Drawing.Size(331, 132);
         this.pnlWave1.TabIndex = 3;
         this.pnlWave1.Visible = false;
         // 
         // lblWave
         // 
         this.lblWave.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblWave.Location = new System.Drawing.Point(0, 0);
         this.lblWave.Margin = new System.Windows.Forms.Padding(3);
         this.lblWave.Name = "lblWave";
         this.lblWave.Size = new System.Drawing.Size(75, 132);
         this.lblWave.TabIndex = 2;
         this.lblWave.Tag = "GUI.Battle.UC.Wave";
         this.lblWave.Text = "lblWave1";
         this.lblWave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lvWave
         // 
         this.lvWave.AutoArrange = false;
         this.lvWave.Dock = System.Windows.Forms.DockStyle.Right;
         this.lvWave.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.lvWave.HideSelection = false;
         this.lvWave.Location = new System.Drawing.Point(75, 0);
         this.lvWave.Margin = new System.Windows.Forms.Padding(0);
         this.lvWave.MultiSelect = false;
         this.lvWave.Name = "lvWave";
         this.lvWave.Size = new System.Drawing.Size(256, 132);
         this.lvWave.SmallImageList = this.imgList;
         this.lvWave.TabIndex = 1;
         this.lvWave.UseCompatibleStateImageBehavior = false;
         this.lvWave.View = System.Windows.Forms.View.SmallIcon;
         // 
         // imgList
         // 
         this.imgList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
         this.imgList.ImageSize = new System.Drawing.Size(16, 16);
         this.imgList.TransparentColor = System.Drawing.Color.Transparent;
         // 
         // pnlContent
         // 
         this.pnlContent.Controls.Add(this.tableLayoutPanel2);
         this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pnlContent.Location = new System.Drawing.Point(0, 0);
         this.pnlContent.Margin = new System.Windows.Forms.Padding(0);
         this.pnlContent.Name = "pnlContent";
         this.pnlContent.Size = new System.Drawing.Size(331, 44);
         this.pnlContent.TabIndex = 5;
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 3;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.14804F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.61934F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.90155F));
         this.tableLayoutPanel2.Controls.Add(this.lblStageType, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.lblProgress, 1, 0);
         this.tableLayoutPanel2.Controls.Add(this.lblChest, 2, 0);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 2;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(331, 44);
         this.tableLayoutPanel2.TabIndex = 0;
         // 
         // lblStageType
         // 
         this.lblStageType.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblStageType.Location = new System.Drawing.Point(3, 0);
         this.lblStageType.Name = "lblStageType";
         this.lblStageType.Size = new System.Drawing.Size(64, 22);
         this.lblStageType.TabIndex = 0;
         this.lblStageType.Tag = "";
         this.lblStageType.Text = "Stage";
         this.lblStageType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lblProgress
         // 
         this.lblProgress.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblProgress.Location = new System.Drawing.Point(73, 0);
         this.lblProgress.Name = "lblProgress";
         this.lblProgress.Size = new System.Drawing.Size(145, 22);
         this.lblProgress.TabIndex = 1;
         this.lblProgress.Text = "label2";
         this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // lblChest
         // 
         this.lblChest.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblChest.Location = new System.Drawing.Point(224, 0);
         this.lblChest.Name = "lblChest";
         this.lblChest.Size = new System.Drawing.Size(104, 22);
         this.lblChest.TabIndex = 2;
         this.lblChest.Text = "label2";
         this.lblChest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // pnlOwnArmy
         // 
         this.pnlOwnArmy.Controls.Add(this.label1);
         this.pnlOwnArmy.Controls.Add(this.lvOwnArmy);
         this.pnlOwnArmy.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pnlOwnArmy.Location = new System.Drawing.Point(0, 176);
         this.pnlOwnArmy.Margin = new System.Windows.Forms.Padding(0);
         this.pnlOwnArmy.Name = "pnlOwnArmy";
         this.pnlOwnArmy.Size = new System.Drawing.Size(331, 69);
         this.pnlOwnArmy.TabIndex = 7;
         // 
         // label1
         // 
         this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.label1.Location = new System.Drawing.Point(0, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(75, 69);
         this.label1.TabIndex = 0;
         this.label1.Tag = "GUI.Battle.OwnArmySuggestion";
         this.label1.Text = "lblOwnArmy";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // lvOwnArmy
         // 
         this.lvOwnArmy.Dock = System.Windows.Forms.DockStyle.Right;
         this.lvOwnArmy.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.lvOwnArmy.HideSelection = false;
         this.lvOwnArmy.Location = new System.Drawing.Point(75, 0);
         this.lvOwnArmy.Margin = new System.Windows.Forms.Padding(0);
         this.lvOwnArmy.Name = "lvOwnArmy";
         this.lvOwnArmy.Size = new System.Drawing.Size(256, 69);
         this.lvOwnArmy.TabIndex = 1;
         this.lvOwnArmy.UseCompatibleStateImageBehavior = false;
         this.lvOwnArmy.View = System.Windows.Forms.View.SmallIcon;
         // 
         // ucBattle
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Margin = new System.Windows.Forms.Padding(0);
         this.Name = "ucBattle";
         this.Size = new System.Drawing.Size(430, 245);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.pnlWave1.ResumeLayout(false);
         this.pnlContent.ResumeLayout(false);
         this.tableLayoutPanel2.ResumeLayout(false);
         this.pnlOwnArmy.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Button btnFight;
      private System.Windows.Forms.Panel pnlWave1;
      private System.Windows.Forms.Label lblWave;
      private System.Windows.Forms.Panel pnlContent;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.Label lblStageType;
      private System.Windows.Forms.Label lblProgress;
      private System.Windows.Forms.Panel pnlOwnArmy;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label lblChest;
      public System.Windows.Forms.ListView lvWave;
      public System.Windows.Forms.ListView lvOwnArmy;
      public System.Windows.Forms.ImageList imgList;
   }
}
