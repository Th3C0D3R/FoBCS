using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class OwnTavernStates
   {
      public int[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class FriendTavernState
   {
      public int ownerId { get; set; }
      public string state { get; set; }
      public int unlockedChairCount { get; set; }
      public int sittingPlayerCount { get; set; }
      public string __class__ { get; set; }
   }
}
