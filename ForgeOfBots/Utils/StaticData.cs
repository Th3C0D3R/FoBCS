using CefSharp.OffScreen;
using ForgeOfBots.DataHandler;
using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using CUpdate = ForgeOfBots.DataHandler.Update;

namespace ForgeOfBots.Utils
{
   public static class StaticData
   {
      public static ChromiumWebBrowser cwb = null;
      public static ResourceManager resMgr = new ResourceManager("ForgeOfBots.Properties.Resources", Assembly.GetExecutingAssembly());
      public static Dictionary<string, string> AllCookies = new Dictionary<string, string>();
      public static RequestBuilder ReqBuilder = new RequestBuilder();
      public static CUpdate Updater;
      public static BotData BotData = new BotData();
      public static Log LogWnd;
      public static SettingData SettingData = new SettingData();
      public static Settings UserData;
      public static Assembly PremAssembly = null;
      public static Version Version = Assembly.GetExecutingAssembly().GetName().Version;
      public static UserDataInput usi = null;
      public static Browser Browser = null;
      public static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      public static string ProgramPath = Path.Combine(AppDataPath, Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().ManifestModule.Name));
      public static Stopwatch RunningTime = new Stopwatch();
      public static bool DEBUGMODE = false;
      public static string ForgeHX_FilePath = "";
      public static object MainInstance = null;
      public static GuildExpedition GEX = null;
      public static bool HasLastCrash = false;
   }
}
