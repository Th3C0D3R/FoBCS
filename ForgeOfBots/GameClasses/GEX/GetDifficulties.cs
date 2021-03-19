using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GEX.GetDifficulties
{
   public class GetDifficulties
   {
      public Data[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Data
   {
      public string description { get; set; }
      public bool unlocked { get; set; }
      public bool unlockable { get; set; }
      public bool playable { get; set; }
      public string __class__ { get; set; }
      public int id { get; set; }
   }

}
