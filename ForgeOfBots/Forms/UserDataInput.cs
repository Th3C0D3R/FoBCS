using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ForgeOfBots.Utils;

namespace ForgeOfBots.Forms
{
   public partial class UserDataInput : Form
   {
      public UserDataInput(Dictionary<string, string> serverList)
      {
         InitializeComponent();
         Text += $" | FoBots v{StaticData.Version.Major}.{StaticData.Version.Minor} by TH3C0D3R";
         label2.Text = $"{label2.Tag}{StaticData.Version.Major}.{StaticData.Version.Minor}";
         cbServer.Items.Clear();
         foreach (var item in serverList)
         {
            cbServer.Items.Add(item.Key + " (" + item.Value + ")");
         }
         cbServer.SelectedIndex = 0;
      }

      public event UserdataEnteredEvent UserDataEntered;
      public delegate void UserdataEnteredEvent(string username, string password, string server);

      private void button1_Click(object sender, EventArgs e)
      {
         string server = cbServer.SelectedItem.ToString();
         UserDataEntered?.Invoke(txbUsername.Text, txbPassword.Text, server.Substring(0, server.IndexOf(" ")));
      }
   }
}
