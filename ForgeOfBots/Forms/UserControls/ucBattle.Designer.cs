
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
         this.btnArmySubmit = new System.Windows.Forms.Button();
         this.panel1 = new System.Windows.Forms.Panel();
         this.lvArmy = new System.Windows.Forms.ListView();
         this.cmsArmySelection = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.tsmiSelect = new System.Windows.Forms.ToolStripMenuItem();
         this.lvSelectedArmy = new System.Windows.Forms.ListBox();
         this.lblSelectedArmy = new System.Windows.Forms.Label();
         this.tableLayoutPanel1.SuspendLayout();
         this.panel1.SuspendLayout();
         this.cmsArmySelection.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.67442F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.32558F));
         this.tableLayoutPanel1.Controls.Add(this.btnArmySubmit, 1, 3);
         this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.lvSelectedArmy, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.lblSelectedArmy, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 4;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.69388F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.79592F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.59184F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.5102F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(430, 245);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // btnArmySubmit
         // 
         this.btnArmySubmit.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnArmySubmit.Location = new System.Drawing.Point(290, 206);
         this.btnArmySubmit.Margin = new System.Windows.Forms.Padding(0);
         this.btnArmySubmit.Name = "btnArmySubmit";
         this.btnArmySubmit.Size = new System.Drawing.Size(140, 39);
         this.btnArmySubmit.TabIndex = 0;
         this.btnArmySubmit.Tag = "GUI.Army.Submit";
         this.btnArmySubmit.Text = "GUI.Army.Submit";
         this.btnArmySubmit.UseVisualStyleBackColor = true;
         this.btnArmySubmit.Click += new System.EventHandler(this.BtnArmySubmit_Click);
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.lvArmy);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Margin = new System.Windows.Forms.Padding(0);
         this.panel1.Name = "panel1";
         this.tableLayoutPanel1.SetRowSpan(this.panel1, 4);
         this.panel1.Size = new System.Drawing.Size(290, 245);
         this.panel1.TabIndex = 1;
         // 
         // lvArmy
         // 
         this.lvArmy.ContextMenuStrip = this.cmsArmySelection;
         this.lvArmy.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lvArmy.HideSelection = false;
         this.lvArmy.Location = new System.Drawing.Point(0, 0);
         this.lvArmy.Margin = new System.Windows.Forms.Padding(0);
         this.lvArmy.Name = "lvArmy";
         this.lvArmy.Size = new System.Drawing.Size(290, 245);
         this.lvArmy.TabIndex = 0;
         this.lvArmy.Tag = "";
         this.lvArmy.UseCompatibleStateImageBehavior = false;
         this.lvArmy.View = System.Windows.Forms.View.SmallIcon;
         // 
         // cmsArmySelection
         // 
         this.cmsArmySelection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelect});
         this.cmsArmySelection.Name = "cmsArmySelection";
         this.cmsArmySelection.Size = new System.Drawing.Size(174, 26);
         this.cmsArmySelection.Opening += new System.ComponentModel.CancelEventHandler(this.CmsArmySelection_Opening);
         // 
         // tsmiSelect
         // 
         this.tsmiSelect.Name = "tsmiSelect";
         this.tsmiSelect.Size = new System.Drawing.Size(173, 22);
         this.tsmiSelect.Tag = "GUI.Army.DeSelect";
         this.tsmiSelect.Text = "GUI.Army.DeSelect";
         this.tsmiSelect.Click += new System.EventHandler(this.TsmiSelect_Click);
         // 
         // lvSelectedArmy
         // 
         this.lvSelectedArmy.ContextMenuStrip = this.cmsArmySelection;
         this.lvSelectedArmy.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lvSelectedArmy.FormattingEnabled = true;
         this.lvSelectedArmy.Location = new System.Drawing.Point(290, 36);
         this.lvSelectedArmy.Margin = new System.Windows.Forms.Padding(0);
         this.lvSelectedArmy.Name = "lvSelectedArmy";
         this.tableLayoutPanel1.SetRowSpan(this.lvSelectedArmy, 2);
         this.lvSelectedArmy.Size = new System.Drawing.Size(140, 170);
         this.lvSelectedArmy.TabIndex = 2;
         this.lvSelectedArmy.Tag = "";
         this.lvSelectedArmy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LvSelectedArmy_MouseDown);
         // 
         // lblSelectedArmy
         // 
         this.lblSelectedArmy.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblSelectedArmy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.lblSelectedArmy.Location = new System.Drawing.Point(293, 0);
         this.lblSelectedArmy.Name = "lblSelectedArmy";
         this.lblSelectedArmy.Size = new System.Drawing.Size(134, 36);
         this.lblSelectedArmy.TabIndex = 3;
         this.lblSelectedArmy.Tag = "GUI.Army.SelectedArmy";
         this.lblSelectedArmy.Text = "GUI.Army.SelectedArmy";
         this.lblSelectedArmy.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
         this.panel1.ResumeLayout(false);
         this.cmsArmySelection.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Button btnArmySubmit;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label lblSelectedArmy;
      private System.Windows.Forms.ContextMenuStrip cmsArmySelection;
      private System.Windows.Forms.ToolStripMenuItem tsmiSelect;
      public System.Windows.Forms.ListBox lvSelectedArmy;
      public System.Windows.Forms.ListView lvArmy;
   }
}
