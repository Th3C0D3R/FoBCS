using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses
{
   public class Player
   {
      public string PlayerName { get; set; }
      public string World { get; set; }
      public int Supplies { get; set; } = 0;
      public int Coins { get; set; } = 0;
      public int Diamonds { get; set; } = 0;
      public int Meds { get; set; } = 0;
      public int Silver { get; set; } = 0;
      public bool isSelf { get; set; } = false;
      public bool canMotivate { get; set; } = false;
      public bool canTavern { get; set; } = false;

      public List<Good> Stock { get; set; } = new List<Good>();
      public List<Building> Buildings { get; set; } = new List<Building>();
      public Tavern Tavern { get; set; }
   }
}
