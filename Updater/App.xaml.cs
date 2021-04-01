using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Updater
{
   /// <summary>
   /// Interaktionslogik für "App.xaml"
   /// </summary>
   public partial class App : Application
   {
      private void Application_Startup(object sender, StartupEventArgs e)
      {
         var args = e.Args;
         if (args.Length > 0)
         {
            switch (args[0])
            {
               case "restart":
                  Process.Start("ForgeOfBots.exe", (args.Length == 2 ? args[1].ToString() : ""));
                  Process.GetCurrentProcess().Kill();
                  break;
               default:
                  MainWindow mw = new MainWindow();
                  mw.Show();
                  break;
            }
         }
         else
         {
            Process.Start("ForgeOfBots.exe");
            Process.GetCurrentProcess().Kill();
         }
      }
   }
}
