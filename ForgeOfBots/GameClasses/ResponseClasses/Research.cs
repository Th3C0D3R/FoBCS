namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class ResearchEraRoot
   {
      public ResearchEra[] reserach { get; set; }
   }

   public class ResearchEra
   {
      public string era { get; set; }
      public string name { get; set; }
      public string overviewImage { get; set; }
      public string overviewBackgroundColor { get; set; }
      public string fontColor { get; set; }
      public Highlight[] highlights { get; set; }
      public Erareachedreward[] eraReachedRewards { get; set; }
      public Instantunlock[] instantUnlocks { get; set; }
      public Upcomingunlock[] upcomingUnlocks { get; set; }
      public Instantrule[] instantRules { get; set; }
      public Upcomingrule[] upcomingRules { get; set; }
      public string __class__ { get; set; }
   }

   public class Highlight
   {
      public string[] descriptions { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }

   public class Erareachedreward
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
   }

   public class Instantunlock
   {
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
   }

   public class Upcomingunlock
   {
      public EraUnit unit { get; set; }
      public string id { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public string[] flags { get; set; }
      public string type { get; set; }
      public string subType { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
   }

   public class EraUnit
   {
      public EraAbility[] abilities { get; set; }
      public object[] bonuses { get; set; }
      public string unitTypeId { get; set; }
      public string __class__ { get; set; }
   }

   public class EraAbility
   {
      public object value { get; set; }
      public string type { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string icon { get; set; }
      public string __class__ { get; set; }
      public string[] terrains { get; set; }
   }

   public class Instantrule
   {
      public string[] descriptions { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }

   public class Upcomingrule
   {
      public string[] descriptions { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public string iconAssetName { get; set; }
      public bool isHighlighted { get; set; }
      public object[] flags { get; set; }
      public string type { get; set; }
      public string __class__ { get; set; }
   }

}
