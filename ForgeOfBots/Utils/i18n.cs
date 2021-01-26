using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ForgeOfBots.Forms;
using Newtonsoft;
using Newtonsoft.Json;

namespace ForgeOfBots.Utils
{
   public static class i18n
   {
      public static bool initialized = false;
      public static dynamic jsonObject = null;
      public static Form MainForm = null;
      public static void Initialize(string language, Form main)
      {
         MainForm = main;
         try
         {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith($"{language}.json"));
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
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
            return $"{key} not defined";
         }
      }
      public static void TranslateForm()
      {
         TranslateControl(MainForm.Controls);
      }
      private static void TranslateControl(Control.ControlCollection control)
      {
         foreach (Control item in control)
         {
            if (item.Tag != null)
            {
               if (item.Tag.ToString().StartsWith("GUI."))
               {
                  item.Text = getString(item.Tag.ToString());
               }
            }
            if(item.Controls.Count > 0)
            {
               TranslateControl(item.Controls);
            }
         }
      }
      public static void TranslateCMS(ContextMenuStrip cms)
      {
         foreach (ToolStripItem item in cms.Items)
         {
            if (item.Tag != null)
            {
               if (item.Tag.ToString().StartsWith("GUI."))
               {
                  item.Text = getString(item.Tag.ToString());
               }
            }
         }
      }
   }
}
