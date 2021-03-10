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

namespace ForgeOfBots.DataHandler
{
   public static class GBGHandler
   {
      private static readonly RequestBuilder ReqBuilder = StaticData.ReqBuilder;

      public static Battleground CurrentBattleground { get; private set; } = null;
      public static State CurrentState { get; private set; } = null;

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
            GetArmyInfoGBG response = JsonConvert.DeserializeObject<GetArmyInfoGBG>(ret);
            return response.armyresponse[0].responseData;
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
            GetGBG response = JsonConvert.DeserializeObject<GetGBG>(ret);
            return response.gbgresponse[0].responseData;
         }
         catch (Exception)
         {
            return null;
         }
      }
      public static State GetState()
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
      public static StartFight StartFight(int provinceID)
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
