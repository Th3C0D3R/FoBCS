using CefSharp.WinForms;
using ForgeOfBots.DataHandler;
using ForgeOfBots.GameClasses;
//using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ForgeOfBots.Utils;
using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses.ResponseClasses;
using WorldSelection = ForgeOfBots.GameClasses.ResponseClasses.WorldSelection;

namespace ForgeOfBots.CefBrowserHandler
{
   class ResponseHandler
   {
      public static ChromiumWebBrowser browser;
      public static event WorldsLoadedEvent WorldsLoaded;
      public static void HookEventHandler(jsMapInterface.hookEvent hookEventArgs)
      {
         var x = hookEventArgs;
         string methode = x.methode;
         switch (x.source)
         {
            case "Data":
               HandleResponse(x.message, methode);
               break;
            case "MetaData":

               break;
            case "Cities":
               HandleCities(x.message);
               break;
            default:
               break;
         }
      }
      public static void HandleCities(string msg)
      {
         WorldData wd = JsonConvert.DeserializeObject<WorldData>(msg);
         ListClass.AllWorlds = wd.worlds;
         foreach (KeyValuePair<string, int> world in wd.player_worlds)
         {
            string worldname = "";
            foreach (World aworld in wd.worlds)
            {
               if (aworld.id == world.Key)
               {
                  worldname = aworld.name;
                  break;
               }
            }
            ListClass.WorldList.Add(new Tuple<string, string, WorldState>(world.Key, worldname, WorldState.active));
         }
         WorldsLoaded?.Invoke("HandleCities");
      }
      public static void HandleResponse(string msg, string method)
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
               Root<ClanMember> clan = JsonConvert.DeserializeObject<Root<ClanMember>>(msg);
               ListClass.ClanMemberList = clan.responseData;
               break;
            case RequestType.GetEntities:
               break;
            case RequestType.GetFriends:
               Root<Friend> friends = JsonConvert.DeserializeObject<Root<Friend>>(msg);
               ListClass.FriendList = friends.responseData;
               break;
            case RequestType.GetNeighbor:
               Root<Neighbor> neighbor = JsonConvert.DeserializeObject<Root<Neighbor>>(msg);
               ListClass.NeighborList = neighbor.responseData;
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
               Root<WorldSelection> ws = JsonConvert.DeserializeObject<Root<WorldSelection>>(msg);
               foreach (WorldSelection item in ws.responseData)
               {
                  if (!ListClass.WorldList.HasCityID(item.id))
                     ListClass.WorldList.Add(new Tuple<string, string, WorldState>(item.id, item.name, (WorldState)Enum.Parse(typeof(WorldState), item.status)));
                  else
                     ListClass.WorldList = ListClass.WorldList.ChangeTuple(item.id, item.name, (WorldState)Enum.Parse(typeof(WorldState), item.status));
               }
               break;
            default:
               break;
         }
      }
   }
   public delegate void WorldsLoadedEvent(object sender);
}
