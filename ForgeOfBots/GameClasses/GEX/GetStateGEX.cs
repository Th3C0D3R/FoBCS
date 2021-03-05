using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GEX.GetState
{
   public class GetStateGEX
   {
      public GetStateResponse[] getStateresponses { get; set; }
   }

   public class GetStateResponse
   {
      public Data[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Data
   {
      public int currentEntityId { get; set; }
      public bool isMapCompleted { get; set; }
      public int difficulty { get; set; }
      public string __class__ { get; set; }
   }

}
