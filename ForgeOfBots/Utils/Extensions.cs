using ForgeOfBots.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.Utils
{
   public static class Extensions
   {
      public static bool HasCityID(this List<Tuple<string, string, WorldState>> list, string ID)
      {
         foreach (Tuple<string, string, WorldState> item in list)
            if (item.Item1 == ID) return true;
         return false;
      }
      public static List<Tuple<string, string, WorldState>> ChangeTuple(this List<Tuple<string, string, WorldState>> list, string id = null, string name = null, WorldState state = WorldState.none)
      {
         List<Tuple<string, string, WorldState>> newList = new List<Tuple<string, string, WorldState>>();
         foreach (Tuple<string, string, WorldState> item in list)
         {
            if (id != null)
            {
               if (id == item.Item1)
               {
                  if (name != null)
                  {
                     if (state != WorldState.none)
                        newList.Add(new Tuple<string, string, WorldState>(id, name, state));
                     else
                        newList.Add(new Tuple<string, string, WorldState>(id, name, item.Item3));
                  }
                  else
                     newList.Add(new Tuple<string, string, WorldState>(id, item.Item2, item.Item3));
               }
               else
                  newList.Add(item);
            }
            else
               newList.Add(item);
         }
         return newList;
      }
   }
}
