//using CefSharp.WinForms;
using ForgeOfBots.DataHandler;
using ForgeOfBots.GameClasses;
using CefSharp.OffScreen;
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
using System.Windows.Forms;

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
        
        public static bool[] ImportantLoaded = Enumerable.Repeat(false, 20).ToArray();
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
        public static void HandleResponse(string msg, string method)
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
                                ListClass.HiddenRewards = rootHiddenReward.responseData.hiddenRewards.ToList();
                                ImportantLoaded[2] = true;
                                break;
                            case "getResourceDefinitions":
                                ResourceDefinitionRoot rootResourceDefinition = JsonConvert.DeserializeObject<ResourceDefinitionRoot>(body);
                                ListClass.ResourceDefinitions = rootResourceDefinition.responseData.ToList();
                                ImportantLoaded[3] = true;
                                break;
                            case "getPlayerResources":
                                ResourceRoot rootResource = JsonConvert.DeserializeObject<ResourceRoot>(body);
                                ListClass.Resources = rootResource.responseData.resources;
                                ImportantLoaded[4] = true;
                                break;
                            case "getMetadata":
                                MetadataRoot rootMetadata = JsonConvert.DeserializeObject<MetadataRoot>(body);
                                ListClass.MetaDataList = rootMetadata.responseData.ToList();
                                ImportantLoaded[5] = true;
                                if (Main.cwb == null) break;
                                string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "city_entities"); }).url;
                                string script = Main.ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.city_entities);
                                Main.cwb.ExecuteScriptAsync(script);
                                url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "research_eras"); }).url;
                                script = Main.ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.research_eras);
                                Main.cwb.ExecuteScriptAsync(script);
                                break;
                            case "getUpdates":
                                QuestServiceRoot rootQuest = JsonConvert.DeserializeObject<QuestServiceRoot>(body);
                                ListClass.QuestList = rootQuest.responseData.ToList();
                                ImportantLoaded[6] = true;
                                break;
                            case "getData":
                                StartupRoot rootStartup = JsonConvert.DeserializeObject<StartupRoot>(body);
                                ListClass.Startup = rootStartup.responseData;
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
                    Polivate motivation = JsonConvert.DeserializeObject<Polivate>(msg);
                    if(motivation.responseData.action == "polish")
                        ListClass.doneMotivate.Add(motivation.responseData.mapEntity.player_id, true);
                    else
                        MessageBox.Show($"unknown Action: {motivation.responseData.action}");
                    break;
                case RequestType.CollectIncident:
                    break;
                case RequestType.VisitTavern:
                    try
                    {
                        _taverSitted?.Invoke(null);
                    }
                    catch (Exception)
                    {
                        TavernResult tavernresult = JsonConvert.DeserializeObject<TavernResult>(msg);
                        if (tavernresult.responseData.state == "isSitting")
                            ListClass.doneTavern.Add(tavernresult.responseData.ownerId, true);
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
                    break;
                case RequestType.QueryProduction:
                    break;
                case RequestType.CancelProduction:
                    break;
                case RequestType.CollectTavern:
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
                    foreach (Entity cityEntity in ListClass.Startup.city_map.entities.ToList())
                    {
                        foreach (Building metaEntity in ListClass.AllBuildings)
                        {
                            if (cityEntity.cityentity_id == metaEntity.id)
                            {
                                EntityEx entity = GameClassHelper.CopyFrom(cityEntity);
                                entity.name = metaEntity.name;
                                if (metaEntity.available_products != null)
                                    entity.available_products = metaEntity.available_products.ToList();
                                entity.type = metaEntity.type;
                                if (entity.type == "production" && metaEntity.available_products != null && entity.connected == 1 && GameClassHelper.hasOnlySupplyProduction(entity.available_products))
                                    ListClass.ProductionList.Add(entity);
                                else if (entity.type == "residential" && entity.connected == 1)
                                    ListClass.ResidentialList.Add(entity);
                                else if (entity.type == "goods" && entity.connected == 1)
                                    ListClass.GoodProductionList.Add(entity);
                            }
                        }
                    }
                    ImportantLoaded[14] = ImportantLoaded[15] = ImportantLoaded[16] = true;
                    break;
                case MetaRequestType.research_eras:
                    ResearchEraRoot rootResearch = JsonConvert.DeserializeObject<ResearchEraRoot>("{\"reserach\":" + msg + "}");
                    ListClass.Eras = rootResearch.reserach.ToList();
                    ImportantLoaded[17] = true;
                    ListClass.GoodsDict = Helper.GetGoodsEraSorted(ListClass.Eras, ListClass.Resources, ListClass.ResourceDefinitions);
                    ImportantLoaded[18] = true;
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
    public delegate void CustomEvent(object sender);
}
