using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
   public class GuildExpedition
   {
      public static List<Tuple<GEStage, bool>> GEStagePart = new List<Tuple<GEStage, bool>>();
      public string state = "";
      public List<Chest> chests;
      public GEProgress progress;

   }
   public class Chest
   {
      public int id;
      public dynamic chest;
   }
   public class GEProgress
   {
      public int CurrentEntityId;
      public E_Difficulty difficulty;
      public bool isMapCompleted;
   }
   public enum E_Difficulty
   {
      Level1 = 0,
      Level2,
      Level3,
      Level4
   }

   public static class GuildExpeditionExtension
   {
      public static bool isChest(this GuildExpedition ge)
      {
         return ge.chests.Any(c => c.id == ge.progress.CurrentEntityId);
      }
   }
}
