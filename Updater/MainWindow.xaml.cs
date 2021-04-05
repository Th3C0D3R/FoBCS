using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
using ByteSizeLib;

namespace Updater
{
   /// <summary>
   /// Interaktionslogik für MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public static ByteSize? MaxFileSize = null;
      public static DateTime? TimeStarted = null;
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
            if (Utils.Helper.ReleaseVersion != null)
            {
               if (Utils.Version.IsUpdateAvailable(Utils.Helper.ReleaseVersion))
               {
                  tbUpdateInfo.Text = Utils.Helper.ReleaseVersion.changelog;
                  tbUpdateProgress.Text = $"0/0";

                  try
                  {
                     string URLLang = $"https://raw.githubusercontent.com/Th3C0D3R/FoBCS/master/ForgeOfBots/FoBRelease/ForgeOfBots.exe";
                     using (var webClient = new WebClient())
                     {
                        webClient.Encoding = Encoding.Default;
                        webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                        webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                        webClient.DownloadFile(URLLang, "ForgeOfBots.exe");
                     }
                  }
                  catch (Exception ex)
                  {
                     MessageBox.Show($"Failed to download latest Version {Utils.Helper.ReleaseVersion.version}");
                     Console.WriteLine(ex.ToString());
                     Debug.WriteLine(ex.ToString());
                  }
               }
            }
         }
      }

      private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
      {
         MessageBoxResult dlgRes = MessageBox.Show("Press 'ok' to start the Bot!", "Update finished", MessageBoxButton.OKCancel);
         if (dlgRes == MessageBoxResult.OK)
         {
            Process.Start("ForgeOfBots.exe");
            Process.GetCurrentProcess().Kill();
         }
         else
            Process.GetCurrentProcess().Kill();
      }

      private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
      {
         if (MaxFileSize == null)
         {
            MaxFileSize = ByteSize.FromBytes(e.TotalBytesToReceive);
            pbProgress.Maximum = (int)e.TotalBytesToReceive;
         }
         if (TimeStarted == null) TimeStarted = DateTime.Now;
         var diff = e.BytesReceived - pbProgress.Value;
         var currentSize = ByteSize.FromBytes(e.BytesReceived);
         var BpS = Math.Round(e.BytesReceived / (DateTime.Now - TimeStarted).Value.TotalSeconds);
         var speedSize = ByteSize.FromBytes(BpS);
         pbProgress.Value += diff;
         tbUpdateProgress.Text = $"{Math.Round(currentSize.LargestWholeNumberDecimalValue, 2)} {currentSize.LargestWholeNumberDecimalSymbol} / {Math.Round(MaxFileSize.Value.LargestWholeNumberDecimalValue, 2)} {MaxFileSize.Value.LargestWholeNumberDecimalSymbol} ({Math.Round(speedSize.LargestWholeNumberDecimalValue, 2)} {speedSize.LargestWholeNumberDecimalSymbol}/s)";
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
