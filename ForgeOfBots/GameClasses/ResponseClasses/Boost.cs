using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class BoostRoot
   {
      public Boost[] responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Boost
   {
      public string id { get; set; }
      public string name { get; set; }
      public string icon { get; set; }
      public string description { get; set; }
      public Entry[] entries { get; set; }
      public string __class__ { get; set; }
   }

   public class Entry
   {
      public string categoryId { get; set; }
      public string originType { get; set; }
      public string icon { get; set; }
      public string boostType { get; set; }
      public int boostValue { get; set; }
      public string tooltipHeadline { get; set; }
      public string tooltipText { get; set; }
      public int amount { get; set; }
      public string __class__ { get; set; }
      public int usages { get; set; }
      public int mapEntityId { get; set; }
   }


}
