using ForgeOfBots.Forms;
using ForgeOfBots.Forms.UserControls;
using System.Drawing;
using System.Timers;
using static ForgeOfBots.Utils.Helper;

namespace ForgeOfBots.Utils.Testing
{


   public class Rootobject
   {
      public string languageCode { get; set; }
      public string languageName { get; set; }
      public Root Root { get; set; }
   }

   public class Root
   {
      public string Text { get; set; }
      public Child[] Children { get; set; }
   }

   public class Child
   {
      public string Title { get; set; }
      public bool HasChildren { get; set; }
      public string Text { get; set; }
   }

   public class ChildWithChild:Child
   {
      public Child[] Children { get; set; }
   }



}
