using CefSharp;
using CefSharp.Handler;
using CefSharp.ResponseFilter;
using ForgeOfBots.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ForgeOfBots.CefBrowserHandler
{
   public class CustomResourceRequestHandler : ResourceRequestHandler
   {
      private readonly MemoryStream memoryStream = new MemoryStream();

      private static EventHandler<TwoStringArgs> _ForgeHXFoundEvent;
      public static event EventHandler<TwoStringArgs> ForgeHXFoundEvent
      {
         add
         {
            if (_ForgeHXFoundEvent == null || !_ForgeHXFoundEvent.GetInvocationList().ToList().Contains(value))
               _ForgeHXFoundEvent += value;
         }
         remove
         {
            _ForgeHXFoundEvent -= value;
         }
      }
      private static EventHandler<TwoStringArgs> _UidFoundEvent;
      public static event EventHandler<TwoStringArgs> UidFoundEvent
      {
         add
         {
            if (_UidFoundEvent == null || !_UidFoundEvent.GetInvocationList().ToList().Contains(value))
               _UidFoundEvent += value;
         }
         remove
         {
            _UidFoundEvent -= value;
         }
      }
      private static EventHandler _ServerStartpageLoadedEvent;
      public static event EventHandler ServerStartpageLoadedEvent
      {
         add
         {
            if (_ServerStartpageLoadedEvent == null || !_ServerStartpageLoadedEvent.GetInvocationList().ToList().Contains(value))
               _ServerStartpageLoadedEvent += value;
         }
         remove
         {
            _ServerStartpageLoadedEvent -= value;
         }
      }
      protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
      {
         return new StreamResponseFilter(memoryStream);
      }
      protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
      {
         var bytes = memoryStream.ToArray();

         if (response.Charset == "utf-8")
         {
            if (frame.IsValid && frame.Url.Contains("/game/index?"))
            {
               var str = Encoding.UTF8.GetString(bytes);
               if (str.Length > 0)
               {
                  var regExUserID = new Regex(@"https:\/\/(\w{1,2}\d{1,2})\.forgeofempires\.com\/game\/json\?h=(.+)'", RegexOptions.IgnoreCase);
                  var regExForgeHX = new Regex(@"https:\/\/foe\w{1,4}\.innogamescdn\.com\/\/cache\/ForgeHX(.+.js)'", RegexOptions.IgnoreCase);
                  var FHXMatch = regExForgeHX.Match(str);
                  var UIDMatch = regExUserID.Match(str);
                  if (FHXMatch.Success)
                  {
                     var ForgeHX = FHXMatch.Value;
                     var Filename = "ForgeHX" + FHXMatch.Groups[1].Value;
                     _ForgeHXFoundEvent?.Invoke(null, new TwoStringArgs { s1 = ForgeHX, s2 = Filename });
                  }
                  if (UIDMatch.Success)
                  {
                     var UID = UIDMatch.Groups[2].Value;
                     var WID = UIDMatch.Groups[1].Value;
                     _UidFoundEvent?.Invoke(null, new TwoStringArgs { s1 = UID, s2 = WID });
                  }
               }
            }
            else if (frame.IsValid && frame.Url.Contains("/page/"))
            {
               _ServerStartpageLoadedEvent?.Invoke(null,EventArgs.Empty);
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
