using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Windows.Forms;
using ForgeOfBots.Utils;
using System.Threading;
using System.Globalization;

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
         //if (Settings.SettingsExists())
         //{
         //   Settings UserData = Settings.ReadSettings();
         //   Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
         //   Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
         //   CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
         //   CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(UserData.Language.Code);
         //   if (UserData.DarkMode)
         //   {
         //      Application.Run(new MainDark(args));
         //   }
         //   else
         //      Application.Run(new Main(args));
         //}
         //else
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
