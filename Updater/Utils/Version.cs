using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Updater.Utils
{
   public class Version
   {
      public string version { get; set; }
      public string changelog { get; set; }
      public static bool IsUpdateAvailable(Version v)
      {
         System.Version current = AssemblyName.GetAssemblyName("ForgeOfBots.exe").Version;
         string[] versionSplit = v.version.Split('.');
         return (int.Parse(versionSplit[0]) > current.Major || (int.Parse(versionSplit[1]) > current.Minor && int.Parse(versionSplit[0]) == current.Major) || (int.Parse(versionSplit[2]) > current.Build && int.Parse(versionSplit[1]) == current.Minor && int.Parse(versionSplit[0]) == current.Major));
      }
      public static void Init()
      {
         try
         {
#if RELEASE
            string URLLang = $"https://raw.githubusercontent.com/Th3C0D3R/FoBCS/master/ForgeOfBots/version.json";
            using (var webClient = new WebClient())
            {
               int codePage = Encoding.Default.CodePage;
               webClient.Encoding = Encoding.GetEncoding(codePage);
               string resultStrings = webClient.DownloadString(URLLang);
               Helper.ReleaseVersion = JsonConvert.DeserializeObject<Version>(resultStrings);
            }
#elif DEBUG
            string versionFile = File.ReadAllText("version.json");
            Helper.ReleaseVersion = JsonConvert.DeserializeObject<Version>(versionFile);
#endif
         }
         catch (WebException)
         {
            Helper.ReleaseVersion = new Version() { version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3) };
         }
      }
   }

}
