using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class Player
   {
      public int? player_id { get; set; } = null;
      public string name { get; set; }
      public int next_interaction_in { get; set; }
      public bool is_friend { get; set; }
      public bool is_neighbor { get; set; }
      public bool is_guild_member { get; set; }
      public bool is_active { get; set; }
      public int score { get; set; }
      public bool is_self { get; set; }
      public string profile_text { get; set; }
      public string city_name { get; set; }
      public bool has_great_building { get; set; }
      public string avatar { get; set; }
      public List<LGSnip> LGs { get; set; }

      public override string ToString()
      {
         return name;
      }
   }
   public partial class Neighbor : Player
   {
      public int rank { get; set; }
      public bool is_online { get; set; }
      public bool isInvitedToClan { get; set; }
      public int clan_id { get; set; }
      public Clan clan { get; set; }
      public bool canSabotage { get; set; }
      public Topachievement[] topAchievements { get; set; }
      public string __class__ { get; set; }

   }
   public partial class Friend : Player
   {
      public int id { get; set; }
      public bool incoming { get; set; }
      public bool accepted { get; set; }
      public bool registered { get; set; }
      public bool rewarded { get; set; }
      public int rank { get; set; }
      public bool isInvitedToClan { get; set; }
      public bool isInvitedFriend { get; set; }
      public int clan_id { get; set; }
      public Clan clan { get; set; }
      public bool canSabotage { get; set; }
      public string __class__ { get; set; }
      public Topachievement[] topAchievements { get; set; }
   }
   public class ClanMember : Player
   {
      public string title { get; set; }
      public int rank { get; set; }
      public bool is_online { get; set; }
      public int clan_id { get; set; }
      public Clan clan { get; set; }
      public int won_battles { get; set; }
      public bool canSabotage { get; set; }
      public Topachievement[] topAchievements { get; set; }
      public string __class__ { get; set; }
   }
   public partial class Clan
   {
      public string description { get; set; }
      public Settings settings { get; set; }
      public int treasury_id { get; set; }
      public int id { get; set; }
      public string name { get; set; }
      public int membersNum { get; set; }
      public string flag { get; set; }
      public string __class__ { get; set; }
   }
   public class Settings
   {
      public string join { get; set; }
      public string __class__ { get; set; }
   }
   public class Topachievement
   {
      public int slot { get; set; }
      public Achievement achievement { get; set; }
      public string __class__ { get; set; }
   }
   public class Achievement
   {
      public Currentlevel currentLevel { get; set; }
      public Nextlevel nextLevel { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string descriptionTemplate { get; set; }
      public string descriptionCityTooltip { get; set; }
      public string __class__ { get; set; }
   }
   public class Currentlevel
   {
      public int level { get; set; }
      public long progress { get; set; }
      public int requiredProgress { get; set; }
      public int obtainedAt { get; set; }
      public string __class__ { get; set; }
   }
   public class Nextlevel
   {
      public int level { get; set; }
      public long progress { get; set; }
      public string __class__ { get; set; }
   }
   public static class UsernameList
   {
      public static string[] Get
      {
         get
         {
            List<string> nameList = new List<string>();
            if (ListClass.NeighborList.Count > 0) nameList.AddRange(ListClass.NeighborList.Select(n => $"{n.name} ({n.player_id})"));
            if (ListClass.FriendList.Count > 0) nameList.AddRange(ListClass.FriendList.Select(f => $"{f.name} ({f.player_id})"));
            if (ListClass.ClanMemberList.Count > 0) nameList.AddRange(ListClass.ClanMemberList.Select(c => $"{c.name} ({c.player_id})"));
            return nameList.ToArray();
         }
      }
   }
}
