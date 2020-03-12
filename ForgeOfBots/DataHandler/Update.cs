using CefSharp;
using CefSharp.OffScreen;
using ForgeOfBots.CefBrowserHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.DataHandler
{
   public class Update
   {
      public ChromiumWebBrowser browser { get; private set; }
      public RequestBuilder ReqBuilder { get; private set; }

      public static event UpdateFinishedEvent UpdateFinished;

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

      private void OnUpdateFinished(RequestType type)
      {
         UpdateFinished?.Invoke(type);
      }
   }


   public delegate void UpdateFinishedEvent(RequestType type);

}
