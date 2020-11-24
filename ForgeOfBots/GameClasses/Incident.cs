using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses
{
   public class Incident
   {
      public int ID {get; set;}
      public string Position {get; set;}
      public eRarity Rarity {get;set;}
   }
   
   public enum eRarity{
      common,
      rare
   }
}
