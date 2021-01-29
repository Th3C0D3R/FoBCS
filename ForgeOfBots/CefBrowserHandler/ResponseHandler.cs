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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WorldSelection = ForgeOfBots.GameClasses.ResponseClasses.WorldSelection;
using static ForgeOfBots.Utils.StaticData;
using Microsoft.AppCenter.Crashes;
using System.IO;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ForgeOfBots.Forms.UserControls;
using ForgeOfBots.Forms;

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
      private static CustomEvent _entitiesUpdated;
      public static event CustomEvent EntitiesUpdated
      {
         add
         {
            if (_entitiesUpdated == null || !_entitiesUpdated.GetInvocationList().Contains(value))
               _entitiesUpdated += value;
         }
         remove
         {
            _entitiesUpdated -= value;
         }
      }
      private static CustomEvent _IncidentUpdated;
      public static event CustomEvent IncidentUpdated
      {
         add
         {
            if (_IncidentUpdated == null || !_IncidentUpdated.GetInvocationList().Contains(value))
               _IncidentUpdated += value;
         }
         remove
         {
            _IncidentUpdated -= value;
         }
      }
      private static CustomEvent _AllLGsStepped;
      public static event CustomEvent AllLGsStepped
      {
         add
         {
            if (_AllLGsStepped == null || !_AllLGsStepped.GetInvocationList().Contains(value))
               _AllLGsStepped += value;
         }
         remove
         {
            _AllLGsStepped -= value;
         }
      }

      public static bool[] ImportantLoaded = Enumerable.Repeat(false, 20).ToArray();
      static readonly object _locker = new object();
      static readonly object _lockerLG = new object();
      static readonly object _lockerSnip = new object();
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
            if (cwb != null)
            {
               ImportantLoaded[19] = true;
               string script = ReqBuilder.GetRequestScript(RequestType.GetOwnTavern, "[]");
               cwb.ExecuteScriptAsync(script);
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
                        Updater.UpdatedSortedGoodList();
                        ImportantLoaded[18] = ListClass.GoodsDict.Count > 0;
                        ImportantLoaded[3] = true;
                        break;
                     case "getPlayerResources":
                        ress = JsonConvert.DeserializeObject(body);
                        ListClass.Resources = ress;
                        Updater.UpdatedSortedGoodList();
                        ImportantLoaded[18] = ListClass.GoodsDict.Count > 0;
                        ImportantLoaded[4] = true;
                        break;
                     case "getMetadata":
                        MetadataRoot rootMetadata = JsonConvert.DeserializeObject<MetadataRoot>(body);
                        ListClass.MetaDataList = rootMetadata.responseData.ToList();
                        ImportantLoaded[5] = true;
                        if (cwb == null) break;
                        if (ListClass.AllBuildings.Count <= 0)
                        {
                           string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "city_entities"); }).url;
                           string script = ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.city_entities);
                           cwb.ExecuteScriptAsync(script);
                        }
                        if (ListClass.Eras.Count <= 0)
                        {
                           string url = ListClass.MetaDataList.Find((m) => { return (m.identifier == "research_eras"); }).url;
                           string script = ReqBuilder.GetMetaDataRequestScript(url, MetaRequestType.research_eras);
                           cwb.ExecuteScriptAsync(script);
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
                        ListClass.UserData = ress["responseData"]["user_data"];
                        Updater.UpdateBuildings(ListClass.Startup["responseData"]["city_map"]["entities"]);
                        ImportantLoaded[16] = ImportantLoaded[15] = ImportantLoaded[14] = ListClass.Startup.Count > 0;
                        ImportantLoaded[7] = true;
                        break;
                     case "getLimitedBonuses":
                        BonusServiceRoot rootBonusService = JsonConvert.DeserializeObject<BonusServiceRoot>(body);
                        ListClass.Bonus = rootBonusService.responseData.ToList();
                        ListClass.ArcBonus = ListClass.Bonus.Sum(e => { if (e.type == "contribution_boost") return e.value; else return 0; });
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
               if (motivation == null && msg.Contains("\"error_code\":202,\"__class__\":\"Error\""))
               {
                  if (!ListClass.doneMotivate.ContainsKey(int.Parse(idData)))
                     ListClass.doneMotivate.Add(int.Parse(idData), (false, null));
                  break;
               }
               else
               {
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
               }
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
                     var methode = res.Substring(0, res.IndexOf("{"));
                     var body = res.Substring(res.IndexOf("{"));
                     dynamic resItem = JsonConvert.DeserializeObject(body);
                     if (resItem["requestMethod"] == "getOtherTavern")
                     {
                        rewardTavern = resItem["responseData"]["rewardResources"];
                        TavernResultSitting = resItem["responseData"]["state"];
                     }
                     else if (resItem["requestMethod"] == "getOtherTavernState")
                     {
                        tavernresult = JsonConvert.DeserializeObject<TavernResult>(body);
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
               dynamic entities = JsonConvert.DeserializeObject(msg);
               Updater.UpdateBuildings(entities["responseData"]);
               _entitiesUpdated?.Invoke(ListClass.State, RequestType.GetEntities);
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
               //if (msg.Length <= 4) break;
               //LGRootObject lgs = JsonConvert.DeserializeObject<LGRootObject>(msg);
               //List<LGData> LGData = lgs.responseData.ToList();
               //if (LGData.Count == 0) break;
               //ListClass.SnipablePlayers.Find(p => p.player_id == LGData[0].player.player_id).LGs = LGData;
               //foreach (Player item in ListClass.SnipablePlayers)
               //{
               //   foreach (LGData lg in item.LGs)
               //   {
               //      lock (_lockerLG)
               //      {
               //         int UnderScorePos = lg.city_entity_id.IndexOf("_");
               //         string AgeString = lg.city_entity_id.Substring(UnderScorePos + 1);
               //         UnderScorePos = AgeString.IndexOf("_");
               //         AgeString = AgeString.Substring(0, UnderScorePos);
               //         if (lg.current_progress == null)
               //            lg.current_progress = 0;
               //         int P1 = Helper.GetP1(AgeString, lg.level);
               //         ListClass.ArcBonus = (ListClass.ArcBonus == 0 ? 1 : ListClass.ArcBonus);
               //         if (ListClass.ArcBonus >= 2) ListClass.ArcBonus = (ListClass.ArcBonus / 100) + 1;
               //         if (lg.rank == null && P1 * ListClass.ArcBonus >= (lg.max_progress - lg.current_progress) / 2)
               //         {
               //            ListClass.PossibleSnipLGs.Add(new LGSnip()
               //            {
               //               player = lg.player,
               //               entity_id = lg.entity_id,
               //               city_entity_id = lg.city_entity_id,
               //               name = item.name,
               //               level = lg.level,
               //               state = lg.state,
               //               GewinnString = "",
               //               KurzString = "",
               //               current_progress = lg.current_progress,
               //               max_progress = lg.max_progress,
               //               rank = lg.rank,
               //               __class__ = lg.__class__
               //            });
               //         }
               //      }
               //   }
               //}
               //_AllLGsStepped?.Invoke(null, null);
               break;
            case RequestType.getConstruction:
               lock (_lockerSnip)
               {
                  string[] snipResponse = msg.Split(new[] { "##@##" }, StringSplitOptions.RemoveEmptyEntries);
                  LGSnip LGsnip = ListClass.PossibleSnipLGs.Find(e => e.entity_id.ToString() == idData);
                  foreach (var res in snipResponse)
                  {
                     var methode = res.Substring(0, res.IndexOf("{"));
                     var body = res.Substring(res.IndexOf("{"));
                     dynamic resItem = JsonConvert.DeserializeObject(body);
                     if (resItem["requestMethod"] == "updateEntity")
                     {
                        LGsnip.state = new LGState()
                        {
                           forge_points_for_level_up = (int?)resItem["responseData"][0]["state"]["forge_points_for_level_up"],
                           invested_forge_points = (int?)resItem["responseData"][0]["state"]["invested_forge_points"]
                        };
                        LGsnip.maxLevel = (int)resItem["responseData"][0]["max_level"];
                     }
                     else if (resItem["requestMethod"] == "getConstruction")
                     {
                        SnipRoot SnipeRoot = JsonConvert.DeserializeObject<SnipRoot>(body);
                        SnipResponse SnipResponse = SnipeRoot.responseData;
                        Ranking[] Rankings = SnipResponse.rankings;

                        ArrayList hFordern = new ArrayList();
                        ArrayList hBPMeds = new ArrayList();
                        ArrayList hSnipen = new ArrayList();
                        var arc = ListClass.ArcBonus >= 2 ? (ListClass.ArcBonus / 100) + 1 : ListClass.ArcBonus;
                        var ForderArc = 1.90f;
                        int EigenPos = 0, EigenBetrag = 0;
                        for (int i = 0; i < Rankings.Length; i++)
                        {
                           if (Rankings[i].player != null && Rankings[i].player.player_id == int.Parse(ListClass.UserData["player_id"].ToString()))
                           {
                              EigenPos = i;
                              EigenBetrag = Rankings[i].forge_points >= 0 ? Rankings[i].forge_points : 0;
                              break;
                           }
                        }
                        string[] ForderStates = new string[6];
                        string[] SnipeStates = new string[6];
                        int[] FPNettoRewards = new int[6];
                        int[] FPRewards = new int[6];
                        int[] BPRewards = new int[6];
                        int[] MedalRewards = new int[6];
                        int[] ForderFPRewards = new int[6];
                        int[] ForderRankCosts = new int[6];
                        int[] SnipeRankCosts = new int[6];
                        int[] Einzahlungen = new int[6];
                        int BestGewinn = -999999;
                        int RankProfit = -1;

                        for (int i = 0; i < Rankings.Length; i++)
                        {
                           Ranking rang = Rankings[i];
                           int Rank = -1, CurrentFP = 0, TotalFP = 0, RestFP = 0;
                           bool IsSelf = false;
                           if (rang.rank < 0) continue;
                           else Rank = rang.rank - 1;
                           if (rang.reward == null) continue;
                           ForderStates[Rank] = "";
                           SnipeStates[Rank] = "";
                           FPNettoRewards[Rank] = 0;
                           FPRewards[Rank] = 0;
                           BPRewards[Rank] = 0;
                           MedalRewards[Rank] = 0;
                           ForderFPRewards[Rank] = 0;
                           ForderRankCosts[Rank] = -1;
                           SnipeRankCosts[Rank] = -1;
                           Einzahlungen[Rank] = 0;

                           if (rang.reward.strategy_point_amount >= 0)
                              FPNettoRewards[Rank] = rang.reward.strategy_point_amount;
                           if (rang.reward.blueprints >= 0)
                              BPRewards[Rank] = rang.reward.blueprints;
                           if (rang.reward.resources.medals >= 0)
                              MedalRewards[Rank] = rang.reward.resources.medals;

                           FPRewards[Rank] = (int)Math.Round((double)FPNettoRewards[Rank] * arc);
                           BPRewards[Rank] = (int)Math.Round((double)BPRewards[Rank] * arc);
                           MedalRewards[Rank] = (int)Math.Round((double)MedalRewards[Rank] * arc);
                           ForderFPRewards[Rank] = (int)Math.Round((double)FPNettoRewards[Rank] * ForderArc);

                           if (rang.player != null && Rankings[i].player.player_id == int.Parse(ListClass.UserData["player_id"].ToString()))
                              IsSelf = true;

                           if (rang.forge_points >= 0)
                              Einzahlungen[Rank] = rang.forge_points;
                           if (LGsnip.state != null)
                           {
                              if (LGsnip.state.invested_forge_points == null) LGsnip.state.invested_forge_points = 0;
                              CurrentFP = (LGsnip.state.invested_forge_points.Value >= 0 ? LGsnip.state.invested_forge_points.Value : 0) - EigenBetrag;
                              TotalFP = LGsnip.state.forge_points_for_level_up.Value;
                              RestFP = TotalFP - CurrentFP;
                           }
                           if (!IsSelf)
                           {
                              SnipeRankCosts[Rank] = (int)Math.Round((double)(Einzahlungen[Rank] + RestFP) / 2);
                              ForderRankCosts[Rank] = Math.Max(ForderFPRewards[Rank], SnipeRankCosts[Rank]);
                              ForderRankCosts[Rank] = Math.Min(ForderRankCosts[Rank], RestFP);
                              bool exitLoop = false;
                              if (SnipeRankCosts[Rank] <= Einzahlungen[Rank])
                              {
                                 ForderRankCosts[Rank] = 0;
                                 ForderStates[Rank] = "NotPossible";
                                 exitLoop = true;
                              }
                              else
                              {
                                 if (ForderRankCosts[Rank] == RestFP)
                                    ForderStates[Rank] = "LevelWarning";
                                 else if (ForderRankCosts[Rank] <= ForderFPRewards[Rank])
                                    ForderStates[Rank] = "Profit";
                                 else
                                    ForderStates[Rank] = "NegativeProfit";
                              }
                              if (SnipeRankCosts[Rank] <= Einzahlungen[Rank])
                              {
                                 SnipeRankCosts[Rank] = 0;
                                 SnipeStates[Rank] = "NotPossible";
                                 exitLoop = true;
                              }
                              else
                              {
                                 if (SnipeRankCosts[Rank] == RestFP)
                                    SnipeStates[Rank] = "LevelWarning";
                                 else if (FPRewards[Rank] <= SnipeRankCosts[Rank])
                                    SnipeStates[Rank] = "NegativeProfit";
                                 else
                                 {
                                    SnipeStates[Rank] = "Profit";
                                    var CurrentGewinn = FPRewards[Rank] - SnipeRankCosts[Rank];
                                    if (CurrentGewinn > BestGewinn)
                                    {
                                       if (SnipeStates[Rank] == "Profit")
                                       {
                                          BestGewinn = CurrentGewinn;
                                          RankProfit = Rank;
                                          exitLoop = true;
                                       }
                                    }
                                 }
                              }
                              if (exitLoop)
                                 continue;
                           }
                        }
                        if (SnipeStates.Contains("Profit"))
                        {
                           if (ListClass.SnipWithProfit.Find(e => e.entity_id == LGsnip.entity_id && e.player.player_id == LGsnip.player.player_id) != null) continue;
                           if (LGsnip.level == LGsnip.maxLevel) continue;
                           if (BestGewinn < StaticData.UserData.MinProfit) continue;
                           int SnipCost = FPRewards[RankProfit] - BestGewinn;
                           LGsnip.KurzString = $"{(int)((float)FPRewards[RankProfit] / SnipCost * 100)} %";
                           LGsnip.GewinnString = $"{BestGewinn}";
                           ListClass.SnipWithProfit.Add(LGsnip);
                        }
                        if (ListClass.AllLGs.Find(e => e.entity_id == LGsnip.entity_id && e.player.player_id == LGsnip.player.player_id) != null) continue;
                        ListClass.AllLGs.Add(LGsnip);
                     }
                  }
               }
               break;
            case RequestType.LogService:
               break;
            case RequestType.CollectProduction:
               JToken ColRes = JsonConvert.DeserializeObject<JToken>(msg);
               try
               {
                  if (ColRes["responseData"]?["updatedEntities"]?.ToList().Count > 0
                     && ColRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "IdleState")
                  {
                     lock (_locker)
                     {
                        List<int> CollectedIDs = new List<int>();
                        foreach (var item in ColRes["responseData"]?["updatedEntities"].ToList())
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
                        if (DEBUGMODE) Helper.Log($"[{DateTime.Now}] CollectedIDs Count = {CollectedIDs.Count}");
                        ListClass.CollectedIDs = CollectedIDs;
                        _productionCollected?.Invoke(RequestType.CollectProduction, ListClass.CollectedIDs);
                     }
                  }
               }
               catch (Exception ex)
               {
                  NLog.LogManager.Flush();
                  var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
                  var properties = new Dictionary<string, string> { { "CollectProduction", msg } };
                  Crashes.TrackError(ex, properties, attachments);
               }
               break;
            case RequestType.QueryProduction:
               JToken QueryRes = JsonConvert.DeserializeObject<JToken>(msg);
               try
               {
                  if (QueryRes["responseData"]?["updatedEntities"]?.ToList().Count > 0
                     && QueryRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "ProducingState")
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
                           ListClass.doneQuery.Clear();
                           ListClass.AddedToQuery.Clear();
                           if (DEBUGMODE) Helper.Log($"[{DateTime.Now}] doneQuery Count = {ListClass.doneQuery.Count}\n[{DateTime.Now}] AddedToQuery Count = {ListClass.AddedToQuery.Count}");
                           _productionStarted?.Invoke(RequestType.QueryProduction);
                        }
                     }
                  }
               }
               catch (Exception ex)
               {
                  NLog.LogManager.Flush();
                  var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
                  var properties = new Dictionary<string, string> { { "QueryProduction", msg } };
                  Crashes.TrackError(ex, properties, attachments);
               }
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
               catch (Exception ex)
               {
                  NLog.LogManager.Flush();
                  var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
                  var properties = new Dictionary<string, string> { { "CollectTavern", msg } };
                  Crashes.TrackError(ex, properties, attachments);
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
            case RequestType.GetIncidents:
               HiddenRewardRoot newHiddenRewards = JsonConvert.DeserializeObject<HiddenRewardRoot>(msg);
               foreach (HiddenReward item in newHiddenRewards.responseData.hiddenRewards)
               {
                  DateTime endTime = Helper.UnixTimeStampToDateTime(item.expireTime);
                  DateTime startTime = Helper.UnixTimeStampToDateTime(item.startTime);
                  bool vis = (endTime > DateTime.Now) && (startTime < DateTime.Now);
                  item.isVisible = vis;
               }
               ListClass.HiddenRewards = newHiddenRewards.responseData.hiddenRewards.ToList();
               _IncidentUpdated?.Invoke(null, -1);
               break;
            case RequestType.GEServiceOverview:
               dynamic GEOverview = JsonConvert.DeserializeObject(msg);
               GEOverview = GEOverview["responseData"];
               GuildExpedition ge = new GuildExpedition
               {
                  state = GEOverview["state"].ToString(),
                  chests = new List<Chest>()
               };
               GEProgress progress = new GEProgress
               {
                  CurrentEntityId = int.Parse(GEOverview["progress"]["currentEntityId"].ToString()),
                  difficulty = Enum.Parse(typeof(E_Difficulty), GEOverview["progress"]["difficulty"].ToString())
               };
               ge.progress = progress;
               foreach (dynamic item in GEOverview["chests"])
               {
                  Chest c = new Chest
                  {
                     chest = item["chest"],
                     id = int.Parse(item["id"].ToString())
                  };
                  ge.chests.Add(c);
               }
               break;
            case RequestType.GEServiceEncounter:
               dynamic GEEncounter = JsonConvert.DeserializeObject(msg);
               if (GEX != null)
               {
                  if (!GEX.isChest())
                  {

                  }
               }
               break;
            case RequestType.GEServiceCollectChest:
               dynamic GECollectChest = JsonConvert.DeserializeObject(msg);
               break;
            case RequestType.GEAttack:
               dynamic GEAttack = JsonConvert.DeserializeObject(msg);
               break;
            default:
               break;
         }
         if (!ImportantLoaded[19] && ImportantLoaded.ToList().FindAll(p => p).Count == 19)
         {
            if (cwb != null)
            {
               ImportantLoaded[19] = true;
               string script = ReqBuilder.GetRequestScript(RequestType.GetOwnTavern, "[]");
               cwb.ExecuteScriptAsync(script);
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
               Updater.UpdateBuildings(ListClass.Startup["responseData"]["city_map"]["entities"]);
               ImportantLoaded[14] = ListClass.Startup.Count > 0;
               ImportantLoaded[15] = ListClass.ProductionList.Count > 0;
               ImportantLoaded[16] = ListClass.ResidentialList.Count > 0;
               break;
            case MetaRequestType.research_eras:
               ResearchEraRoot rootResearch = JsonConvert.DeserializeObject<ResearchEraRoot>("{\"reserach\":" + msg + "}");
               ListClass.Eras = rootResearch.reserach.ToList();
               ImportantLoaded[17] = true;
               Updater.UpdatedSortedGoodList();
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
}
