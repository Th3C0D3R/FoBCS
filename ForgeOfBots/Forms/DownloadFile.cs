using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ByteSizeLib;
using ForgeOfBots.Utils;

namespace ForgeOfBots.Forms
{
   public partial class DownloadFile : Form
   {
      public static ByteSize MaxFileSize;
      public static DateTime? TimeStarted = null;
      public static Dictionary<string, DownloadProgressChangedEventArgs> Files = new Dictionary<string, DownloadProgressChangedEventArgs>();
      public DownloadFile(string filename, long maxbytes)
      {
         InitializeComponent();
         MaxFileSize = ByteSize.FromBytes(maxbytes);
         lblProgressValue.Text = $"0 / {Math.Round(MaxFileSize.LargestWholeNumberDecimalValue,2)}{MaxFileSize.LargestWholeNumberDecimalSymbol}";
         lblFilenameValue.Text = filename;
         mpbDownloadProgress.Maximum = (int)maxbytes;
         Text = $"{i18n.getString("GUI.Download.Header")} {filename}";
         lblDownloadFile.Text = $"{i18n.getString(lblDownloadFile.Tag.ToString())}";
         lblProgress.Text = $"{i18n.getString(lblProgress.Tag.ToString())}";
      }

      public void UpdateProgressbar(DownloadProgressChangedEventArgs args, string filename = "")
      {
         if (TimeStarted == null) TimeStarted = DateTime.Now;
         var diff = args.BytesReceived - mpbDownloadProgress.Value;
         var currentSize = ByteSize.FromBytes(args.BytesReceived);
         var BpS = Math.Round(args.BytesReceived / (DateTime.Now - TimeStarted).Value.TotalSeconds);
         var speedSize = ByteSize.FromBytes(BpS);
         Invoker.CallMethode(mpbDownloadProgress, () => mpbDownloadProgress.Increment((int)diff));
         Invoker.SetProperty(lblProgressValue, () => lblProgressValue.Text, $"{Math.Round(currentSize.LargestWholeNumberDecimalValue,2)} {currentSize.LargestWholeNumberDecimalSymbol} / {Math.Round(MaxFileSize.LargestWholeNumberDecimalValue,2)} {MaxFileSize.LargestWholeNumberDecimalSymbol} ({speedSize.LargestWholeNumberDecimalValue} {speedSize.LargestWholeNumberDecimalSymbol}/s)");
      }
   }
}
