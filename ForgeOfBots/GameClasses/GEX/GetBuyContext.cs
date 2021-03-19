using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GEX.GetBuyContext
{
   public class GetBuyContext
   {
      public Data[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Data
   {
      public string context { get; set; }
      public string title { get; set; }
      public Offer[] offers { get; set; }
      public string __class__ { get; set; }
   }

   public class Offer
   {
      public string id { get; set; }
      public string title { get; set; }
      public string image { get; set; }
      public Gains gains { get; set; }
      public Costs costs { get; set; }
      public bool capped { get; set; }
      public Formula formula { get; set; }
      public string __class__ { get; set; }
   }

   public class Gains
   {
      public Resources resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources
   {
      public int guild_expedition_attempt { get; set; }
   }

   public class Costs
   {
      public Resources1 resources { get; set; }
      public string __class__ { get; set; }
   }

   public class Resources1
   {
      public int medals { get; set; }
      public int premium { get; set; }
   }

   public class Formula
   {
      public string type { get; set; }
      public float factor { get; set; }
      public int baseValue { get; set; }
      public int boughtCount { get; set; }
      public string __class__ { get; set; }
   }

}
