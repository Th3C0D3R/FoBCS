using CefSharp.WinForms;
//using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.CefBrowserHandler
{
   class InteractionHandler
   {
      public static ChromiumWebBrowser browser;
      public static void HookEventHandler(jsMapInterface.hookEvent hookEventArgs)
      {

         var x = hookEventArgs;

      }
   }
}
