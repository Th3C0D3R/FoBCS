using ArmyData = ForgeOfBots.GameClasses.GBG.ArmyInfo.Data;
using Battleground = ForgeOfBots.GameClasses.GBG.Get.Data;
using StartFight = ForgeOfBots.GameClasses.GBG.StartFight.Data;
using State = ForgeOfBots.GameClasses.GBG.State.Data;
using ForgeOfBots.Utils;
using Newtonsoft.Json;
using System;
using ForgeOfBots.GameClasses.GBG.ArmyInfo;
using ForgeOfBots.GameClasses.GBG.Get;
using ForgeOfBots.GameClasses.GBG.State;
using ForgeOfBots.GameClasses.GBG.StartFight;
using ForgeOfBots.GameClasses.GBG.BuildingsGBG;
using ForgeOfBots.GameClasses;
using System.Linq;
using System.Collections.Generic;

namespace ForgeOfBots.DataHandler
{
   public static class GBGHelper
   {
      private static readonly RequestBuilder ReqBuilder = StaticData.ReqBuilder;

      public static Battleground CurrentBattleground { get; private set; } = null;
      public static string CurrentState { get; private set; } = "";
      public static bool IsParticipating
      {
         get
         {
            if (CurrentState == "participating") return true;
            return false;
         }
         private set { }
      }
      public static void UpdateGBG()
      {
         CurrentBattleground = GetBattleground();
         CurrentState = GetState();
      }
      public static ArmyData[] GetArmyInfo(int provinceID)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGgetArmyInfo, provinceID);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            ArmyResponse response = JsonConvert.DeserializeObject<ArmyResponse>(ret);
            return response.responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static Battleground GetBattleground()
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGgetBattleground, "");
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GBGResponse response = JsonConvert.DeserializeObject<GBGResponse>(ret);
            return response.responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }

      public static string GetState()
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGgetState, "");
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            dynamic response = JsonConvert.DeserializeObject(ret);
            return response["responseData"]["stateId"].ToString();
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static StartFight StartFight(int provinceID)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGstartAttack, provinceID);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            StartFightResponse startFightGBGresponse = JsonConvert.DeserializeObject<StartFightResponse>(ret);
            return startFightGBGresponse.responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static BuildingResponse GetBuilding(int provinceID)
      {
         try
         {
            if (ListClass.ProvincesGBG.Find(p => p.id == provinceID).totalBuildingSlots == 0) return null;
            string script = ReqBuilder.GetRequestScript(RequestType.getBuildings, provinceID);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GBGBuilding GBGBuildingresponse = JsonConvert.DeserializeObject<GBGBuilding>(ret);
            return GBGBuildingresponse.responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static bool IsBuildingFinished(Placedbuilding response)
      {
         return DateTime.Now >= Helper.UnixTimeStampToDateTime(response.readyAt);
      }
      public static Province GetSiegeCount(Province item)
      {
            int siegeCount = 0;
            if (item.connections != null)
            {
               foreach (var conID in item.connections)
               {
                  var buildingsForProvince = GetBuilding(conID);
                  if (buildingsForProvince == null) continue;
                  foreach (var bitem in buildingsForProvince.placedBuildings)
                  {
                     if (IsBuildingFinished(bitem) && bitem.id == "siege_camp")
                        siegeCount += 1;
                  }
               }
            }
            item.SiegeCount = siegeCount;
         return item;
      }
      public static List<Province> GetProvinces()
      {
         if (CurrentBattleground != null)
         {
            List<Province> tmp = CurrentBattleground.map.provinces.ToList();
            foreach (var item in ListClass.MetaMap.provinces)
            {
               if (item.id != null)
               {
                  tmp.Find(p => p.id == item.id).name = item.name;
                  tmp.Find(p => p.id == item.id).connections = item.connections;
               }
               else if (!item.name.IsEmpty() && item.id == null)
               {
                  tmp.Find(p => p.id == null).name = item.name;
                  tmp.Find(p => p.id == null).id = 0;
                  tmp.Find(p => p.id == 0).connections = item.connections;
               }
            }
            return tmp;
         }
         return new List<Province>();
      }
      public static bool CanFightProvince(int id)
      {
         Province Target = ListClass.ProvincesGBG.Find(p => p.id == id);
         DateTime dtLocked = Helper.UnixTimeStampToDateTime(Target.lockedUntil);
         if (dtLocked > DateTime.Now) return false;
         if (Target.ownerId == CurrentBattleground.currentParticipantId) return false;
         List<Province> OwnedByGuild = ListClass.ProvincesGBG.FindAll(p => p.ownerId == CurrentBattleground.currentParticipantId).ToList();
         List<int> connectedIds = OwnedByGuild.SelectMany(obg => obg.connections).ToList();
         return connectedIds.Contains(id);
      }

      //GBG Map ProvinseNames: metadata guild_battleground_maps
   }
}
