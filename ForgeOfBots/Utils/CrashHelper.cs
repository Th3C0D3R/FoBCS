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
                     
                  };
                  StaticData.UserData.AllowSendCrashLog = UserConfirmation.DontSend;
                  break;
               case DialogResult.Yes:
                  Crashes.NotifyUserConfirmation(UserConfirmation.AlwaysSend);
                  StaticData.UserData.AllowSendCrashLog = UserConfirmation.AlwaysSend;
                  break;
               case DialogResult.No:
                  Crashes.NotifyUserConfirmation(UserConfirmation.Send);
                  StaticData.UserData.AllowSendCrashLog = UserConfirmation.Send;
                  break;
               default:
                  break;
            }
         }
      }

   }
}
