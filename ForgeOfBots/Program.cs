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
using ForgeOfBots.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Linq;
using System.IO;

namespace ForgeOfBots
{
   static class Program
   {

      public static Loading LoadingFrm = null;

      /// <summary>
      /// Der Haupteinstiegspunkt für die Anwendung.
      /// </summary>
      [STAThread]
      static void Main(string[] args)
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

         if (File.Exists("tmp_Updater.exe"))
         {
            File.Delete("Updater.exe");
            File.Move("tmp_Updater.exe", "Updater.exe");
         }

         Process updater = Process.Start("Updater.exe", "check");
         while (!updater.HasExited)
         {
            Application.DoEvents();
            Thread.Sleep(10);
         }
         if (updater.ExitCode == 10)
         {
            updater.StartInfo.Arguments = "update";
            updater.Start();
            return;
         }

         AppCenter.Start("03071928-d7cf-4bf5-b512-da1c9bb25975", typeof(Analytics), typeof(Crashes));
         AppCenter.LogLevel = LogLevel.Verbose;

         Console.WriteLine("[INIT] Killing all old chromedriver and chrome instances");
         try
         {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
               try
               {
                  process.Kill();
               }
               catch (Exception)
               { }
            }
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
               try
               {
                  string commandLine = process.GetCommandLine();
                  if (commandLine == null) continue;
                  if (commandLine.Contains("--no-sandbox") &&
                     commandLine.Contains("--enable-automation") &&
                     commandLine.Contains("--test-type=webdriver") &&
                     commandLine.Contains("--remote-debugging-port=1337"))
                  {
                     process.Kill();
                  }
               }
               catch (Exception)
               { }
            }
         }
         catch (Exception)
         { }

         var config = new NLog.Config.LoggingConfiguration();
         var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.foblog" };
         config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
         NLog.LogManager.Configuration = config;
         if (Crashes.IsEnabledAsync().Result)
         {
            StaticData.HasLastCrash = CrashHelper.HasCrashedLastSession();
         }
         StaticData.RunningTime.Start();

         LoadingFrm = new Loading();
         LoadingFrm.Show();

         frmMain main = new frmMain(args);
         Application.Run(main);
      }
      private static string GetCommandLine(this Process process)
      {
         using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
         using (ManagementObjectCollection objects = searcher.Get())
         {
            return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
         }

      }
   }
}
