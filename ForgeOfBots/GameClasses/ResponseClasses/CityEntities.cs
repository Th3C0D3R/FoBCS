namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class BuildingsRoot
   {
      public Allbuilding[] allbuildings { get; set; }
   }

   public class Allbuilding
   {
      public string id { get; set; }
      public string asset_id { get; set; }
      public string name { get; set; }
      public string type { get; set; }
      public int width { get; set; }
      public int length { get; set; }
      public int construction_time { get; set; }
      public bool is_special { get; set; }
      public int provided_happiness { get; set; }
      public bool is_multi_age { get; set; }
      public Entity_Levels[] entity_levels { get; set; }
      public Ability[] abilities { get; set; }
      public string __class__ { get; set; }
      public Available_Products[] available_products { get; set; }
      public float points { get; set; }
      public int usable_slots { get; set; }
      public int demand_for_happiness { get; set; }
      public string description { get; set; }
      public int[] strategy_points_for_upgrade { get; set; }
      public string suggested_by { get; set; }
   }
   public class Entity_Levels
   {
      public string era { get; set; }
      public string __class__ { get; set; }
      public int provided_happiness { get; set; }
      public int points { get; set; }
      public int ranking_points { get; set; }
      public string unit_id { get; set; }
      public string unit_name { get; set; }
      public string unit_class { get; set; }
      public string unit_asset_name { get; set; }
      public Production_Values[] production_values { get; set; }
      public int required_population { get; set; }
      public int provided_population { get; set; }
      public int demand_for_happiness { get; set; }
      public int produced_money { get; set; }
      public int produced_blueprints_when_motivated { get; set; }
      public int produced_goods_when_motivated { get; set; }
      public int clan_power { get; set; }
   }
   public class Production_Values
   {
      public string type { get; set; }
      public string __class__ { get; set; }
      public int value { get; set; }
   }
   public class Available_Products
   {
      public Resources resources { get; set; }
      public Requirements requirements { get; set; }
      public string name { get; set; }
      public int production_time { get; set; }
      public string asset_name { get; set; }
      public int production_option { get; set; }
      public string __class__ { get; set; }
      public int deposit_boost_factor { get; set; }
      public string deposit_id { get; set; }
      public Product product { get; set; }
      public string unit_type_id { get; set; }
      public string unit_class { get; set; }
      public int amount { get; set; }
      public int time_to_heal { get; set; }
      public int time_to_train { get; set; }
   }
}
