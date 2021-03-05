﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GBG.Get
{

   public class GetGBG
   {
      public GBGResponse[] gbgresponse { get; set; }
   }

   public class GBGResponse
   {
      public Data responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Data
   {
      public League league { get; set; }
      public Map map { get; set; }
      public Battlegroundparticipant[] battlegroundParticipants { get; set; }
      public Currentplayerparticipant currentPlayerParticipant { get; set; }
      public int endsAt { get; set; }
      public int currentParticipantId { get; set; }
      public string __class__ { get; set; }
   }

   public class League
   {
      public string id { get; set; }
      public string name { get; set; }
      public int rating { get; set; }
      public string __class__ { get; set; }
   }

   public class Map
   {
      public string id { get; set; }
      public Province[] provinces { get; set; }
      public string __class__ { get; set; }
   }

   public class Province
   {
      public int victoryPoints { get; set; }
      public int ownerId { get; set; }
      public int lockedUntil { get; set; }
      public bool isSpawnSpot { get; set; }
      public Conquestprogress[] conquestProgress { get; set; }
      public int totalBuildingSlots { get; set; }
      public string __class__ { get; set; }
      public int id { get; set; }
   }

   public class Conquestprogress
   {
      public int participantId { get; set; }
      public int progress { get; set; }
      public int maxProgress { get; set; }
      public string __class__ { get; set; }
   }

   public class Currentplayerparticipant
   {
      public Attrition attrition { get; set; }
      public string __class__ { get; set; }
   }

   public class Attrition
   {
      public int level { get; set; }
      public int negotiationMultiplier { get; set; }
      public int defendingArmyBonus { get; set; }
      public string __class__ { get; set; }
   }

   public class Battlegroundparticipant
   {
      public int participantId { get; set; }
      public Clan clan { get; set; }
      public string colour { get; set; }
      public int victoryPoints { get; set; }
      public string __class__ { get; set; }
      public Signal[] signals { get; set; }
   }

   public class Clan
   {
      public int id { get; set; }
      public string name { get; set; }
      public int membersNum { get; set; }
      public string flag { get; set; }
      public string __class__ { get; set; }
   }

   public class Signal
   {
      public string signal { get; set; }
      public string __class__ { get; set; }
      public int provinceId { get; set; }
   }

}
