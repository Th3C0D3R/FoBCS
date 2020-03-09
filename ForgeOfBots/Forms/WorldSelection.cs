using ForgeOfBots.GameClasses;
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
      private Dictionary<string, string> ServerList = new Dictionary<string, string>();
      public WorldSelection(List<Tuple<string, string, WorldState>> serverList)
      {
         InitializeComponent();
         foreach (var item in serverList)
         {
            cbCities.Items.Add(item.Item1 + " (" + item.Item2 + ")");
            ServerList.Add(item.Item1, item.Item2);
         }
         cbCities.SelectedIndex = 0;
      }

      public event WorldDataEnteredEvent WorldDataEntered;
      public delegate void WorldDataEnteredEvent(Form that,string key, string value);

      private void button1_Click(object sender, EventArgs e)
      {
         string sel = cbCities.SelectedItem.ToString();
         string sKey = sel.Substring(0, sel.IndexOf(" "));
         string sValue = sel.Replace(sKey + " (", "").Replace(")", "");
         if (ServerList.ContainsKey(sKey) && ServerList[sKey] == sValue)
            WorldDataEntered?.Invoke(this,sKey, sValue);
      }
   }
}
