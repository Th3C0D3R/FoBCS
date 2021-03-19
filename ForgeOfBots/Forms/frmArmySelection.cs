using ForgeOfBots.Forms.UserControls;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            foreach (KeyValuePair<string, List<Unit>> item in ArmySelection.UnitList)
            {
               if (lastEra != item.Key)
               {
                  group = new ListViewGroup(item.Key, HorizontalAlignment.Left);
               }
               foreach (Unit unit in item.Value)
               {
                  ListViewItem lvi = new ListViewItem($"{unit.name} ({unit.count})", $"armyuniticons_50x50_{unit.unit.unitTypeId}")
                  {
                     Group = group,
                     Tag = unit
                  };
                  Invoker.CallMethode(ArmySelection.lvArmy, () => ArmySelection.lvArmy.Items.Add(lvi));
                  if (group != null && group.Header != lastEra)
                  {
                     Invoker.CallMethode(ArmySelection.lvArmy, () => ArmySelection.lvArmy.Groups.Add(group));
                     lastEra = item.Key;
                  }
               }
               
            }
            List<int> addedIDs = new List<int>();
            foreach (var unitType in ArmySelection.SelectedArmyTypes)
            {
               Invoker.CallMethode(ArmySelection.lvSelectedArmy, () =>
               {
                  Unit unit = ListClass.UnitList.Find(u => u.unit.unitTypeId == unitType);
                  ArmySelection.lvSelectedArmy.Items.Add(unit);
               });
            }
         }
         else
         {
            ArmySelection.lvSelectedArmy.Items.Clear();
            ArmySelection.lvArmy.Items.Clear();
            ArmySelection.lvArmy.SmallImageList = ArmySelection.imgList;
            foreach (KeyValuePair<string, List<Unit>> item in ArmySelection.UnitList)
            {
               if (lastEra != item.Key)
               {
                  group = new ListViewGroup(item.Key, HorizontalAlignment.Left);
               }
               foreach (Unit unit in item.Value)
               {
                  ListViewItem lvi = new ListViewItem($"{unit.name} ({unit.count})", $"armyuniticons_50x50_{unit.unit.unitTypeId}")
                  {
                     Group = group,
                     Tag = unit
                  };
                  ArmySelection.lvArmy.Items.Add(lvi);
                  if (group != null && group.Header != lastEra)
                  {
                     ArmySelection.lvArmy.Groups.Add(group);
                     lastEra = item.Key;
                  }
               }
            }
            foreach (var unitType in ArmySelection.SelectedArmyTypes)
            {
               Unit unit = ListClass.UnitList.Find(u => u.unit.unitTypeId == unitType);
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
