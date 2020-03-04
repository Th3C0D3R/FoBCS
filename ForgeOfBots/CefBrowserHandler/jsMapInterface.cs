using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ForgeOfBots.CefBrowserHandler
{
   public class jsMapInterface
   {
      public Action<hookEvent> onHookEvent = null;
      public void hook(string message, string method, string url, bool async, string user, string pass, string context) // Must be lowercase!
      {
         onHookEvent?.Invoke(new hookEvent()
         {
            message = message,
            url = url,
            async = async,
            user = user,
            pass = pass,
            context = context
         });
      }

      public class hookEvent
      {
         public string message { get; set; }
         public string url { get; set; }
         public bool async { get; set; }
         public string user { get; set; }
         public string pass { get; set; }
         public string context { get; set; }
      }
   }
}