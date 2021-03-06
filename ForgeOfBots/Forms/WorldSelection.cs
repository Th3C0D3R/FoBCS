﻿using ForgeOfBots.GameClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ForgeOfBots.Utils;

namespace ForgeOfBots.Forms
{
   public partial class WorldSelection : Form
   {
      private readonly Dictionary<string, string> ServerList = new Dictionary<string, string>();
      public WorldSelection(List<Tuple<string, string, WorldState>> serverList)
      {
         InitializeComponent();
         TopMost = true;
         Text += $" | FoBots v{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build} by TH3C0D3R";
         label2.Text += $"{StaticData.Version.Major}.{StaticData.Version.Minor}.{StaticData.Version.Build}";
         foreach (var item in serverList)
         {
            if (!cbCities.Items.Contains(item.Item1 + " (" + item.Item2 + ")"))
               cbCities.Items.Add(item.Item1 + " (" + item.Item2 + ")");
            if (!ServerList.ContainsKey(item.Item1))
               ServerList.Add(item.Item1, item.Item2);
         }
         cbCities.SelectedIndex = 0;
      }

      public event WorldDataEnteredEvent WorldDataEntered;
      public delegate void WorldDataEnteredEvent(Form that, string key, string value);

      private void button1_Click(object sender, EventArgs e)
      {
         string sel = cbCities.SelectedItem.ToString();
         string sKey = sel.Substring(0, sel.IndexOf(" "));
         string sValue = sel.Replace(sKey + " (", "").Replace(")", "");
         if (ServerList.ContainsKey(sKey) && ServerList[sKey] == sValue)
            WorldDataEntered?.Invoke(this, sKey, sValue);
      }
   }
}
