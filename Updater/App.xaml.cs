using System.Diagnostics;
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
         Debugger.Launch();
         var args = e.Args;
         if (args.Length > 0)
         {
            switch (args[0])
            {
               case "restart":
                  Process.Start("ForgeOfBots.exe", (args.Length == 2 ? args[1].ToString() : ""));
                  Process.GetCurrentProcess().Kill();
                  break;
               case "check":
                  Utils.Version.Init();
                  if (Utils.Version.IsUpdateAvailable(Utils.Helper.ReleaseVersion))
                  {
                     Current.Shutdown(10);
                  }
                  else
                  {
                     Current.Shutdown(1);
                  }
                  break;
               case "update":
                  MainWindow mw = new MainWindow();
                  mw.Show();
                  break;
               default:
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
