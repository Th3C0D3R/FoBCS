using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.GBG.Map;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
         ListClass.ClanMemberList = clan.responseData.FindAll(c => c.is_self == false);

         script = ReqBuilder.GetRequestScript(RequestType.GetFriends, "");
         ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         Root<Friend> friends = JsonConvert.DeserializeObject<Root<Friend>>(ret);
         ListClass.FriendList = friends.responseData.FindAll(f => f.is_self == false && f.is_guild_member == false);

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
                     script = ReqBuilder.GetMetaDataRequestScript(url);
                     string cityMeta = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
                     BuildingsRoot rootBuilding = JsonConvert.DeserializeObject<BuildingsRoot>("{\"buildings\":" + cityMeta + "}");
                     ListClass.AllBuildings = rootBuilding.buildings.ToList();
                     if (ListClass.Startup.Count > 0)
                        UpdateBuildings(ListClass.Startup["responseData"]["city_map"]["entities"]);
                  }
                  if (ListClass.Eras.Count <= 0)
                  {
                     string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "research_eras"); }).url;
                     script = ReqBuilder.GetMetaDataRequestScript(url);
                     string researchMeta = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
                     ResearchEraRoot rootResearch = JsonConvert.DeserializeObject<ResearchEraRoot>("{\"reserach\":" + researchMeta + "}");
                     ListClass.Eras = rootResearch.reserach.ToList();
                     ResearchEra noage = new ResearchEra();
                     noage.era = "NoAge";
                     noage.name = "---";
                     noage.__class__ = "ResearchEra";
                     ListClass.Eras.Insert(0, noage);
                     UpdatedSortedGoodList();
                  }
                  if (ListClass.UnitTypes.Count <= 0)
                  {
                     string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "unit_types"); }).url;
                     script = ReqBuilder.GetMetaDataRequestScript(url);
                     string unit_types_Meta = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
                     UnitTypesRoot rootUnitType = JsonConvert.DeserializeObject<UnitTypesRoot>("{\"unit_types\":" + unit_types_Meta + "}");
                     ListClass.UnitTypes = rootUnitType.unit_types.ToList();
                     UpdateSortedArmyList();
                  }
                  if (ListClass.MetaMap == null)
                  {
                     string url = ListClass.MetaDataList.Find(m => m.identifier == "guild_battleground_maps").url;
                     script = ReqBuilder.GetMetaDataRequestScript(url);
                     string gbg_map_meta = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
                     GBGMapRoot map = JsonConvert.DeserializeObject<GBGMapRoot>("{\"map\":" + gbg_map_meta + "}");
                     ListClass.MetaMap = map.map[0];
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
      public void UpdateEntities(bool onlyFinished = false)
      {
         string script = StaticData.ReqBuilder.GetRequestScript(RequestType.GetEntities, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         dynamic entities = JsonConvert.DeserializeObject(ret);
         UpdateBuildings(entities["responseData"], onlyFinished);
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
      public void UpdateResources()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.getPlayerResources, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         dynamic ress = JsonConvert.DeserializeObject(ret);
         ListClass.Resources = ress;
         UpdatedSortedGoodList();
      }
      public void UpdateBoost()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetBoostOverview, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         BoostRoot b = JsonConvert.DeserializeObject<BoostRoot>(ret);
         ListClass.BoostList = b.responseData.ToList();
         try
         {
            StaticData.AttackBoost[0] = ListClass.BoostList.Find(d => d.id == "attackingUnits").entries.ToList().Where(eu => eu.boostType.Equals("att_boost_attacker") || eu.boostType.Equals("att_def_boost_attacker")).Sum(e => e.boostValue * e.amount);
            StaticData.AttackBoost[1] = ListClass.BoostList.Find(d => d.id == "attackingUnits").entries.ToList().Where(eu => eu.boostType.Equals("def_boost_attacker") || eu.boostType.Equals("att_def_boost_attacker")).Sum(e => e.boostValue * e.amount);
            StaticData.DefenseBoost[0] = ListClass.BoostList.Find(d => d.id == "defendingUnits").entries.ToList().Where(eu => eu.boostType.Equals("att_boost_defender") || eu.boostType.Equals("att_def_boost_defender")).Sum(e => e.boostValue * e.amount);
            StaticData.DefenseBoost[1] = ListClass.BoostList.Find(d => d.id == "defendingUnits").entries.ToList().Where(eu => eu.boostType.Equals("def_boost_defender") || eu.boostType.Equals("att_def_boost_defender")).Sum(e => e.boostValue * e.amount);
         }
         catch (Exception)
         {
            StaticData.AttackBoost = new int[] { 0, 0 };
            StaticData.DefenseBoost = new int[] { 0, 0 };
         }
      }
      public void UpdateArmy()
      {
         DebugWatch.Start("UpdateArmy Script");
         string script = ReqBuilder.GetRequestScript(RequestType.getArmyInfo, "");
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         DebugWatch.Stop();
         DebugWatch.Start("UpdateArmy Deserialize");
         ArmyRoot ress = JsonConvert.DeserializeObject<ArmyRoot>(ret);
         DebugWatch.Stop();
         ListClass.Army = ress;
         UpdateSortedArmyList();
      }
      public void UpdateBattle()
      {

      }
      public bool UpdateAttackPool()
      {
         int[,] army = new int[2, 8];
         List<int> addedIds = new List<int>();
         var tmp = ListClass.UnitList;
         for (int i = 0; i < StaticData.UserData.ArmySelection[StaticData.UserData.LastWorld.Split('|')[0]].Count; i++)
         {
            int healthThreshold = 10;
            string item = StaticData.UserData.ArmySelection[StaticData.UserData.LastWorld.Split('|')[0]][i];
            var units = (from ut in tmp
                    from u in ut.unit
                    where u.unitTypeId == item && u.currentHitpoints >= healthThreshold
                    select u).ToList();
            if (units.Count >= 1)
            {
               GameUnit un = units.Where(u => !addedIds.Contains(u.unitId)).ToList().First();
               army[0, i] = un.unitId;
               addedIds.Add(army[0, i]);
            }
            else
            {
               while (units.Where(u => !addedIds.Contains(u.unitId)).ToList().Count == 0 && healthThreshold > 0)
               {
                  healthThreshold -= 1;
                  units = (from ut in tmp
                           from u in ut.unit
                           where u.unitTypeId == item && u.currentHitpoints >= healthThreshold
                           select u).ToList();
               }
               army[0, i] = units.Where(u => !addedIds.Contains(u.unitId)).First().unitId;
               addedIds.Add(army[0, i]);
            }
         }
         if (army.GetTroopRow(0).Count() < 8) return false;
         string script = ReqBuilder.GetRequestScript(RequestType.updatePools, army);
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         return ret == "";
      }
      public void UpdateMessages(string type)
      {
         string script = ReqBuilder.GetRequestScript(RequestType.getOverviewForCategory, type);
         string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         MessageRoot ress = JsonConvert.DeserializeObject<MessageRoot>(ret);
         ListClass.MessageCenter = ress;

      }
      public void UpdateSortedArmyList()
      {
         if (ListClass.UnitTypes.Count <= 0 || ListClass.Army.responseData == null) return;
         ListClass.UnitListEraSorted = Helper.GetUnitEraSorted(ListClass.Eras, ListClass.UnitTypes, ListClass.Army.responseData);
         ListClass.UnitList = Helper.GetUnitSorted(ListClass.UnitTypes, ListClass.Army.responseData);
      }
      public void UpdatedSortedGoodList()
      {
         if (ListClass.Eras != null && ListClass.ResourceDefinitions != null && ListClass.Resources != null)
            ListClass.GoodsDict = Helper.GetGoodsEraSorted(ListClass.Eras, ListClass.Resources, ListClass.ResourceDefinitions);
      }
      public void UpdateBuildings(JToken entities, bool onlyFinished = false)
      {
         if (ListClass.Startup.Count <= 0 || ListClass.AllBuildings.Count <= 0) return;
         if (!onlyFinished)
         {
            ListClass.ProductionList.Clear();
            ListClass.ResidentialList.Clear();
            ListClass.GoodProductionList.Clear();
         }
         ListClass.FinishedProductions.Clear();
         ListClass.EveryProduction.Clear();
         Parallel.ForEach(entities.ToList(), cityEntity =>
         {
            Parallel.ForEach(ListClass.AllBuildings, metaEntity =>
            {
               if (cityEntity["cityentity_id"]?.ToString() == metaEntity.id)
               {
                  EntityEx entity = GameClassHelper.CopyFrom(cityEntity);
                  entity.name = metaEntity.name;
                  if (metaEntity.available_products != null)
                     entity.available_products = metaEntity.available_products.ToList();
                  entity.type = metaEntity.type;
                  if (entity.state["__class__"].ToString().ToLower() == "ProductionFinishedState".ToLower())
                     ListClass.FinishedProductions.Add(entity);
                  ListClass.EveryProduction.Add(entity);
                  if (onlyFinished) return;
                  if (entity.type == "production" && metaEntity.available_products != null && entity.connected >= 1 && /*entity.hasSupplyProdAt(StaticData.UserData.ProductionOption)|| */GameClassHelper.hasOnlySupplyProduction(entity.available_products) && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
                     ListClass.ProductionList.Add(entity);
                  else if (entity.type == "residential" && entity.connected >= 1 && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
                     ListClass.ResidentialList.Add(entity);
                  else if (entity.type == "goods" && entity.connected >= 1 && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
                     ListClass.GoodProductionList.Add(entity);
               }
            });
         });
         //foreach (JToken cityEntity in entities.ToList())
         //{
         //   foreach (Building metaEntity in ListClass.AllBuildings)
         //   {
         //      if (cityEntity["cityentity_id"]?.ToString() == metaEntity.id)
         //      {
         //         EntityEx entity = GameClassHelper.CopyFrom(cityEntity);
         //         entity.name = metaEntity.name;
         //         if (metaEntity.available_products != null)
         //            entity.available_products = metaEntity.available_products.ToList();
         //         entity.type = metaEntity.type;
         //         if (entity.state["__class__"].ToString().ToLower() == "ProductionFinishedState".ToLower())
         //            ListClass.FinishedProductions.Add(entity);
         //         if (onlyFinished) continue;
         //         if (entity.type == "production" && metaEntity.available_products != null && entity.connected >= 1 && /*entity.hasSupplyProdAt(StaticData.UserData.ProductionOption)|| */GameClassHelper.hasOnlySupplyProduction(entity.available_products) && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
         //            ListClass.ProductionList.Add(entity);
         //         else if (entity.type == "residential" && entity.connected >= 1 && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
         //            ListClass.ResidentialList.Add(entity);
         //         else if (entity.type == "goods" && entity.connected >= 1 && entity.state["__class__"].ToString().ToLower() != "ConstructionState".ToLower())
         //            ListClass.GoodProductionList.Add(entity);
         //      }
         //   }
         //}
      }
   }
}
