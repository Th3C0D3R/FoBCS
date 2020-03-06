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
      public string PlayableWorlds { get; set; }
      public string WorldServer { get; set; }
      public string Language { get; set; } = "en";

      public void SaveSettings()
      {
         string json = JsonConvert.SerializeObject(this);
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         File.WriteAllText(DataPath, json);
         SettingsSaved?.Invoke(this);
      }

      public static Settings ReadSettings()
      {
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         string json = File.ReadAllText(DataPath);
         Settings s = JsonConvert.DeserializeObject<Settings>(json);
         SettingsLoaded?.Invoke(s);
         return s;
      }

      public static bool SettingsExists()
      {
         string DataPath = Path.Combine(Main.ProgramPath, "userdata.json");
         return File.Exists(DataPath);
      }

      public static event SettingsSavedEvent SettingsSaved;
      public static event SettingsLoadedEvent SettingsLoaded;
   }

   public delegate void SettingsSavedEvent(Settings e);
   public delegate void SettingsLoadedEvent(Settings e);
}
