using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GEX.GetChests
{
   public class GetChestsGEX
   {
      public GetChestsResponse[] getChestsResponses { get; set; }
   }
   public class GetChestsResponse
   {
      public Data[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class Data
   {
      public int id { get; set; }
      public Chest chest { get; set; }
      public string __class__ { get; set; }
   }
   public class Chest
   {
      public Possible_Rewards[] possible_rewards { get; set; }
      public bool allowReroll { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }
   public class Possible_Rewards
   {
      public int drop_chance { get; set; }
      public Reward reward { get; set; }
      public string __class__ { get; set; }
   }
   public class Reward
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public int totalAmount { get; set; }
      public Unit? unit { get; set; }
   }
   public class Unit
   {
      public dynamic abilities { get; set; }
      public object[] bonuses { get; set; }
      public string unitTypeId { get; set; }
      public bool is_defending { get; set; }
      public bool isArenaDefending { get; set; }
      public bool is_attacking { get; set; }
      public bool healing_disabled { get; set; }
      public string __class__ { get; set; }
   }
}
