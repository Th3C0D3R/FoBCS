using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GBG.Map
{
   public class GBGMapRoot
   {
      public GBGMap[] map { get; set; }
   }

   public class GBGMap
   {
      public string id { get; set; }
      public Province[] provinces { get; set; }
      public string __class__ { get; set; }
   }

   public class Province
   {
      public string name { get; set; }
      public int[] connections { get; set; }
      public string __class__ { get; set; }
      public int? id { get; set; } = null;
   }

}
