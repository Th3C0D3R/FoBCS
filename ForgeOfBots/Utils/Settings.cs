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
      public string Username { get; set; } = "";
      public string Password { get; set; } = "";
      public string LastWorld { get; set; } = "";
      public List<string> PlayableWorlds { get; set; } = new List<string>();
      public string WorldServer { get; set; } = "";
      public LanguageItem Language { get; set; } = new LanguageItem() { Code = "en",Description="English",Language=0};
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
      public string CustomUserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0";
      public bool AutoLogin { get; set; } = false;

      public void SaveSettings()
      {
         string json = JsonConvert.SerializeObject(this);
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         Directory.CreateDirectory(Main.ProgramPath);
         File.WriteAllText(DataPath, json);
         _SettingsSaved?.Invoke(null, new OneTArgs<Settings> { t1 = this });
      }
      public static Settings ReadSettings()
      {
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         string json = File.ReadAllText(DataPath);
         Settings s = JsonConvert.DeserializeObject<Settings>(json);
         _SettingsLoaded?.Invoke(null, new OneTArgs<Settings> { t1 = s });
         return s;
      }
      public static bool SettingsExists()
      {
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         return File.Exists(DataPath);
      }
      public void Delete()
      {
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         if (File.Exists(DataPath))
            File.Delete(DataPath);
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

   }
   public enum Languages
   {
      [Description("English|en|0")]
      English,
      [Description("German|de|1")]
      German
   }

}
