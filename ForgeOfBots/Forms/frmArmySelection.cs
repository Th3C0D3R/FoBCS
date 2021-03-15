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
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.GameClasses;

namespace ForgeOfBots.Forms
{
   public partial class frmArmySelection : Form
   {
      public ucBattle ArmySelection
      {
         get
         {
            return (ucBattle)Controls["armySelection"];
         }
         private set { }
      }
      public frmArmySelection()
      {
         InitializeComponent();
         ucBattle battle = new ucBattle();
         battle.Name = "armySelection";
         battle.Dock = DockStyle.Fill;
         battle.Margin = new Padding(0);
         Controls.Add(battle);
      }
      public void Fill()
      {
         ListViewGroup group = null;
         string lastEra = "";
         if (InvokeRequired)
         {
            Invoker.CallMethode(ArmySelection.lvSelectedArmy, () => ArmySelection.lvSelectedArmy.Items.Clear());
            Invoker.CallMethode(ArmySelection.lvArmy, () => ArmySelection.lvArmy.Items.Clear());
            Invoker.SetProperty(ArmySelection.lvArmy, () => ArmySelection.lvArmy.SmallImageList, ArmySelection.imgList);
            foreach (Unit item in ArmySelection.UnitList)
            {
               string era = ListClass.Eras.Find(re => re.era == (ListClass.UnitTypes.Find(ut => ut.unitTypeId == item.unit.unitTypeId).minEra)).era;
               if (lastEra != era)
               {
                  group = new ListViewGroup(era, HorizontalAlignment.Left);
               }
               ListViewItem lvi = new ListViewItem($"{item.name} ({item.count})", $"armyuniticons_50x50_{item.unit.unitTypeId}")
               {
                  Group = group,
                  Tag = item
               };
               Invoker.CallMethode(ArmySelection.lvArmy, () => ArmySelection.lvArmy.Items.Add(lvi));
               if (group != null && group.Header != lastEra)
               {
                  Invoker.CallMethode(ArmySelection.lvArmy, () => ArmySelection.lvArmy.Groups.Add(group));
                  lastEra = era;
               }
            }

            foreach (var unitType in ArmySelection.SelectedArmyTypes)
            {
               Invoker.CallMethode(ArmySelection.lvSelectedArmy, () =>
               {
                  Unit unit = ListClass.UnitList.Find(u => u.unit.unitTypeId == unitType && u.unit.currentHitpoints >= 10);
                  ArmySelection.lvSelectedArmy.Items.Add(unit);
               });
            }
         }
         else
         {
            ArmySelection.lvSelectedArmy.Items.Clear();
            ArmySelection.lvArmy.Items.Clear();
            ArmySelection.lvArmy.SmallImageList = ArmySelection.imgList;
            foreach (Unit item in ArmySelection.UnitList)
            {
               string era = ListClass.Eras.Find(re => re.era == (ListClass.UnitTypes.Find(ut => ut.unitTypeId == item.unit.unitTypeId).minEra)).era;
               if (lastEra != era)
               {
                  group = new ListViewGroup(era, HorizontalAlignment.Left);
               }
               ListViewItem lvi = new ListViewItem($"{item.name} ({item.count})", $"armyuniticons_50x50_{item.unit.unitTypeId}")
               {
                  Group = group,
                  Tag = item
               };
               ArmySelection.lvArmy.Items.Add(lvi);
               if (group != null && group.Header != lastEra)
               {
                  ArmySelection.lvArmy.Groups.Add(group);
                  lastEra = era;
               }
            }
            foreach (var unitType in ArmySelection.SelectedArmyTypes)
            {
               Unit unit = ListClass.UnitList.Find(u => u.unit.unitTypeId == unitType && u.unit.currentHitpoints >= 10);
               ArmySelection.lvSelectedArmy.Items.Add(unit);
            }
         }
      }

      private void FrmArmySelection_Shown(object sender, EventArgs e)
      {
         Fill();
      }
   }
}
