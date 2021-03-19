using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using ForgeOfBots.Utils;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ForgeOfBots.Forms.UserControls
{
   public partial class ProdListItem : UserControl
   {
      private CustomEvent _UpdateGUI;
      public event CustomEvent UpdateGUIEvent
      {
         add
         {
            if (_UpdateGUI == null || !_UpdateGUI.GetInvocationList().Contains(value))
               _UpdateGUI += value;
         }
         remove
         {
            _UpdateGUI -= value;
         }
      }
      public ProductionState ProductionState;
      private DateTime _endTime;
      private TimeSpan _duraction;
      private TimeSpan _diff
      {
         get
         {
            return TimeSpan.FromTicks(_endTime.Ticks).Add(new TimeSpan(0, 0, 2)) - TimeSpan.FromTicks(DateTime.Now.Ticks);
         }
      }
      private readonly System.Timers.Timer timer = new System.Timers.Timer();
      public List<int> EntityIDs = new List<int>();
      public bool isGoodBuilding { get; set; } = false;
      public IJavaScriptExecutor jsExecutor { get; set; }

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
         if (StaticData.UserData.ProductionBot)
         {
            switch (ProductionState)
            {
               case ProductionState.Idle:
                  StartProduction();
                  break;
               case ProductionState.Finished:
                  CollectProduction();
                  break;
               default:
                  break;
            }
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
            if (ProductionState != ProductionState.Idle && timer.Enabled)
            {
               timer.Enabled = false;
               timer.Stop();
               Debug.WriteLine($"[{DateTime.Now}] Production done");
               ProductionState = ProductionState.Finished;
               if (lblState.InvokeRequired)
                  Invoker.SetProperty(lblState, () => lblState.Text, i18n.getString("ProductionFinishedState"));
               else
                  lblState.Text = i18n.getString("ProductionFinishedState");
               if (ProductionState == ProductionState.Finished)
               {
                  _UpdateGUI?.Invoke(this);
               }
               return;
            }
            else if (ProductionState == ProductionState.Idle && timer.Enabled)
            {
               timer.Enabled = false;
               timer.Stop();
               Debug.WriteLine($"[{DateTime.Now}] Production idle");
               ProductionState = ProductionState.Idle;
               if (lblState.InvokeRequired)
                  Invoker.SetProperty(lblState, () => lblState.Text, i18n.getString("ProductionIdle"));
               else
                  lblState.Text = i18n.getString("ProductionIdle");
               if (ProductionState == ProductionState.Idle)
               {
                  _UpdateGUI?.Invoke(this);
               }
               return;
            }
            else if (!timer.Enabled)
               return;
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
                  Invoker.SetProperty(lblState, () => lblState.Text, i18n.getString("ReadyToCollect"));
               else if (ProductionState == ProductionState.Idle)
                  Invoker.SetProperty(lblState, () => lblState.Text, i18n.getString("ProductionIdle"));
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
                  lblState.Text = i18n.getString("ReadyToCollect");
               else if (ProductionState == ProductionState.Idle)
                  lblState.Text = i18n.getString("ProductionIdle");
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
      public void StartProduction()
      {
         if (EntityIDs.Count <= 0) return;
         foreach (int id in EntityIDs)
         {
            string script;
            if (isGoodBuilding)
               script = StaticData.ReqBuilder.GetRequestScript(DataHandler.RequestType.QueryProduction, new int[] { id, StaticData.UserData.GoodProductionOption.id });
            else
               script = StaticData.ReqBuilder.GetRequestScript(DataHandler.RequestType.QueryProduction, new int[] { id, StaticData.UserData.ProductionOption.id });
            string ret = (string)jsExecutor.ExecuteAsyncScript(script);
            try
            {
               JToken ColRes = JsonConvert.DeserializeObject<JToken>(ret);
               if (ColRes["responseData"]?["updatedEntities"]?.ToList().Count > 0 && ColRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "ProducingState")
               {
                  ProductionState = ProductionState.Producing;
                  StaticData.Updater.UpdateEntities();
               }
               else
               {
                  if (StaticData.DEBUGMODE)
                     Helper.Log($"[{DateTime.Now}] Failed to Start Production");
               }
               if (StaticData.DEBUGMODE) Helper.Log($"[{DateTime.Now}] CollectedIDs Count = {EntityIDs.Count}");
            }
            catch (Exception ex)
            {
               NLog.LogManager.Flush();
               var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
               var properties = new Dictionary<string, string> { { "CollectProduction", ret } };
               Crashes.TrackError(ex, properties, attachments);
            }
            Thread.Sleep(100);
         }
         UpdateGUI();
      }
      public void CollectProduction()
      {
         if (EntityIDs.Count <= 0) return;
         string script = StaticData.ReqBuilder.GetRequestScript(DataHandler.RequestType.CollectProduction, EntityIDs.ToArray());
         string ret = (string)jsExecutor.ExecuteAsyncScript(script);
         try
         {
            JToken ColRes = JsonConvert.DeserializeObject<JToken>(ret);
            if (ColRes["responseData"]?["updatedEntities"]?.ToList().Count > 0 && ColRes["responseData"]?["updatedEntities"]?[0]?["state"]?["__class__"]?.ToString() == "IdleState")
            {
               ProductionState = ProductionState.Idle;
               StaticData.Updater.UpdateEntities();
               UpdateGUI();
            }
            else
            {
               if (StaticData.DEBUGMODE)
                  Helper.Log($"[{DateTime.Now}] Failed to Start Production");
            }
            if (StaticData.DEBUGMODE) Helper.Log($"[{DateTime.Now}] CollectedIDs Count = {EntityIDs.Count}");
         }
         catch (Exception ex)
         {
            NLog.LogManager.Flush();
            var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
            var properties = new Dictionary<string, string> { { "CollectProduction", ret } };
            Crashes.TrackError(ex, properties, attachments);
         }
      }
   }
}
