using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GBG.GetPlayerParticipant
{
   public class GetPlayerParticipantGBG
   {
      public GetPlayerParticipantResponse[] getPlayerParticipantResponses { get; set; }
   }
   public class GetPlayerParticipantResponse
   {
      public Data responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class Data
   {
      public Attrition attrition { get; set; }
      public string __class__ { get; set; }
   }
   public class Attrition
   {
      public int level { get; set; }
      public int negotiationMultiplier { get; set; }
      public int defendingArmyBonus { get; set; }
      public string __class__ { get; set; }
   }
}
