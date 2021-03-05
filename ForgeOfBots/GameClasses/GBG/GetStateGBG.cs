using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GBG.State
{

   public class GBGState
   {
      public StateResponse[] stateresponse { get; set; }
   }

   public class StateResponse
   {
      public Data responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Data
   {
      public string stateId { get; set; }
      public string __class__ { get; set; }
   }

}
