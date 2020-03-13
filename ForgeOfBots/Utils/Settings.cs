using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.Utils
{
   public class Settings
   {
      public string Username { get; set; }
      public string Password { get; set; }
      public string LastWorld { get; set; }
      public List<string> PlayableWorlds { get; set; }
      public string WorldServer { get; set; }
      public string Language { get; set; } = "en";

      public void SaveSettings()
      {
         string json = JsonConvert.SerializeObject(this);
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         File.WriteAllText(DataPath, json);
         _SettingsSaved?.Invoke(null, new OneTArgs<Settings> { t1 = this });
      }

      public static Settings ReadSettings()
      {
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         string json = File.ReadAllText(DataPath);
         Settings s = JsonConvert.DeserializeObject<Settings>(json);
         _SettingsLoaded?.Invoke(null,new OneTArgs<Settings> { t1 = s});
         return s;
      }

      public static bool SettingsExists()
      {
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         return File.Exists(DataPath);
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
}
