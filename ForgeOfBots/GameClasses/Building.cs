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
      public JToken state { get; set; }
      public int level { get; set; }
      public string __class__ { get; set; }
      public JToken unitSlots { get; set; }
      public int max_level { get; set; }
      public JToken bonus { get; set; }
   }
   public static class GameClassHelper
   {
      public static EntityEx CopyFrom(JToken other)
      {
         EntityEx ex = new EntityEx
         {
            id = other["id"] != null ? other["id"].ToObject<int>() : -1,
            player_id = other["player_id"] != null ? other["player_id"].ToObject<int>() : -1,
            cityentity_id = other["cityentity_id"] != null ? other["cityentity_id"].ToString() : "",
            type = other["type"] != null ? other["type"].ToString() : "",
            x = other["x"] != null ? other["x"].ToObject<int>() : -1,
            y = other["y"] != null ? other["y"].ToObject<int>() : -1,
            connected = other["connected"] != null ? other["connected"].ToObject<int>() : -1,
            state = other["state"] != null ? other["state"] : null,
            level = other["level"] != null ? other["level"].ToObject<int>() : -1,
            unitSlots = other["unitSlots"] != null ? other["unitSlots"] : null,
            max_level = other["max_level"] != null ? other["max_level"].ToObject<int>() : -1,
            bonus = other["bonus"] != null ? other["bonus"] : null,
         };
         return ex;
      }
      public static bool hasOnlySupplyProduction(List<Available_Products> list)
      {
         int i = 0;
         bool[] checkBool = { false, false, false, false, false, false };
         foreach (Available_Products item in list)
         {
            if (item.product != null)
               if (item.product.resources != null)
               {
                  JObject o = (JObject)item.product.resources;
                  if (o.GetValue("supplies") != null)
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
