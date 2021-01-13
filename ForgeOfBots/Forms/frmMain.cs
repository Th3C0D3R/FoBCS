using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Forms
{
   public partial class frmMain : Form
   {
      public const int WM_NCLBUTTONDOWN = 0xA1;
      public const int HT_CAPTION = 0x2;
      [DllImport("user32.dll")]
      public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
      [DllImport("user32.dll")]
      public static extern bool ReleaseCapture();


      public frmMain()
      {
         InitializeComponent();
         FillText();
      }

      private void MetroPanel1_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
         }
      }

      private void PbCLose_Click(object sender, EventArgs e)
      {
#if DEBUG
         Close();
#else
         Environment.Exit(0);
#endif
      }
      private void Pbminimize_Click(object sender, EventArgs e)
      {
         WindowState = FormWindowState.Minimized;
      }
      private void PbFull_Click(object sender, EventArgs e)
      {
         WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
      }














      private void FillText()
      {
         tpDashbord.Text = i18n.getString(tpDashbord.Tag.ToString());
         tpSocial.Text = i18n.getString(tpSocial.Tag.ToString());
         tpMessageCenter.Text = i18n.getString(tpMessageCenter.Tag.ToString());
         tpChat.Text = i18n.getString(tpChat.Tag.ToString());
         tpArmy.Text = i18n.getString(tpArmy.Tag.ToString());
         tpProduction.Text = i18n.getString(tpProduction.Tag.ToString());
         tpCity.Text = i18n.getString(tpCity.Tag.ToString());
         tpSettings.Text = i18n.getString(tpSettings.Tag.ToString());
         gbLog.Text = i18n.getString(gbLog.Tag.ToString());
         gbStatistic.Text = i18n.getString(gbStatistic.Tag.ToString());
         gbGoods.Text = i18n.getString(gbGoods.Tag.ToString());
         mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
      }
   }
}
