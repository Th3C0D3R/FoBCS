using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class PlayerInventory
   {
      public Items[] Items { get; set; }
      public int FPStock { get; set; }
   }
   public class Inventory
   {
      public Items[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
   }

   public class Items
   {
      public int id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public int inStock { get; set; }
      public Item item { get; set; }
      public string itemAssetName { get; set; }
      public Exchangebasevalue exchangeBaseValue { get; set; }
      public bool locked { get; set; }
      public int[] sortPriority { get; set; }
      public string __class__ { get; set; }
      public int availableFrom { get; set; }
   }

   public class Item
   {
      public string boostType { get; set; }
      public int value { get; set; }
      public int duration { get; set; }
      public string __class__ { get; set; }
      public RewardInv reward { get; set; }
      public string cityEntityId { get; set; }
      public int level { get; set; }
      public Resource_Package resource_package { get; set; }
      public string target { get; set; }
      public string selectionKitId { get; set; }
      public string upgradeItemId { get; set; }
   }

   public class RewardInv
   {
      public Possible_Rewards[] possible_rewards { get; set; }
      public bool allowReroll { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
      public AssembledrewardInv assembledReward { get; set; }
      public int requiredAmount { get; set; }
      public string description { get; set; }
      public bool isHighlighted { get; set; }
      public int amount { get; set; }
   }

   public class AssembledrewardInv
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public bool isHighlighted { get; set; }
      public string[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public string iconAssetName { get; set; }
   }

   public class Possible_Rewards
   {
      public int drop_chance { get; set; }
      public RewardInv1 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class RewardInv1
   {
      public Possible_RewardsInv1[] possible_rewards { get; set; }
      public bool allowReroll { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_RewardsInv1
   {
      public int drop_chance { get; set; }
      public RewardInv2 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class RewardInv2
   {
      public int duration { get; set; }
      public int value { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
   }

   public class Resource_Package
   {
      public CostInv cost { get; set; }
      public int gain { get; set; }
      public string __class__ { get; set; }
   }

   public class CostInv
   {
      public string __class__ { get; set; }
   }

   public class Exchangebasevalue
   {
      public Minvalues minValues { get; set; }
      public Maxvalues maxValues { get; set; }
      public string __class__ { get; set; }
   }

   public class Minvalues
   {
      public float trade_coins { get; set; }
      public float gemstones { get; set; }
   }

   public class Maxvalues
   {
      public float trade_coins { get; set; }
      public float gemstones { get; set; }
   }

}
