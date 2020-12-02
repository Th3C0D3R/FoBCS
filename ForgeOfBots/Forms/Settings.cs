using ForgeOfBots.CefBrowserHandler;
using ForgeOfBots.GameClasses;
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
      public Form mainForm = null;
      readonly BackgroundWorker bw = new BackgroundWorker();
      bool Canceled = false;
      bool blockExpireBox = false;
      public Settings()
      {
         Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(StaticData.UserData.Language.Code);
         Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(StaticData.UserData.Language.Code);
         CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(StaticData.UserData.Language.Code);
         CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(StaticData.UserData.Language.Code);
         InitializeComponent();
         Control.ControlCollection mrb5s = mpProdCycle.Controls;
         foreach (Control control in mrb5s)
         {
            try
            {
               MetroRadioButton mrb = (MetroRadioButton)control;
               if (int.Parse(mrb.Tag.ToString()) == StaticData.UserData.ProductionOption.time)
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
               if (int.Parse(mrb.Tag.ToString()) == StaticData.UserData.GoodProductionOption.time)
               {
                  mrb.Checked = true;
                  break;
               }
            }
            catch (Exception)
            {
            }
         }
         mtView.Checked = StaticData.UserData.GroupedView;
         mtIncident.Checked = StaticData.UserData.IncidentBot;
         mtMoppel.Checked = StaticData.UserData.MoppelBot;
         mtRQBot.Checked = StaticData.UserData.RQBot;
         mtProduction.Checked = StaticData.UserData.ProductionBot;
         mtTavern.Checked = StaticData.UserData.TavernBot;
         mtBigRoads.Checked = StaticData.UserData.ShowBigRoads;
         mtbCustomUserAgent.Text = StaticData.UserData.CustomUserAgent;
         mtbSerialKey.Text = StaticData.UserData.SerialKey;
         mtAutoLogin.Checked = StaticData.UserData.AutoLogin;
         mtDarkMode.Checked = StaticData.UserData.DarkMode;

         if (StaticData.UserData.PlayableWorlds == null || StaticData.UserData.PlayableWorlds.Count == 0)
         {
            mcbCitySelection.Enabled = false;
            mbSaveReload.Enabled = false;
         }
         else
         {
            foreach (string item in StaticData.UserData.PlayableWorlds)
            {
               mcbCitySelection.Items.Add(new PlayAbleWorldItem() { WorldID = item, WorldName = item.ToUpper() });
            }
            mcbCitySelection.DisplayMember = "WorldName";
            mcbCitySelection.AutoCompleteSource = AutoCompleteSource.ListItems;
            mcbCitySelection.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            mcbCitySelection.SelectedIndex = StaticData.UserData.PlayableWorlds.FindIndex(e => e == StaticData.UserData.LastWorld);
         }
         foreach (Language item in ListClass.AvailableLanguages.Languages)
         {
            LanguageItem languageItem = new LanguageItem()
            {
               Code = item.code,
               Description = item.language,
               Language = item.index
            };
            mcbLanguage.Items.Add(languageItem);
         }
         mcbLanguage.DisplayMember = "Description";
         mcbLanguage.AutoCompleteSource = AutoCompleteSource.ListItems;
         mcbLanguage.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
         mcbLanguage.SelectedIndex = (int)StaticData.UserData.Language.Language;

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
         StaticData.UserData.ProductionOption = GetProductionOption(time);
         StaticData.UserData.SaveSettings();
      }
      private void mrbG4h_CheckedChanged(object sender, EventArgs e)
      {
         if (mainForm == null) return;
         MetroRadioButton mrb = (MetroRadioButton)sender;
         if (!mrb.Checked) return;
         int time = int.Parse(mrb.Tag.ToString());
         StaticData.UserData.ProductionOption = GetGoodProductionOption(time);
         StaticData.UserData.SaveSettings();
      }
      private void mtView_CheckedChanged(object sender, EventArgs e)
      {
         StaticData.UserData.GroupedView = mtView.Checked;
         StaticData.UserData.SaveSettings();
      }
      private void mtProduction_CheckedChanged(object sender, EventArgs e)
      {
         StaticData.UserData.ProductionBot = mtProduction.Checked;
         if (mainForm != null)
         {
            if (StaticData.UserData.DarkMode) ((MainDark)mainForm).UpdateBotView();
            else ((Main)mainForm).UpdateBotView();
         }
         StaticData.UserData.SaveSettings();
      }
      private void mtTavern_CheckedChanged(object sender, EventArgs e)
      {
         StaticData.UserData.TavernBot = mtTavern.Checked;
         if (mainForm != null)
         {
            if (StaticData.UserData.DarkMode) ((MainDark)mainForm).UpdateBotView();
            else ((Main)mainForm).UpdateBotView();
         }
         StaticData.UserData.SaveSettings();
      }
      private void mtMoppel_CheckedChanged(object sender, EventArgs e)
      {
         StaticData.UserData.MoppelBot = mtMoppel.Checked;
         if (mainForm != null)
         {
            if (StaticData.UserData.DarkMode) ((MainDark)mainForm).UpdateBotView();
            else ((Main)mainForm).UpdateBotView();
         }
         StaticData.UserData.SaveSettings();
      }
      private void mtIncident_CheckedChanged(object sender, EventArgs e)
      {
         StaticData.UserData.IncidentBot = mtIncident.Checked;
         if (mainForm != null)
         {
            if (StaticData.UserData.DarkMode) ((MainDark)mainForm).UpdateBotView();
            else ((Main)mainForm).UpdateBotView();
         }
         StaticData.UserData.SaveSettings();
      }
      private void mtRQBot_CheckedChanged(object sender, EventArgs e)
      {
         StaticData.UserData.RQBot = mtRQBot.Checked;
         if (mainForm != null)
         {
            if (StaticData.UserData.DarkMode) ((MainDark)mainForm).UpdateBotView();
            else ((Main)mainForm).UpdateBotView();
         }
         StaticData.UserData.SaveSettings();
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
         StaticData.UserData.ShowBigRoads = mtBigRoads.Checked;
         StaticData.UserData.SaveSettings();
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
         StaticData.UserData.CustomUserAgent = mtbCustomUserAgent.Text;
         StaticData.UserData.Language = (LanguageItem)mcbLanguage.SelectedItem;
         StaticData.UserData.SaveSettings();
      }
      private void mcbLanguage_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (StaticData.UserData.Language.Language != ((LanguageItem)mcbLanguage.SelectedItem).Language)
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
         StaticData.UserData.LastWorld = ((PlayAbleWorldItem)mcbCitySelection.SelectedItem).WorldID;
         StaticData.UserData.SaveSettings();
      }
      private void mbDeleteData_Click(object sender, EventArgs e)
      {
         StaticData.UserData.Delete();
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

               mainForm.Text = mainForm.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({strings.Premium}) | by TH3C0D3R";
               StaticData.UserData.SerialKey = mtbSerialKey.Text;
               StaticData.UserData.SaveSettings();
               object retList = Helper.ExecuteMethod(StaticData.PremAssembly, "EntryPoint", "AddPremiumControl", null);
               if (retList is List<UCPremium> list)
               {
                  if (StaticData.UserData.DarkMode)
                  {
                     ((MainDark)mainForm).tlpPremium.Controls.AddRange(list.ToArray());
                     ((MainDark)mainForm).tlpPremium.Invalidate(true);
                  }
                  else
                  {
                     ((Main)mainForm).tlpPremium.Controls.AddRange(list.ToArray());
                     ((Main)mainForm).tlpPremium.Invalidate(true);
                  }
               }
            }
            else if (result == Result.Expired)
            {
               Invoker.SetProperty(this, () => Text, Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} ({strings.Expired}) | by TH3C0D3R");
               if (!blockExpireBox)
                  MessageBox.Show(Owner, $"{strings.SubscriptionExpired}", $"{strings.SubExpiredTitle}");
               blockExpireBox = true;
            }
            else
            {
               mainForm.Text = mainForm.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
               MessageBox.Show(Owner, $"{strings.LicenceNotValid}", $"{strings.FailedToActivate}");
            }
         }
      }
      private void MtAutoLogin_CheckedChanged(object sender, EventArgs e)
      {
         StaticData.UserData.AutoLogin = mtAutoLogin.Checked;
         StaticData.UserData.SaveSettings();
      }
      private void MtDarkMode_CheckedChanged(object sender, EventArgs e)
      {
         if (mtDarkMode.Checked != StaticData.UserData.DarkMode)
         {
            lblRestartNeeded.Visible = true;
            lblRestartNeeded.CustomForeColor = true;
            lblRestartNeeded.ForeColor = Color.Red;
         }
         else lblRestartNeeded.Visible = false;
         StaticData.UserData.DarkMode = mtDarkMode.Checked;
         ToggleDarkMode(StaticData.UserData.DarkMode);
      }


      private void ToggleDarkMode(bool dark = false)
      {
         MetroFramework.MetroThemeStyle mode = dark ? MetroFramework.MetroThemeStyle.Dark : MetroFramework.MetroThemeStyle.Default;
         mtcSettings.UseStyleColors = dark;
         mtcSettings.Theme = mode;

         mtpProduction.Theme = mode;
         mtpMisc.Theme = mode;
         mtpData.Theme = mode;
         mtpManually.Theme = mode;
         mtpPremium.Theme = mode;
         mtpBots.Theme = mode;

         mpProdCycle.Theme = mode;
         mpGoodCycle.Theme = mode;

         mtbSave.UseVisualStyleBackColor = !dark;
         mtbSave.Theme = mode;
         mtbCustomUserAgent.Theme = mode;
         mtbCustomUserAgent.UseStyleColors = dark;
         mtbSerialKey.Theme = mode;
         mtbSerialKey.UseStyleColors = dark;
         mbCheckSerial.UseVisualStyleBackColor = !dark;
         mbCheckSerial.Theme = mode;
         mbSaveReload.UseVisualStyleBackColor = !dark;
         mbSaveReload.Theme = mode;
         mbDeleteData.UseVisualStyleBackColor = !dark;
         mbDeleteData.Theme = mode;

         mcbLanguage.Theme = mode;
         mcbCitySelection.Theme = mode;

         metroLabel5.UseStyleColors = dark;
         metroLabel5.Theme = mode;
         metroLabel3.UseStyleColors = dark;
         metroLabel3.Theme = mode;
         metroLabel2.UseStyleColors = dark;
         metroLabel2.Theme = mode;
         metroLabel1.UseStyleColors = dark;
         metroLabel1.Theme = mode;
         metroLabel12.UseStyleColors = dark;
         metroLabel12.Theme = mode;
         lblAutoLogin.UseStyleColors = dark;
         lblAutoLogin.Theme = mode;
         lblRestartNeeded.UseStyleColors = dark;
         lblRestartNeeded.Theme = mode;
         lblRestartNeeded.UseStyleColors = dark;
         lblRestartNeeded.Theme = mode;
         lblCustomUserAgent.UseStyleColors = dark;
         lblCustomUserAgent.Theme = mode;
         lblLanguage.UseStyleColors = dark;
         lblLanguage.Theme = mode;
         metroLabel10.UseStyleColors = dark;
         metroLabel10.Theme = mode;
         metroLabel9.UseStyleColors = dark;
         metroLabel9.Theme = mode;
         metroLabel8.UseStyleColors = dark;
         metroLabel8.Theme = mode;
         metroLabel7.UseStyleColors = dark;
         metroLabel7.Theme = mode;
         metroLabel6.UseStyleColors = dark;
         metroLabel6.Theme = mode;
         metroLabel4.UseStyleColors = dark;
         metroLabel4.Theme = mode;
         lblSerialKey.UseStyleColors = dark;
         lblSerialKey.Theme = mode;
         metroLabel11.UseStyleColors = dark;
         metroLabel11.Theme = mode;
         lblChooseWorld.UseStyleColors = dark;
         lblChooseWorld.Theme = mode;
         lblDeleteData.UseStyleColors = dark;
         lblDeleteData.Theme = mode;



         mrbG1d.Theme = mode;
         mrbG1d.UseVisualStyleBackColor = !dark;
         mrbG1d.UseStyleColors = dark;
         mrbG2d.Theme = mode;
         mrbG2d.UseVisualStyleBackColor = !dark;
         mrbG2d.UseStyleColors = dark;
         mrbG4h.Theme = mode;
         mrbG4h.UseVisualStyleBackColor = !dark;
         mrbG4h.UseStyleColors = dark;
         mrbG8h.Theme = mode;
         mrbG8h.UseVisualStyleBackColor = !dark;
         mrbG8h.UseStyleColors = dark;

         mrb5.Theme = mode;
         mrb5.UseVisualStyleBackColor = !dark;
         mrb5.UseStyleColors = dark;
         mrb15.Theme = mode;
         mrb15.UseVisualStyleBackColor = !dark;
         mrb15.UseStyleColors = dark;
         mrb1.Theme = mode;
         mrb1.UseVisualStyleBackColor = !dark;
         mrb1.UseStyleColors = dark;
         mrb4.Theme = mode;
         mrb4.UseVisualStyleBackColor = !dark;
         mrb4.UseStyleColors = dark;
         mrb8.Theme = mode;
         mrb8.UseVisualStyleBackColor = !dark;
         mrb8.UseStyleColors = dark;
         mrb1d.Theme = mode;
         mrb1d.UseVisualStyleBackColor = !dark;
         mrb1d.UseStyleColors = dark;

         mtView.Theme = mode;
         mtView.UseVisualStyleBackColor = !dark;
         mtView.UseStyleColors = dark;
         mtDarkMode.Theme = mode;
         mtDarkMode.UseVisualStyleBackColor = !dark;
         mtDarkMode.UseStyleColors = dark;
         mtAutoLogin.Theme = mode;
         mtAutoLogin.UseVisualStyleBackColor = !dark;
         mtAutoLogin.UseStyleColors = dark;
         mtRQBot.Theme = mode;
         mtRQBot.UseVisualStyleBackColor = !dark;
         mtRQBot.UseStyleColors = dark;
         mtIncident.Theme = mode;
         mtIncident.UseVisualStyleBackColor = !dark;
         mtIncident.UseStyleColors = dark;
         mtMoppel.Theme = mode;
         mtMoppel.UseVisualStyleBackColor = !dark;
         mtMoppel.UseStyleColors = dark;
         mtTavern.Theme = mode;
         mtTavern.UseVisualStyleBackColor = !dark;
         mtTavern.UseStyleColors = dark;
         mtBigRoads.Theme = mode;
         mtBigRoads.UseVisualStyleBackColor = !dark;
         mtBigRoads.UseStyleColors = dark;
         Refresh();
      }

      private void MtbIntervalIncident_TextChanged(object sender, EventArgs e)
      {
         if(int.TryParse(mtbIntervalIncident.Text,out int interval))
            StaticData.UserData.IntervalIncidentBot = interval;
         else
            StaticData.UserData.IntervalIncidentBot = 1;
         StaticData.UserData.SaveSettings();
      }
   }
}
