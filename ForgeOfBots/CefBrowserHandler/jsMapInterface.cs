﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ForgeOfBots.CefBrowserHandler
{
   public class jsMapInterface
   {
      public Action<hookEvent> onHookEvent { get; set; } = null;
      public void hook(string message, string source, string methode)
      {
         onHookEvent?.Invoke(new hookEvent()
         {
            message = message,
            source = source,
            methode = methode
         });
      }

      public class hookEvent
      {
         public string message { get; set; }
         public string source { get; set; }
         public string methode { get; set; }
      }
   }
}