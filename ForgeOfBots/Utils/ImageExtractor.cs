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
using ForgeOfBots.Forms;
using Newtonsoft.Json;
using static ForgeOfBots.Utils.StaticData;

namespace ForgeOfBots.Utils
{
   public class ImageExtractor
   {
      public string GoodImageFileURL { get; set; } = "https://foe##server##.innogamescdn.com/assets/shared/icons/goods_large/fine_goods_large_0.png";
      public string GoodJSONFileURL { get; set; } = "https://foe##server##.innogamescdn.com/assets/shared/icons/goods_large/fine_goods_large_0.json";
      public string UnitImageFileURL { get; set; } = "https://foe##server##.innogamescdn.com/assets/shared/unit_portraits/armyuniticons_50x50/armyuniticons_50x50_0-4711b7ef7.png";
      public string UnitJSONFileURL { get; set; } = "https://foe##server##.innogamescdn.com/assets/shared/unit_portraits/armyuniticons_50x50/armyuniticons_50x50_0-087b562bb.json";

      public string ImageFilePath { get; set; } = "";
      public string JSONFilePath { get; set; } = "";

      private bool ImageLoaded = false;
      private bool JSONLoaded = false;
      private object _lock = new object();
      private Size CropSize = new Size(32, 32);
      private ImageExtractorKey key = ImageExtractorKey.Goods;
      private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

      public void GetGoodImages(string server)
      {
         key = ImageExtractorKey.Goods;
         ImageLoaded = false;
         JSONLoaded = false;
         DownloadImageFile(server, GoodImageFileURL);
         DownloadJSONFile(server, GoodJSONFileURL);
      }
      public void GetUnitImages(string server, Size size)
      {
         ImageLoaded = false;
         key = ImageExtractorKey.Units;
         JSONLoaded = false;
         CropSize = size;
         DownloadImageFile(server, UnitImageFileURL);
         DownloadJSONFile(server, UnitJSONFileURL);
      }
      public void DownloadImageFile(string server, string imgurl)
      {
         string imageURL = imgurl.Replace("##server##", server);
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
      public void DownloadJSONFile(string server, string jsonurl)
      {
         string jsonURL = jsonurl.Replace("##server##", server);
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
      private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
      {
         lock (_lock)
         {
            if (e == null) logger.Info($"LOCAL FILE FOUND");
            else logger.Info($"]");
            string FileName = ((WebClient)sender).QueryString["file"];
            Console.WriteLine($"Downloading {FileName} complete");
            if (Path.GetExtension(FileName) == ".png")
            {
               ImageFilePath = FileName;
               if (!JSONLoaded) ImageLoaded = true;
               else
               {
                  ImageLoaded = true;
                  if (key == ImageExtractorKey.Goods)
                  {
                     GoodImageList = SplitGoodImage(JSONFilePath, ImageFilePath);
                     GoodImageList.ImageSize = new System.Drawing.Size(CropSize.w, CropSize.h);
                     GoodImageList.ColorDepth = ColorDepth.Depth32Bit;
                  }
                  else
                  {
                     UnitImageLise = SplitUnitImage(JSONFilePath, ImageFilePath);
                     UnitImageLise.ImageSize = new System.Drawing.Size(CropSize.w, CropSize.h);
                     UnitImageLise.ColorDepth = ColorDepth.Depth32Bit;
                  }
               }
            }
            else if (Path.GetExtension(FileName) == ".json")
            {
               JSONFilePath = FileName;
               if (!ImageLoaded) JSONLoaded = true;
               else
               {
                  JSONLoaded = true;
                  if (key == ImageExtractorKey.Goods)
                  {
                     GoodImageList = SplitGoodImage(JSONFilePath, ImageFilePath);
                     GoodImageList.ImageSize = new System.Drawing.Size(CropSize.w, CropSize.h);
                     GoodImageList.ColorDepth = ColorDepth.Depth32Bit;
                  }
                  else
                  {
                     UnitImageLise = SplitUnitImage(JSONFilePath, ImageFilePath);
                     UnitImageLise.ImageSize = new System.Drawing.Size(CropSize.w, CropSize.h);
                     UnitImageLise.ColorDepth = ColorDepth.Depth32Bit;
                  }
               }
            }
         }
      }
      private ImageList SplitGoodImage(string Json, string image)
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
      private ImageList SplitUnitImage(string Json, string image)
      {
         ImageList il = new ImageList();
         Bitmap bmImage = (Bitmap)Image.FromFile(image);
         var content = File.ReadAllText(Json);
         UnitJson json = JsonConvert.DeserializeObject<UnitJson>(content);
         foreach (var item in json.frames)
         {
            int width = int.Parse(item[3].ToString());
            int height = int.Parse(item[4].ToString());
            Bitmap unitImage = CropImage(bmImage, int.Parse(item[1].ToString()), int.Parse(item[2].ToString()), width, height);
            unitImage = unitImage.Resize(width, height);
            il.Images.Add(item[0].ToString(), unitImage);
         }
         return il;
      }
      private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
      {
         if ((e.ProgressPercentage % 2) == 0)
            Console.Write("#");
      }
      public Bitmap CropImage(Image source, int x, int y, int width, int height)
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
      public Size(int ws, int hs)
      {
         w = ws;
         h = hs;
      }
   }
   public class UnitJson
   {
      public string image { get; set; }
      public Size size { get; set; }
      public int scale { get; set; }
      public object[][] frames { get; set; }
   }
   public enum ImageExtractorKey
   {
      Goods,
      Units
   }
}
