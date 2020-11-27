using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoUpdaterDotNET;

namespace ForgeOfBots.FoBUpdater
{
   public static class FoBUpdater
   {
      public static void CheckForUpdate()
      {
         AutoUpdater.ShowSkipButton = false;
         AutoUpdater.ShowRemindLaterButton = false;
         AutoUpdater.HttpUserAgent = "AutoUpdater";
         AutoUpdater.ReportErrors = true;
         AutoUpdater.RunUpdateAsAdmin = false;
         AutoUpdater.Start("https://th3c0d3r.xyz/version.xml");
      }
   }
}
