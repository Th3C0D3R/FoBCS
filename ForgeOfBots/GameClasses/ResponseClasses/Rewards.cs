using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class HiddenRewardList
   {
      public HiddenReward[] hiddenRewards { get; set; }
      public string __class__ { get; set; }
   }
   public class HiddenReward
   {
      public int hiddenRewardId { get; set; }
      public string type { get; set; }
      public int startTime { get; set; }
      public int expireTime { get; set; }
      public Position position { get; set; }
      public bool animated { get; set; }
      public string rarity { get; set; }
      public bool isVisible { get; set; } = false;
      public string __class__ { get; set; }
   }
   public class Position
   {
      public string context { get; set; }
      public string __class__ { get; set; }
   }
}
