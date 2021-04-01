using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Updater
{
   /// <summary>
   /// Interaktionslogik für MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public static Utils.Version ReleaseVersion = null;
      public MainWindow()
      {
         InitializeComponent();
         Utils.Version.Init();
      }

      private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
      {
         if (IsVisible)
         {
            pnlUpdate.Visibility = Visibility.Hidden;
            pnlUpdateAvailable.Visibility = Visibility.Visible;
            if(ReleaseVersion != null)
            {
               if (Utils.Version.IsUpdateAvailable(ReleaseVersion))
               {
                  //https://github.com/Th3C0D3R/ForgeOfBot/releases/download/v{version}/ForgeOfBots.exe
               }
            }
         }
      }

      private void Button_Click(object sender, RoutedEventArgs e)
      {
         if (IsVisible)
         {
            pnlUpdate.Visibility = Visibility.Visible;
            pnlUpdateAvailable.Visibility = Visibility.Hidden;
         }
      }
   }
}
