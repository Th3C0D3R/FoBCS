using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using Convert = Newtonsoft.Json.JsonConvert;
using OpenQA.Selenium;
using System;
using System.ComponentModel;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ForgeOfBots.DataHandler
{
   public class WSWorker
   {
      private IWebDriver driver = StaticData.driver;
      public BackgroundWorkerEX worker = null;
      public frmMain Main = null;
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
                     dynamic wsJson = Convert.DeserializeObject(wsdata.Message);
                     try
                     {
                        if (wsJson["message"]["method"] == "Network.webSocketFrameReceived")
                        {
                           JToken payloadData = wsJson["message"]["params"]["response"]["payloadData"];
                           if (payloadData.ToString().Contains("responseData") && payloadData.ToString().Contains("requestClass") && payloadData.ToString().Contains("requestMethod"))
                           {
                              var token = payloadData;
                              WSResponse response = Convert.DeserializeObject<List<WSResponse>>(token.ToString())[0];
                              if (Main != null)
                              {
                                 try
                                 {
                                    Invoker.CallMethode(Main.lvWSMessages, () => Main.lvWSMessages.Items.Add($"Get: {response.requestClass} {response.requestMethod}: {response.responseData}"));
                                 }
                                 catch (Exception)
                                 {}
                              }
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
                              var token = payloadData;
                              WSRequest request = Convert.DeserializeObject<WSRequest>(token.ToString());
                              if (Main != null)
                              {
                                 try
                                 {
                                    Invoker.CallMethode(Main.lvWSMessages, () => Main.lvWSMessages.Items.Add($"Sent: {request.requestClass} {request.requestMethod}: {request.requestData}"));
                                 }
                                 catch (Exception)
                                 { }
                              }
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
