using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Utils.BattleAI
{
   public class BattleAI
   {
      public List<Unit> SuggestedUnits = new List<Unit>();

      public List<Unit> OwnUnits = new List<Unit>();
      public List<Unit> EnemyUnits = new List<Unit>();

      public double[][] CalcIdentifier()
      {
         OpenFileDialog ofd = new OpenFileDialog();
         ofd.Filter = "Fights (fights.json)|fights.json";
         DialogResult dlgRes = ofd.ShowDialog();
         if (dlgRes == DialogResult.Cancel || dlgRes == DialogResult.Abort) return null;
         Testdata td = new Testdata(ofd.FileName);
         if (td.Fights.Count <= 0 || ListClass.UnitTypes.Count <= 0 || !td.Fights.All(e => e.Item1.Count > 0 && e.Item2.Count > 0)) return null;

         var t = new List<double[]>();
         //var training = new double[td.Fights.Count][]{ };
         Parallel.ForEach(td.Fights, fight =>
         {
            List<double> army = new List<double>();
            foreach (var own in fight.Item1)
               army.Add(GetID(own));
            if (fight.Item1.Count < 8)
            {
               int d = 8 - fight.Item1.Count;
               for (int i = 0; i < d; i++)
               {
                  army.Add(0d);
               }
            }

            foreach (var enemy in fight.Item2)
               army.Add(GetID(enemy));
            if (fight.Item2.Count < 16)
            {
               int d = 16 - fight.Item2.Count;
               for (int i = 0; i < d; i++)
               {
                  army.Add(0d);
               }
            }

            army.Add(fight.Item3 ? 1d : 0d);
            t.Add(army.ToArray());
         });

         //foreach (var fight in td.Fights)
         //{
         //   List<double> army = new List<double>();
         //   foreach (var own in fight.Item1)
         //      army.Add(GetID(own));
         //   if (fight.Item1.Count < 8)
         //   {
         //      int d = 8 - fight.Item1.Count;
         //      for (int i = 0; i < d; i++)
         //      {
         //         army.Add(0d);
         //      }
         //   }
         //   foreach (var enemy in fight.Item2)
         //      army.Add(GetID(enemy));
         //   if (fight.Item2.Count < 16)
         //   {
         //      int d = 16 - fight.Item2.Count;
         //      for (int i = 0; i < d; i++)
         //      {
         //         army.Add(0d);
         //      }
         //   }

         //   army.Add(fight.Item3 ? 1d : 0d);
         //   t.Add(army.ToArray());
         //}
         return t.ToArray();
      }
      private int GetID(Unit unit)
      {
         int id = 0;
         id += unit.unit.Sum(u=>u.currentHitpoints);
         foreach (var item in unit.unit.Select(u=>u.unitTypeId.ToCharArray()))
         {
            id += item.Sum(i=>i);
         }
         foreach (var item in unit.unit.Select(u=>u.bonuses))
         {
            id += item.Sum(i => i.value);
         }
         foreach (var item in unit.unit.Select(u=>u.abilities))
         {
            id+=item.Sum(i =>
            {
               if (i.value == null) 
                  return  0;
               if (int.TryParse(i.value.ToString(), out int val))
                  return val;
              return 0;
            });
         }
         UnitType type = ListClass.UnitTypes.Find(ut => ut.unitTypeId == unit.unit[0].unitTypeId);
         foreach (var item in type.unitClass.ToCharArray().ToList())
         {
            id += item;
         }
         id += type.range;
         id += type.movementPoints;
         id += (type.baseArmor * StaticData.AttackBoost[1]) + type.baseArmor;
         id += (type.baseDamage * StaticData.AttackBoost[0]) + type.baseDamage;
         var tau = type.attackBonus.units;
         id += (tau.fast + tau.heavy_melee + tau.light_melee + tau.long_ranged + tau.short_ranged);
         var tat = type.attackBonus.terrains;
         id += (tat.barbwire + tat.blockade_a + tat.blockade_b + tat.bushes + tat.crater + tat.forest + tat.hills + tat.house_a + tat.house_b + tat.house_c + tat.house_d + tat.house_e + tat.plain + tat.rocks + tat.rubble + tat.sandbagcircle + tat.swamp + tat.trench + tat.water);
         var tdu = type.defenseBonus.units;
         id += (tdu.fast + tdu.heavy_melee + tdu.light_melee + tdu.long_ranged + tdu.short_ranged);
         var tdt = type.defenseBonus.terrains;
         id += (tdt.barbwire + tdt.blockade_a + tdt.blockade_b + tdt.bushes + tdt.crater + tdt.forest + tdt.hills + tdt.house_a + tdt.house_b + tdt.house_c + tdt.house_d + tdt.house_e + tdt.plain + tdt.rocks + tdt.rubble + tdt.sandbagcircle + tdt.swamp + tdt.trench + tdt.water);
         return id;
      }
      public bool SearchBestUnits()
      {
         if (OwnUnits.Count <= 0 || EnemyUnits.Count <= 0 || ListClass.UnitTypes.Count <= 0) return false;
         foreach (Unit enemy in EnemyUnits)
         {
            //UnitType type = ListClass.UnitTypes.Find(ut => ut.unitTypeId == enemy.unit.unitTypeId);
         }
         return true;
      }
   }
}
