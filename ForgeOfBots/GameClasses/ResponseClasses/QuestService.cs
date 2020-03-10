using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class Quest
   {
      public int id { get; set; }
      public string windowTitle { get; set; }
      public string title { get; set; }
      public string type { get; set; }
      public Questgiver questGiver { get; set; }
      public int priority { get; set; }
      public string state { get; set; }
      public bool autoCompleted { get; set; }
      public Successcondition[] successConditions { get; set; }
      public Successconditiongroup[] successConditionGroups { get; set; }
      public Reward[] rewards { get; set; }
      public Genericreward[] genericRewards { get; set; }
      public string headline { get; set; }
      public string description { get; set; }
      public string accomplishedHeadline { get; set; }
      public string accomplishedDescription { get; set; }
      public bool abortable { get; set; }
      public string category { get; set; }
      public string context { get; set; }
      public string[] flags { get; set; }
      public string __class__ { get; set; }
      public int maxSeasonProgress { get; set; }
   }
   public class Questgiver
   {
      public string id { get; set; }
      public string jobname { get; set; }
      public string name { get; set; }
      public string __class__ { get; set; }
   }
   public class Successcondition
   {
      public int id { get; set; }
      public string iconType { get; set; }
      public string description { get; set; }
      public string hint { get; set; }
      public string hintIcon { get; set; }
      public int maxProgress { get; set; }
      public Payload[] payload { get; set; }
      public string[] flags { get; set; }
      public string __class__ { get; set; }
      public int currentProgress { get; set; }
   }
   public class Payload
   {
      public Cost cost { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }
   public class Cost
   {
      public Resources resources { get; set; }
      public string __class__ { get; set; }
   }
   public class Successconditiongroup
   {
      public int[] conditionIds { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }
   public class Reward
   {
      public string name { get; set; }
      public string iconType { get; set; }
      public bool hidden { get; set; }
      public int quest_id { get; set; }
      public bool random { get; set; }
      public bool isBoosted { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
   }
   public class Genericreward
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string rewardWindowDescription { get; set; }
      public string[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public Assembledreward assembledReward { get; set; }
      public int requiredAmount { get; set; }
   }
   public class Assembledreward
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
   }
}
