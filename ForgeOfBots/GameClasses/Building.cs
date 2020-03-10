using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
      public static EntityEx DeepCopy(Entity other)
      {
         using (MemoryStream ms = new MemoryStream())
         {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Context = new StreamingContext(StreamingContextStates.Clone);
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (EntityEx)formatter.Deserialize(ms);
         }
      }
      public static bool hasOnlySupplyProduction(List<Available_Products> list)
      {
         int i = 0;
         bool[] checkBool = { false, false, false, false, false, false };
         foreach (Available_Products item in list)
         {
            if(item.product != null)
               if(item.product.resources != null)
                  if(item.product.resources.supplies > 0)
                  {
                     checkBool[i] = true;
                     i += 1;
                  }
         }
         return checkBool.All(x => { return x; });
      }
   }
}
