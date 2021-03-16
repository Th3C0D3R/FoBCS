using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using ForgeOfBots.Utils;
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
      public static int RequestID { get { return requestID; } private set { } }
      private static readonly ResourceManager resMgr = StaticData.resMgr;
      public string WorldID { get; set; }

      public string GetRequestScript(RequestType type, dynamic data)
      {
         int[] queryData = new int[] { };
         int idData = 0;
         string world = StaticData.UserData.LastWorld;
         string messageCategory = "";
         int[,] armyData = new int[2, 8];
         if (type == RequestType.QueryProduction || type == RequestType.CollectProduction || type == RequestType.getConstruction || type == RequestType.contributeForgePoints)
            queryData = (int[])data;
         else if (type == RequestType.getOverviewForCategory)
            messageCategory = $"{data}";
         else if (type == RequestType.switchWorld)
            world = (data == StaticData.UserData.LastWorld ? data : StaticData.UserData.LastWorld);
         else if (type == RequestType.updatePools)
            armyData = (int[,])data;
         else if (type == RequestType.GEXstartAttak)
            idData = int.Parse(data);
         else if (data is int iData)
            idData = iData;
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
            case RequestType.GetBoostOverview:
               _data = new JArray();
               _class = "BoostService";
               _methode = "getOverview";
               break;
            case RequestType.QueryProduction:
               _data = new JArray(queryData[0], queryData[1]);
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
            case RequestType.getConstruction:
               _data = new JArray(queryData[0], queryData[1]);
               _class = "GreatBuildingsService";
               _methode = "getConstruction";
               onlyOne = "false";
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
            case RequestType.GEXgetOverview:
               _data = new JArray();
               _class = "GuildExpeditionService";
               _methode = "getOverview";
               break;
            case RequestType.GEXgetEncounter:
               _data = new JArray(idData);
               _class = "GuildExpeditionService";
               _methode = "getEncounter";
               break;
            case RequestType.GEXopenChest:
               _data = new JArray(idData);
               _class = "GuildExpeditionService";
               _methode = "openChest";
               break;
            case RequestType.GEXstartAttak:
               var gexstart = new JObject
               {
                  ["__class__"] = "GuildExpeditionBattleType",
                  ["attackerPlayerId"] = 0,
                  ["defenderPlayerId"] = 0,
                  ["era"] = null,
                  ["type"] = "guild_expedition",
                  ["currentWaveId"] = 0,
                  ["totalWaves"] = 0,
                  ["encounterId"] = idData,
                  ["armyId"] = 0
               };
               _data = new JArray(gexstart, true);
               _class = "BattlefieldService";
               _methode = "startByBattleType";
               break;
            case RequestType.GBGgetBattleground:
               _data = new JArray();
               _class = "GuildBattlegroundService";
               _methode = "getBattleground";
               break;
            case RequestType.GBGgetState:
               _data = new JArray();
               _class = "GuildBattlegroundStateService";
               _methode = "getState";
               break;
            case RequestType.GBGgetArmyInfo:
               var gbgarmyinfo = new JObject
               {
                  ["__class__"] = "BattlegroundBattleType",
                  ["attackerPlayerId"] = 0,
                  ["defenderPlayerId"] = 0,
                  ["era"] = null,
                  ["type"] = "battleground",
                  ["currentWaveId"] = 0,
                  ["totalWaves"] = 0,
                  ["provinceId"] = idData,
                  ["battlesWon"] = 0
               };
               _data = new JArray(gbgarmyinfo);
               _class = "BattlefieldService";
               _methode = "getArmyPreview";
               break;
            case RequestType.GBGstartAttack:
               var gbgstart = new JObject
               {
                  ["__class__"] = "BattlegroundBattleType",
                  ["attackerPlayerId"] = 0,
                  ["defenderPlayerId"] = 0,
                  ["era"] = null,
                  ["type"] = "battleground",
                  ["currentWaveId"] = 0,
                  ["totalWaves"] = 0,
                  ["provinceId"] = idData,
                  ["battlesWon"] = 0
               };
               _data = new JArray(gbgstart,true);
               _class = "BattlefieldService";
               _methode = "startByBattleType";
               break;
            case RequestType.updatePools:
               _data = new JArray();
               doHack = true;
               _class = "ArmyUnitManagementService";
               _methode = "updatePools";
               break;
            case RequestType.contributeForgePoints:
               _data = new JArray(queryData); //LG-ID, Player-ID, Current Level, ForgePoints to contribute, some bool mostly false
               doHack = true;
               _class = "GreatBuildingsService";
               _methode = "contributeForgePoints";
               break;
            case RequestType.switchWorld:
               _data = new JArray(world.Split('|')[0]);
               _class = "WorldService";
               _methode = "switchWorld";
               break;
            case RequestType.getItems:
               _data = new JArray();
               _class = "InventoryService";
               _methode = "getItems";
               break;
            case RequestType.getContributions:
               _data = new JArray();
               _class = "GreatBuildingsService";
               _methode = "getContributions";
               break;
            case RequestType.getPlayerResources:
               _data = new JArray();
               _class = "ResourceService";
               _methode = "getPlayerResources";
               break;
            case RequestType.getOverviewForCategory:
               _data = new JArray(messageCategory, 99, true, "none");
               _class = "ConversationService";
               _methode = "getOverviewForCategory";
               break;
            case RequestType.getArmyInfo:
               var armyJData = new JObject
               {
                  ["__class__"] = "ArmyContext",
                  ["content"] = "main"
               };
               _data = new JArray(armyJData);
               _class = "ArmyUnitManagementService";
               _methode = "getArmyInfo";
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
         if (doHack)
         {
            if (type == RequestType.updatePools)
            {
               if (armyData.Length != 16) return "";
               string before = "[[{\"__class__\": \"ArmyPool\",\"units\": [";
               string attack = string.Join(",", armyData.GetTroopRow(0));
               string after = "],\"type\":\"attacking\"},{\"__class__\":\"ArmyPool\",\"units\":[";
               string defending = "";
               if(!armyData.GetTroopRow(1).All(r => r == 0))
               {
                  defending = string.Join(",", armyData.GetTroopRow(1));
               }
               string end = "],\"type\":\"defending\"},{\"__class__\":\"ArmyPool\",\"units\":[],\"type\":\"arena_defending\"}],{\"__class__\":\"ArmyContext\",\"content\":\"main\"}]";
               string startJson = jsonString.Substring(0, jsonString.IndexOf("\"requestData\":[") + ("\"requestData\":[").Length - 1);
               string endJson = jsonString.Substring(jsonString.IndexOf(",\"requestClass\""));
               jsonString = $"{startJson}{before}{attack}{after}{defending}{end}{endJson}";
            }
            else if (type == RequestType.CollectProduction)
               jsonString = jsonString.Replace("\"requestData\":[", "\"requestData\":[[").Replace("],\"requestClass\"", "]],\"requestClass\"");
            else if (type == RequestType.contributeForgePoints)
               jsonString = jsonString.Replace(",0]", ",false]");
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
         else if (type == RequestType.getConstruction)
            RequestScript = RequestScript.Replace("##IdData##", queryData[0].ToString());
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
      GetBoostOverview,
      GetAllWorlds,
      GetIncidents,
      OwnArmy,
      getConstruction,
      contributeForgePoints,
      switchWorld,
      getItems,
      updatePools,
      getContributions,
      getPlayerResources,
      getArmyInfo,
      getOverviewForCategory,
      GEXgetOverview,
      GEXgetEncounter,
      GEXstartAttak,
      GEXopenChest,
      GBGgetBattleground,
      GBGgetState,
      GBGgetArmyInfo,
      GBGstartAttack
   }
   public enum MessageType
   {
      social,
      guild
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
      unit_types
   }
}
