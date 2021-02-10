using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.Utils
{
   public class Settings
   {
      public Version BotVersion { get; set; } = StaticData.Version;
      public string Username { get; set; } = "";
      public string Password { get; set; } = "";
      public string LastWorld { get; set; } = "";
      public List<string> PlayableWorlds { get; set; } = new List<string>();
      public string WorldServer { get; set; } = "";
      public LanguageItem Language { get; set; } = new LanguageItem() { Code = "en", Description = "English", Language = 0 };
      public string SerialKey { get; set; } = "";
      public ProductionOption ProductionOption { get; set; } = Extensions.GetProductionOption();
      public ProductionOption GoodProductionOption { get; set; } = Extensions.GetGoodProductionOption();
      public bool GroupedView { get; set; } = true;
      public bool ShowBigRoads { get; set; } = true;
      public bool ProductionBot { get; set; } = false;
      public bool MoppelBot { get; set; } = false;
      public bool TavernBot { get; set; } = false;
      public bool IncidentBot { get; set; } = false;
      public bool RQBot { get; set; } = false;
      public bool SnipBot { get; set; } = false;
      public string CustomUserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0";
      public bool AutoLogin { get; set; } = false;
      public bool HideBrowser { get; set; } = true;
      public bool DarkMode { get; set; } = false;
      public int MinProfit { get; set; } = 20;
      public int IntervalSnip { get; set; } = 60;
      public bool AutoInvest { get; set; } = false;
      public bool ShowWarning { get; set; } = true;
      public SnipTarget SelectedSnipTarget { get; set; } = SnipTarget.neighbors | SnipTarget.friends;
      public UserConfirmation AllowSendCrashLog { get; set; } = UserConfirmation.Send;
      public int IntervalIncidentBot { get; set; } = 1;
      public DateTime LastPolivateTime { get; set; } = DateTime.MinValue;
      public DateTime LastIncidentTime { get; set; } = DateTime.MinValue;
      public DateTime LastSnipTime { get; set; } = DateTime.MinValue;
      public List<int> IgnoredPlayers { get; set; } = new List<int>();

      #region "Methodes no Changes needed"
      public void SaveSettings()
      {
         string json = JsonConvert.SerializeObject(this, Formatting.Indented);
         string DataPath = Path.Combine(StaticData.ProgramPath, "userdata.json");
         Directory.CreateDirectory(StaticData.ProgramPath);
         File.WriteAllText(DataPath, json);
         _SettingsSaved?.Invoke(null, new OneTArgs<Settings> { t1 = this });
      }
      public static Settings ReadSettings()
      {
         string DataPath = Path.Combine(StaticData.ProgramPath, "userdata.json");
         string json = File.ReadAllText(DataPath);
         Settings s = JsonConvert.DeserializeObject<Settings>(json);
         _SettingsLoaded?.Invoke(null, new OneTArgs<Settings> { t1 = s });
         return s;
      }
      public static bool SettingsExists()
      {
         string DataPath = Path.Combine(StaticData.ProgramPath, "userdata.json");
         return File.Exists(DataPath);
      }
      public void Delete()
      {
         string DataPath = Path.Combine(StaticData.ProgramPath, "userdata.json");
         if (File.Exists(DataPath))
            File.Delete(DataPath);
      }
      public void Delete(bool deletewholeDirectory)
      {
         if (Directory.Exists(StaticData.ProgramPath))
         {
            Directory.Delete(StaticData.ProgramPath);
         }
      }
      private static EventHandler<OneTArgs<Settings>> _SettingsLoaded;
      public static event EventHandler<OneTArgs<Settings>> SettingsLoaded
      {
         add
         {
            if (_SettingsLoaded == null || !_SettingsLoaded.GetInvocationList().ToList().Contains(value))
               _SettingsLoaded += value;
         }
         remove
         {
            _SettingsLoaded -= value;
         }
      }
      private EventHandler<OneTArgs<Settings>> _SettingsSaved;
      public event EventHandler<OneTArgs<Settings>> SettingsSaved
      {
         add
         {
            if (_SettingsSaved == null || !_SettingsSaved.GetInvocationList().ToList().Contains(value))
               _SettingsSaved += value;
         }
         remove
         {
            _SettingsSaved -= value;
         }
      }
      #endregion
   }
}
