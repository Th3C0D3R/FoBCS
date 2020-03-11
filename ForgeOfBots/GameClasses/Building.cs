using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class EntityEx
   {
      public int id { get; set; }
      public int player_id { get; set; }
      public string cityentity_id { get; set; }
      public string name { get; set; } = "";
      public List<Available_Products> available_products { get; set; } = new List<Available_Products>();
      public string type { get; set; }
      public int x { get; set; }
      public int y { get; set; }
      public int connected { get; set; }
      public State state { get; set; }
      public int level { get; set; }
      public string __class__ { get; set; }
      public Unitslot[] unitSlots { get; set; }
      public int max_level { get; set; }
      public Bonus bonus { get; set; }
   }
   public static class GameClassHelper
   {
      public static EntityEx CopyFrom(Entity other)
      {
         EntityEx ex = new EntityEx
         {
            id = other.id,
            player_id = other.player_id,
            cityentity_id = other.cityentity_id,
            type = other.type,
            x = other.x,
            y = other.y,
            connected = other.connected,
            state = other.state,
            level = other.level,
            unitSlots = other.unitSlots,
            max_level = other.max_level,
            bonus = other.bonus
         };
         return ex;
      }
      public static bool hasOnlySupplyProduction(List<Available_Products> list)
      {
         int i = 0;
         bool[] checkBool = { false, false, false, false, false, false };
         foreach (Available_Products item in list)
         {
            if(item.product != null)
               if(item.product.resources != null)
               {
                  JObject o = (JObject)item.product.resources;
                  if(o.GetValue("supplies") != null)
                  {
                     checkBool[i] = true;
                     i += 1;
                  }
               }
         }
         return checkBool.All(x => { return x; });
      }
   }
}
