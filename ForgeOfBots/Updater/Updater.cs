using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoUpdaterDotNET;

namespace ForgeOfBots.FoBUpdater
{
   public static class FoBUpdater
   {
      public static void CheckForUpdate()
      {
         
      }

      private static void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
      {
         if (args.Error == null)
         {
            if (args.IsUpdateAvailable)
            {
               DialogResult dialogResult;
               if (args.Mandatory.Value)
               {
                  dialogResult =
                      MessageBox.Show(
                          $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. This is required update. Press Ok to begin updating the application.", @"Update Available",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
               }
               else
               {
                  dialogResult =
                      MessageBox.Show(
                          $@"There is new version {args.CurrentVersion} available. You are using version { args.InstalledVersion }. Do you want to update the application now?", @"Update Available",
                          MessageBoxButtons.YesNo,
                          MessageBoxIcon.Information);
               }

               // Uncomment the following line if you want to show standard update dialog instead.
               // AutoUpdater.ShowUpdateForm(args);

               if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
               {
                  try
                  {
                     if (AutoUpdater.DownloadUpdate(args))
                     {
                        Application.Exit();
                     }
                  }
                  catch (Exception exception)
                  {
                     MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                  }
               }
            }
            else
            {
               Debug.WriteLine(@"There is no update available please try again later.", @"No update available",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         else
         {
            if (args.Error is WebException)
            {
               Debug.WriteLine(
                   @"There is a problem reaching update server. Please check your internet connection and try again later.",
                   @"Update Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
               Debug.WriteLine(args.Error.Message,
                   args.Error.GetType().ToString(), MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
         }
      }
   }
}
