using CefSharp.WinForms;
using ForgeOfBots.DataHandler;
using ForgeOfBots.GameClasses;
//using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ForgeOfBots.CefBrowserHandler
{
   class ResponseHandler
   {
      public static ChromiumWebBrowser browser;
      public static event WorldsLoadedEvent WorldsLoaded;
      public static void HookEventHandler(jsMapInterface.hookEvent hookEventArgs)
      {
         var x = hookEventArgs;
         List<object> msg = (List<object>)x.message;
         string methode = x.methode;
         switch (x.source)
         {
            case "Data":
               HandleResponse(msg, methode);
               break;
            case "MetaData":

               break;
            case "Cities":
               HandleCities(msg);
               break;
            default:
               break;
         }
      }
      public static void HandleCities(List<object> msg)
      {
         foreach (object world in msg)
         {
            ListClass.WorldList.Add(new Tuple<string, string, WorldState>(world.ToString(),world.ToString(),WorldState.active));
         }
         WorldsLoaded?.Invoke("HandleCities");
      }
      public static void HandleResponse(List<object> msg, string method)
      {
         RequestType type = (RequestType)Enum.Parse(typeof(RequestType), method);
         switch (type)
         {
            case RequestType.Startup:
               break;
            case RequestType.Motivate:
               break;
            case RequestType.CollectIncident:
               break;
            case RequestType.VisitTavern:
               break;
            case RequestType.GetClanMember:
               break;
            case RequestType.GetEntities:
               break;
            case RequestType.GetFriends:
               break;
            case RequestType.GetNeighbor:
               break;
            case RequestType.GetLGs:
               break;
            case RequestType.LogService:
               break;
            case RequestType.CollectProduction:
               break;
            case RequestType.QueryProduction:
               break;
            case RequestType.CancelProduction:
               break;
            case RequestType.CollectTavern:
               break;
            case RequestType.GetOwnTavern:
               break;
            case RequestType.RemovePlayer:
               break;
            case RequestType.GetAllWorlds:
               foreach (ExpandoObject item in msg)
               {
                  Dictionary<string, object> dItem = new Dictionary<string, object>(item);
                  if(dItem["requestMethod"].ToString() == "getWorlds")
                  {
                     List<object> worlds = (List<object>)dItem["responseData"];
                     foreach (ExpandoObject world in worlds)
                     {
                        Dictionary<string, object> dWorld = new Dictionary<string, object>(world);
                        ListClass.WorldList.Add(new Tuple<string,string,WorldState>(dWorld["id"].ToString(), dWorld["name"].ToString(), (WorldState)Enum.Parse(typeof(WorldState), dWorld["status"].ToString())));
                     }
                  }
               }
               break;
            default:
               break;
         }
      }
   }
   public delegate void WorldsLoadedEvent(object sender);
}
