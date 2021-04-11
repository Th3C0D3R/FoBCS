using System;
using System.Collections.Generic;
using ForgeOfBots.Utils;
using ForgeOfBots.GameClasses.ResponseClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using ForgeOfBots.Forms.UserControls;
using ForgeOfBots.GameClasses.GBG.Map;

namespace ForgeOfBots.GameClasses
{
   public class ListClass
   {
      public static List<Tuple<string, string, WorldState>> WorldList { get; set; } = new List<Tuple<string, string, WorldState>>();
      public static Dictionary<string, string> ServerList { get; set; } = new Dictionary<string, string>()
      {
         {"en", "en.forgeofempires.com"},
         {"de", "de.forgeofempires.com"},
         {"zz (Beta)", "zz.forgeofempires.com"},
         {"us", "us.forgeofempires.com"},
         {"fr", "fr.forgeofempires.com"},
         {"nl", "nl.forgeofempires.com"},
         {"pl", "pl.forgeofempires.com"},
         {"gr", "gr.forgeofempires.com"},
         {"it", "it.forgeofempires.com"},
         {"es", "es.forgeofempires.com"},
         {"pt", "pt.forgeofempires.com"},
         {"ru", "ru.forgeofempires.com"},
         {"ro", "ro.forgeofempires.com"},
         {"br", "br.forgeofempires.com"},
         {"cz", "cz.forgeofempires.com"},
         {"hu", "hu.forgeofempires.com"},
         {"se", "se.forgeofempires.com"},
         {"sk", "sk.forgeofempires.com"},
         {"tr", "tr.forgeofempires.com"},
         {"dk", "dk.forgeofempires.com"},
         {"no", "no.forgeofempires.com"},
         {"th", "th.forgeofempires.com"},
         {"ar", "ar.forgeofempires.com"},
         {"mx", "mx.forgeofempires.com"},
         {"fi", "fi.forgeofempires.com"}
      };
      public static List<World> AllWorlds { get; set; } = new List<World>();
      public static List<Player> AllPlayers { get; set; } = new List<Player>();
      public static List<Friend> FriendList { get; set; } = new List<Friend>();
      public static List<Neighbor> NeighborList { get; set; } = new List<Neighbor>();
      public static List<ClanMember> ClanMemberList { get; set; } = new List<ClanMember>();
      public static OwnTavernStates OwnTavern { get; set; } = new OwnTavernStates();
      public static List<FriendTavernState> FriendTaverns { get; set; } = new List<FriendTavernState>();
      public static List<HiddenReward> HiddenRewards { get; set; } = new List<HiddenReward>();
      public static JObject ResourceDefinitions { get; set; } = new JObject();
      public static JObject Resources { get; set; } = new JObject();
      public static List<Metadata> MetaDataList { get; set; } = new List<Metadata>();
      public static List<Quest> QuestList { get; set; } = new List<Quest>();
      public static JObject Startup { get; set; } = new JObject();
      public static List<BonusService> Bonus { get; set; } = new List<BonusService>();
      public static List<Building> AllBuildings { get; set; } = new List<Building>();
      public static List<ResearchEra> Eras { get; set; } = new List<ResearchEra>();
      public static List<UnitType> UnitTypes { get; set; } = new List<UnitType>();
      public static ArmyRoot Army { get; set; } = new ArmyRoot();
      public static GBGMap MetaMap { get; set; } = null;
      public static List<GBG.Get.Province> ProvincesGBG { get; set; } = new List<GBG.Get.Province>();
      public static List<EntityEx> ResidentialList { get; set; } = new List<EntityEx>();
      public static List<EntityEx> ProductionList { get; set; } = new List<EntityEx>();
      public static List<EntityEx> GoodProductionList { get; set; } = new List<EntityEx>();
      public static List<EntityEx> FinishedProductions { get; set; } = new List<EntityEx>();
      public static List<EntityEx> EveryProduction { get; set; } = new List<EntityEx>();
      public static Dictionary<string, List<Good>> GoodsDict { get; set; } = new Dictionary<string, List<Good>>();
      public static Dictionary<string, List<Unit>> UnitListEraSorted { get; set; } = new Dictionary<string, List<Unit>>();
      public static List<Unit> UnitList { get; set; } = new List<Unit>();
      public static List<Player> Motivateable { get; set; } = new List<Player>();
      public static TavernData OwnTavernData { get; set; } = new TavernData();
      public static MessageRoot MessageCenter { get; set; } = new MessageRoot();
      public static List<LGContribution> Contributions { get; set; } = new List<LGContribution>();
      public static LanguageList AvailableLanguages { get; set; } = new LanguageList();
      public static List<LGSnip> AllLGs { get; set; } = new List<LGSnip>();
      public static List<LGSnip> SnipWithProfit { get; set; } = new List<LGSnip>();
      public static List<Player> SnipablePlayers { get; set; } = new List<Player>();
      public static List<ProdListItem> ProductionItems { get; set; } = new List<ProdListItem>();
      public static List<Boost> BoostList { get; set; } = new List<Boost>();
      public static Dictionary<int, (bool, object)> doneMotivate { get; set; } = new Dictionary<int, (bool, object)>();
      public static Dictionary<int, (bool, object)> doneTavern { get; set; } = new Dictionary<int, (bool, object)>();
      public static List<int> doneQuery { get; set; } = new List<int>();
      public static List<int> AddedToQuery { get; set; } = new List<int>();
      public static List<BackgroundWorkerEX> BackgroundWorkers { get; set; } = new List<BackgroundWorkerEX>();
      public static Dictionary<string, string> PortraitList { get; set; } = new Dictionary<string, string>();

      /// <summary>
      /// Indicates the state of the ProductionCycle
      /// 0 = Idle -> Producing
      /// 1 = Producing Finished -> Idle
      /// 2 = Producing
      /// </summary>
      public static float ArcBonus { get; set; } = 190;
      public static JObject UserData { get; set; } = new JObject();
      public static Inventory Inventory { get; set; } = null;

      public static void ClearListClass()
      {
         Startup = new JObject();
         Resources = new JObject();
         ResourceDefinitions = new JObject();
         AllBuildings.Clear();
         AllLGs.Clear();
         SnipWithProfit.Clear();
         SnipablePlayers.Clear();
         ResidentialList.Clear();
         QuestList.Clear();
         ProductionList.Clear();
         ProductionItems.Clear();
         OwnTavernData = new TavernData();
         OwnTavern = new OwnTavernStates();
         NeighborList.Clear();
         FriendList.Clear();
         ClanMemberList.Clear();
         Motivateable.Clear();
         HiddenRewards.Clear();
         GoodProductionList.Clear();
         GoodsDict.Clear();
         FriendTaverns.Clear();
         Eras.Clear();
         Contributions.Clear();
         Bonus.Clear();
         doneMotivate.Clear();
         doneQuery.Clear();
         doneTavern.Clear();
         Inventory = new Inventory();
         PortraitList.Clear();
         AllPlayers.Clear();
      }

   }
   public enum WorldState
   {
      available,
      current,
      active,
      suggestion,
      none,
   }
   public enum ListType
   {
      friends,
      members,
      neighbors
   }
   [Flags]
   public enum SnipTarget
   {
      none = 0,
      friends = 1,
      members = 2,
      neighbors = 4,
   }
}
