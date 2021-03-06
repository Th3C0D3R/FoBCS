﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GBG.ArmyInfo
{
   public class GetArmyInfoGBG
   {
      public ArmyResponse[] armyresponse { get; set; }
   }
   public class ArmyResponse
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
      public Unit[] units { get; set; }
      public string __class__ { get; set; }
   }
   public class Unit
   {
      public int currentHitpoints { get; set; }
      public dynamic abilities { get; set; }
      public dynamic bonuses { get; set; }
      public int entity_id { get; set; }
      public string unitTypeId { get; set; }
      public bool is_defending { get; set; }
      public bool isArenaDefending { get; set; }
      public bool is_attacking { get; set; }
      public bool healing_disabled { get; set; }
      public string __class__ { get; set; }
   }
}
