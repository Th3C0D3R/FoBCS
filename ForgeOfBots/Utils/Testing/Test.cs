using ForgeOfBots.Forms;
using ForgeOfBots.Forms.UserControls;
using ForgeOfBots.LanguageFiles;
using System.Drawing;
using System.Timers;
using static ForgeOfBots.Utils.Helper;

namespace ForgeOfBots.Utils.Testing
{
   public static class Test
   {
      public static int TestInt = 0;
      public static int TestStep = 1;
      public static void TestWorkerList(int x, int y)
      {
         WorkerItem wi = new WorkerItem()
         {
            Title = strings.VisitAllTitle,
            BeforeCountText = strings.Status,
            CountText = strings.CountLabel.Replace("##Done##", TestInt.ToString()).Replace("##End##", "100".ToString()),
            ID = StaticData.TavernVisitWorkerID,
            Name = StaticData.TavernVisitWorkerID.ToString()
         };
         StaticData.WorkerList = new WorkerList();
         StaticData.WorkerList.Show();
         if (!IsOnScreen(StaticData.WorkerList))
            StaticData.WorkerList.DesktopLocation = new Point(x, y);
         StaticData.WorkerList.AddWorkerItem(wi);

         Timer t = new Timer
         {
            Interval = 200
         };
         t.Elapsed += elapsed;
         t.Start();
      }

      private static void elapsed(object sender, ElapsedEventArgs e)
      {
         Timer that = (Timer)sender;
         if (that.Enabled)
         {
            StaticData.WorkerList.UpdateWorkerLabel(StaticData.TavernVisitWorkerID, strings.CountLabel.Replace("##Done##", TestInt.ToString()).Replace("##End##", "100".ToString()));
            StaticData.WorkerList.UpdateWorkerProgressBar(StaticData.TavernVisitWorkerID, TestInt);
            TestInt += TestStep;
            if (TestInt > 100)
            {
               that.Stop();
               StaticData.WorkerList.RemoveWorkerByID(StaticData.TavernVisitWorkerID);
            }
         }
      }
   }
}
