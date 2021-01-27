using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.GameClasses.ResponseClasses;
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
      public LGSnip LGSnip { get; set; }
      public LGSnipItem()
      {
         InitializeComponent();
      }
   }
}
