using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Newtonsoft;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;

namespace Updater.Utils
{
   public static class i18n
   {
      public static bool initialized = false;
      public static dynamic jsonObject = null;
      public static void Initialize(string language)
      {
         if (initialized) return;
         try
         {
            string URLLang = $"https://raw.githubusercontent.com/Th3C0D3R/FoBCS/master/ForgeOfBots/LanguageFiles/{language}.json";
            using (var webClient = new WebClient())
            {
               int codePage = Encoding.Default.CodePage;
               if (language == "fr")
               {
                  codePage = 28591;
               }
               webClient.Encoding = Encoding.GetEncoding(codePage);
               string resultStrings = webClient.DownloadString(URLLang);
               if(language == "de")
               {
                  byte[] bytes = Encoding.GetEncoding(0).GetBytes(resultStrings);
                  resultStrings = Encoding.UTF8.GetString(bytes);
               }
               jsonObject = JsonConvert.DeserializeObject(resultStrings);
               initialized = true;
            }
         }
         catch (WebException)
         {
            MessageBox.Show($"LANGUAGE {language} NOT FOUND IN REPOSITORY!\n\nUSING DEFAULT 'en'", "LANGUAGE NOT FOUND", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
            var assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
               if (resourceName.EndsWith($"{language}.json"))
               {
                  using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                  using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
                  {
                     string jsonString = reader.ReadToEnd();
                     jsonObject = JsonConvert.DeserializeObject(jsonString);
                  }
               }
            }
            if (jsonObject != null) initialized = true;
         }
      }
      public static string getString(string key, params KeyValuePair<string, string>[] param)
      {
         try
         {
            var Keys = key.Split('.');
            dynamic currentObject = jsonObject["items"];
            for (int i = 0; i < Keys.Length; i++)
            {
               if (currentObject[Keys[i]] != null)
                  currentObject = currentObject[Keys[i]];
            }
            string s = currentObject.ToString();
            foreach (KeyValuePair<string, string> item in param)
            {
               s = s.Replace(item.Key, item.Value);
            }
            return s;
         }
         catch (Exception)
         {
            return $"{key} not defined";
         }
      }
   }
}
