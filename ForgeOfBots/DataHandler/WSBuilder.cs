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
      public static string GetWSRequestString(WSRequestClass wsClass,WSRequestMethode wsMethode, dynamic data)
      {
         string _methode = "";
         string _class = "";
         JToken _data = new JArray();
         string ping = "";
         switch (wsClass)
         {
            case WSRequestClass.SocketAuthenticationService:
               _data = new JArray(data);
               _class = wsClass.ToString();
               _methode = wsMethode.ToString();
               break;
            default:
               ping = "PING";
               break;
         }
         if(ping == "")
         {
            var j = new JObject
            {
               ["requestData"] = _data,
               ["requestClass"] = _class,
               ["requestMethod"] = _methode,
               ["requestId"] = RequestBuilder.RequestID
            };
            string jsonString = "[" + JsonConvert.SerializeObject(j) + "]";
            return jsonString;
         }
         else
         {
            return ping;
         }
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
