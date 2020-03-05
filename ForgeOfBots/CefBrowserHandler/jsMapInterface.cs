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
      public void hook(object message, string source)
      {
         onHookEvent?.Invoke(new hookEvent()
         {
            message = message,
            source = source
         });
      }

      public class hookEvent
      {
         public object message { get; set; }
         public string source { get; set; }
      }
   }
}