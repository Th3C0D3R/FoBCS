using ForgeOfBots.LanguageFiles;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace ForgeOfBots.Utils
{
   public static class CrashHelper
   {
      private static readonly Logger logger = LogManager.GetCurrentClassLogger();
      public static bool HasCrashedLastSession()
      {
         return Crashes.HasCrashedInLastSessionAsync().Result;
      }
      public static void WaitForUserConfirmation(bool alwaysSend)
      {
         Crashes.ShouldAwaitUserConfirmation = () =>
         {
            return !alwaysSend;
         };
         if (!alwaysSend)
         {
            DialogResult dlres = MessageBox.Show(strings.TextCrashLog, strings.AskCrashLog, MessageBoxButtons.YesNoCancel);
            switch (dlres)
            {
               case DialogResult.Cancel:
                  Crashes.NotifyUserConfirmation(UserConfirmation.DontSend);
                  Crashes.SendingErrorReport += (sender, e) =>
                  {
                     logger.Info($"Sending Error Report...");
                  };
                  Crashes.SentErrorReport += (sender, e) =>
                  {
                     logger.Info($"Error Report send successfull");
                     MessageBox.Show(strings.SuccessTitle, strings.SuccessText);
                  };
                  StaticData.UserData.AllowSendCrashLog = UserConfirmation.DontSend;
                  break;
               case DialogResult.Yes:
                  Crashes.NotifyUserConfirmation(UserConfirmation.AlwaysSend);
                  StaticData.UserData.AllowSendCrashLog = UserConfirmation.AlwaysSend;
                  logger.Info($"Allow AlwaysSend Error Report...");
                  break;
               case DialogResult.No:
                  Crashes.NotifyUserConfirmation(UserConfirmation.Send);
                  StaticData.UserData.AllowSendCrashLog = UserConfirmation.Send;
                  logger.Info($"Allow Error Report send once...");
                  break;
               default:
                  break;
            }
         }
      }

   }
}
