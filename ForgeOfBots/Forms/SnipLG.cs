using ForgeOfBots.Forms.UserControls;
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

namespace ForgeOfBots.Forms
{
   public partial class SnipLG : Form
   {
      public SnipLG()
      {
         InitializeComponent();
      }
      public void Add(LGSnipItem item)
      {
         if (flpItems.InvokeRequired)
            Invoker.CallMethode(flpItems, () => flpItems.Controls.Add(item));
         else
            flpItems.Controls.Add(item);
      }
   }
}
