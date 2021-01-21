using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static ForgeOfBots.Utils.StaticData;

namespace ForgeOfBots.Utils
{
   public static class GoodImageExtractor
   {
      public static string GoodImageFileURL { get; set; } = "https://foe##server##.innogamescdn.com/assets/shared/icons/goods_large/fine_goods_large_0.png";
      public static string GoodJSONFileURL { get; set; } = "https://foe##server##.innogamescdn.com/assets/shared/icons/goods_large/fine_goods_large_0.json";
      public static string GoodImageFilePath { get; set; } = "";
      public static string GoodJSONFilePath { get; set; } = "";

      private static bool ImageLoaded = false;
      private static bool JSONLoaded = false;

      public static void GetGoodImages(string server)
      {
         DownloadImageFile(server);
         DownloadJSONFile(server);
      }
      public static void DownloadImageFile(string server)
      {
         string imageURL = GoodImageFileURL.Replace("##server##", server);
         string Image_FilePath = Path.Combine(ProgramPath, Path.GetFileName(imageURL));
         if (!Directory.Exists(ProgramPath)) Directory.CreateDirectory(ProgramPath);
         FileInfo fi = new FileInfo(Image_FilePath);
         Console.Write($"Downloading {Path.GetFileName(imageURL)} [");
         using (var client = new WebClient())
         {
            Uri uri = new Uri(imageURL.Replace("'", ""));
            client.Headers.Add("User-Agent", UserData.CustomUserAgent);
            client.QueryString.Add("file", Image_FilePath);
            if (!fi.Exists || fi.Length <= 0)
            {
               client.DownloadProgressChanged += Client_DownloadProgressChanged;
               client.DownloadFileCompleted += Client_DownloadFileCompleted;
               client.DownloadFileAsync(uri, Image_FilePath);
            }
            else
            {
               Client_DownloadFileCompleted(client, null);
            }
         }
      }
      public static void DownloadJSONFile(string server)
      {
         string jsonURL = GoodJSONFileURL.Replace("##server##", server);
         string JSON_FilePath = Path.Combine(ProgramPath, Path.GetFileName(jsonURL));
         if (!Directory.Exists(ProgramPath)) Directory.CreateDirectory(ProgramPath);
         FileInfo fi = new FileInfo(JSON_FilePath);
         Console.Write($"Downloading {Path.GetFileName(jsonURL)} [");
         using (var client = new WebClient())
         {
            Uri uri = new Uri(jsonURL.Replace("'", ""));
            client.Headers.Add("User-Agent", UserData.CustomUserAgent);
            client.QueryString.Add("file", JSON_FilePath);
            if (!fi.Exists || fi.Length <= 0)
            {
               client.DownloadProgressChanged += Client_DownloadProgressChanged;
               client.DownloadFileCompleted += Client_DownloadFileCompleted;
               client.DownloadFileAsync(uri, JSON_FilePath);
            }
            else
            {
               Client_DownloadFileCompleted(client, null);
            }
         }
      }
      private static void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
      {
         if (e == null) Console.Write("FOUND LOCAL FILE");
         Console.WriteLine("]");
         string FileName = ((WebClient)sender).QueryString["file"];
         Console.WriteLine($"Downloading {FileName} complete");
         if (Path.GetExtension(FileName) == ".png")
         {
            GoodImageFilePath = FileName;
            if (!JSONLoaded) ImageLoaded = true;
            else
            {
               ImageLoaded = true;
               GoodImageList = SplitImage(GoodJSONFilePath, GoodImageFilePath);
               GoodImageList.ImageSize = new System.Drawing.Size(32, 32);
               GoodImageList.ColorDepth = ColorDepth.Depth32Bit;
            }
         }
         else if (Path.GetExtension(FileName) == ".json")
         {
            GoodJSONFilePath = FileName;
            if (!ImageLoaded) JSONLoaded = true;
            else
            {
               JSONLoaded = true;
               GoodImageList = SplitImage(GoodJSONFilePath, GoodImageFilePath);
               GoodImageList.ImageSize = new System.Drawing.Size(32, 32);
               GoodImageList.ColorDepth = ColorDepth.Depth32Bit;
            }
         }
      }
      private static ImageList SplitImage(string Json, string image)
      {
         ImageList il = new ImageList();
         Bitmap bmImage = (Bitmap)Image.FromFile(image);
         var content = File.ReadAllText(Json);
         GoodJson json = JsonConvert.DeserializeObject<GoodJson>(content);
         foreach (var item in json.frames)
         {
            int width = int.Parse(item[3].ToString());
            int height = int.Parse(item[4].ToString());
            Bitmap goodImage = CropImage(bmImage, int.Parse(item[1].ToString()), int.Parse(item[2].ToString()), width, height);
            if (item[item.Length - 1].ToString().ToLower() == "true")
               goodImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            goodImage = goodImage.Resize(32, 32);
            il.Images.Add(item[0].ToString(), goodImage);
         }
         return il;
      }
      private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
      {
         if ((e.ProgressPercentage % 2) == 0)
            Console.Write("#");
      }
      public static Bitmap CropImage(Image source, int x, int y, int width, int height)
      {
         Rectangle crop = new Rectangle(x, y, width, height);

         var bmp = new Bitmap(crop.Width, crop.Height);
         using (var gr = Graphics.FromImage(bmp))
         {
            gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
         }
         return bmp;
      }
   }
   public class GoodJson
   {
      public string image { get; set; }
      public Size size { get; set; }
      public int scale { get; set; }
      public object[][] frames { get; set; }
   }
   public class Size
   {
      public int w { get; set; }
      public int h { get; set; }
   }
}
