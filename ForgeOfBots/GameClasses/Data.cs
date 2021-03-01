﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses
{
   public class SettingData
   {
      public string Version { get; set; }
      public string Version_Secret { get; set; }
   }

   public class BotData
   {
      public string XSRF { get; set; }
      public string CSRF { get; set; }
      public string CID { get; set; }
      public string SID { get; set; }
      public string UID { get; set; }
      public string WID { get; set; }
      public string ForgeHX { get; set; }
      public string WSecret { get; set; }
      public string WSUrl { get; set; }
   }

   public static class DataCenter
   {
      public static string LGSnipMessage { get; set; }
   }
}
