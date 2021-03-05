using ForgeOfBots.GameClasses.GEX.OpenChest;
using ForgeOfBots.GameClasses.GEX.Get;
using ForgeOfBots.GameClasses.GEX.GetEncounter;
using ForgeOfBots.GameClasses.GEX.StartFight;
using ForgeOfBots.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.DataHandler
{
   public static class GEXHelper
   {
      public static RequestBuilder ReqBuilder = StaticData.ReqBuilder;

      public static GameClasses.GEX.OpenChest.Data OpenChest(int id)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXopenChest, id);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            OpenChestGEX openChestResponse = JsonConvert.DeserializeObject<OpenChestGEX>(ret);
            return openChestResponse.openChestResponses[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static GameClasses.GEX.Get.Data GetOverview()
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXgetOverview, "");
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetGEX getGEXresponse = JsonConvert.DeserializeObject<GetGEX>(ret);
            return getGEXresponse.getresponse[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static GameClasses.GEX.GetEncounter.Armywave[] GetEncounter(int id)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXgetEncounter, id);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetEncounterGEX getEncounterGEXresponse = JsonConvert.DeserializeObject<GetEncounterGEX>(ret);
            return getEncounterGEXresponse.getencounterresponse[0].responseData.armyWaves;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static GameClasses.GEX.StartFight.Data StartFight(int stateID)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GEXstartAttak, stateID);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            StartFightGEX startFightGEXresponse = JsonConvert.DeserializeObject<StartFightGEX>(ret);
            return startFightGEXresponse.startfightresponse[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
   }
}
