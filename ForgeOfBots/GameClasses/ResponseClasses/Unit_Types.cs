using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class UnitTypesRoot
   {
      public UnitType[] unit_types { get; set; }
   }

   public class UnitType
   {
      public string unitTypeId { get; set; }
      public string unitAssetName { get; set; }
      public string unitClass { get; set; }
      public string name { get; set; }
      public int hitpoints { get; set; }
      public int range { get; set; }
      public int initiative { get; set; }
      public int movementPoints { get; set; }
      public int baseDamage { get; set; }
      public int baseArmor { get; set; }
      public int healingSpeed { get; set; }
      public Attackbonus attackBonus { get; set; }
      public string minEra { get; set; }
      public Defensebonus defenseBonus { get; set; }
      public Movementcosts movementCosts { get; set; }
      public int points { get; set; }
      public string __class__ { get; set; }
   }

   public class Attackbonus
   {
      public Units units { get; set; }
      public Terrains terrains { get; set; }
   }

   public class Units
   {
      public int light_melee { get; set; }
      public int heavy_melee { get; set; }
      public int fast { get; set; }
      public int short_ranged { get; set; }
      public int long_ranged { get; set; }
   }

   public class Terrains
   {
      public int plain { get; set; }
      public int hills { get; set; }
      public int water { get; set; }
      public int bushes { get; set; }
      public int rocks { get; set; }
      public int swamp { get; set; }
      public int forest { get; set; }
      public int trench { get; set; }
      public int crater { get; set; }
      public int barbwire { get; set; }
      public int sandbagcircle { get; set; }
      public int house_a { get; set; }
      public int house_b { get; set; }
      public int house_c { get; set; }
      public int house_d { get; set; }
      public int house_e { get; set; }
      public int blockade_a { get; set; }
      public int blockade_b { get; set; }
      public int rubble { get; set; }
   }

   public class Defensebonus
   {
      public Units1 units { get; set; }
      public Terrains1 terrains { get; set; }
   }

   public class Units1
   {
      public int light_melee { get; set; }
      public int heavy_melee { get; set; }
      public int fast { get; set; }
      public int short_ranged { get; set; }
      public int long_ranged { get; set; }
   }

   public class Terrains1
   {
      public int plain { get; set; }
      public int hills { get; set; }
      public int water { get; set; }
      public int bushes { get; set; }
      public int rocks { get; set; }
      public int swamp { get; set; }
      public int forest { get; set; }
      public int trench { get; set; }
      public int crater { get; set; }
      public int barbwire { get; set; }
      public int sandbagcircle { get; set; }
      public int house_a { get; set; }
      public int house_b { get; set; }
      public int house_c { get; set; }
      public int house_d { get; set; }
      public int house_e { get; set; }
      public int blockade_a { get; set; }
      public int blockade_b { get; set; }
      public int rubble { get; set; }
   }

   public class Movementcosts
   {
      public object plain { get; set; }
      public object hills { get; set; }
      public object water { get; set; }
      public object bushes { get; set; }
      public object rocks { get; set; }
      public object swamp { get; set; }
      public object forest { get; set; }
      public object trench { get; set; }
      public object crater { get; set; }
      public object barbwire { get; set; }
      public object sandbagcircle { get; set; }
      public object house_a { get; set; }
      public object house_b { get; set; }
      public object house_c { get; set; }
      public object house_d { get; set; }
      public object house_e { get; set; }
      public object blockade_a { get; set; }
      public object blockade_b { get; set; }
      public object rubble { get; set; }
   }

}
