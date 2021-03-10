using GEXOverview = ForgeOfBots.GameClasses.GEX.Get.Data;
using StartFight = ForgeOfBots.GameClasses.GEX.StartFight.Data;
using OpenChest = ForgeOfBots.GameClasses.GEX.OpenChest.Data;
using ForgeOfBots.GameClasses.GEX.Get;
using ForgeOfBots.GameClasses.GEX.GetEncounter;
using ForgeOfBots.GameClasses.GEX.OpenChest;
using ForgeOfBots.GameClasses.GEX.StartFight;
using ForgeOfBots.Utils;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ForgeOfBots.DataHandler
{
   public static class GEXHelper
   {
      private static readonly RequestBuilder ReqBuilder = StaticData.ReqBuilder;
      public static GEXOverview GEXOverview { get; private set; } = null;

      public static int GetCurrentState
      {
         get
         {
            if (GEXOverview == null) return -1;
            if (GEXOverview.state.Equals("inactive")) return -1;
            if (GEXOverview.progress.currentEntityId >= 127) return -1;
            if (GEXOverview.progress.isMapCompleted) return -1;
            return GEXOverview.progress.currentEntityId;
         }
      }
      public static bool IsCurrentStateChest
      {
         get
         {
            if (GEXOverview == null) return false;
            if (GEXOverview.chests.ToList().Any(c => c.id == GetCurrentState))
               return true;
            return false;
         }
      }
      public static Armywave[] Armywaves
      {
         get
         {
            if (!IsCurrentStateChest)
            {
               return GetEncounter(GetCurrentState);
            }
            else
            {
               return null;
            }
         }
      }
      public static int GetChestID
      {
         get
         {
            if (GEXOverview == null) return -1;
            if (GEXOverview.chests.ToList().Any(c => c.id == GetCurrentState))
               return GetCurrentState;
            return -1;
         }
      }
      public static void UpdateGEX()
      {
         GEXOverview = GetOverview();
      }
      public static OpenChest OpenChest(int id)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXopenChest, id);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            OpenChestResponse openChestResponse = JsonConvert.DeserializeObject<OpenChestResponse>(ret);
            return openChestResponse.responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static GEXOverview GetOverview()
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXgetOverview, "");
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetResponse getGEXresponse = JsonConvert.DeserializeObject<GetResponse>(ret);
            return getGEXresponse.responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static Armywave[] GetEncounter(int id)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXgetEncounter, id);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetEncounterResponse getEncounterGEXresponse = JsonConvert.DeserializeObject<GetEncounterResponse>(ret);
            return getEncounterGEXresponse.responseData.armyWaves;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static StartFight StartFight(int stateID)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXstartAttak, stateID);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            StartFightResponse startFightGEXresponse = JsonConvert.DeserializeObject<StartFightResponse>(ret);
            return startFightGEXresponse.responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
   }
}
