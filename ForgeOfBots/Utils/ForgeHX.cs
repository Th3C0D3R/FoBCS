using ForgeOfBots.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ForgeOfBots.Utils.StaticData;

namespace ForgeOfBots.Utils
{
   public static class ForgeHX
   {
      private static EventHandler _ForgeHXLoaded;
      public static event EventHandler ForgeHXDownloaded
      {
         add
         {
            if (_ForgeHXLoaded == null || !_ForgeHXLoaded.GetInvocationList().ToList().Contains(value))
               _ForgeHXLoaded += value;
         }
         remove
         {
            _ForgeHXLoaded -= value;
         }
      }

      private static bool Complete = false;
      private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
      public static DownloadFile DownloadFile = null;

      public static string ForgeHXURL { get; set; }
      public static string FileName { get; set; }
      public static bool ForgeHXLoaded { get; set; } = false;
      public static bool SECRET_LOADED { get; set; } = false;

      public static void DownloadForge()
      {
         if (ForgeHXLoaded) return;
         ForgeHXLoaded = true;
         string ForgeHX_FilePath = Path.Combine(ProgramPath, FileName);
         if (!Directory.Exists(ProgramPath)) Directory.CreateDirectory(ProgramPath);
         FileInfo fi = new FileInfo(ForgeHX_FilePath);
         Console.Write($"Downloading {FileName} [");
         if (!fi.Exists || fi.Length <= 0)
         {
            using (var client = new WebClient())
            {
               Uri uri = new Uri(ForgeHXURL.Replace("'", ""));
               client.Headers.Add("User-Agent", UserData.CustomUserAgent);
               client.DownloadProgressChanged += Client_DownloadProgressChanged;
               client.DownloadFileCompleted += Client_DownloadFileCompleted;
               client.DownloadFileAsync(uri, ForgeHX_FilePath);
            }
         }
         else
         {
            Client_DownloadFileCompleted(null, null);
         }
      }

      private static void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
      {
         Complete = true;
         if (DownloadFile != null)
            DownloadFile.Close();
         if (sender == null && e == null) logger.Info($"LOCAL FILE FOUND");
         else logger.Info($"]"); 
         logger.Info($"Downloading {FileName} complete");
         string ForgeHX_FilePath = Path.Combine(ProgramPath, FileName);
         FileInfo fi = new FileInfo(ForgeHX_FilePath);
         var content = File.ReadAllText(ForgeHX_FilePath);
         try
         {
            var startIndex = content.IndexOf(".BUILD_NUMBER=\"");
            var endIndex = content.IndexOf(".TILE_SPEC_NAME_CONTEMPORARY_BUSHES=\"");
            content = content.Substring(startIndex, endIndex - startIndex);
            content = content.Replace("\n", "").Replace("\r", "");
            var regExSecret = new Regex("\\.VERSION_SECRET=\"([a-zA-Z0-9_\\-\\+\\/==]+)\";", RegexOptions.IgnoreCase);
            var regExVersion = new Regex("\\.VERSION_MAJOR_MINOR=\"([0-9+.0-9+.0-9+]+)\";", RegexOptions.IgnoreCase);
            var VersionMatch = regExVersion.Match(content);
            var SecretMatch = regExSecret.Match(content);
            if (VersionMatch.Success)
            {
               SettingData.Version = VersionMatch.Groups[1].Value;
            }
            if (SecretMatch.Success)
            {
               SettingData.Version_Secret = SecretMatch.Groups[1].Value;
               _ForgeHXLoaded?.Invoke(null, null);
            }
         }
         catch (Exception ex)
         {
            logger.Info($"EXCEPTION: {ex.StackTrace}");
            fi.Delete();
            ForgeHXLoaded = false;
            DownloadForge();
         }
      }

      private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
      {
         if(DownloadFile == null)
         {
            DownloadFile = new DownloadFile(FileName, e.TotalBytesToReceive);
            DownloadFile.FormClosing += DownloadFile_FormClosing;
            DownloadFile.TopMost = true;
            DownloadFile.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            DownloadFile.Show();
         }
         else
         {
            DownloadFile.UpdateProgressbar(e);
         }
      }

      private static void DownloadFile_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
      {
         e.Cancel = !Complete;
      }
   }
}
