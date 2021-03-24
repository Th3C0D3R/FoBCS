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
using ForgeOfBots.GameClasses.ResponseClasses;
using Settings = ForgeOfBots.Utils.Settings;
using System.Globalization;
using System.Threading;

namespace ForgeOfBots.Forms.UserControls
{
   public partial class ucBattle : UserControl
   {
      private CustomEvent _SubmitArmy;
      public event CustomEvent SubmitArmy
      {
         add
         {
            if (_SubmitArmy == null || !_SubmitArmy.GetInvocationList().ToList().Contains(value))
               _SubmitArmy += value;
         }
         remove
         {
            _SubmitArmy -= value;
         }
      }
      public List<string> SelectedArmyTypes = new List<string>();
      public ImageList imgList { get; set; } = null;
      public Dictionary<string, List<Unit>> UnitList { get; set; } = new Dictionary<string, List<Unit>>();
      public ucBattle()
      {
         InitializeComponent();
         lblSelectedArmy.Text = $"{i18n.getString(lblSelectedArmy.Tag.ToString())}";
         btnArmySubmit.Text = $"{i18n.getString(btnArmySubmit.Tag.ToString())}";
      }
      public void FillArmyList(Dictionary<string, List<Unit>> unitlist)
      {
         if (unitlist.Count <= 0) return;
         UnitList = unitlist;
      }
      public void FillSelectedArmy(List<string> selectredArmy)
      {
         SelectedArmyTypes = selectredArmy;
      }

      private void BtnArmySubmit_Click(object sender, EventArgs e)
      {
         SelectedArmyTypes.Clear();
         foreach (var item in lvSelectedArmy.Items)
         {
            Unit unit = (Unit)item;
            SelectedArmyTypes.Add(unit.unit[0].unitTypeId);
         }
         _SubmitArmy?.Invoke(null, SelectedArmyTypes);
      }

      private void CmsArmySelection_Opening(object sender, CancelEventArgs e)
      {
         ContextMenuStrip cms = (ContextMenuStrip)sender;
         if (cms.SourceControl.Name == lvSelectedArmy.Name)
         {
            cmsArmySelection.Items[0].Text = i18n.getString("GUI.Army.Deselect");
            cmsArmySelection.Items[0].Tag = "deselect";
         }
         else if (cms.SourceControl.Name == lvArmy.Name)
         {
            cmsArmySelection.Items[0].Text = i18n.getString("GUI.Army.Select");
            cmsArmySelection.Items[0].Tag = "select";
         }
      }

      private void TsmiSelect_Click(object sender, EventArgs e)
      {
         ToolStripItem tsi = (ToolStripItem)sender;
         if (tsi.Tag.ToString() == "deselect")
         {
            if (lvSelectedArmy.SelectedItems.Count > 0)
            {
               lvSelectedArmy.Items.RemoveAt(lvSelectedArmy.SelectedIndex);
            }
         }
         else if (tsi.Tag.ToString() == "select")
         {
            if (lvSelectedArmy.Items.Count < 8)
            {
               if (lvArmy.SelectedItems.Count > 0)
               {
                  Unit unit = (Unit)lvArmy.SelectedItems[0].Tag;
                  lvSelectedArmy.Items.Add(unit);
               }
            }
         }
      }

      private void LvSelectedArmy_MouseDown(object sender, MouseEventArgs e)
      {
         lvSelectedArmy.SelectedIndex = lvSelectedArmy.IndexFromPoint(e.X, e.Y);
      }
   }
}
