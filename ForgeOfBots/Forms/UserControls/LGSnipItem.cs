using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Forms.UserControls
{
   public partial class LGSnipItem : UserControl
   {
      public string LG
      {
         get
         {
            return lblLG.Text;
         }
         set
         {
            if (InvokeRequired)
               Invoker.SetProperty(lblLG, () => lblLG.Text, value);
            else
               lblLG.Text = value;
         }
      }
      public string Profit
      {
         get
         {
            return lblProfit.Text;
         }
         set
         {
            if (InvokeRequired)
               Invoker.SetProperty(lblProfit, () => lblProfit.Text, value);
            else
               lblProfit.Text = value;
         }
      }
      public string SnipTag
      {
         get
         {
            return btnSnip.Tag.ToString();
         }
         set
         {
            if (InvokeRequired)
               Invoker.SetProperty(btnSnip, () => btnSnip.Tag, value);
            else
               btnSnip.Tag = value;
         }
      }

      private CustomEvent _SnipLG;
      public event CustomEvent SnipLG
      {
         add
         {
            if (_SnipLG == null || !_SnipLG.GetInvocationList().Contains(value))
               _SnipLG += value;
         }
         remove
         {
            _SnipLG -= value;
         }
      }

      public LGSnipItem()
      {
         InitializeComponent();
      }

      private void BtnSnip_Click(object sender, EventArgs e)
      {
         _SnipLG?.Invoke(null, SnipTag);
      }
   }
}
