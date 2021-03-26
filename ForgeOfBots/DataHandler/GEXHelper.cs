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
using ForgeOfBots.GameClasses.GEX.GetBuyContext;
using Data = ForgeOfBots.GameClasses.GEX.GetBuyContext.Data;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.GameClasses.GEX.GetDifficulties;

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
            if (GEXOverview.progress.isMapCompleted && GEXOverview.progress.difficulty == 3) return -1;
            return GEXOverview.progress.currentEntityId;
         }
         private set { }
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
         if (GEXOverview.state == "inactive") return;
         if (GEXOverview.progress.difficulty < 3 && GEXOverview.progress.isMapCompleted)
         {
            if (NextDiffOpen(GEXOverview.progress.difficulty))
            {
               ChangeDiff(GEXOverview.progress.difficulty + 1);
            }
            else
            {
               GetCurrentState = -1;
            }
         }
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
         var ret = "";
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXgetOverview, "");
            ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetResponse getGEXresponse = JsonConvert.DeserializeObject<GetResponse>(ret);
            return getGEXresponse.responseData;
         }
         catch (Exception)
         {
            try
            {
               GetGEX getGEXresponse = JsonConvert.DeserializeObject<GetGEX>(ret);
               return getGEXresponse.getresponse[0].responseData;
            }
            catch (Exception)
            {
               return null;
            }
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
      public static bool ChangeDiff(int newDiff)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.changeDifficulty, newDiff);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetResponse getGEXresponse = JsonConvert.DeserializeObject<GetResponse>(ret);
            GEXOverview = getGEXresponse.responseData;
            return getGEXresponse.responseData.progress.difficulty == newDiff && !getGEXresponse.responseData.progress.isMapCompleted;
         }
         catch (Exception)
         {
            return false;
         }
      }
      public static bool NextDiffOpen(int currDiff)
      {
         try
         {
            if (currDiff == 3) return false;
            string script = ReqBuilder.GetRequestScript(RequestType.getDifficulties, "");
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetDifficulties response = JsonConvert.DeserializeObject<GetDifficulties>(ret);
            if (response.responseData.Length - 1 > currDiff+1)
            {
               if (response.responseData[currDiff+1].unlocked && response.responseData[currDiff+1].playable) return true;
               else return false;
            }
            return false;
         }
         catch (Exception)
         {
            return false;
         }
      }
      public static bool NeedChangeDiff()
      {
         if (GEXOverview.progress.isMapCompleted && GEXOverview.progress.difficulty < 3)
            return true;

         return false;
      }
      public static bool BuyNextAttempt()
      {
         if (GEXOverview.state.Equals("inactive") || GetCurrentState == -1) return false;
         string script = ReqBuilder.GetRequestScript(RequestType.getContexts, "guildExpedition");
         var ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
         GetBuyContext buyContext = JsonConvert.DeserializeObject<GetBuyContext>(ret);
         Data d = buyContext.responseData[0];
         int currentCost = 0;
         if (d.context == "guildExpedition")
         {
            currentCost = d.offers[0].costs.resources.medals;
         }
         ResearchEra noAge = ListClass.Eras.Find(re => re.era == "NoAge");
         if (CanBuyNextAttempt(ListClass.GoodsDict[noAge.name].Find(g => g.good_id == "medals").value, currentCost))
         {
            script = ReqBuilder.GetRequestScript(RequestType.buyOffer, "guild_expedition_attempt1medals0");
            ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            if (ret.Contains("ResourceShopService"))
            {
               return true;
            }
         }
         return false;
      }
      public static bool CanBuyNextAttempt(int medal, int currentCost)
      {
         if (medal < Math.Floor(currentCost * 1.2)) return false;
         return true;
      }
   }
}
