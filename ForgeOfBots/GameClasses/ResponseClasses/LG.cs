using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class SnipRoot
   {
      public SnipResponse responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class SnipResponse
   {
      public Ranking[] rankings { get; set; }
      public string __class__ { get; set; }
   }
   public class Ranking
   {
      public int rank { get; set; }
      public Player player { get; set; }
      public int forge_points { get; set; }
      public SnipReward reward { get; set; }
      public string __class__ { get; set; }
   }
   public class SnipReward
   {
      public int blueprints { get; set; }
      public int strategy_point_amount { get; set; }
      public SnipResources resources { get; set; }
      public string __class__ { get; set; }
   }
   public class SnipResources
   {
      public int medals { get; set; }
      public string __class__ { get; set; }
   }

   public class LGRootObject
   {
      public LGSnip[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class LGData
   {
      public int entity_id { get; set; }
      public string city_entity_id { get; set; }
      public string name { get; set; }
      public int level { get; set; }
      public int? current_progress { get; set; }
      public int max_progress { get; set; }
      public Player player { get; set; }
      public LGState state { get; set; }
      public string __class__ { get; set; }
      public int? rank { get; set; } = null;
   }
   public class LGState
   {
      public Paused_State paused_state { get; set; }
      public int? invested_forge_points { get; set; }
      public int? forge_points_for_level_up { get; set; }
   }
   public class Paused_State
   {
      public dynamic current_product { get; set; }
      public bool boosted { get; set; }
      public bool is_motivated { get; set; }
      public string __class__ { get; set; }
   }

   public class LGSnip : LGData
   {
      public string GewinnString = "";
      public string KurzString = "";
      public int Invest = 0;
      public bool check = false;
      public int maxLevel = -1;
   }
   public static class LG
   {
      public static List<Player> HasGB(params List<Player>[] playerlist)
      {
         List<Player> PlayersWithGB = new List<Player>();
         for (int i = 0; i < playerlist.Length; i++)
         {
            foreach (Player item in playerlist[i])
            {
               if (item.has_great_building && item.is_active && !item.is_self)
               {
                  PlayersWithGB.Add(item);
               }
            }
         }
         return PlayersWithGB;
      }
   }



   public class ResourcesC
   {
      public int medals { get; set; }
      public string __class__ { get; set; }
   }
   public class RewardC
   {
      public int blueprints { get; set; }
      public int strategy_point_amount { get; set; }
      public ResourcesC resources { get; set; }
      public string __class__ { get; set; }
   }
   public class LGContribution
   {
      public int entity_id { get; set; }
      public string city_entity_id { get; set; }
      public string name { get; set; }
      public int level { get; set; }
      public int current_progress { get; set; }
      public int max_progress { get; set; }
      public int rank { get; set; }
      public Player player { get; set; }
      public int forge_points { get; set; }
      public RewardC reward { get; set; }
      public string __class__ { get; set; }
   }
   public class Contribution
   {
      public LGContribution[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
}
