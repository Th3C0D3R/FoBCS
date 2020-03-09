using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ForgeOfBots.Utils.Helper;

namespace ForgeOfBots.DataHandler
{
   public class RequestBuilder
   {
      private static int _requestId = 0;
      public string User_Key { get; set; }
      public string VersionSecret { get; set; }
      public string Version { get; set; }
      private static int requestID => _requestId++;
      public int RequestID { get { return requestID; } private set { } }
      private static ResourceManager resMgr = Main.resMgr;
      public string WorldID { get; set; }

      public string GetRequestScript(RequestType type, string data)
      {
         string RequestScript = resMgr.GetString("fetchData");
         string _methode;
         string _class;
         JToken _data = new JArray();
         switch (type)
         {
            case RequestType.Startup:
               _data = new JArray();
               _class = "StartupService";
               _methode = "getData";
               RequestID = 1;
               break;
            case RequestType.Motivate:
               _data = data;
               _class = "OtherPlayerService";
               _methode = "polivateRandomBuilding";
               break;
            case RequestType.CollectIncident:
               _data = data;
               _class = "HiddenRewardService";
               _methode = "collectReward";
               break;
            case RequestType.VisitTavern:
               _data = data;
               _class = "FriendsTavernService";
               _methode = "getOtherTavern";
               break;
            case RequestType.GetClanMember:
               _data = new JArray();
               _class = "OtherPlayerService";
               _methode = "getClanMemberList";
               break;
            case RequestType.GetEntities:
               _data = new JArray();
               _class = "CityMapService";
               _methode = "getEntities";
               break;
            case RequestType.GetFriends:
               _data = new JArray();
               _class = "OtherPlayerService";
               _methode = "getFriendsList";
               break;
            case RequestType.GetNeighbor:
               _data = new JArray();
               _class = "OtherPlayerService";
               _methode = "getNeighborList";
               break;
            case RequestType.GetLGs:
               _data = data;
               _class = "GreatBuildingsService";
               _methode = "getOtherPlayerOverview";
               break;
            case RequestType.LogService:
               var x = new JObject();
               x["__class__"] = "FPSPerformance";
               x["module"] = "City";
               x["fps"] = 30;
               x["vram"] = 0;
               _data = new JArray(x);
               _class = "LogService";
               _methode = "logPerformanceMetrics";
               break;
            case RequestType.CollectProduction:
               _data = data;
               _class = "CityProductionService";
               _methode = "pickupProduction";
               break;
            case RequestType.QueryProduction:
               _data = data;
               _class = "CityProductionService";
               _methode = "startProduction";
               break;
            case RequestType.CancelProduction:
               _data = data;
               _class = "CityProductionService";
               _methode = "cancelProduction";
               break;
            case RequestType.CollectTavern:
               _data = new JArray();
               _class = "FriendsTavernService";
               _methode = "collectReward";
               break;
            case RequestType.GetOwnTavern:
               _data = new JArray();
               _class = "FriendsTavernService";
               _methode = "getOwnTavern";
               break;
            case RequestType.RemovePlayer:
               _data = data;
               _class = "FriendService";
               _methode = "deleteFriend";
               break;
            case RequestType.GetAllWorlds:
               _data = new JArray();
               _class = "WorldService";
               _methode = "getWorlds";
               break;
            default:
               _data = new JArray();
               _class = "StartupService";
               _methode = "getData";
               break;
         }
         var j = new JObject();
         j["__class__"] = "ServerRequest";
         j["requestData"] = _data;
         j["requestClass"] = _class;
         j["requestMethod"] = _methode;
         j["requestId"] = RequestID;

         string jsonString = "["+JsonConvert.SerializeObject(j)+"]";
         Console.WriteLine(jsonString);
         RequestScript = RequestScript
            .Replace("##RequestData##", jsonString)
            .Replace("##sig##", CalcSig(jsonString, User_Key, VersionSecret))
            .Replace("##WorldID##", WorldID)
            .Replace("##UserKey##", User_Key)
            .Replace("##Version##", Version)
            .Replace("##methode##", _methode)
            .Replace("##resType##", type.ToString());
         return RequestScript;
      }

   }

   public enum RequestType
   {
      Startup,
      Motivate,
      CollectIncident,
      VisitTavern,
      GetClanMember,
      GetEntities,
      GetFriends,
      GetNeighbor,
      GetLGs,
      LogService,
      CollectProduction,
      QueryProduction,
      CancelProduction,
      CollectTavern,
      GetOwnTavern,
      RemovePlayer,
      GetAllWorlds,
   }

}
