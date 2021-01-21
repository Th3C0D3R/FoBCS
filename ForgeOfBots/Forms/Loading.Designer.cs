
namespace ForgeOfBots.Forms
{
   partial class Loading
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Loading));
         this.pnlLoading = new System.Windows.Forms.Panel();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.lblPleaseLogin = new System.Windows.Forms.Label();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
         this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
         this.pbminimize = new System.Windows.Forms.PictureBox();
         this.pbFull = new System.Windows.Forms.PictureBox();
         this.pbCLose = new System.Windows.Forms.PictureBox();
         this.mlVersion = new MetroFramework.Controls.MetroLabel();
         this.mlTitle = new MetroFramework.Controls.MetroLabel();
         this.pnlLoading.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         this.metroPanel1.SuspendLayout();
         this.metroPanel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pbminimize)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbFull)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbCLose)).BeginInit();
         this.SuspendLayout();
         // 
         // pnlLoading
         // 
         this.pnlLoading.Controls.Add(this.tableLayoutPanel2);
         this.pnlLoading.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pnlLoading.Location = new System.Drawing.Point(0, 42);
         this.pnlLoading.Margin = new System.Windows.Forms.Padding(2);
         this.pnlLoading.Name = "pnlLoading";
         this.pnlLoading.Size = new System.Drawing.Size(456, 295);
         this.pnlLoading.TabIndex = 13;
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 1;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.Controls.Add(this.lblPleaseLogin, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 1);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 2;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(456, 295);
         this.tableLayoutPanel2.TabIndex = 2;
         // 
         // lblPleaseLogin
         // 
         this.lblPleaseLogin.AutoSize = true;
         this.lblPleaseLogin.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblPleaseLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
         this.lblPleaseLogin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.lblPleaseLogin.Location = new System.Drawing.Point(2, 0);
         this.lblPleaseLogin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.lblPleaseLogin.Name = "lblPleaseLogin";
         this.lblPleaseLogin.Size = new System.Drawing.Size(452, 147);
         this.lblPleaseLogin.TabIndex = 1;
         this.lblPleaseLogin.Tag = "GUI.Loading.Wait";
         this.lblPleaseLogin.Text = "Please wait...";
         this.lblPleaseLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // pictureBox1
         // 
         this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
         this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
         this.pictureBox1.Location = new System.Drawing.Point(2, 149);
         this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(452, 144);
         this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.pictureBox1.TabIndex = 0;
         this.pictureBox1.TabStop = false;
         // 
         // metroPanel1
         // 
         this.metroPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.metroPanel1.Controls.Add(this.metroPanel2);
         this.metroPanel1.Controls.Add(this.mlVersion);
         this.metroPanel1.Controls.Add(this.mlTitle);
         this.metroPanel1.CustomBackground = true;
         this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.metroPanel1.HorizontalScrollbarBarColor = true;
         this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
         this.metroPanel1.HorizontalScrollbarSize = 10;
         this.metroPanel1.Location = new System.Drawing.Point(0, 0);
         this.metroPanel1.Name = "metroPanel1";
         this.metroPanel1.Size = new System.Drawing.Size(456, 42);
         this.metroPanel1.TabIndex = 14;
         this.metroPanel1.VerticalScrollbarBarColor = true;
         this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
         this.metroPanel1.VerticalScrollbarSize = 10;
         // 
         // metroPanel2
         // 
         this.metroPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.metroPanel2.Controls.Add(this.pbminimize);
         this.metroPanel2.Controls.Add(this.pbFull);
         this.metroPanel2.Controls.Add(this.pbCLose);
         this.metroPanel2.CustomBackground = true;
         this.metroPanel2.ForeColor = System.Drawing.Color.Transparent;
         this.metroPanel2.HorizontalScrollbarBarColor = true;
         this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
         this.metroPanel2.HorizontalScrollbarSize = 10;
         this.metroPanel2.Location = new System.Drawing.Point(339, 6);
         this.metroPanel2.Name = "metroPanel2";
         this.metroPanel2.Size = new System.Drawing.Size(112, 31);
         this.metroPanel2.TabIndex = 1;
         this.metroPanel2.VerticalScrollbarBarColor = true;
         this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
         this.metroPanel2.VerticalScrollbarSize = 10;
         // 
         // pbminimize
         // 
         this.pbminimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.pbminimize.Image = global::ForgeOfBots.Properties.Resources.minus;
         this.pbminimize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.pbminimize.Location = new System.Drawing.Point(40, 0);
         this.pbminimize.Name = "pbminimize";
         this.pbminimize.Size = new System.Drawing.Size(32, 30);
         this.pbminimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.pbminimize.TabIndex = 3;
         this.pbminimize.TabStop = false;
         this.pbminimize.Visible = false;
         // 
         // pbFull
         // 
         this.pbFull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.pbFull.Image = global::ForgeOfBots.Properties.Resources.plus;
         this.pbFull.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.pbFull.Location = new System.Drawing.Point(2, 0);
         this.pbFull.Name = "pbFull";
         this.pbFull.Size = new System.Drawing.Size(32, 30);
         this.pbFull.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.pbFull.TabIndex = 2;
         this.pbFull.TabStop = false;
         this.pbFull.Visible = false;
         // 
         // pbCLose
         // 
         this.pbCLose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.pbCLose.Image = global::ForgeOfBots.Properties.Resources.error;
         this.pbCLose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.pbCLose.Location = new System.Drawing.Point(77, 0);
         this.pbCLose.Name = "pbCLose";
         this.pbCLose.Size = new System.Drawing.Size(32, 30);
         this.pbCLose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.pbCLose.TabIndex = 1;
         this.pbCLose.TabStop = false;
         // 
         // mlVersion
         // 
         this.mlVersion.AutoSize = true;
         this.mlVersion.CustomBackground = true;
         this.mlVersion.CustomForeColor = true;
         this.mlVersion.ForeColor = System.Drawing.Color.White;
         this.mlVersion.Location = new System.Drawing.Point(126, 9);
         this.mlVersion.Name = "mlVersion";
         this.mlVersion.Size = new System.Drawing.Size(17, 19);
         this.mlVersion.TabIndex = 4;
         this.mlVersion.Tag = "v.";
         this.mlVersion.Text = "v.";
         // 
         // mlTitle
         // 
         this.mlTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
         this.mlTitle.CustomBackground = true;
         this.mlTitle.CustomForeColor = true;
         this.mlTitle.Dock = System.Windows.Forms.DockStyle.Left;
         this.mlTitle.FontSize = MetroFramework.MetroLabelSize.Tall;
         this.mlTitle.FontWeight = MetroFramework.MetroLabelWeight.Regular;
         this.mlTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(117)))), ((int)(((byte)(172)))));
         this.mlTitle.Location = new System.Drawing.Point(0, 0);
         this.mlTitle.Name = "mlTitle";
         this.mlTitle.Size = new System.Drawing.Size(120, 42);
         this.mlTitle.TabIndex = 3;
         this.mlTitle.Tag = "Forge of Bots";
         this.mlTitle.Text = "Forge of Bots";
         this.mlTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // Loading
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(456, 337);
         this.Controls.Add(this.pnlLoading);
         this.Controls.Add(this.metroPanel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "Loading";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Loading";
         this.pnlLoading.ResumeLayout(false);
         this.tableLayoutPanel2.ResumeLayout(false);
         this.tableLayoutPanel2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         this.metroPanel1.ResumeLayout(false);
         this.metroPanel1.PerformLayout();
         this.metroPanel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pbminimize)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbFull)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pbCLose)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel pnlLoading;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.Label lblPleaseLogin;
      private System.Windows.Forms.PictureBox pictureBox1;
      private MetroFramework.Controls.MetroPanel metroPanel1;
      private MetroFramework.Controls.MetroPanel metroPanel2;
      private System.Windows.Forms.PictureBox pbminimize;
      private System.Windows.Forms.PictureBox pbFull;
      private System.Windows.Forms.PictureBox pbCLose;
      private MetroFramework.Controls.MetroLabel mlVersion;
      private MetroFramework.Controls.MetroLabel mlTitle;
   }
}