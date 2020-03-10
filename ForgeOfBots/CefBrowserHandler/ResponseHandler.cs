using CefSharp.WinForms;
using ForgeOfBots.DataHandler;
using ForgeOfBots.GameClasses;
//using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ForgeOfBots.Utils;
using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses.ResponseClasses;
using WorldSelection = ForgeOfBots.GameClasses.ResponseClasses.WorldSelection;
using System.Linq;
using CefSharp;

namespace ForgeOfBots.CefBrowserHandler
{
   class ResponseHandler
   {
      public static ChromiumWebBrowser browser;
      public static event WorldsLoadedEvent WorldsLoaded;
      public static void HookEventHandler(jsMapInterface.hookEvent hookEventArgs)
      {
         var x = hookEventArgs;
         string methode = x.methode;
         switch (x.source)
         {
            case "Data":
               HandleResponse(x.message, methode);
               break;
            case "MetaData":
               HandleMetadata(x.message, methode);
               break;
            case "Cities":
               HandleCities(x.message);
               break;
            default:
               break;
         }
      }
      public static void HandleCities(string msg)
      {
         WorldData wd = JsonConvert.DeserializeObject<WorldData>(msg);
         ListClass.AllWorlds = wd.worlds;
         foreach (KeyValuePair<string, int> world in wd.player_worlds)
         {
            string worldname = "";
            foreach (World aworld in wd.worlds)
            {
               if (aworld.id == world.Key)
               {
                  worldname = aworld.name;
                  break;
               }
            }
            ListClass.WorldList.Add(new Tuple<string, string, WorldState>(world.Key, worldname, WorldState.active));
         }
         WorldsLoaded?.Invoke("HandleCities");
      }
      public static void HandleResponse(string msg, string method)
      {
         RequestType type = (RequestType)Enum.Parse(typeof(RequestType), method);
         switch (type)
         {
            case RequestType.Startup:
               string[] responses = msg.Split(new[]{"##@##"}, StringSplitOptions.RemoveEmptyEntries);
               foreach (string res in responses)
               {
                  var methode = res.Substring(0, res.IndexOf("{"));
                  var body = res.Substring(res.IndexOf("{"));
                  switch (methode)
                  {
                     case "getSittingPlayersCount":
                        ListClass.OwnTavern = JsonConvert.DeserializeObject<OwnTavernStates>(body);
                        break;
                     case "getOtherTavernStates":
                        FriendsTavernRoot rootTavern = JsonConvert.DeserializeObject<FriendsTavernRoot>(body);
                        ListClass.FriendTaverns = rootTavern.responseData.ToList();
                        break;
                     case "getOverview":
                        HiddenRewardRoot rootHiddenReward = JsonConvert.DeserializeObject<HiddenRewardRoot>(body);
                        ListClass.HiddenRewards = rootHiddenReward.responseData.hiddenRewards.ToList();
                        break;
                     case "getResourceDefinitions":
                        ResourceDefinitionRoot rootResourceDefinition = JsonConvert.DeserializeObject<ResourceDefinitionRoot>(body);
                        ListClass.ResourceDefinitions = rootResourceDefinition.responseData.ToList();
                        break;
                     case "getPlayerResources":
                        ResourceRoot rootResource = JsonConvert.DeserializeObject<ResourceRoot>(body);
                        ListClass.Resources = rootResource.responseData.resources;
                        break;
                     case "getMetadata":
                        MetadataRoot rootMetadata = JsonConvert.DeserializeObject<MetadataRoot>(body);
                        ListClass.MetaDataList = rootMetadata.responseData.ToList();
                        if (Main.cwb == null) break;
                        string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "city_entities"); }).url;
                        string script = Main.ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.city_entities);
                        Main.cwb.ExecuteScriptAsync(script);
                        url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "research_eras"); }).url;
                        script = Main.ReqBuilder.GetMetaDataRequestScript(url,MetaRequestType.research_eras);
                        Main.cwb.ExecuteScriptAsync(script);
                        break;
                     case "getUpdates":
                        QuestServiceRoot rootQuest = JsonConvert.DeserializeObject<QuestServiceRoot>(body);
                        ListClass.QuestList = rootQuest.responseData.ToList();
                        break;
                     case "getData":
                        StartupRoot rootStartup = JsonConvert.DeserializeObject<StartupRoot>(body);
                        ListClass.Startup = rootStartup.responseData;
                        break;
                     case "getLimitedBonuses":
                        BonusServiceRoot rootBonusService = JsonConvert.DeserializeObject<BonusServiceRoot>(body);
                        ListClass.Bonus = rootBonusService.responseData.ToList();
                        break;
                     default:
                        break;
                  }
               }
               break;
            case RequestType.Motivate:
               break;
            case RequestType.CollectIncident:
               break;
            case RequestType.VisitTavern:
               break;
            case RequestType.GetClanMember:
               Root<ClanMember> clan = JsonConvert.DeserializeObject<Root<ClanMember>>(msg);
               ListClass.ClanMemberList = clan.responseData;
               break;
            case RequestType.GetEntities:
               break;
            case RequestType.GetFriends:
               Root<Friend> friends = JsonConvert.DeserializeObject<Root<Friend>>(msg);
               ListClass.FriendList = friends.responseData;
               break;
            case RequestType.GetNeighbor:
               Root<Neighbor> neighbor = JsonConvert.DeserializeObject<Root<Neighbor>>(msg);
               ListClass.NeighborList = neighbor.responseData;
               break;
            case RequestType.GetLGs:
               break;
            case RequestType.LogService:
               break;
            case RequestType.CollectProduction:
               break;
            case RequestType.QueryProduction:
               break;
            case RequestType.CancelProduction:
               break;
            case RequestType.CollectTavern:
               break;
            case RequestType.GetOwnTavern:
               break;
            case RequestType.RemovePlayer:
               break;
            case RequestType.GetAllWorlds:
               Root<WorldSelection> ws = JsonConvert.DeserializeObject<Root<WorldSelection>>(msg);
               foreach (WorldSelection item in ws.responseData)
               {
                  if (!ListClass.WorldList.HasCityID(item.id))
                     ListClass.WorldList.Add(new Tuple<string, string, WorldState>(item.id, item.name, (WorldState)Enum.Parse(typeof(WorldState), item.status)));
                  else
                     ListClass.WorldList = ListClass.WorldList.ChangeTuple(item.id, item.name, (WorldState)Enum.Parse(typeof(WorldState), item.status));
               }
               break;
            default:
               break;
         }
      }
      public static void HandleMetadata(string msg, string metaType)
      {
         MetaRequestType type = (MetaRequestType)Enum.Parse(typeof(MetaRequestType), metaType);
         switch (type)
         {
            case MetaRequestType.city_entities:
               BuildingsRoot rootBuilding = JsonConvert.DeserializeObject<BuildingsRoot>("{\"allbuildings\":" + msg + "}");
               ListClass.AllBuildings = rootBuilding.allbuildings.ToList();
               foreach (Entity cityEntity in ListClass.Startup.city_map.entities.ToList())
               {
                  foreach (Allbuilding metaEntity in ListClass.AllBuildings)
                  {
                     if (cityEntity.cityentity_id == metaEntity.id)
                     {
                        EntityEx entity = GameClassHelper.DeepCopy(cityEntity);
                        entity.name = metaEntity.name;
                        entity.available_products = metaEntity.available_products.ToList();
                        entity.type = metaEntity.type;
                        if (entity.type == "production" && entity.connected == 1 && GameClassHelper.hasOnlySupplyProduction(entity.available_products))
                           ListClass.ProductionList.Add(entity);
                        else if (entity.type == "residential" && entity.connected == 1)
                           ListClass.ResidentialList.Add(entity);
                        else if (entity.type == "goods" && entity.connected == 1)
                           ListClass.GoodProductionList.Add(entity);
                     }
                  }
               }
               break;
            case MetaRequestType.research_eras:
               ResearchEraRoot rootResearch = JsonConvert.DeserializeObject<ResearchEraRoot>("{\"reserach\":" + msg+"}");
               ListClass.Eras = rootResearch.reserach.ToList();
               Dictionary<string, List<Good>> x = Helper.GetGoodsEraSorted(ListClass.Eras, ListClass.Resources, ListClass.ResourceDefinitions);
               break;
            default:
               break;
         }
      }
   }
   public delegate void WorldsLoadedEvent(object sender);
}
