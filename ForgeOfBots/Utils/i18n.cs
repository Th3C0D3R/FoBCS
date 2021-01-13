using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;
using Newtonsoft.Json;

namespace ForgeOfBots.Utils
{
   public static class i18n
   {
      public static bool initialized = false;
      public static dynamic jsonObject = null;
      public static void Initialize(string language)
      {
         try
         {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($"{language}.json"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
               string jsonString = reader.ReadToEnd();
               jsonObject = JsonConvert.DeserializeObject(jsonString);
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message,"EXCEPTION",MessageBoxButtons.OK,MessageBoxIcon.Error);
            Environment.Exit(1);
         }
      }
      public static string getString(string key)
      {
         try
         {
            return jsonObject["items"][key].ToString();
         }
         catch (Exception)
         {
            return $"{key} is not defined";
         }
      }
   }
}
