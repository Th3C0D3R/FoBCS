using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Windows.Forms;
using ForgeOfBots.Utils;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

using static ForgeOfBots.Utils.Extensions;

namespace ForgeOfBots
{
   static class Program
   {
      /// <summary>
      /// Der Haupteinstiegspunkt für die Anwendung.
      /// </summary>
      [STAThread]
      static void Main(string[] args)
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         AppCenter.Start("03071928-d7cf-4bf5-b512-da1c9bb25975", typeof(Analytics), typeof(Crashes));
         AppCenter.LogLevel = LogLevel.Verbose;

         var config = new NLog.Config.LoggingConfiguration();
         // Targets where to log to: File and Console
         var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.foblog" };
         // Rules for mapping loggers to targets            
         config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
         NLog.LogManager.Configuration = config;
         if (Crashes.IsEnabledAsync().Result)
         {
               StaticData.HasLastCrash = CrashHelper.HasCrashedLastSession();
         }
         Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
         Application.Run(new Main(args));
      }
   }
}
