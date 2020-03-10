using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class BonusService
   {
      public int entityId { get; set; }
      public int amount { get; set; }
      public int value { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }

}
