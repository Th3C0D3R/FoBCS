using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ForgeOfBots.Utils.Helper;

namespace ForgeOfBots.DataHandler
{
   public class RequestBuilder
   {
      private static int _requestId = 1;
      public string User_Key { get; set; }
      public string VersionSecret { get; set; }
      public string Version { get; set; }
      private static int requestID => _requestId++;
      public int RequestID { get { return requestID; } private set { } }
      private static readonly ResourceManager resMgr = Main.resMgr;
      public string WorldID { get; set; }

      public string GetRequestScript(RequestType type, dynamic data)
      {
         int[] queryData = new int[] { };
         int idData = 0;
         if (type == RequestType.QueryProduction || type == RequestType.CollectProduction)
            queryData = (int[])data;
         else if (((object)data).GetType() == typeof(int))
            idData = (int)data;
         else if (data != "" && data != "[]")
            idData = int.Parse(data);
         string RequestScript = resMgr.GetString("fetchData");
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
            case RequestType.Motivate:
               _data = new JArray(idData);
               _class = "OtherPlayerService";
               _methode = "polivateRandomBuilding";
               break;
            case RequestType.CollectIncident:
               _data = new JArray(idData);
               _class = "HiddenRewardService";
               _methode = "collectReward";
               break;
            case RequestType.GetIncidents:
               _data = new JArray(idData);
               _class = "HiddenRewardService";
               _methode = "getOverview";
               break;
            case RequestType.VisitTavern:
               _data = new JArray(idData);
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
               _data = new JArray(idData);
               _class = "GreatBuildingsService";
               _methode = "getOtherPlayerOverview";
               break;
            case RequestType.LogService:
               var x = new JObject
               {
                  ["__class__"] = "FPSPerformance",
                  ["module"] = "City",
                  ["fps"] = 30,
                  ["vram"] = 0
               };
               _data = new JArray(x);
               _class = "LogService";
               _methode = "logPerformanceMetrics";
               break;
            case RequestType.CollectProduction:
               _data = new JArray(queryData);
               doHack = true;
               _class = "CityProductionService";
               _methode = "pickupProduction";
               break;
            case RequestType.QueryProduction:
               _data = new JArray(queryData[0],queryData[1]);
               _class = "CityProductionService";
               _methode = "startProduction";
               break;
            case RequestType.CancelProduction:
               _data = new JArray(idData);
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
               _data = new JArray(idData);
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
         var j = new JObject
         {
            ["__class__"] = "ServerRequest",
            ["requestData"] = _data,
            ["requestClass"] = _class,
            ["requestMethod"] = _methode,
            ["requestId"] = RequestID
         };

         string jsonString = "["+JsonConvert.SerializeObject(j)+"]";
         if (doHack)
         {
            jsonString = jsonString.Replace("\"requestData\":[", "\"requestData\":[[").Replace("],\"requestClass\"", "]],\"requestClass\"");
         }
         //Console.WriteLine(jsonString);
         RequestScript = RequestScript
            .Replace("##RequestData##", jsonString)
            .Replace("##sig##", CalcSig(jsonString, User_Key, VersionSecret))
            .Replace("##WorldID##", WorldID)
            .Replace("##UserKey##", User_Key)
            .Replace("##Version##", Version)
            .Replace("##methode##", _methode)
            .Replace("##onlyOne##", onlyOne)
            .Replace("##resType##", type.ToString());
         if (type == RequestType.VisitTavern || type == RequestType.Motivate)
            RequestScript = RequestScript.Replace("##IdData##", idData.ToString());
         else
            RequestScript = RequestScript.Replace("##IdData##", "");
         return RequestScript;
      }

      public string GetMetaDataRequestScript(string url, MetaRequestType type)
      {
         string RequestScript = resMgr.GetString("fetchMetaData");
         //Console.WriteLine(jsonString);
         RequestScript = RequestScript
            .Replace("##WorldID##", WorldID)
            .Replace("##url##", url)
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
      GetIncidents,
   }

    public enum E_Motivate
    {
        Clan,
        Neighbor,
        Friend,
        All
    }

   public enum MetaRequestType
   {
      city_entities,
      research_eras,
   }

}
