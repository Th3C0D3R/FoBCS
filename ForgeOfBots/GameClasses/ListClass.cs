using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses
{
   public static class ListClass
   {
      public static List<Tuple<string, string, WorldState>> WorldList = new List<Tuple<string, string, WorldState>>();
      public static Dictionary<string, string> ServerList = new Dictionary<string, string>()
      {
         {"en", "en.forgeofempires.com"},
         {"de", "de.forgeofempires.com"},
         {"zz (Beta)", "beta.forgeofempires.com"},
         {"us", "us.forgeofempires.com"},
         {"fr", "fr.forgeofempires.com"},
         {"nl", "nl.forgeofempires.com"},
         {"pl", "pl.forgeofempires.com"},
         {"gr", "gr.forgeofempires.com"},
         {"it", "it.forgeofempires.com"},
         {"es", "es.forgeofempires.com"},
         {"pt", "pt.forgeofempires.com"},
         {"ru", "ru.forgeofempires.com"},
         {"ro", "ro.forgeofempires.com"},
         {"br", "br.forgeofempires.com"},
         {"cz", "cz.forgeofempires.com"},
         {"hu", "hu.forgeofempires.com"},
         {"se", "se.forgeofempires.com"},
         {"sk", "sk.forgeofempires.com"},
         {"tr", "tr.forgeofempires.com"},
         {"dk", "dk.forgeofempires.com"},
         {"no", "no.forgeofempires.com"},
         {"th", "th.forgeofempires.com"},
         {"ar", "ar.forgeofempires.com"},
         {"mx", "mx.forgeofempires.com"},
         {"fi", "fi.forgeofempires.com"}
      };
   }
   public enum WorldState
   {
      available,
      current,
      active,
   }
}
