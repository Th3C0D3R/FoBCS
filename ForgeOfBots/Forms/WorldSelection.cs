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
   public partial class WorldSelection : Form
   {
      public WorldSelection(Dictionary<string, string> serverList)
      {
         InitializeComponent();
         foreach (var item in serverList)
         {
            cbCities.Items.Add(item.Key + " (" + item.Value + ")");
         }
         cbCities.SelectedIndex = 0;
      }

      public event UserdataEnteredEvent UserDataEntered;
      public delegate void UserdataEnteredEvent(string username, string password, string server);

      private void button1_Click(object sender, EventArgs e)
      {

      }
   }
}
