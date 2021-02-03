using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ForgeOfBots.DataHandler
{
   public class Update
   {
      public RequestBuilder ReqBuilder { get; private set; }

      public Update(RequestBuilder reqbuilder)
      {
         ReqBuilder = reqbuilder;
      }

      public void UpdatePlayerLists()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetClanMember, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         Root<ClanMember> clan = JsonConvert.DeserializeObject<Root<ClanMember>>(ret);
         ListClass.ClanMemberList = clan.responseData.FindAll(c => c.is_self == false && c.is_friend == false);

         script = ReqBuilder.GetRequestScript(RequestType.GetFriends, "");
         ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         Root<Friend> friends = JsonConvert.DeserializeObject<Root<Friend>>(ret);
         ListClass.FriendList = friends.responseData.FindAll(f => f.is_self == false);

         script = ReqBuilder.GetRequestScript(RequestType.GetNeighbor, "");
         ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         Root<Neighbor> neighbor = JsonConvert.DeserializeObject<Root<Neighbor>>(ret);
         ListClass.NeighborList = neighbor.responseData.FindAll(n => n.is_self == false && n.is_friend == false);
      }
      public void UpdateFirends()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetFriends, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         Root<Friend> friends = JsonConvert.DeserializeObject<Root<Friend>>(ret);
         ListClass.FriendList = friends.responseData.FindAll(f => f.is_self == false);
      }
      public void UpdateStartUp()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.Startup, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         string[] responses = ret.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
         foreach (string res in responses)
         {
            var methode = res.Substring(0, res.IndexOf("{"));
            var body = res.Substring(res.IndexOf("{"));
            dynamic ress = null;
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
                  foreach (HiddenReward item in rootHiddenReward.responseData.hiddenRewards)
                  {
                     DateTime endTime = Helper.UnixTimeStampToDateTime(item.expireTime);
                     DateTime startTime = Helper.UnixTimeStampToDateTime(item.startTime);
                     bool vis = (endTime > DateTime.Now) && (startTime < DateTime.Now);
                     item.isVisible = vis;
                  }
                  ListClass.HiddenRewards = rootHiddenReward.responseData.hiddenRewards.ToList();
                  break;
               case "getResourceDefinitions":
                  ress = JsonConvert.DeserializeObject(body);
                  ListClass.ResourceDefinitions = ress;
                  UpdatedSortedGoodList();
                  break;
               case "getPlayerResources":
                  ress = JsonConvert.DeserializeObject(body);
                  ListClass.Resources = ress;
                  UpdatedSortedGoodList();
                  break;
               case "getMetadata":
                  MetadataRoot rootMetadata = JsonConvert.DeserializeObject<MetadataRoot>(body);
                  ListClass.MetaDataList = rootMetadata.responseData.ToList();
                  if (StaticData.jsExecutor == null) break;
                  if (ListClass.AllBuildings.Count <= 0)
                  {
                     string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "city_entities"); }).url;
                     script = ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.city_entities);
                     string cityMeta = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
                     BuildingsRoot rootBuilding = JsonConvert.DeserializeObject<BuildingsRoot>("{\"buildings\":" + cityMeta + "}");
                     ListClass.AllBuildings = rootBuilding.buildings.ToList();
                     if(ListClass.Startup.Count > 0)
                        UpdateBuildings(ListClass.Startup["responseData"]["city_map"]["entities"]);
                  }
                  if (ListClass.Eras.Count <= 0)
                  {
                     string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "research_eras"); }).url;
                     script = ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.research_eras);
                     string researchMeta = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
                     ResearchEraRoot rootResearch = JsonConvert.DeserializeObject<ResearchEraRoot>("{\"reserach\":" + researchMeta + "}");
                     ListClass.Eras = rootResearch.reserach.ToList();
                     UpdatedSortedGoodList();
                  }
                  break;
               case "getUpdates":
                  QuestServiceRoot rootQuest = JsonConvert.DeserializeObject<QuestServiceRoot>(body);
                  ListClass.QuestList = rootQuest.responseData.ToList();
                  break;
               case "getData":
                  ress = JsonConvert.DeserializeObject(body);
                  ListClass.Startup = ress;
                  ListClass.UserData = ress["responseData"]["user_data"];
                     UpdateBuildings(ListClass.Startup["responseData"]["city_map"]["entities"]);
                  break;
               case "getLimitedBonuses":
                  BonusServiceRoot rootBonusService = JsonConvert.DeserializeObject<BonusServiceRoot>(body);
                  ListClass.Bonus = rootBonusService.responseData.ToList();
                  ListClass.ArcBonus = ListClass.Bonus.Sum(e => { if (e.type == "contribution_boost") return e.value; else return 0; });
                  break;
               default:
                  break;
            }
         }
         
      }
      public void UpdateOwnTavern()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetOwnTavern, "[]");
         string ownTavern = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         OwnTavernDataRoot otdr = JsonConvert.DeserializeObject<OwnTavernDataRoot>(ownTavern);
         ListClass.OwnTavernData = otdr.responseData;
      }
      public void UpdateContribution()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.getContributions, "[]");
         string contribution = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         Contribution otdr = JsonConvert.DeserializeObject<Contribution>(contribution);
         ListClass.Contributions = otdr.responseData.ToList();
      }
      public void UpdateInventory()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.getItems, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         ListClass.Inventory = JsonConvert.DeserializeObject<Inventory>(ret);
      }
      public void UpdatedSortedGoodList()
      {
         ListClass.GoodsDict = Helper.GetGoodsEraSorted(ListClass.Eras, ListClass.Resources, ListClass.ResourceDefinitions);
      }
      public void UpdateBuildings(JToken entities)
      {
         if (ListClass.Startup.Count <= 0 || ListClass.AllBuildings.Count <= 0) return;
         ListClass.ProductionList.Clear();
         ListClass.ResidentialList.Clear();
         ListClass.GoodProductionList.Clear();
         foreach (JToken cityEntity in entities.ToList())
         {
            foreach (Building metaEntity in ListClass.AllBuildings)
            {
               if (cityEntity["cityentity_id"]?.ToString() == metaEntity.id)
               {
                  EntityEx entity = GameClassHelper.CopyFrom(cityEntity);
                  entity.name = metaEntity.name;
                  if (metaEntity.available_products != null)
                     entity.available_products = metaEntity.available_products.ToList();
                  entity.type = metaEntity.type;
                  if (entity.type == "production" && metaEntity.available_products != null && entity.connected >= 1 && /*entity.hasSupplyProdAt(StaticData.UserData.ProductionOption)|| */GameClassHelper.hasOnlySupplyProduction(entity.available_products) && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
                     ListClass.ProductionList.Add(entity);
                  else if (entity.type == "residential" && entity.connected >= 1 && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
                     ListClass.ResidentialList.Add(entity);
                  else if (entity.type == "goods" && entity.connected >= 1 && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
                     ListClass.GoodProductionList.Add(entity);
               }
            }
         }
      }
   }
}
