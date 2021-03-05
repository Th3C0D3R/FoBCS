using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GEX.GetGuildReward
{
   public class GetGuildRewardGEX
   {
      public GetGuildRewardResponse[] getGuildRewardResponses { get; set; }
   }

   public class GetGuildRewardResponse
   {
      public Data[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Data
   {
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
   }

}
