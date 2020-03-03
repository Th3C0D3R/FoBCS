using CefSharp;
using System.IO;
using System.Text;
using CefSharp.ResponseFilter;
using CefSharp.Handler;

namespace ForgeOfBots.CefBrowserHandler
{
   public class CustomResourceRequestHandler : ResourceRequestHandler
   {
      private readonly MemoryStream memoryStream = new MemoryStream();

      protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
      {
         return new StreamResponseFilter(memoryStream);
      }

      protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
      {
         //You can now get the data from the stream
         var bytes = memoryStream.ToArray();

         if (response.Charset == "utf-8")
         {
            var str = Encoding.UTF8.GetString(bytes);
            if(str.Length > 0)
            {
               var x = str;
            }
         }
         else
         {
            //Deal with different encoding here
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
