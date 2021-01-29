#if RELEASE || DEBUG
using CefSharp.OffScreen;
#elif DEBUGFORM
using CefSharp.WinForms;
#endif
using ForgeOfBots.DataHandler;
using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
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
      public static SettingData SettingData = new SettingData();
      public static Settings UserData;
      public static Assembly PremAssembly = null;
      public static Version Version = Assembly.GetExecutingAssembly().GetName().Version;
      public static UserDataInput usi = null;
      public static ImageList GoodImageList = null;
      public static ImageList Portraits = new ImageList();
      public static IJavaScriptExecutor jsExecutor;
      public static ICookieJar cookieJar;
      public static IWebDriver driver;
      public static string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      public static string ProgramPath = Path.Combine(AppDataPath, Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().ManifestModule.Name));
      public static Stopwatch RunningTime = new Stopwatch();
      public static bool DEBUGMODE = false;
      public static string ForgeHX_FilePath = "";
      public static object MainInstance = null;
      public static GuildExpedition GEX = null;
      public static WorkerList WorkerList = null;
      public static bool HasLastCrash = false;
      public static List<PropertyData> PCData = new List<PropertyData>();
      public static int TavernVisitWorkerID = 0;
      public static int PolivateWorkerID = 1;
      public static int LGSnipWorkerID = 2;

      public static string InnoCDN_Avatars = "https://foe###server###.innogamescdn.com/assets/shared/avatars/";

      public const string _WCS = "Win32_ComputerSystem";
      public const string _WCS_Model = "Model";
      public const string _WCS_SystemType = "SystemType";

      public const string _WOS = "Win32_OperatingSystem";
      public const string _WOS_Caption = "Caption";
   }
}
