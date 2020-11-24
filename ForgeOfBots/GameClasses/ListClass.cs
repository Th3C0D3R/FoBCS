using System;
using System.Collections.Generic;
using ForgeOfBots.Utils;
using ForgeOfBots.GameClasses.ResponseClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ForgeOfBots.GameClasses
{
   public static class ListClass
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
      public static List<EntityEx> ResidentialList { get; set; } = new List<EntityEx>();
      public static List<EntityEx> ProductionList { get; set; } = new List<EntityEx>();
      public static List<EntityEx> GoodProductionList { get; set; } = new List<EntityEx>();
      public static Dictionary<string, List<Good>> GoodsDict { get; set; } = new Dictionary<string, List<Good>>();
      public static List<Player> Motivateable { get; set; } = new List<Player>();
      public static TavernData OwnTavernData { get; set; } = new TavernData();

      public static Dictionary<int, (bool, object)> doneMotivate { get; set; } = new Dictionary<int, (bool, object)>();
      public static Dictionary<int, (bool, object)> doneTavern { get; set; } = new Dictionary<int, (bool, object)>();
      public static List<int> doneQuery { get; set; } = new List<int>();
      public static List<int> AddedToQuery { get; set; } = new List<int>();
      public static List<int> CollectedIDs { get; set; } = new List<int>();
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
}
