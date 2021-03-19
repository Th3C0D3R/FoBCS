using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.GBG.BuildingsGBG
{
   public class GBGBuilding
   {
      public BuildingResponse responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class BuildingResponse
   {
      public Placedbuilding[] placedBuildings { get; set; }
      public Availablebuilding[] availableBuildings { get; set; }
      public int? provinceId { get; set; } = null;
      public string __class__ { get; set; }
   }

   public class Placedbuilding
   {
      public string id { get; set; }
      public int readyAt { get; set; }
      public string __class__ { get; set; }
      public int slotId { get; set; }
   }

   public class Availablebuilding
   {
      public string buildingId { get; set; }
      public Costs costs { get; set; }
      public string __class__ { get; set; }
   }

   public class Costs
   {
      public dynamic resources { get; set; }
      public string __class__ { get; set; }
   }
}
