
namespace ForgeOfBots.Forms
{
   partial class FinProdForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinProdForm));
         this.panel1 = new System.Windows.Forms.Panel();
         this.lvFinProds = new System.Windows.Forms.CheckedListBox();
         this.btnCollectSelected = new System.Windows.Forms.Button();
         this.btnCollectAll = new System.Windows.Forms.Button();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.panel1.Controls.Add(this.lvFinProds);
         this.panel1.Location = new System.Drawing.Point(2, 2);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(286, 437);
         this.panel1.TabIndex = 1;
         // 
         // lvFinProds
         // 
         this.lvFinProds.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lvFinProds.FormattingEnabled = true;
         this.lvFinProds.Location = new System.Drawing.Point(0, 0);
         this.lvFinProds.Name = "lvFinProds";
         this.lvFinProds.Size = new System.Drawing.Size(286, 437);
         this.lvFinProds.TabIndex = 1;
         // 
         // btnCollectSelected
         // 
         this.btnCollectSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.btnCollectSelected.Location = new System.Drawing.Point(1, 440);
         this.btnCollectSelected.Name = "btnCollectSelected";
         this.btnCollectSelected.Size = new System.Drawing.Size(80, 23);
         this.btnCollectSelected.TabIndex = 2;
         this.btnCollectSelected.Tag = "GUI.FinProd.CollectSelected";
         this.btnCollectSelected.Text = "button1";
         this.btnCollectSelected.UseVisualStyleBackColor = true;
         this.btnCollectSelected.Click += new System.EventHandler(this.BtnCollectSelected_Click);
         // 
         // btnCollectAll
         // 
         this.btnCollectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCollectAll.Location = new System.Drawing.Point(195, 440);
         this.btnCollectAll.Name = "btnCollectAll";
         this.btnCollectAll.Size = new System.Drawing.Size(95, 23);
         this.btnCollectAll.TabIndex = 3;
         this.btnCollectAll.Tag = "GUI.FinProd.CollectAll";
         this.btnCollectAll.Text = "button2";
         this.btnCollectAll.UseVisualStyleBackColor = true;
         this.btnCollectAll.Click += new System.EventHandler(this.BtnCollectAll_Click);
         // 
         // FinProdForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(293, 473);
         this.Controls.Add(this.btnCollectAll);
         this.Controls.Add(this.btnCollectSelected);
         this.Controls.Add(this.panel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "FinProdForm";
         this.ShowIcon = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Tag = "GUI.Production.ListFinProd";
         this.Text = "GUI.Production.ListFinProd";
         this.Shown += new System.EventHandler(this.FinProdForm_Shown);
         this.panel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.CheckedListBox lvFinProds;
      private System.Windows.Forms.Button btnCollectSelected;
      private System.Windows.Forms.Button btnCollectAll;
   }
}