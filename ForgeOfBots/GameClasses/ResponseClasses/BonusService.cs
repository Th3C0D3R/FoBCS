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
      public float amount { get; set; }
      public float value { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }

}
