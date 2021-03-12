using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class Unit
   {
      public string name { get; set; } = "";
      public int count { get; set; } = 0;
      public GameUnit unit { get; set; } = null;
      public List<int> ids { get; set; } = new List<int>();

      public override string ToString()
      {
         return name;
      }
   }
   public class ArmyRoot
   {
      public string __class__ { get; set; }
      public string requestClass { get; set; }
      public int requestId { get; set; }
      public string requestMethod { get; set; }
      public Army responseData { get; set; } = null;
   }

   public class Army
   {
      public string __class__ { get; set; }
      public Count[] counts { get; set; }
      public GameUnit[] units { get; set; }
   }

   public class Count
   {
      public string __class__ { get; set; }
      public int unattached { get; set; }
      public string unitTypeId { get; set; }
   }

   public class GameUnit
   {
      public string __class__ { get; set; }
      public Ability[] abilities { get; set; }
      public Bonus[] bonuses { get; set; }
      public int currentHitpoints { get; set; }
      public int fully_healed_at { get; set; }
      public bool healing_disabled { get; set; }
      public bool isArenaDefending { get; set; }
      public bool is_attacking { get; set; }
      public bool is_defending { get; set; }
      public int next_healing_step_size { get; set; }
      public int ownerId { get; set; }
      public int unitId { get; set; }
      public string unitTypeId { get; set; }
   }

   public class Ability
   {
      public string __class__ { get; set; }
      public string description { get; set; }
      public string icon { get; set; }
      public string name { get; set; }
      public string type { get; set; }
      public int used_at_step { get; set; }
      public object value { get; set; }
      public string[] terrains { get; set; }
   }

   public class Bonus
   {
      public string __class__ { get; set; }
      public string type { get; set; }
      public int value { get; set; }
   }

}
