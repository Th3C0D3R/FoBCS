using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Types;
using Message = Telegram.Bot.Types.Message;

namespace ForgeOfBots.Utils
{
   public static class TelegramNotify
   {
      private static string APIKEY = "1538712047:AAFCO2llk01V-_2A602D6oXnTRePl52nklE";
      private static TelegramBotClient Bot = null;
      private static User Me = null;
      private static List<string> tmpMessages = new List<string>();
      public static void Init()
      {
         if (Bot != null && Me != null) return;
         Bot = new TelegramBotClient(APIKEY);
         Me = Bot.GetMeAsync().Result;
         Console.WriteLine(
           $"Hello Debugger! I am user {Me.Id} and my name is {Me.FirstName}."
         ); 
         Bot.StartReceiving();
         Bot.OnMessage += Bot_OnMessageReceive;
      }

      private static void Bot_OnMessageReceive(object sender, Telegram.Bot.Args.MessageEventArgs e)
      {
         if (e.Message.Text.StartsWith("/start") && StaticData.UserData.ChatID == -1)
         { //u'\U0001F44D'
            StaticData.UserData.ChatID = e.Message.Chat.Id;
            StaticData.UserData.SaveSettings();
            Message retMsg = Bot.SendTextMessageAsync(new ChatId(StaticData.UserData.ChatID), "👍").Result;
            foreach (string item in tmpMessages)
            {
               retMsg = Bot.SendTextMessageAsync(new ChatId(StaticData.UserData.ChatID), item).Result;
            }
         }
      }

      public static void Send(string msg)
      {
         if (StaticData.UserData.ChatID == -1)
         {
            MessageBox.Show(i18n.getString("GUI.MessageBox.NoTelegramChatText"), i18n.getString("GUI.MessageBox.NoTelegramChatTitle"));
         }
         else
         {
            try
            {
               Message retMsg = Bot.SendTextMessageAsync(new ChatId(StaticData.UserData.ChatID), msg).Result;
            }
            catch (AggregateException ex)
            {
               tmpMessages.Add(msg);
               string message = ex.InnerExceptions[0].Message.ToLower();
               if (message.Contains("chat") && message.Contains("not") && message.Contains("found"))
               {
                  MessageBox.Show(i18n.getString("GUI.MessageBox.NoTelegramChatText"), i18n.getString("GUI.MessageBox.NoTelegramChatTitle"));
               }
            }
         }
      }
   }
}
