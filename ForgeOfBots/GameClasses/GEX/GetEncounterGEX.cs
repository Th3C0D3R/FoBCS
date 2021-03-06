﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GEX.GetEncounter
{
   public class GetEncounterGEX
   {
      public GetEncounterResponse[] getencounterresponse { get; set; }
   }
   public class GetEncounterResponse
   {
      public Data responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class Data
   {
      public Armywave[] armyWaves { get; set; }
      public string __class__ { get; set; }
   }
   public class Armywave
   {
      public int id { get; set; }
      public Unit[] units { get; set; }
      public string __class__ { get; set; }
   }
   public class Unit
   {
      public int currentHitpoints { get; set; }
      public Ability[] abilities { get; set; }
      public Bonus[] bonuses { get; set; }
      public int entity_id { get; set; }
      public string unitTypeId { get; set; }
      public bool is_defending { get; set; }
      public bool isArenaDefending { get; set; }
      public bool is_attacking { get; set; }
      public bool healing_disabled { get; set; }
      public string __class__ { get; set; }
   }
   public class Ability
   {
      public int used_at_step { get; set; }
      public object value { get; set; }
      public string type { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string icon { get; set; }
      public string __class__ { get; set; }
      public string[] terrains { get; set; }
   }
   public class Bonus
   {
      public int value { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }
}
