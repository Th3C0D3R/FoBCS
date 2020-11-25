using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses
{
   public class Language
   {
      public string code { get; set; }
      public string language { get; set; }
      public int index { get; set; }
   }

   public class LanguageList
   {
      public List<Language> Languages { get; set; }
   }
}
