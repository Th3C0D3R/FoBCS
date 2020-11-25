using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.LanguageFiles;
using ForgeOfBots.Utils;
using ForgeOfBots.Utils.Premium;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UCPremiumLibrary;
using static ForgeOfBots.Utils.Extensions;
using static ForgeOfBots.Utils.Helper;

namespace ForgeOfBots.Forms
{
   public partial class Settings : Form
   {
      public Main mainForm = null;
      readonly BackgroundWorker bw = new BackgroundWorker();
      bool Canceled = false;
      bool blockExpireBox = false;
      public Settings()
      {
         Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Main.UserData.Language.Code);
         Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Main.UserData.Language.Code);
         CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(Main.UserData.Language.Code);
         CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(Main.UserData.Language.Code);
         InitializeComponent();
         Control.ControlCollection mrb5s = mpProdCycle.Controls;
         foreach (Control control in mrb5s)
         {
            try
            {
               MetroRadioButton mrb = (MetroRadioButton)control;
               if (int.Parse(mrb.Tag.ToString()) == Main.UserData.ProductionOption.time)
               {
                  mrb.Checked = true;
                  break;
               }
            }
            catch (Exception)
            {
            }

         }
         Control.ControlCollection mrbG4h = mpGoodCycle.Controls;
         foreach (Control control in mrbG4h)
         {
            try
            {
               MetroRadioButton mrb = (MetroRadioButton)control;
               if (int.Parse(mrb.Tag.ToString()) == Main.UserData.GoodProductionOption.time)
               {
                  mrb.Checked = true;
                  break;
               }
            }
            catch (Exception)
            {
            }
         }
         mtView.Checked = Main.UserData.GroupedView;
         mtIncident.Checked = Main.UserData.IncidentBot;
         mtMoppel.Checked = Main.UserData.MoppelBot;
         mtRQBot.Checked = Main.UserData.RQBot;
         mtProduction.Checked = Main.UserData.ProductionBot;
         mtTavern.Checked = Main.UserData.TavernBot;
         mtBigRoads.Checked = Main.UserData.ShowBigRoads;
         mtbCustomUserAgent.Text = Main.UserData.CustomUserAgent;
         mtbSerialKey.Text = Main.UserData.SerialKey;
         mtAutoLogin.Checked = Main.UserData.AutoLogin;

         if (Main.UserData.PlayableWorlds == null || Main.UserData.PlayableWorlds.Count == 0)
         {
            mcbCitySelection.Enabled = false;
            mbSaveReload.Enabled = false;
         }
         else
         {
            foreach (string item in Main.UserData.PlayableWorlds)
            {
               mcbCitySelection.Items.Add(new PlayAbleWorldItem() { WorldID = item, WorldName = item.ToUpper() });
            }
            mcbCitySelection.DisplayMember = "WorldName";
            mcbCitySelection.AutoCompleteSource = AutoCompleteSource.ListItems;
            mcbCitySelection.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            mcbCitySelection.SelectedIndex = Main.UserData.PlayableWorlds.FindIndex(e => e == Main.UserData.LastWorld);
         }
         foreach (string item in GetDescriptions(typeof(Languages)).ToList())
         {
            string[] code = item.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            LanguageItem languageItem = new LanguageItem()
            {
               Code = code[1],
               Description = code[0],
               Language = (Languages)Enum.Parse(typeof(Languages), code[2])
            };
            mcbLanguage.Items.Add(languageItem);
         }
         mcbLanguage.DisplayMember = "Description";
         mcbLanguage.AutoCompleteSource = AutoCompleteSource.ListItems;
         mcbLanguage.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
         mcbLanguage.SelectedIndex = (int)Main.UserData.Language.Language;

         bw.DoWork += Bw_DoWork;
         bw.WorkerSupportsCancellation = true;
         bw.RunWorkerAsync();
      }

      private void Bw_DoWork(object sender, DoWorkEventArgs e)
      {
         while (!bw.CancellationPending)
         {
            foreach (MetroFramework.MetroColorStyle foo in Enum.GetValues(typeof(MetroFramework.MetroColorStyle)))
            {
               if (foo == MetroFramework.MetroColorStyle.White ||
                  foo == MetroFramework.MetroColorStyle.Black ||
                  foo == MetroFramework.MetroColorStyle.Default ||
                  foo == MetroFramework.MetroColorStyle.Silver) continue;
               Invoker.SetProperty(mtcSettings, () => mtcSettings.Style, foo);
               Thread.Sleep(150);
            }
         }
         Canceled = true;
      }
      private void mrb5_CheckedChanged(object sender, EventArgs e)
      {
         if (mainForm == null) return;
         MetroRadioButton mrb = (MetroRadioButton)sender;
         if (!mrb.Checked) return;
         int time = int.Parse(mrb.Tag.ToString());
         Main.UserData.ProductionOption = GetProductionOption(time);
         Main.UserData.SaveSettings();
      }
      private void mrbG4h_CheckedChanged(object sender, EventArgs e)
      {
         if (mainForm == null) return;
         MetroRadioButton mrb = (MetroRadioButton)sender;
         if (!mrb.Checked) return;
         int time = int.Parse(mrb.Tag.ToString());
         Main.UserData.ProductionOption = GetGoodProductionOption(time);
         Main.UserData.SaveSettings();
      }
      private void mtView_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.GroupedView = mtView.Checked;
         Main.UserData.SaveSettings();
      }
      private void mtProduction_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.ProductionBot = mtProduction.Checked;
         if (mainForm != null)
            mainForm.UpdateBotView();
         Main.UserData.SaveSettings();
      }
      private void mtTavern_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.TavernBot = mtTavern.Checked;
         if (mainForm != null)
            mainForm.UpdateBotView();
         Main.UserData.SaveSettings();
      }
      private void mtMoppel_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.MoppelBot = mtMoppel.Checked;
         if (mainForm != null)
            mainForm.UpdateBotView();
         Main.UserData.SaveSettings();
      }
      private void mtIncident_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.IncidentBot = mtIncident.Checked;
         if (mainForm != null)
            mainForm.UpdateBotView();
         Main.UserData.SaveSettings();
      }
      private void mtRQBot_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.RQBot = mtRQBot.Checked;
         if (mainForm != null)
            mainForm.UpdateBotView();
         Main.UserData.SaveSettings();
      }
      private CustomEvent _GroupedChanged;
      public event CustomEvent GroupedChanged
      {
         add
         {
            if (_GroupedChanged == null || !_GroupedChanged.GetInvocationList().Contains(value))
               _GroupedChanged += value;
         }
         remove
         {
            _GroupedChanged -= value;
         }
      }
      private void metroToggle1_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.ShowBigRoads = mtBigRoads.Checked;
         Main.UserData.SaveSettings();
      }
      private CustomEvent _ShowBigRoadsChanged;
      public event CustomEvent ShowBigRoadsChanged
      {
         add
         {
            if (_ShowBigRoadsChanged == null || !_ShowBigRoadsChanged.GetInvocationList().Contains(value))
               _ShowBigRoadsChanged += value;
         }
         remove
         {
            _ShowBigRoadsChanged -= value;
         }
      }
      private void Settings_FormClosing(object sender, FormClosingEventArgs e)
      {
         bw.CancelAsync();
         while (!Canceled)
         {
            Application.DoEvents();
         };
      }
      private void mtbSave_Click(object sender, EventArgs e)
      {
         Main.UserData.CustomUserAgent = mtbCustomUserAgent.Text;
         Main.UserData.Language = (LanguageItem)mcbLanguage.SelectedItem;
         Main.UserData.SaveSettings();
      }
      private void mcbLanguage_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (Main.UserData.Language.Language != ((LanguageItem)mcbLanguage.SelectedItem).Language)
         {
            lblRestartNeeded.Visible = true;
            lblRestartNeeded.CustomForeColor = true;
            lblRestartNeeded.ForeColor = Color.Red;
         }
         else
            lblRestartNeeded.Visible = false;
      }
      private void mbSaveReload_Click(object sender, EventArgs e)
      {
         Main.UserData.LastWorld = ((PlayAbleWorldItem)mcbCitySelection.SelectedItem).WorldID;
         Main.UserData.SaveSettings();
      }
      private void mbDeleteData_Click(object sender, EventArgs e)
      {
         Main.UserData.Delete();
         Process.Start(Application.ExecutablePath);
         Environment.Exit(0);
      }
      private void mbCheckSerial_Click(object sender, EventArgs e)
      {
         object ret = TcpConnection.SendAuthData(mtbSerialKey.Text, FingerPrint.Value(), false);
         if (ret is Result)
         {
            Enum.TryParse(ret.ToString(), out Result result);
            if (result == Result.Success)
            {

               mainForm.Text = mainForm.Tag.ToString() + $"{Main.Version.Major}.{Main.Version.Minor} ({strings.Premium}) | by TH3C0D3R";
               Main.UserData.SerialKey = mtbSerialKey.Text;
               Main.UserData.SaveSettings();
               object retList = Helper.ExecuteMethod(Main.PremAssembly, "EntryPoint", "AddPremiumControl", null);
               if (retList is List<UCPremium> list)
               {
                  mainForm.tlpPremium.Controls.AddRange(list.ToArray());
                  mainForm.tlpPremium.Invalidate(true);
               }
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(this, () => Text, Tag.ToString() + $"{Main.Version.Major}.{Main.Version.Minor} ({strings.Expired}) | by TH3C0D3R");
               if (!blockExpireBox)
                  MessageBox.Show(Owner, $"{strings.SubscriptionExpired}", $"{strings.SubExpiredTitle}");
               blockExpireBox = true;
            }
            else
            {
               mainForm.Text = mainForm.Tag.ToString() + $"{Main.Version.Major}.{Main.Version.Minor} | by TH3C0D3R";
               MessageBox.Show(Owner, $"{strings.LicenceNotValid}", $"{strings.FailedToActivate}");
            }
         }
      }
      private void MtAutoLogin_CheckedChanged(object sender, EventArgs e)
      {
         Main.UserData.AutoLogin = mtAutoLogin.Checked;
         Main.UserData.SaveSettings();
      }
   }
}
