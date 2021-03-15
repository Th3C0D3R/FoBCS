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
      public List<Unit> UnitList { get; set; } = new List<Unit>();
      public ucBattle()
      {
         InitializeComponent();
      }
      public void FillArmyList(List<Unit> unitlist)
      {
         if (imgList == null) return;
         if (unitlist.Count <= 0) return;
         UnitList = unitlist;
      }
      public void FillSelectedArmy(List<string> selectredArmy)
      {
         if (imgList == null) return;
         if (UnitList.Count <= 0) return;
         SelectedArmyTypes = selectredArmy;
      }

      private void BtnArmySubmit_Click(object sender, EventArgs e)
      {
         SelectedArmyTypes.Clear();
         foreach (var item in lvSelectedArmy.Items)
         {
            Unit unit = (Unit)item;
            SelectedArmyTypes.Add(unit.unit.unitTypeId);
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
   }
}
