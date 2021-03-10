using ForgeOfBots.DataHandler;
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
   public partial class ucBattle : UserControl
   {
      private CustomEvent _Fight;
      public event CustomEvent Fight
      {
         add
         {
            if (_Fight == null || !_Fight.GetInvocationList().ToList().Contains(value))
               _Fight += value;
         }
         remove
         {
            _Fight -= value;
         }
      }

      public int ID { get; set; } = -1;
      public ImageList UnitImageList
      {
         get
         {
            return imgList;
         }
         set
         {
            imgList = value;
         }
      }

      public ucBattle()
      {
         InitializeComponent();
         ToggleWave1(false);
         ToggleOwn(false);
         lblWave.Text = i18n.getString("GUI.Battle.UC.Wave");
         lblChest.Visible = false;
      }
      public void FillControl(string type)
      {
         Invoker.SetProperty(lblProgress, () => lblProgress.Text, $"{((GEXHelper.GetCurrentState % 2) == 1 ? (GEXHelper.GetCurrentState + 1) / 2 : GEXHelper.GetCurrentState / 2)}");
         Invoker.SetProperty(lblStageType, () => lblStageType.Text, type);
      }

      public void FillWave1(ListViewItem lvi)
      {
         ListViewGroup group = new ListViewGroup(i18n.getString("GUI.Battle.UC.Wave1"), HorizontalAlignment.Left);
         if (lvWave.Groups.Count == 0)
         {
            group.Name = "wave1";
            lvi.Group = group;
         }
         else
         {
            lvi.Group = lvWave.Groups["wave1"];
         }
         lvWave.Items.Add(lvi);
         if(lvWave.Groups.Count == 0)
            lvWave.Groups.Add(group);
         ToggleWave1(true);
      }
      public void FillWave2(ListViewItem lvi)
      {
         ListViewGroup group = new ListViewGroup(i18n.getString("GUI.Battle.UC.Wave2"), HorizontalAlignment.Left);
         if (lvWave.Groups.Count == 1)
         {
            group.Name = "wave2";
            lvi.Group = group;
         }
         else
         {
            lvi.Group = lvWave.Groups["wave2"];
         }
         lvWave.Items.Add(lvi);
         if (lvWave.Groups.Count == 1)
            lvWave.Groups.Add(group);
         ToggleWave1(true);
      }
      public void FillSuggestion(ListViewItem lvi)
      {
         lvOwnArmy.Items.Add(lvi);
         ToggleOwn(true);
      }
      public void FillChest(int getChestID)
      {
         ID = getChestID;
         lblChest.Text = i18n.getString("GUI.Battle.GEX.Chest");
         btnFight.Text = i18n.getString("GUI.Battle.GEX.ChestOpen");
      }

      public void ToggleWave1(bool visible)
      {
         Invoker.SetProperty(pnlWave1, () => pnlWave1.Visible, visible);
      }
      public void ToggleOwn(bool visible)
      {
         Invoker.SetProperty(pnlOwnArmy, () => pnlOwnArmy.Visible, visible);
      }
      public void ToggleChest(bool visible)
      {
         if (visible)
         {
            ToggleWave1(false);
            ToggleOwn(false);
            Invoker.SetProperty(lblChest, () => lblChest.Visible, visible);
         }
      }


      private void BtnFight_Click(object sender, EventArgs e)
      {
         if (ID > -1)
            _Fight?.Invoke(null, ID);
         btnFight.Text = i18n.getString(btnFight.Tag.ToString());
      }
   }
}
