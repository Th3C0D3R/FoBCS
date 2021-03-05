using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GBG.StartFight
{
   public class StartFightGBG
   {
      public StartFightResponse[] startfight { get; set; }
   }
   public class StartFightResponse
   {
      public Data responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }
   public class Data
   {
      public Attrition attrition { get; set; }
      public string __class__ { get; set; }
      public int battleId { get; set; }
      public Battletype battleType { get; set; }
      public int attackerPlayerId { get; set; }
      public int startTime { get; set; }
      public Ais ais { get; set; }
      public bool isAutoBattle { get; set; }
      public State state { get; set; }
   }
   public class Attrition
   {
      public int level { get; set; }
      public int negotiationMultiplier { get; set; }
      public int defendingArmyBonus { get; set; }
      public string __class__ { get; set; }
   }
   public class Battletype
   {
      public int provinceId { get; set; }
      public int battlesWon { get; set; }
      public string era { get; set; }
      public string type { get; set; }
      public int totalWaves { get; set; }
      public string __class__ { get; set; }
   }
   public class Ais
   {
      public string _1 { get; set; }
      public string _2 { get; set; }
   }
   public class State
   {
      public dynamic stepHistory { get; set; }
      public int winnerBit { get; set; }
      public Unitsorder[] unitsOrder { get; set; }
      public object[] retiredUnits { get; set; }
      public int activeUnitId { get; set; }
      public int round { get; set; }
      public Ranking_Data ranking_data { get; set; }
      public int _w { get; set; }
      public int _z { get; set; }
      public float distance_scale_factor { get; set; }
      public string __class__ { get; set; }
   }
   public class Ranking_Data
   {
      public int winner { get; set; }
      public int mood_factor { get; set; }
      public Ranking[] ranking { get; set; }
      public bool tournament_running { get; set; }
      public Details details { get; set; }
      public string __class__ { get; set; }
   }
   public class Details
   {
      public int damageDealt { get; set; }
      public int damageRecieved { get; set; }
      public int happinessModifier { get; set; }
      public int sum { get; set; }
      public string __class__ { get; set; }
   }
   public class Ranking
   {
      public string era { get; set; }
      public int place { get; set; }
      public Player player { get; set; }
      public int total_points { get; set; }
      public int fights { get; set; }
      public string __class__ { get; set; }
      public int points { get; set; }
      public Battle_Result_Details battle_result_details { get; set; }
   }
   public class Player
   {
      public bool canSabotage { get; set; }
      public string era { get; set; }
      public bool showAvatarFrame { get; set; }
      public int player_id { get; set; }
      public string name { get; set; }
      public string avatar { get; set; }
      public string __class__ { get; set; }
   }
   public class Battle_Result_Details
   {
      public int damageDealt { get; set; }
      public int damageRecieved { get; set; }
      public int happinessModifier { get; set; }
      public int sum { get; set; }
      public string __class__ { get; set; }
   }
   public class Unitsorder
   {
      public int teamFlag { get; set; }
      public dynamic pos { get; set; }
      public dynamic startpos { get; set; }
      public int startHitpoints { get; set; }
      public int lastRetaliationRound { get; set; }
      public int teamInitiative { get; set; }
      public int unitId { get; set; }
      public int ownerId { get; set; }
      public dynamic abilities { get; set; }
      public dynamic bonuses { get; set; }
      public int entity_id { get; set; }
      public string unitTypeId { get; set; }
      public bool is_defending { get; set; }
      public bool isArenaDefending { get; set; }
      public bool is_attacking { get; set; }
      public bool healing_disabled { get; set; }
      public string __class__ { get; set; }
      public int initialUnitOrderIndex { get; set; }
      public int next_healing_step_size { get; set; }
      public int currentHitpoints { get; set; }
   }

}
