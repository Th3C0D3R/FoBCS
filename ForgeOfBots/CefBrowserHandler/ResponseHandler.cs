using CefSharp;
#if RELEASE || DEBUG
using CefSharp.OffScreen;
#elif DEBUGFORM
using CefSharp.WinForms;
#endif
using ForgeOfBots.DataHandler;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.LanguageFiles;
using ForgeOfBots.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WorldSelection = ForgeOfBots.GameClasses.ResponseClasses.WorldSelection;

namespace ForgeOfBots.CefBrowserHandler
{
   class ResponseHandler
   {
      public static ChromiumWebBrowser browser;
      private static WorldsLoadedEvent _WorldsLoaded;
      public static event WorldsLoadedEvent WorldsLoaded
      {
         add
         {
            if (_WorldsLoaded == null || !_WorldsLoaded.GetInvocationList().Contains(value))
               _WorldsLoaded += value;
         }
         remove
         {
            _WorldsLoaded -= value;
         }
      }
      private static EverythingImportantLoadedEvent _EverythingImportantLoaded;
      public static event EverythingImportantLoadedEvent EverythingImportantLoaded
      {
         add
         {
            if (_EverythingImportantLoaded == null || !_EverythingImportantLoaded.GetInvocationList().Contains(value))
               _EverythingImportantLoaded += value;
         }
         remove
         {
            _EverythingImportantLoaded -= value;
         }
      }
      private static StartupLoadedEvent _StartupLoaded;
      public static event StartupLoadedEvent StartupLoaded
      {
         add
         {
            if (_StartupLoaded == null || !_StartupLoaded.GetInvocationList().Contains(value))
               _StartupLoaded += value;
         }
         remove
         {
            _StartupLoaded -= value;
         }
      }
      private static ListLoadedEvent _ListLoaded;
      public static event ListLoadedEvent ListLoaded
      {
         add
         {
            if (_ListLoaded == null || !_ListLoaded.GetInvocationList().Contains(value))
               _ListLoaded += value;
         }
         remove
         {
            _ListLoaded -= value;
         }
      }
      private static CustomEvent _friendRemoved;
      public static event CustomEvent FriendRemoved
      {
         add
         {
            if (_friendRemoved == null || !_friendRemoved.GetInvocationList().Contains(value))
               _friendRemoved += value;
         }
         remove
         {
            _friendRemoved -= value;
         }
      }
      private static CustomEvent _taverSitted;
      public static event CustomEvent TaverSitted
      {
         add
         {
            if (_taverSitted == null || !_taverSitted.GetInvocationList().Contains(value))
               _taverSitted += value;
         }
         remove
         {
            _taverSitted -= value;
         }
      }
      private static CustomEvent _collectTavern;
      public static event CustomEvent TavernCollected
      {
         add
         {
            if (_collectTavern == null || !_collectTavern.GetInvocationList().Contains(value))
               _collectTavern += value;
         }
         remove
         {
            _collectTavern -= value;
         }
      }
      private static CustomEvent _productionCollected;
      public static event CustomEvent ProdCollected
      {
         add
         {
            if (_productionCollected == null || !_productionCollected.GetInvocationList().Contains(value))
               _productionCollected += value;
         }
         remove
         {
            _productionCollected -= value;
         }
      }
      private static CustomEvent _productionStarted;
      public static event CustomEvent ProdStarted
      {
         add
         {
            if (_productionStarted == null || !_productionStarted.GetInvocationList().Contains(value))
               _productionStarted += value;
         }
         remove
         {
            _productionStarted -= value;
         }
      }
      private static CustomEvent _productionCanceled;
      public static event CustomEvent ProdCanceled
      {
         add
         {
            if (_productionCanceled == null || !_productionCanceled.GetInvocationList().Contains(value))
               _productionCanceled += value;
         }
         remove
         {
            _productionCanceled -= value;
         }
      }
      private static CustomEvent _incidentCollected;
      public static event CustomEvent IncidentCollected
      {
         add
         {
            if (_incidentCollected == null || !_incidentCollected.GetInvocationList().Contains(value))
               _incidentCollected += value;
         }
         remove
         {
            _incidentCollected -= value;
         }
      }

      public static bool[] ImportantLoaded = Enumerable.Repeat(false, 20).ToArray();
      static readonly object _locker = new object();
      public static void HookEventHandler(jsMapInterface.hookEvent hookEventArgs)
      {
         var x = hookEventArgs;
         string methode = x.methode;
         switch (x.source)
         {
            case "Data":
               HandleResponse(x.message, methode, x.idData);
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
         if (!ImportantLoaded[19] && ImportantLoaded.ToList().FindAll(p => p).Count == 19)
         {
            if (Main.cwb != null)
            {
               ImportantLoaded[19] = true;
               string script = Main.ReqBuilder.GetRequestScript(RequestType.GetOwnTavern, "[]");
               Main.cwb.ExecuteScriptAsync(script);
            }

         }
         else if (ImportantLoaded.All(b => { return b; }))
         {
            ImportantLoaded = Enumerable.Repeat(false, 20).ToArray();
            _EverythingImportantLoaded?.Invoke(null);
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
         _WorldsLoaded?.Invoke("HandleCities");
      }
      public static void HandleResponse(string msg, string method, string idData = "")
      {
         RequestType type = (RequestType)Enum.Parse(typeof(RequestType), method);
         switch (type)
         {
            case RequestType.Startup:
               string[] responses = msg.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
               foreach (string res in responses)
               {
                  var methode = res.Substring(0, res.IndexOf("{"));
                  var body = res.Substring(res.IndexOf("{"));
                  dynamic ress = null;
                  switch (methode)
                  {
                     case "getSittingPlayersCount":
                        ListClass.OwnTavern = JsonConvert.DeserializeObject<OwnTavernStates>(body);
                        ImportantLoaded[0] = true;
                        break;
                     case "getOtherTavernStates":
                        FriendsTavernRoot rootTavern = JsonConvert.DeserializeObject<FriendsTavernRoot>(body);
                        ListClass.FriendTaverns = rootTavern.responseData.ToList();
                        ImportantLoaded[1] = true;
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
                        ImportantLoaded[2] = true;
                        break;
                     case "getResourceDefinitions":
                        ress = JsonConvert.DeserializeObject(body);
                        ListClass.ResourceDefinitions = ress;
                        Main.Updater.UpdatedSortedGoodList();
                        ImportantLoaded[18] = ListClass.GoodsDict.Count > 0;
                        ImportantLoaded[3] = true;
                        break;
                     case "getPlayerResources":
                        ress = JsonConvert.DeserializeObject(body);
                        ListClass.Resources = ress;
                        Main.Updater.UpdatedSortedGoodList();
                        ImportantLoaded[18] = ListClass.GoodsDict.Count > 0;
                        ImportantLoaded[4] = true;
                        break;
                     case "getMetadata":
                        MetadataRoot rootMetadata = JsonConvert.DeserializeObject<MetadataRoot>(body);
                        ListClass.MetaDataList = rootMetadata.responseData.ToList();
                        ImportantLoaded[5] = true;
                        if (Main.cwb == null) break;
                        if (ListClass.AllBuildings.Count <= 0)
                        {
                           string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "city_entities"); }).url;
                           string script = Main.ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.city_entities);
                           Main.cwb.ExecuteScriptAsync(script);
                        }
                        if (ListClass.Eras.Count <= 0)
                        {
                           string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "research_eras"); }).url;
                           string script = Main.ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.research_eras);
                           Main.cwb.ExecuteScriptAsync(script);
                        }
                        break;
                     case "getUpdates":
                        QuestServiceRoot rootQuest = JsonConvert.DeserializeObject<QuestServiceRoot>(body);
                        ListClass.QuestList = rootQuest.responseData.ToList();
                        ImportantLoaded[6] = true;
                        break;
                     case "getData":
                        ress = JsonConvert.DeserializeObject(body);
                        ListClass.Startup = ress;
                        Main.Updater.UpdateBuildings();
                        ImportantLoaded[16] = ImportantLoaded[15] = ImportantLoaded[14] = ListClass.Startup.Count > 0;
                        ImportantLoaded[7] = true;
                        break;
                     case "getLimitedBonuses":
                        BonusServiceRoot rootBonusService = JsonConvert.DeserializeObject<BonusServiceRoot>(body);
                        ListClass.Bonus = rootBonusService.responseData.ToList();
                        ImportantLoaded[8] = true;
                        break;
                     default:
                        break;
                  }
                  _StartupLoaded?.Invoke(RequestType.Startup);
               }
               break;
            case RequestType.Motivate:
               string[] motResponse = msg.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
               Polivate motivation = null;
               object rewardResources = null;
               foreach (string res in motResponse)
               {
                  var methode = res.Substring(0, res.IndexOf("{"));
                  var body = res.Substring(res.IndexOf("{"));
                  switch (methode)
                  {
                     case "rewardResources":
                        rewardResources = JsonConvert.DeserializeObject(body);
                        break;
                     case "polivateRandomBuilding":
                        motivation = JsonConvert.DeserializeObject<Polivate>(body);
                        break;
                     default:
                        break;
                  }
               }
               if (motivation == null) MessageBox.Show($"{strings.CriticalError} {strings.ResponseNotValid}");
               if (motivation.responseData.action == "polish" || motivation.responseData.action == "motivate" || (motivation.responseData.action == "polivate_failed" && rewardResources != null))
               {
                  int playerid = 0;
                  if (motivation.responseData.action == "polivate_failed")
                     playerid = int.Parse(idData);
                  else
                     playerid = motivation.responseData.mapEntity.player_id;
                  ListClass.doneMotivate.Add(playerid, (true, rewardResources));
               }
               else
                  MessageBox.Show($"{strings.UnknownAction} {motivation.responseData.action}");
               break;
            case RequestType.CollectIncident:
               string[] ciResponse = msg.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
               bool successed = false;
               dynamic Reward = null;
               foreach (string res in ciResponse)
               {
                  var methode = res.Substring(0, res.IndexOf("{"));
                  var body = res.Substring(res.IndexOf("{"));
                  dynamic ColIncRes = JsonConvert.DeserializeObject(body);
                  if (ColIncRes["requestClass"].ToString() == "HiddenRewardService")
                  {
                     if (ColIncRes["responseData"]["__class__"].ToString() == "Success")
                        successed = true;
                     if (Reward != null)
                     {
                        _incidentCollected?.Invoke(RequestType.CollectIncident, Reward);
                     }
                  }
                  else if (ColIncRes["requestClass"].ToString() == "RewardService")
                  {
                     Reward = ColIncRes["responseData"][0][0]["name"];
                     if (successed)
                     {
                        _incidentCollected?.Invoke(RequestType.CollectIncident, Reward);
                     }
                  }
               }
               break;
            case RequestType.VisitTavern:
               string[] TavResponse = msg.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
               if (_taverSitted != null)
                  _taverSitted?.Invoke(null);
               else
               {
                  object rewardTavern = null;
                  object TavernResultSitting = null;
                  TavernResult tavernresult = null;
                  foreach (string res in TavResponse)
                  {
                     dynamic resItem = JsonConvert.DeserializeObject(res);
                     if (resItem["requestMethod"] == "getOtherTavern")
                     {
                        rewardTavern = resItem["responseData"]["rewardResources"];
                     }
                     else if (resItem["requestMethod"] == "getOtherTavernState")
                     {
                        tavernresult = JsonConvert.DeserializeObject<TavernResult>(res);
                     }
                     else if (resItem["requestMethod"] == "getOtherTavern")
                     {
                        TavernResultSitting = resItem["responseData"]["state"];
                     }
                  }

                  if (TavernResultSitting.ToString() == "satDown")
                  {
                     if (idData != "" && tavernresult.responseData.ownerId == int.Parse(idData)) ListClass.doneTavern.Add(tavernresult.responseData.ownerId, (true, rewardTavern));
                  }
                  if (TavernResultSitting == null)
                     MessageBox.Show($"{strings.UnknownAction} {JsonConvert.SerializeObject(TavernResultSitting)}");
               }
               break;
            case RequestType.GetClanMember:
               Root<ClanMember> clan = JsonConvert.DeserializeObject<Root<ClanMember>>(msg);
               ListClass.ClanMemberList = clan.responseData.FindAll(c => c.is_self == false && c.is_friend == false);
               ImportantLoaded[9] = true;
               _ListLoaded?.Invoke(RequestType.GetClanMember);
               break;
            case RequestType.GetEntities:
               break;
            case RequestType.GetFriends:
               Root<Friend> friends = JsonConvert.DeserializeObject<Root<Friend>>(msg);
               ListClass.FriendList = friends.responseData.FindAll(f => f.is_self == false);
               ImportantLoaded[10] = true;
               _ListLoaded?.Invoke(RequestType.GetFriends);
               break;
            case RequestType.GetNeighbor:
               Root<Neighbor> neighbor = JsonConvert.DeserializeObject<Root<Neighbor>>(msg);
               ListClass.NeighborList = neighbor.responseData.FindAll(n => n.is_self == false && n.is_friend == false);
               ImportantLoaded[11] = true;
               _ListLoaded?.Invoke(RequestType.GetNeighbor);
               break;
            case RequestType.GetLGs:
               break;
            case RequestType.LogService:
               break;
            case RequestType.CollectProduction:
               dynamic ColRes = JsonConvert.DeserializeObject(msg);
               try
               {
                  if (ColRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "IdleState")
                  {
                     lock (_locker)
                     {
                        List<int> CollectedIDs = new List<int>();
                        foreach (var item in ColRes["responseData"]?["updatedEntities"])
                        {
                           CollectedIDs.Add(int.Parse(item["id"].ToString()));
                           int exIndex = ListClass.ProductionList.FindIndex(e => e.id == int.Parse(item["id"].ToString()));
                           if (exIndex >= 0)
                           {
                              EntityEx old = ListClass.ProductionList[exIndex];
                              ListClass.ProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(item.ToString());
                              ListClass.ProductionList[exIndex].name = old.name;
                              ListClass.ProductionList[exIndex].type = old.type;
                              ListClass.ProductionList[exIndex].available_products = old.available_products;
                           }
                           else
                           {
                              exIndex = ListClass.GoodProductionList.FindIndex(e => e.id == int.Parse(item["id"].ToString()));
                              if (exIndex >= 0)
                              {
                                 EntityEx old = ListClass.GoodProductionList[exIndex];
                                 ListClass.GoodProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(item.ToString());
                                 ListClass.GoodProductionList[exIndex].name = old.name;
                                 ListClass.GoodProductionList[exIndex].type = old.type;
                                 ListClass.GoodProductionList[exIndex].available_products = old.available_products;
                              }
                           }
                        }
                        if (Main.DEBUGMODE) Helper.Log($"[{DateTime.Now}] CollectedIDs Count = {CollectedIDs.Count}");
                        ListClass.CollectedIDs = CollectedIDs;
                        _productionCollected?.Invoke(RequestType.CollectProduction, ListClass.CollectedIDs);
                        return;
                     }
                  }
               }
               catch (Exception ex)
               { MessageBox.Show(ex.ToString(), strings.CollectingError); }
               break;
            case RequestType.QueryProduction:
               dynamic QueryRes = JsonConvert.DeserializeObject(msg);
               try
               {
                  if (QueryRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "ProducingState")
                  {
                     lock (_locker)
                     {
                        int id = int.Parse(QueryRes["responseData"]?["updatedEntities"]?[0]?["id"].ToString());
                        if (ListClass.AddedToQuery.Contains(id))
                        {
                           ListClass.doneQuery.Add(id);
                           int exIndex = ListClass.ProductionList.FindIndex(e => e.id == id);
                           if (exIndex >= 0)
                           {
                              EntityEx old = ListClass.ProductionList[exIndex];
                              ListClass.ProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(QueryRes["responseData"]?["updatedEntities"][0].ToString());
                              ListClass.ProductionList[exIndex].name = old.name;
                              ListClass.ProductionList[exIndex].type = old.type;
                              ListClass.ProductionList[exIndex].available_products = old.available_products;
                           }
                           else
                           {
                              exIndex = ListClass.GoodProductionList.FindIndex(e => e.id == int.Parse(QueryRes["responseData"]?["updatedEntities"][0]?["id"].ToString()));
                              if (exIndex >= 0)
                              {
                                 EntityEx old = ListClass.GoodProductionList[exIndex];
                                 ListClass.GoodProductionList[exIndex] = JsonConvert.DeserializeObject<EntityEx>(QueryRes["responseData"]?["updatedEntities"][0].ToString());
                                 ListClass.GoodProductionList[exIndex].name = old.name;
                                 ListClass.GoodProductionList[exIndex].type = old.type;
                                 ListClass.GoodProductionList[exIndex].available_products = old.available_products;
                              }
                           }
                        }
                        var a = ListClass.doneQuery.All(ListClass.AddedToQuery.Contains) && ListClass.AddedToQuery.Count == ListClass.doneQuery.Count;
                        if (a)
                        {
                           if (Main.DEBUGMODE) Helper.Log($"[{DateTime.Now}] doneQuery Count = {ListClass.doneQuery.Count}\n[{DateTime.Now}] AddedToQuery Count = {ListClass.AddedToQuery.Count}");
                           _productionStarted?.Invoke(RequestType.QueryProduction);
                        }
                        return;
                     }
                  }
               }
               catch (Exception ex)
               { MessageBox.Show(ex.ToString(), strings.StartingError); }
               break;
            case RequestType.CancelProduction:
               var can = msg;
               _productionCanceled?.Invoke(RequestType.CancelProduction);
               break;
            case RequestType.CollectTavern:
               try
               {
                  if (msg == "{}") return;
                  CollectResult cr = JsonConvert.DeserializeObject<CollectResult>(msg);
                  if (cr.responseData.__class__.ToLower() == "success")
                     _collectTavern?.Invoke(null);
                  else
                  {
                     MessageBox.Show(strings.CollectingTavernError);
                  }
               }
               catch (Exception)
               {
                  MessageBox.Show($"{strings.UnknownAction} {msg}");
               }
               break;
            case RequestType.GetOwnTavern:
               OwnTavernDataRoot otdr = JsonConvert.DeserializeObject<OwnTavernDataRoot>(msg);
               ListClass.OwnTavernData = otdr.responseData;
               ImportantLoaded[19] = true;
               break;
            case RequestType.RemovePlayer:
               _friendRemoved?.Invoke(null);
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
               ImportantLoaded[12] = true;
               break;
            default:
               break;
         }
         if (!ImportantLoaded[19] && ImportantLoaded.ToList().FindAll(p => p).Count == 19)
         {
            if (Main.cwb != null)
            {
               ImportantLoaded[19] = true;
               string script = Main.ReqBuilder.GetRequestScript(RequestType.GetOwnTavern, "[]");
               Main.cwb.ExecuteScriptAsync(script);
            }

         }
         else if (ImportantLoaded.All(b => { return b; }))
         {
            ImportantLoaded = Enumerable.Repeat(false, 20).ToArray();

            var friendMotivate = ListClass.FriendList.FindAll(f => (f.next_interaction_in == 0));
            var clanMotivate = ListClass.ClanMemberList.FindAll(f => (f.next_interaction_in == 0));
            var neighborlist = ListClass.NeighborList.FindAll(f => (f.next_interaction_in == 0));
            ListClass.Motivateable.AddDistinctRange(friendMotivate.Cast<Player>().ToList(), clanMotivate.Cast<Player>().ToList(), neighborlist.Cast<Player>().ToList());
            _EverythingImportantLoaded?.Invoke(null);
         }
      }
      public static void HandleMetadata(string msg, string metaType)
      {
         MetaRequestType type = (MetaRequestType)Enum.Parse(typeof(MetaRequestType), metaType);
         switch (type)
         {
            case MetaRequestType.city_entities:
               BuildingsRoot rootBuilding = JsonConvert.DeserializeObject<BuildingsRoot>("{\"buildings\":" + msg + "}");
               ListClass.AllBuildings = rootBuilding.buildings.ToList();
               ImportantLoaded[13] = true;
               Main.Updater.UpdateBuildings();
               ImportantLoaded[14] = ListClass.Startup.Count > 0;
               ImportantLoaded[15] = ListClass.ProductionList.Count > 0;
               ImportantLoaded[16] = ListClass.ResidentialList.Count > 0;
               break;
            case MetaRequestType.research_eras:
               ResearchEraRoot rootResearch = JsonConvert.DeserializeObject<ResearchEraRoot>("{\"reserach\":" + msg + "}");
               ListClass.Eras = rootResearch.reserach.ToList();
               ImportantLoaded[17] = true;
               Main.Updater.UpdatedSortedGoodList();
               ImportantLoaded[18] = ListClass.GoodsDict.Count > 0;
               break;
            default:
               break;
         }
         if (ImportantLoaded.All(b => { return b; }))
         {
            ImportantLoaded = Enumerable.Repeat(false, 19).ToArray();
            _EverythingImportantLoaded?.Invoke(null);
         }
      }
   }
   public delegate void WorldsLoadedEvent(object sender);
   public delegate void EverythingImportantLoadedEvent(object sender);
   public delegate void StartupLoadedEvent(RequestType type);
   public delegate void ListLoadedEvent(RequestType type);
   public delegate void CustomEvent(object sender, dynamic data = null);
}
