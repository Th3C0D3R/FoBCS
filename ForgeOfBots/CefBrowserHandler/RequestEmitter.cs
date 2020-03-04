using CefSharp;
using System.IO;
using System.Text;
using CefSharp.ResponseFilter;
using CefSharp.Handler;
using System.Text.RegularExpressions;
using System.Xml;

namespace ForgeOfBots.CefBrowserHandler
{
   public class CustomResourceRequestHandler : ResourceRequestHandler
   {
      private readonly MemoryStream memoryStream = new MemoryStream();
      public delegate void ForgeHX_Found(string forgehx, string filename);
      public static event ForgeHX_Found ForgeHXFoundEvent;
      public delegate void UID_Found(string uid);
      public static event UID_Found UidFoundEvent;
      public delegate void ServerStartpage_Loaded();
      public static event ServerStartpage_Loaded ServerStartpageLoadedEvent;
      protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
      {
         return new StreamResponseFilter(memoryStream);
      }
      protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
      {
         var bytes = memoryStream.ToArray();

         if (response.Charset == "utf-8")
         {
            if (frame.Url.Contains("/game/index?"))
            {
               var str = Encoding.UTF8.GetString(bytes);
               if (str.Length > 0)
               {
                  var regExUserID = new Regex(@"https:\/\/\w{1,2}\d{1,2}\.forgeofempires\.com\/game\/json\?h=(.+)'", RegexOptions.IgnoreCase);
                  var regExForgeHX = new Regex(@"https:\/\/foe\w{1,4}\.innogamescdn\.com\/\/cache\/ForgeHX(.+.js)'", RegexOptions.IgnoreCase);
                  var FHXMatch = regExForgeHX.Match(str);
                  var UIDMatch = regExUserID.Match(str);
                  if (FHXMatch.Success)
                  {
                     var ForgeHX = FHXMatch.Value;
                     var Filename = "ForgeHX" + FHXMatch.Groups[1].Value;
                     ForgeHXFoundEvent?.Invoke(ForgeHX, Filename);
                  }
                  if (UIDMatch.Success)
                  {
                     var UID = UIDMatch.Groups[1].Value;
                     UidFoundEvent?.Invoke(UID);
                  }
               }
            }
            else if (frame.Url.Contains("/page/"))
            {
               ServerStartpageLoadedEvent?.Invoke();
            }
         }
      }
   }

   public class CustomRequestHandler : RequestHandler
   {
      protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
      {
         return new CustomResourceRequestHandler();
      }
   }


}
