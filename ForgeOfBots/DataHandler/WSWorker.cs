using ForgeOfBots.Utils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ForgeOfBots.DataHandler
{
   public class WSWorker
   {
      private IWebDriver driver = StaticData.driver;
      public BackgroundWorkerEX worker = null;
      private readonly object _locker = new object();

      public WSWorker(string wsurl)
      {
         if (worker != null) return;
         worker = new BackgroundWorkerEX();
         worker.DoWork += Worker_DoWork;
         worker.WorkerSupportsCancellation = true;
         worker.param = wsurl;
         worker.RunWorkerAsync(wsurl);
      }
      //{"requestId":13,"requestClass":"SocketAuthenticationService","requestMethod":"authWithToken","requestData":["xxx"]}
      private void Worker_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorkerEX worker = (BackgroundWorkerEX)sender;
         while (!worker.CancellationPending)
         {
            lock (_locker)
            {
               if (driver != null)
               {
                  foreach (var wsdata in driver.Manage().Logs.GetLog("performance"))
                  {
                     dynamic wsJson = JsonConvert.DeserializeObject(wsdata.Message);
                     try
                     {
                        if (wsJson["message"]["method"] == "Network.webSocketFrameReceived")
                        {
                           JToken payloadData = wsJson["message"]["params"]["response"]["payloadData"];
                           if(payloadData.ToString().Contains("requestData") && payloadData.ToString().Contains("requestClass") && payloadData.ToString().Contains("requestMethod"))
                           {
                              //OtherPlayerService_newEvent trade_accepted
                              //OtherPlayerService_newEvent great_building_contribution
                              //GuildBattlegroundService_getProvinces
                              //ConversationService_getConversation Update
                              //ConversationService_getNewMessage
                              //ItemAuctionService_updateBid
                              //GuildExpeditionService_receiveContributionNotification

                           }
                        }
                        if (wsJson["message"]["method"] == "Network.webSocketFrameSent")
                        {
                           JToken payloadData = wsJson["message"]["params"]["response"]["payloadData"];
                           if (payloadData.ToString().Contains("requestId") && payloadData.ToString().Contains("requestClass") && payloadData.ToString().Contains("requestMethod"))
                           {

                           }
                        }
                     }
                     catch (Exception)
                     {
                        continue;
                     }
                  }
               }
            }
            Thread.Sleep(100);
         }
      }
   }
}
