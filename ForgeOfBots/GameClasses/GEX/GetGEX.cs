using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GEX.Get
{
   public class GetGEX
   {
      public GetResponse[] getresponse { get; set; }
   }
   public class GetResponse
   {
      public Data responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class Data
   {
      public Progress progress { get; set; }
      public Chest[] chests { get; set; }
      public dynamic upcomingReward { get; set; }
      public int expeditionPoints { get; set; }
      public string state { get; set; }
      public int nextStateTime { get; set; }
      public string guildFlag { get; set; }
      public bool isPlayerParticipating { get; set; }
      public bool isGuildParticipating { get; set; }
      public object[] missedContributions { get; set; }
      public dynamic championship { get; set; }
      public string __class__ { get; set; }
   }
   public class Progress
   {
      public int currentEntityId { get; set; }
      public bool isMapCompleted { get; set; }
      public int difficulty { get; set; }
      public string __class__ { get; set; }
   }
   public class Upcomingreward
   {
      public int index { get; set; }
      public int amount { get; set; }
      public int requiredPoints { get; set; }
      public string __class__ { get; set; }
   }
   public class Championship
   {
      public bool notify { get; set; }
      public int rank { get; set; }
      public string __class__ { get; set; }
   }
   public class Chest
   {
      public int id { get; set; }
      public Chest1 chest { get; set; }
      public string __class__ { get; set; }
   }
   public class Chest1
   {
      public Possible_Rewards[] possible_rewards { get; set; }
      public bool allowReroll { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }
   public class Possible_Rewards
   {
      public int drop_chance { get; set; }
      public dynamic reward { get; set; }
      public string __class__ { get; set; }
   }

}
