using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class Good
   {
      public string good_id { get; set; }
      public int value { get; set; }
      public string name { get; set; } = "";
      public string __class__ { get; set; }
   }

   public class ResourceDefinition
   {
      public string id { get; set; }
      public string name { get; set; }
      public string nameSingular { get; set; }
      public string namePlural { get; set; }
      public string era { get; set; }
      public object abilities { get; set; }
      public string __class__ { get; set; }
   }
   public class Resource
   {
      public Resources resources { get; set; }
      public string __class__ { get; set; }
   }
   public class Resources
   {
      public int translucent_concrete { get; set; }
      public int rubber { get; set; }
      public int robots { get; set; }
      public int electromagnets { get; set; }
      public int plastics { get; set; }
      public int negotiation_game_turn { get; set; }
      public int strategy_points { get; set; }
      public int supplies { get; set; }
      public int tar { get; set; }
      public int machineparts { get; set; }
      public int coke { get; set; }
      public int nutrition_research { get; set; }
      public int st_patricks_pot_of_gold { get; set; }
      public int diplomacy { get; set; }
      public int expansions { get; set; }
      public int asbestos { get; set; }
      public int armor { get; set; }
      public int explosives { get; set; }
      public int japanese { get; set; }
      public int nanoparticles { get; set; }
      public int whaleoil { get; set; }
      public int flavorants { get; set; }
      public int money { get; set; }
      public int semiconductors { get; set; }
      public int transester_gas { get; set; }
      public int bioplastics { get; set; }
      public int tavern_silver { get; set; }
      public int sandstone { get; set; }
      public int ferroconcrete { get; set; }
      public int luxury_materials { get; set; }
      public int gold { get; set; }
      public int wine { get; set; }
      public int forge_bowl_footballs { get; set; }
      public int trade_coins { get; set; }
      public int total_population { get; set; }
      public int premium { get; set; }
      public int biogeochemical_data { get; set; }
      public int koban_coins { get; set; }
      public int lubricants { get; set; }
      public int biotech_crops { get; set; }
      public int coffee { get; set; }
      public int paper { get; set; }
      public int dna_data { get; set; }
      public int population { get; set; }
      public int superalloys { get; set; }
      public int fusion_reactors { get; set; }
      public int petroleum { get; set; }
      public int tinplate { get; set; }
      public int stars { get; set; }
      public int total_japanese { get; set; }
      public int medals { get; set; }
      public int guild_expedition_attempt { get; set; }
      public int fertilizer { get; set; }
      public int winter_tickets { get; set; }
      public int convenience_food { get; set; }
      public int steel { get; set; }
      public int brick { get; set; }
      public int filters { get; set; }
      public int bionics { get; set; }
      public int silk { get; set; }
      public int purified_water { get; set; }
      public int mars_microbes { get; set; }
      public int paintings { get; set; }
      public int nanites { get; set; }
      public int textiles { get; set; }
      public int glass { get; set; }
      public int winter_reindeer { get; set; }
      public int halloween_candy { get; set; }
      public int halloween_cauldron { get; set; }
      public int halloween_broomstick { get; set; }
      public int halloween_wand { get; set; }
      public int smart_materials { get; set; }
      public int golden_rice { get; set; }
      public int data_crystals { get; set; }
      public int fall_ingredient_caramel { get; set; }
      public int fall_ingredient_cinnamon { get; set; }
      public int fall_ingredient_chocolate { get; set; }
      public int fall_ingredient_apples { get; set; }
      public int porcelain { get; set; }
      public int gemstones { get; set; }
      public int pearls { get; set; }
      public int biolight { get; set; }
      public int corals { get; set; }
      public int summer_doubloons { get; set; }
      public int summer_compass { get; set; }
      public int instruments { get; set; }
      public int soy { get; set; }
      public int soccer_energy { get; set; }
      public int artificial_scales { get; set; }
      public int cryptocash { get; set; }
      public int vikings { get; set; }
      public int copper_coins { get; set; }
      public int total_vikings { get; set; }
      public int renewable_resources { get; set; }
      public int packaging { get; set; }
      public int gas { get; set; }
      public int papercrete { get; set; }
      public int preservatives { get; set; }
      public int algae { get; set; }
      public int superconductors { get; set; }
      public int paper_batteries { get; set; }
      public int ai_data { get; set; }
      public int nanowire { get; set; }
      public int plankton { get; set; }
      public int tea_silk { get; set; }
      public int archeology_scroll { get; set; }
      public int archeology_dynamite { get; set; }
      public int archeology_shovel { get; set; }
      public int archeology_brush { get; set; }
      public int wire { get; set; }
      public int spring_lanterns { get; set; }
      public int carnival_coins { get; set; }
      public int ropes { get; set; }
      public int lead { get; set; }
      public int carnival_tickets { get; set; }
      public int talc { get; set; }
      public int brass { get; set; }
      public int basalt { get; set; }
      public int gunpowder { get; set; }
      public int cloth { get; set; }
      public int salt { get; set; }
      public int herbs { get; set; }
      public int wool { get; set; }
      public int horns { get; set; }
      public int mead { get; set; }
      public int honey { get; set; }
      public int axes { get; set; }
      public int granite { get; set; }
      public int bronze { get; set; }
      public int marble { get; set; }
      public int winter_matches { get; set; }
      public int ebony { get; set; }
      public int limestone { get; set; }
      public int gems { get; set; }
      public int dye { get; set; }
      public int alabaster { get; set; }
      public int cypress { get; set; }
      public int fall_ingredient_pumpkins { get; set; }
      public int summer_tickets { get; set; }
      public int carnival_hearts { get; set; }
      public int carnival_roses { get; set; }
   }


}
