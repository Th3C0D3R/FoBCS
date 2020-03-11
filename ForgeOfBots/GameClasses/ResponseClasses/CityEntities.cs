namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class BuildingsRoot
   {
      public Building[] buildings { get; set; }
   }

   public class Building
   {
      public string id { get; set; }
      public string asset_id { get; set; }
      public string name { get; set; }
      public string type { get; set; }
      public int width { get; set; }
      public int length { get; set; }
      public RequirementsB requirements { get; set; }
      public int construction_time { get; set; }
      public Resaleresources resaleResources { get; set; }
      public Staticresources staticResources { get; set; }
      public bool is_special { get; set; }
      public int provided_happiness { get; set; }
      public bool is_multi_age { get; set; }
      public Entity_Levels[] entity_levels { get; set; }
      public AbilityB[] abilities { get; set; }
      public string __class__ { get; set; }
      public Available_Products[] available_products { get; set; }
      public float points { get; set; }
      public int usable_slots { get; set; }
      public int demand_for_happiness { get; set; }
      public string description { get; set; }
      public Passive_Bonus passive_bonus { get; set; }
      public Production_Bonus production_bonus { get; set; }
      public int[] strategy_points_for_upgrade { get; set; }
      public string suggested_by { get; set; }
   }

   public class RequirementsB
   {
      public CostB cost { get; set; }
      public string min_era { get; set; }
      public int street_connection_level { get; set; }
      public string __class__ { get; set; }
      public string tech_id { get; set; }
   }

   public class CostB
   {
      public ResourcesB resources { get; set; }
      public string __class__ { get; set; }
   }

   public class ResourcesB
   {
      public int money { get; set; }
      public int supplies { get; set; }
      public int population { get; set; }
      public int premium { get; set; }
      public int japanese { get; set; }
      public int vikings { get; set; }
      public int colonists { get; set; }
      public int alabaster { get; set; }
      public int cypress { get; set; }
      public int dye { get; set; }
      public int sandstone { get; set; }
      public int wine { get; set; }
      public int cloth { get; set; }
      public int ebony { get; set; }
      public int gems { get; set; }
      public int lead { get; set; }
      public int limestone { get; set; }
      public int nanowire { get; set; }
      public int transester_gas { get; set; }
      public int ai_data { get; set; }
      public int paper_batteries { get; set; }
      public int bioplastics { get; set; }
      public int wire { get; set; }
      public int tar { get; set; }
      public int porcelain { get; set; }
      public int coffee { get; set; }
      public int paper { get; set; }
      public int electromagnets { get; set; }
      public int robots { get; set; }
      public int plastics { get; set; }
      public int gas { get; set; }
      public int bionics { get; set; }
      public int bronze { get; set; }
      public int gold { get; set; }
      public int granite { get; set; }
      public int honey { get; set; }
      public int marble { get; set; }
      public int biogeochemical_data { get; set; }
      public int purified_water { get; set; }
      public int algae { get; set; }
      public int superconductors { get; set; }
      public int nanoparticles { get; set; }
      public int brick { get; set; }
      public int glass { get; set; }
      public int ropes { get; set; }
      public int salt { get; set; }
      public int herbs { get; set; }
      public int rubber { get; set; }
      public int coke { get; set; }
      public int textiles { get; set; }
      public int whaleoil { get; set; }
      public int fertilizer { get; set; }
      public int basalt { get; set; }
      public int brass { get; set; }
      public int gunpowder { get; set; }
      public int talc { get; set; }
      public int silk { get; set; }
      public int ferroconcrete { get; set; }
      public int flavorants { get; set; }
      public int luxury_materials { get; set; }
      public int packaging { get; set; }
      public int convenience_food { get; set; }
      public int pearls { get; set; }
      public int artificial_scales { get; set; }
      public int corals { get; set; }
      public int biolight { get; set; }
      public int plankton { get; set; }
      public int renewable_resources { get; set; }
      public int steel { get; set; }
      public int semiconductors { get; set; }
      public int filters { get; set; }
      public int dna_data { get; set; }
      public int asbestos { get; set; }
      public int petroleum { get; set; }
      public int machineparts { get; set; }
      public int tinplate { get; set; }
      public int explosives { get; set; }
      public int lubricants { get; set; }
      public int biotech_crops { get; set; }
      public int mars_microbes { get; set; }
      public int fusion_reactors { get; set; }
      public int superalloys { get; set; }
      public int translucent_concrete { get; set; }
      public int smart_materials { get; set; }
      public int papercrete { get; set; }
      public int preservatives { get; set; }
      public int nutrition_research { get; set; }
      public int cryptocash { get; set; }
      public int golden_rice { get; set; }
      public int tea_silk { get; set; }
      public int data_crystals { get; set; }
      public int nanites { get; set; }
   }

   public class Resaleresources
   {
      public object resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Staticresources
   {
      public object resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Passive_Bonus
   {
      public string type { get; set; }
      public int amount { get; set; }
      public float[] values { get; set; }
      public string __class__ { get; set; }
   }

   public class Production_Bonus
   {
      public string type { get; set; }
      public int amount { get; set; }
      public int[] values { get; set; }
      public string __class__ { get; set; }
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

   public class AbilityB
   {
      public string __class__ { get; set; }
      public string gridId { get; set; }
      public Boosthint[] boostHints { get; set; }
      public string setId { get; set; }
      public BonusB[] bonuses { get; set; }
      public string trigger { get; set; }
      public ActionB action { get; set; }
      public string content { get; set; }
      public string text { get; set; }
      public Bonusgiven bonusGiven { get; set; }
      public string chainId { get; set; }
      public Linkposition[] linkPositions { get; set; }
      public string description { get; set; }
      public Additionalresources additionalResources { get; set; }
      public int amount { get; set; }
      public Rewards rewards { get; set; }
      public string context { get; set; }
   }

   public class ActionB
   {
      public string animationId { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
      public int amount { get; set; }
   }

   public class Bonusgiven
   {
      public object revenue { get; set; }
      public object boost { get; set; }
      public string __class__ { get; set; }
   }

   public class Additionalresources
   {
      public Stoneage StoneAge { get; set; }
      public Bronzeage BronzeAge { get; set; }
      public Ironage IronAge { get; set; }
      public Earlymiddleage EarlyMiddleAge { get; set; }
      public Highmiddleage HighMiddleAge { get; set; }
      public Latemiddleage LateMiddleAge { get; set; }
      public Colonialage ColonialAge { get; set; }
      public Industrialage IndustrialAge { get; set; }
      public Progressiveera ProgressiveEra { get; set; }
      public Modernera ModernEra { get; set; }
      public Postmodernera PostModernEra { get; set; }
      public Contemporaryera ContemporaryEra { get; set; }
      public Tomorrowera TomorrowEra { get; set; }
      public Futureera FutureEra { get; set; }
      public Arcticfuture ArcticFuture { get; set; }
      public Oceanicfuture OceanicFuture { get; set; }
      public Virtualfuture VirtualFuture { get; set; }
      public Spaceagemars SpaceAgeMars { get; set; }
      public Spaceageasteroidbelt SpaceAgeAsteroidBelt { get; set; }
      public Allage AllAge { get; set; }
   }

   public class Stoneage
   {
      public Resources1 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources1
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Bronzeage
   {
      public Resources2B resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources2B
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Ironage
   {
      public Resources3B resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources3B
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Earlymiddleage
   {
      public Resources4 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources4
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Highmiddleage
   {
      public Resources5 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources5
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Latemiddleage
   {
      public Resources6 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources6
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Colonialage
   {
      public Resources7 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources7
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Industrialage
   {
      public Resources8 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources8
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Progressiveera
   {
      public Resources9 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources9
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Modernera
   {
      public Resources10 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources10
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Postmodernera
   {
      public Resources11 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources11
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Contemporaryera
   {
      public Resources12 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources12
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Tomorrowera
   {
      public Resources13 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources13
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Futureera
   {
      public Resources14 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources14
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Arcticfuture
   {
      public Resources15 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources15
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Oceanicfuture
   {
      public Resources16 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources16
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Virtualfuture
   {
      public Resources17 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources17
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Spaceagemars
   {
      public Resources18 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources18
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Spaceageasteroidbelt
   {
      public Resources19 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources19
   {
      public int supplies { get; set; }
      public int clan_power { get; set; }
      public int medals { get; set; }
   }

   public class Allage
   {
      public Resources20 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources20
   {
      public int all_goods_of_age { get; set; }
      public int strategy_points { get; set; }
      public int random_good_of_age { get; set; }
      public int random_good_of_age_1 { get; set; }
      public int random_good_of_age_2 { get; set; }
      public int random_good_of_age_3 { get; set; }
   }

   public class Rewards
   {
      public Bronzeage1 BronzeAge { get; set; }
      public Ironage1 IronAge { get; set; }
      public Earlymiddleage1 EarlyMiddleAge { get; set; }
      public Highmiddleage1 HighMiddleAge { get; set; }
      public Latemiddleage1 LateMiddleAge { get; set; }
      public Colonialage1 ColonialAge { get; set; }
      public Industrialage1 IndustrialAge { get; set; }
      public Progressiveera1 ProgressiveEra { get; set; }
      public Modernera1 ModernEra { get; set; }
      public Postmodernera1 PostModernEra { get; set; }
      public Contemporaryera1 ContemporaryEra { get; set; }
      public Tomorrowera1 TomorrowEra { get; set; }
      public Futureera1 FutureEra { get; set; }
      public Arcticfuture1 ArcticFuture { get; set; }
      public Oceanicfuture1 OceanicFuture { get; set; }
      public Virtualfuture1 VirtualFuture { get; set; }
      public Spaceagemars1 SpaceAgeMars { get; set; }
   }

   public class Bronzeage1
   {
      public Possible_RewardsB[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_RewardsB
   {
      public int drop_chance { get; set; }
      public RewardB reward { get; set; }
      public string __class__ { get; set; }
   }

   public class RewardB
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards1[] possible_rewards { get; set; }
   }

   public class Possible_Rewards1
   {
      public int drop_chance { get; set; }
      public Reward1 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward1
   {
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

   public class Ironage1
   {
      public Possible_Rewards2[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards2
   {
      public int drop_chance { get; set; }
      public Reward2 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward2
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards3[] possible_rewards { get; set; }
   }

   public class Possible_Rewards3
   {
      public int drop_chance { get; set; }
      public Reward3 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward3
   {
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

   public class Earlymiddleage1
   {
      public Possible_Rewards4[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards4
   {
      public int drop_chance { get; set; }
      public Reward4 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward4
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards5[] possible_rewards { get; set; }
   }

   public class Possible_Rewards5
   {
      public int drop_chance { get; set; }
      public Reward5 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward5
   {
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

   public class Highmiddleage1
   {
      public Possible_Rewards6[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards6
   {
      public int drop_chance { get; set; }
      public Reward6 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward6
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards7[] possible_rewards { get; set; }
   }

   public class Possible_Rewards7
   {
      public int drop_chance { get; set; }
      public Reward7 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward7
   {
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

   public class Latemiddleage1
   {
      public Possible_Rewards8[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards8
   {
      public int drop_chance { get; set; }
      public Reward8 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward8
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards9[] possible_rewards { get; set; }
   }

   public class Possible_Rewards9
   {
      public int drop_chance { get; set; }
      public Reward9 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward9
   {
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

   public class Colonialage1
   {
      public Possible_Rewards10[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards10
   {
      public int drop_chance { get; set; }
      public Reward10 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward10
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards11[] possible_rewards { get; set; }
   }

   public class Possible_Rewards11
   {
      public int drop_chance { get; set; }
      public Reward11 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward11
   {
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

   public class Industrialage1
   {
      public Possible_Rewards12[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards12
   {
      public int drop_chance { get; set; }
      public Reward12 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward12
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards13[] possible_rewards { get; set; }
   }

   public class Possible_Rewards13
   {
      public int drop_chance { get; set; }
      public Reward13 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward13
   {
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

   public class Progressiveera1
   {
      public Possible_Rewards14[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards14
   {
      public int drop_chance { get; set; }
      public Reward14 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward14
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards15[] possible_rewards { get; set; }
   }

   public class Possible_Rewards15
   {
      public int drop_chance { get; set; }
      public Reward15 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward15
   {
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

   public class Modernera1
   {
      public Possible_Rewards16[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards16
   {
      public int drop_chance { get; set; }
      public Reward16 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward16
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards17[] possible_rewards { get; set; }
   }

   public class Possible_Rewards17
   {
      public int drop_chance { get; set; }
      public Reward17 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward17
   {
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

   public class Postmodernera1
   {
      public Possible_Rewards18[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards18
   {
      public int drop_chance { get; set; }
      public Reward18 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward18
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards19[] possible_rewards { get; set; }
   }

   public class Possible_Rewards19
   {
      public int drop_chance { get; set; }
      public Reward19 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward19
   {
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

   public class Contemporaryera1
   {
      public Possible_Rewards20[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards20
   {
      public int drop_chance { get; set; }
      public Reward20 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward20
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards21[] possible_rewards { get; set; }
   }

   public class Possible_Rewards21
   {
      public int drop_chance { get; set; }
      public Reward21 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward21
   {
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

   public class Tomorrowera1
   {
      public Possible_Rewards22[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards22
   {
      public int drop_chance { get; set; }
      public Reward22 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward22
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards23[] possible_rewards { get; set; }
   }

   public class Possible_Rewards23
   {
      public int drop_chance { get; set; }
      public Reward23 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward23
   {
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

   public class Futureera1
   {
      public Possible_Rewards24[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards24
   {
      public int drop_chance { get; set; }
      public Reward24 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward24
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards25[] possible_rewards { get; set; }
   }

   public class Possible_Rewards25
   {
      public int drop_chance { get; set; }
      public Reward25 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward25
   {
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

   public class Arcticfuture1
   {
      public Possible_Rewards26[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards26
   {
      public int drop_chance { get; set; }
      public Reward26 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward26
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards27[] possible_rewards { get; set; }
   }

   public class Possible_Rewards27
   {
      public int drop_chance { get; set; }
      public Reward27 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward27
   {
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

   public class Oceanicfuture1
   {
      public Possible_Rewards28[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards28
   {
      public int drop_chance { get; set; }
      public Reward28 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward28
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards29[] possible_rewards { get; set; }
   }

   public class Possible_Rewards29
   {
      public int drop_chance { get; set; }
      public Reward29 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward29
   {
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

   public class Virtualfuture1
   {
      public Possible_Rewards30[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards30
   {
      public int drop_chance { get; set; }
      public Reward30 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward30
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards31[] possible_rewards { get; set; }
   }

   public class Possible_Rewards31
   {
      public int drop_chance { get; set; }
      public Reward31 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward31
   {
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

   public class Spaceagemars1
   {
      public Possible_Rewards32[] possible_rewards { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string iconAssetName { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public string __class__ { get; set; }
   }

   public class Possible_Rewards32
   {
      public int drop_chance { get; set; }
      public Reward32 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward32
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Possible_Rewards33[] possible_rewards { get; set; }
   }

   public class Possible_Rewards33
   {
      public int drop_chance { get; set; }
      public Reward33 reward { get; set; }
      public string __class__ { get; set; }
   }

   public class Reward33
   {
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

   public class Boosthint
   {
      public Boosthinteramap boostHintEraMap { get; set; }
      public string __class__ { get; set; }
   }

   public class Boosthinteramap
   {
      public Stoneage1 StoneAge { get; set; }
      public Bronzeage2 BronzeAge { get; set; }
      public Ironage2 IronAge { get; set; }
      public Earlymiddleage2 EarlyMiddleAge { get; set; }
      public Highmiddleage2 HighMiddleAge { get; set; }
      public Latemiddleage2 LateMiddleAge { get; set; }
      public Colonialage2 ColonialAge { get; set; }
      public Industrialage2 IndustrialAge { get; set; }
      public Progressiveera2 ProgressiveEra { get; set; }
      public Modernera2 ModernEra { get; set; }
      public Postmodernera2 PostModernEra { get; set; }
      public Contemporaryera2 ContemporaryEra { get; set; }
      public Tomorrowera2 TomorrowEra { get; set; }
      public Futureera2 FutureEra { get; set; }
      public Arcticfuture2 ArcticFuture { get; set; }
      public Oceanicfuture2 OceanicFuture { get; set; }
      public Virtualfuture2 VirtualFuture { get; set; }
      public Spaceagemars2 SpaceAgeMars { get; set; }
      public Spaceageasteroidbelt1 SpaceAgeAsteroidBelt { get; set; }
      public Allage1 AllAge { get; set; }
   }

   public class Stoneage1
   {
      public string type { get; set; }
      public string __class__ { get; set; }
   }

   public class Bronzeage2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Ironage2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Earlymiddleage2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Highmiddleage2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Latemiddleage2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Colonialage2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Industrialage2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Progressiveera2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Modernera2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Postmodernera2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Contemporaryera2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Tomorrowera2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Futureera2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Arcticfuture2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Oceanicfuture2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Virtualfuture2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Spaceagemars2
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Spaceageasteroidbelt1
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class Allage1
   {
      public string type { get; set; }
      public int value { get; set; }
      public string __class__ { get; set; }
   }

   public class BonusB
   {
      public int level { get; set; }
      public object revenue { get; set; }
      public object boost { get; set; }
      public string __class__ { get; set; }
   }

   public class Linkposition
   {
      public Topleftpoint topLeftPoint { get; set; }
      public Bottomrightpoint bottomRightPoint { get; set; }
      public string __class__ { get; set; }
   }

   public class Topleftpoint
   {
      public int y { get; set; }
      public string __class__ { get; set; }
   }

   public class Bottomrightpoint
   {
      public int x { get; set; }
      public int y { get; set; }
      public string __class__ { get; set; }
   }

   public class Available_Products
   {
      public Resources21 resources { get; set; }
      public Requirements1B requirements { get; set; }
      public string name { get; set; }
      public int production_time { get; set; }
      public string asset_name { get; set; }
      public int production_option { get; set; }
      public string __class__ { get; set; }
      public int deposit_boost_factor { get; set; }
      public string deposit_id { get; set; }
      public ProductB product { get; set; }
      public string unit_type_id { get; set; }
      public string unit_class { get; set; }
      public int amount { get; set; }
      public int time_to_heal { get; set; }
      public int time_to_train { get; set; }
   }

   public class Resources21
   {
      public Resources22 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources22
   {
      public int soy { get; set; }
      public int paintings { get; set; }
      public int armor { get; set; }
      public int instruments { get; set; }
      public int axes { get; set; }
      public int mead { get; set; }
      public int horns { get; set; }
      public int wool { get; set; }
   }

   public class Requirements1B
   {
      public Cost1B cost { get; set; }
      public string __class__ { get; set; }
   }

   public class Cost1B
   {
      public object resources { get; set; }
      public string __class__ { get; set; }
   }

   public class ProductB
   {
      public object resources { get; set; }
      public string __class__ { get; set; }
   }

}
