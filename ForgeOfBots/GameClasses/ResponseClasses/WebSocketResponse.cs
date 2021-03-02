using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class WSRequest
   {
      public int requestId { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public string[] requestData { get; set; }
   }

   public class WSResponse
   {
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public string __class__ { get; set; }
      public object responseData { get; set; }
   }

}
