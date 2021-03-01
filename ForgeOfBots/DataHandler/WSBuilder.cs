using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.DataHandler
{
   //{"requestId":13,"requestClass":"SocketAuthenticationService","requestMethod":"authWithToken","requestData":["xxx"]}
   public static class WSBuilder
   {
      private static int _requestId = 5;
      private static int requestID => _requestId++;
      public static int RequestID { get { return requestID; } private set { } }

      public static string GetWSRequestString(WSRequestClass wsClass,WSRequestMethode wsMethode)
      {
         string _methode;
         string _class;
         string onlyOne = "true";
         bool doHack = false;
         JToken _data;
         switch (type)
         {
            case RequestType.Startup:
               _data = new JArray();
               _class = "StartupService";
               _methode = "getData";
               RequestID = 1;
               onlyOne = "false";
               break;
            default:
               _data = new JArray();
               _class = "StartupService";
               _methode = "getData";
               break;
         }
         var j = new JObject
         {
            ["__class__"] = "ServerRequest",
            ["requestData"] = _data,
            ["requestClass"] = _class,
            ["requestMethod"] = _methode,
            ["requestId"] = RequestID
         };

         string jsonString = "[" + JsonConvert.SerializeObject(j) + "]";
         
         return jsonString;
      }
   }

   public enum WSRequestClass
   {
      SocketAuthenticationService
   }
   public enum WSRequestMethode
   {
      authWithToken
   }
}
