using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.LanguageFiles;
using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ForgeOfBots.Forms.UserControls
{
   public partial class ProdListItem : UserControl
   {
      private CustomEvent _ProductionIdle;
      public event CustomEvent ProductionIdle
      {
         add
         {
            if (_ProductionIdle == null || !_ProductionIdle.GetInvocationList().Contains(value))
               _ProductionIdle += value;
         }
         remove
         {
            _ProductionIdle -= value;
         }
      }
      private CustomEvent _ProductionDone;
      public event CustomEvent ProductionDone
      {
         add
         {
            if (_ProductionDone == null || !_ProductionDone.GetInvocationList().Contains(value))
               _ProductionDone += value;
         }
         remove
         {
            _ProductionDone -= value;
         }
      }
      public ProductionState ProductionState;
      private TimeSpan _duraction;
      private DateTime _endTime;
      private TimeSpan _diff
      {
         get
         {
            return TimeSpan.FromTicks(_endTime.Ticks).Add(new TimeSpan(0, 0, 2)) - TimeSpan.FromTicks(DateTime.Now.Ticks);
         }
      }
      private readonly System.Timers.Timer timer = new System.Timers.Timer();
      public List<int> EntityIDs = new List<int>();
      public bool HasAllNeeded => (EntityIDs.Count > 0 && _duraction.TotalSeconds > 0 && _ProductionDone != null);
      public ProdListItem()
      {
         InitializeComponent();
      }
      public void FillControl(string building, string product, string state)
      {
         if (lblBuilding.InvokeRequired || lblProduct.InvokeRequired || lblState.InvokeRequired)
         {
            Invoker.SetProperty(lblBuilding, () => lblBuilding.Text, building);
            Invoker.SetProperty(lblProduct, () => lblProduct.Text, product);
            Invoker.SetProperty(lblState, () => lblState.Text, state);
         }
         else
         {
            lblBuilding.Text = building;
            lblProduct.Text = product;
            lblState.Text = state;
         }
      }
      public void StartProductionGUI()
      {
         if (ProductionState != ProductionState.Idle)
         {
            UpdateGUI();
         }
         timer.Enabled = true;
         timer.Elapsed += Timer_Elapsed;
         timer.Interval = 1000;
         timer.Start();
         Debug.WriteLine($"[{DateTime.Now}] StartProdGUI");
      }

      private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         if (_diff.TotalSeconds <= 0 || ProductionState == ProductionState.Finished || ProductionState == ProductionState.Idle)
         {
            timer.Stop();
            if (ProductionState != ProductionState.Idle)
            {
               Debug.WriteLine($"[{DateTime.Now}] Production done");
               ProductionState = ProductionState.Finished;
               _ProductionDone?.Invoke(null, EntityIDs);
               //ProductionState = ProductionState.Finished;
               return;
            }
            else if(ProductionState == ProductionState.Idle)
            {
               Debug.WriteLine($"[{DateTime.Now}] Production idle");
               ProductionState = ProductionState.Idle;
               _ProductionIdle?.Invoke(null, EntityIDs);
               //ProductionState = ProductionState.Producing;
               return;
            }
         }
         UpdateGUI();
      }

      public void AddTime(string dur, string end)
      {
         _duraction = TimeSpan.FromSeconds(double.Parse(dur));
         _endTime = Helper.UnixTimeStampToDateTime(double.Parse(end));
      }
      public void AddEntities(params int[] ids)
      {
         if (ids.Length > 0)
            EntityIDs = ids.ToList();
      }
      private void UpdateGUI(bool isFinished = false)
      {
         try
         {
            if (lblState.InvokeRequired)
            {
               if (isFinished)
                  Invoker.SetProperty(lblState, () => lblState.Text, strings.ReadyToCollect);
               else
               {
                  string days = "";
                  if (_diff.Days > 0)
                     days = $"{_diff.Days}d ";
                  Invoker.SetProperty(lblState, () => lblState.Text, $"{days}{_diff.Hours}h {_diff.Minutes}m {_diff.Seconds}s");
               }
            }
            else
            {
               if (isFinished || ProductionState == ProductionState.Finished)
                  lblState.Text = strings.ReadyToCollect;
               else
               {
                  string days = "";
                  if (_diff.Days > 0)
                     days = $"{_diff.Days}d ";
                  lblState.Text = $"{days}{_diff.Hours}h {_diff.Minutes}m {_diff.Seconds}s";
               }
            }
         }
         catch (Exception)
         { }
      }
   }
}
