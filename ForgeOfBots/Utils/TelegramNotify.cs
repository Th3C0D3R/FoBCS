using ForgeOfBots.GameClasses.Telegram;
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
      public static void Init()
      {
         //if (Bot != null && Me != null) return;
         //Bot = new TelegramBotClient(APIKEY);
         //Me = Bot.GetMeAsync().Result;
         //Console.WriteLine(
         //  $"Hello Debugger! I am user {Me.Id} and my name is {Me.FirstName}."
         //);
         //Bot.StartReceiving();
         //Bot.OnMessage += Bot_OnMessageReceive;
      }

      private static void Bot_OnMessageReceive(object sender, Telegram.Bot.Args.MessageEventArgs e)
      {
         return;
         if (StaticData.UserData.TelegramUserName.IsEmpty()) return;
         if (StaticData.UserData.TelegramUserName != e.Message.Chat.Username) return;
         string sCommand = e.Message.Text.TrimStart('/');
         sCommand = sCommand.Substring(0, sCommand.IndexOf(" "));
         if (!Enum.TryParse(sCommand, out ECommands command))
         {
            //_ = Bot.SendTextMessageAsync(new ChatId(StaticData.UserData.ChatID), i18n.getString("GUI.Telegram.CommandNotFound", new KeyValuePair<string, string>("##command##", sCommand))).Result;
         }
         else
         {
            //foreach (string item in tmpMessages)
              // _ = Bot.SendTextMessageAsync(new ChatId(StaticData.UserData.ChatID), item).Result;
            //tmpMessages.Clear();
            switch (command)
            {
               case ECommands.start:
                  if (StaticData.UserData.ChatID == -1)
                  {
                     StaticData.UserData.ChatID = e.Message.Chat.Id;
                     StaticData.UserData.SaveSettings();
                  }
                  //_ = Bot.SendTextMessageAsync(new ChatId(StaticData.UserData.ChatID), "👍").Result;
                  break;
               case ECommands.restart:
                  break;
               case ECommands.login:
                  break;
               case ECommands.exit:
                  break;
               case ECommands.query:
                  break;
               case ECommands.collect:
                  break;
               case ECommands.snip:
                  break;
               case ECommands.incidents:
                  break;
               case ECommands.collect_incidents:
                  break;
               default:
                  break;
            }
         }
      }

      public static void Send(string msg)
      {
         return;
         if (StaticData.UserData.ChatID == -1)
         {
            //tmpMessages.Add(msg);
           // MessageBox.Show(i18n.getString("GUI.MessageBox.NoTelegramChatText"), i18n.getString("GUI.MessageBox.NoTelegramChatTitle"));
         }
         else
         {
            try
            {
               //_ = Bot.SendTextMessageAsync(new ChatId(StaticData.UserData.ChatID), msg).Result;
            }
            catch (AggregateException ex)
            {
               //tmpMessages.Add(msg);
               string message = ex.InnerExceptions[0].Message.ToLower();
               if (message.Contains("chat") && message.Contains("not") && message.Contains("found"))
               {
                  //MessageBox.Show(i18n.getString("GUI.MessageBox.NoTelegramChatText"), i18n.getString("GUI.MessageBox.NoTelegramChatTitle"));
               }
            }
         }
      }
   }
}
