namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class StartupRoot
   {
      public Startup responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class Startup
   {
      public Socket_Connection_Parameter socket_connection_parameter { get; set; }
      public City_Map city_map { get; set; }
      public Resources3 resources { get; set; }
      public User_Data user_data { get; set; }
      public Premium_Packages[] premium_packages { get; set; }
      public Settings settings { get; set; }
      public Goodslist[] goodsList { get; set; }
      public Running_Battle running_battle { get; set; }
      public Unit_Slots[] unit_slots { get; set; }
      public Premium_Prices[] premium_prices { get; set; }
      public Feature_Flags feature_flags { get; set; }
      public Unlocked_Features[] unlocked_features { get; set; }
      public Active_Ab_Tests[] active_ab_tests { get; set; }
      public Buildingrelations buildingRelations { get; set; }
      public Questeffect[] questEffects { get; set; }
      public string __class__ { get; set; }
   }
   public class Socket_Connection_Parameter
   {
      public string socketServerHost { get; set; }
      public int socketServerPort { get; set; }
      public string __class__ { get; set; }
   }
   public class City_Map
   {
      public string gridId { get; set; }
      public Unlocked_Areas[] unlocked_areas { get; set; }
      public Entity[] entities { get; set; }
      public Blocked_Areas[] blocked_areas { get; set; }
      public Tileset[] tilesets { get; set; }
      public string __class__ { get; set; }
   }
   public class Unlocked_Areas
   {
      public int x { get; set; }
      public int y { get; set; }
      public int width { get; set; }
      public int length { get; set; }
      public string __class__ { get; set; }
   }
   public class Entity
   {
      public int id { get; set; }
      public int player_id { get; set; }
      public string cityentity_id { get; set; }
      public string type { get; set; }
      public int x { get; set; }
      public int y { get; set; }
      public int connected { get; set; }
      public State state { get; set; }
      public int level { get; set; }
      public string __class__ { get; set; }
      public Unitslot[] unitSlots { get; set; }
      public int max_level { get; set; }
      public Bonus bonus { get; set; }
   }
   public class State
   {
      public Current_Product current_product { get; set; }
      public bool boosted { get; set; }
      public bool is_motivated { get; set; }
      public int next_state_transition_in { get; set; }
      public int next_state_transition_at { get; set; }
      public string __class__ { get; set; }
      public Paused_State paused_state { get; set; }
      public int forge_points_for_level_up { get; set; }
      public int invested_forge_points { get; set; }
   }
   public class Current_Product
   {
      public Product product { get; set; }
      public string name { get; set; }
      public int production_time { get; set; }
      public string asset_name { get; set; }
      public string __class__ { get; set; }
      public Guildproduct guildProduct { get; set; }
      public int deposit_boost_factor { get; set; }
      public Requirements requirements { get; set; }
      public int production_option { get; set; }
      public Good[] goods { get; set; }
      public int slot { get; set; }
      public int amount { get; set; }
   }
   public class Product
   {
      public Resources resources { get; set; }
      public string __class__ { get; set; }
   }
   public class Guildproduct
   {
      public Resources resources { get; set; }
      public string __class__ { get; set; }
   }
   public class Requirements
   {
      public Cost cost { get; set; }
      public string __class__ { get; set; }
   }
   public class Good
   {
      public string good_id { get; set; }
      public int value { get; set; }
      public string name { get; set; } = "";
      public string __class__ { get; set; }
   }
   public class Paused_State
   {
      public int next_state_transition_at { get; set; }
      public string __class__ { get; set; }
   }
   public class Bonus
   {
      public float amount { get; set; }
      public float value { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }
   public class Unitslot
   {
      public int entity_id { get; set; }
      public int unit_id { get; set; }
      public Unit unit { get; set; }
      public Unlock_Costs unlock_costs { get; set; }
      public Unlockcosts unlockCosts { get; set; }
      public bool unlocked { get; set; }
      public bool is_unlockable { get; set; }
      public bool is_training { get; set; }
      public string __class__ { get; set; }
      public int nr { get; set; }
   }
   public class Unit
   {
      public int unitId { get; set; }
      public int ownerId { get; set; }
      public int currentHitpoints { get; set; }
      public Ability[] abilities { get; set; }
      public object[] bonuses { get; set; }
      public int entity_id { get; set; }
      public string unitTypeId { get; set; }
      public int next_healing_step_size { get; set; }
      public bool is_defending { get; set; }
      public bool is_attacking { get; set; }
      public bool healing_disabled { get; set; }
      public string __class__ { get; set; }
      public int slot_id { get; set; }
   }
   public class Ability
   {
      public int used_at_step { get; set; }
      public string value { get; set; }
      public string type { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string icon { get; set; }
      public string __class__ { get; set; }
   }
   public class Unlock_Costs
   {
      public object[] goods { get; set; }
      public string __class__ { get; set; }
      public int premium { get; set; }
   }
   public class Unlockcosts
   {
      public object resources { get; set; }
      public string __class__ { get; set; }
   }
   public class Blocked_Areas
   {
      public string __class__ { get; set; }
      public int x { get; set; }
      public int y { get; set; }
   }
   public class Tileset
   {
      public bool available { get; set; }
      public string id { get; set; }
      public string asset_id { get; set; }
      public string type { get; set; }
      public int width { get; set; }
      public int length { get; set; }
      public Requirements1 requirements { get; set; }
      public bool is_special { get; set; }
      public object[] entity_levels { get; set; }
      public Ability1[] abilities { get; set; }
      public string __class__ { get; set; }
   }
   public class Requirements1
   {
      public Cost1 cost { get; set; }
      public string min_era { get; set; }
      public string __class__ { get; set; }
   }
   public class Cost1
   {
      public Resources2 resources { get; set; }
      public string __class__ { get; set; }
   }
   public class Resources2
   {
      public int medals { get; set; }
      public int premium { get; set; }
      public int money { get; set; }
   }
   public class Ability1
   {
      public string gridId { get; set; }
      public string __class__ { get; set; }
   }
   public class Resources3
   {
      public string __class__ { get; set; }
   }
   public class User_Data
   {
      public int player_id { get; set; }
      public string city_name { get; set; }
      public string user_name { get; set; }
      public Era era { get; set; }
      public bool is_tester { get; set; }
      public string profile_text { get; set; }
      public string portrait_id { get; set; }
      public string[] unlocked_avatars { get; set; }
      public int clan_id { get; set; }
      public string clan_name { get; set; }
      public bool email_validated { get; set; }
      public bool time_left_to_validate_email { get; set; }
      public bool has_new_event { get; set; }
      public bool has_new_neighbors { get; set; }
      public int clan_permissions { get; set; }
      public Unread_Forum_Ids unread_forum_ids { get; set; }
      public bool is_guest { get; set; }
      public bool hasPiiAccess { get; set; }
      public bool canSetEmail { get; set; }
      public bool isRealEmail { get; set; }
      public string __class__ { get; set; }
   }
   public class Era
   {
      public string era { get; set; }
      public string mainBuildingUrl { get; set; }
      public string __class__ { get; set; }
   }
   public class Unread_Forum_Ids
   {
      public object[] ids { get; set; }
      public string __class__ { get; set; }
   }
   public class Running_Battle
   {
      public string __class__ { get; set; }
   }
   public class Feature_Flags
   {
      public Feature[] features { get; set; }
      public string __class__ { get; set; }
   }
   public class Feature
   {
      public string feature { get; set; }
      public int status { get; set; }
      public string group { get; set; }
      public string __class__ { get; set; }
      public int time_remaining { get; set; }
      public int expiresAt { get; set; }
      public string time_string { get; set; }
   }
   public class Buildingrelations
   {
      public Relation[] relations { get; set; }
      public string __class__ { get; set; }
   }
   public class Relation
   {
      public string main { get; set; }
      public string[] parts { get; set; }
      public string __class__ { get; set; }
   }
   public class Premium_Packages
   {
      public string id { get; set; }
      public string type { get; set; }
      public Cost2 cost { get; set; }
      public int gain { get; set; }
      public bool is_refill { get; set; }
      public string __class__ { get; set; }
   }
   public class Cost2
   {
      public int money { get; set; }
      public string __class__ { get; set; }
   }
   public class Goodslist
   {
      public Allow_Boost_List[] allow_boost_list { get; set; }
      public object[] allow_production_list { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public int premium_price { get; set; }
      public string era { get; set; }
      public string merchant_name { get; set; }
      public string booster_deposit { get; set; }
      public string __class__ { get; set; }
      public string required_production_good { get; set; }
   }
   public class Allow_Boost_List
   {
      public string good_id { get; set; }
      public string building_id { get; set; }
      public string building_name { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }
   public class Unit_Slots
   {
      public int entity_id { get; set; }
      public int unit_id { get; set; }
      public Unit1 unit { get; set; }
      public Unlock_Costs1 unlock_costs { get; set; }
      public Unlockcosts1 unlockCosts { get; set; }
      public bool unlocked { get; set; }
      public bool is_unlockable { get; set; }
      public bool is_training { get; set; }
      public string __class__ { get; set; }
      public int nr { get; set; }
   }
   public class Unit1
   {
      public int unitId { get; set; }
      public int ownerId { get; set; }
      public int currentHitpoints { get; set; }
      public Ability2[] abilities { get; set; }
      public object[] bonuses { get; set; }
      public int entity_id { get; set; }
      public string unitTypeId { get; set; }
      public int next_healing_step_size { get; set; }
      public bool is_defending { get; set; }
      public bool is_attacking { get; set; }
      public bool healing_disabled { get; set; }
      public string __class__ { get; set; }
      public int slot_id { get; set; }
   }
   public class Ability2
   {
      public int used_at_step { get; set; }
      public string value { get; set; }
      public string type { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string icon { get; set; }
      public string __class__ { get; set; }
   }
   public class Unlock_Costs1
   {
      public object[] goods { get; set; }
      public string __class__ { get; set; }
      public int premium { get; set; }
   }
   public class Unlockcosts1
   {
      public object resources { get; set; }
      public string __class__ { get; set; }
   }
   public class Premium_Prices
   {
      public string feature_id { get; set; }
      public Premium_PricesEra premium_prices { get; set; }
      public string __class__ { get; set; }
   }
   public class Premium_PricesEra
   {
      public int StoneAge { get; set; }
      public int BronzeAge { get; set; }
      public int IronAge { get; set; }
      public int EarlyMiddleAge { get; set; }
      public int HighMiddleAge { get; set; }
      public int LateMiddleAge { get; set; }
      public int ColonialAge { get; set; }
      public int IndustrialAge { get; set; }
      public int ProgressiveEra { get; set; }
      public int ModernEra { get; set; }
      public int PostModernEra { get; set; }
   }
   public class Unlocked_Features
   {
      public string feature { get; set; }
      public string __class__ { get; set; }
   }
   public class Active_Ab_Tests
   {
      public string test_name { get; set; }
      public string group { get; set; }
      public string __class__ { get; set; }
   }
   public class Questeffect
   {
      public string effect { get; set; }
      public int duration { get; set; }
      public int[] quests { get; set; }
      public string __class__ { get; set; }
   }

}
