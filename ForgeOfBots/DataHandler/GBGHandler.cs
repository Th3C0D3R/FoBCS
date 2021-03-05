using ForgeOfBots.Utils;
using ForgeOfBots.GameClasses.GBG.ArmyInfo;
using ForgeOfBots.GameClasses.GBG.Get;
using ForgeOfBots.GameClasses.GBG.State;
using ForgeOfBots.GameClasses.GBG.StartFight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.DataHandler
{
   public static class GBGHandler
   {
      public static RequestBuilder ReqBuilder = StaticData.ReqBuilder;

      public static GameClasses.GBG.ArmyInfo.Data[] GetArmyInfo(int provinceID)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGgetArmyInfo, provinceID);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetArmyInfoGBG response = JsonConvert.DeserializeObject<GetArmyInfoGBG>(ret);
            return response.armyresponse[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static GameClasses.GBG.Get.Data GetBattleground()
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGgetBattleground, "");
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GetGBG response = JsonConvert.DeserializeObject<GetGBG>(ret);
            return response.gbgresponse[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static GameClasses.GBG.State.Data GetState()
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGgetState, "");
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            GBGState response = JsonConvert.DeserializeObject<GBGState>(ret);
            return response.stateresponse[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static GameClasses.GBG.StartFight.Data StartFight(int provinceID)
      {
         try
         {
            string script = ReqBuilder.GetRequestScript(RequestType.GBGstartAttack, provinceID);
            string ret = (string)StaticData.jsExecutor.ExecuteAsyncScript(script);
            StartFightGBG startFightGBGresponse = JsonConvert.DeserializeObject<StartFightGBG>(ret);
            return startFightGBGresponse.startfight[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
   }
}
