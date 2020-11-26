using CefSharp;
#if RELEASE || DEBUG
using CefSharp.OffScreen;
#elif DEBUGFORM
using CefSharp.WinForms;
#endif
using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ForgeOfBots.DataHandler
{
   public class Update
   {
      public ChromiumWebBrowser browser { get; private set; }
      public RequestBuilder ReqBuilder { get; private set; }

      private static UpdateFinishedEvent _UpdateFinished;
      public static event UpdateFinishedEvent UpdateFinished
      {
         add
         {
            if (_UpdateFinished == null || !_UpdateFinished.GetInvocationList().Contains(value))
               _UpdateFinished += value;
         }
         remove
         {
            _UpdateFinished -= value;
         }
      }

      public Update(ChromiumWebBrowser cwb, RequestBuilder reqbuilder)
      {
         browser = cwb;
         ReqBuilder = reqbuilder;
      }

      public void UpdatePlayerLists()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetClanMember, "");
         browser.ExecuteScriptAsync(script);
         script = ReqBuilder.GetRequestScript(RequestType.GetFriends, "");
         browser.ExecuteScriptAsync(script);
         script = ReqBuilder.GetRequestScript(RequestType.GetNeighbor, "");
         browser.ExecuteScriptAsync(script);
         ResponseHandler.ListLoaded += OnUpdateFinished;
      }

      public void UpdateFirends()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetFriends, "");
         browser.ExecuteScriptAsync(script);
         ResponseHandler.ListLoaded += OnUpdateFinished;
      }
      public void UpdateStartUp()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.Startup, "[]");
         browser.ExecuteScriptAsync(script);
         ResponseHandler.StartupLoaded += OnUpdateFinished;
      }
      public void UpdateOwnTavern()
      {
         string script = ReqBuilder.GetRequestScript(RequestType.GetOwnTavern, "[]");
         browser.ExecuteScriptAsync(script);
         ResponseHandler.StartupLoaded += OnUpdateFinished;
      }
      public void UpdatedSortedGoodList()
      {
         ListClass.GoodsDict = Helper.GetGoodsEraSorted(ListClass.Eras, ListClass.Resources, ListClass.ResourceDefinitions);
      }
      public void UpdateBuildings(JToken entities)
      {
         if (ListClass.Startup.Count <= 0 || ListClass.AllBuildings.Count <= 0) return;
         ListClass.ProductionList.Clear();
         ListClass.ResidentialList.Clear();
         ListClass.GoodProductionList.Clear();
         foreach (JToken cityEntity in entities.ToList())
         {
            foreach (Building metaEntity in ListClass.AllBuildings)
            {
               if (cityEntity["cityentity_id"]?.ToString() == metaEntity.id)
               {
                  EntityEx entity = GameClassHelper.CopyFrom(cityEntity);
                  entity.name = metaEntity.name;
                  if (metaEntity.available_products != null)
                     entity.available_products = metaEntity.available_products.ToList();
                  entity.type = metaEntity.type;
                  if (entity.type == "production" && metaEntity.available_products != null && entity.connected >= 1 && GameClassHelper.hasOnlySupplyProduction(entity.available_products))
                     ListClass.ProductionList.Add(entity);
                  else if (entity.type == "residential" && entity.connected >= 1)
                     ListClass.ResidentialList.Add(entity);
                  else if (entity.type == "goods" && entity.connected >= 1)
                     ListClass.GoodProductionList.Add(entity);
               }
            }
         }
      }

      private void OnUpdateFinished(RequestType type)
      {
         _UpdateFinished?.Invoke(type);
      }
   }


   public delegate void UpdateFinishedEvent(RequestType type);

}
