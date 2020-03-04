using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.CefBrowserHandler
{
   internal class RenderHandler : IRenderProcessMessageHandler
   {
      public void OnContextCreated(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
      {
         var strScript = Main.resMgr.GetString("RequestData");
         frame.ExecuteJavaScriptAsync(strScript);
      }

      public void OnContextReleased(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
      {
      }

      public void OnFocusedNodeChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IDomNode node)
      {
      }

      public void OnUncaughtException(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, JavascriptException exception)
      {
      }
   }
}
