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

   public class TavernData
   {
      public View view { get; set; }
      public string[] unlockedCustomizationIds { get; set; }
      public int friendCount { get; set; }
      public string __class__ { get; set; }
   }

   public class View
   {
      public int tableLevel { get; set; }
      public int unlockedChairs { get; set; }
      public Visitor[] visitors { get; set; }
      public Selectedcustomizationids selectedCustomizationIds { get; set; }
      public int tavernSilverBase { get; set; }
      public int tavernSilverAdd { get; set; }
      public string __class__ { get; set; }
   }

   public class Selectedcustomizationids
   {
      public string tablecloth { get; set; }
      public string tray { get; set; }
      public string flooring { get; set; }
   }

   public class Visitor
   {
      public int player_id { get; set; }
      public string name { get; set; }
      public string avatar { get; set; }
      public string __class__ { get; set; }
   }


}
