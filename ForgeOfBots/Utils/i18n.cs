using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
      public static dynamic HelpObject = null;
      public static Form MainForm = null;
      public static void Initialize(string language, Form main)
      {
         MainForm = main;
         if (initialized) return;
         try
         {
            string URLLang = $"https://raw.githubusercontent.com/Th3C0D3R/FoBCS/master/ForgeOfBots/LanguageFiles/{language}.json";
            string URLLangHelp = $"https://raw.githubusercontent.com/Th3C0D3R/FoBCS/master/ForgeOfBots/LanguageFiles/help/help_{language}.json";
            using (var webClient = new WebClient())
            {
               int codePage = 1252;
               if(language == "fr")
               {
                  codePage = 28591;
               }
               webClient.Encoding = Encoding.GetEncoding(codePage);
               string resultStrings = webClient.DownloadString(URLLang);
               jsonObject = JsonConvert.DeserializeObject(resultStrings);

               webClient.Encoding = Encoding.GetEncoding(codePage);
               string resultHelp = webClient.DownloadString(URLLangHelp);
               HelpObject = JsonConvert.DeserializeObject(resultHelp);
               initialized = true;
            }
         }
         catch (WebException)
         {
            MessageBox.Show($"LANGUAGE {language} NOT FOUND IN REPOSITORY!\n\nUSING DEFAULT 'en'","LANGUAGE NOT FOUND", MessageBoxButtons.OK, MessageBoxIcon.Error);
            var assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
               if (resourceName.EndsWith($"{language}.json"))
               {
                  using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                  using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
                  {
                     string jsonString = reader.ReadToEnd();
                     if (resourceName.EndsWith($"help_{language}.json"))
                     {
                        HelpObject = JsonConvert.DeserializeObject(jsonString);
                     }
                     else
                     {
                        jsonObject = JsonConvert.DeserializeObject(jsonString);
                     }
                  }
               }
            }
            if (jsonObject != null && HelpObject != null) initialized = true;
         }
      }
      public static string getString(string key, params KeyValuePair<string,string>[] param)
      {
         try
         {
            string s = jsonObject["items"][key].ToString();
            foreach (KeyValuePair<string,string> item in param)
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
            if (item.Controls.Count > 0)
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
      public static void TranslateHelp(TreeView tv)
      {
         tv.Nodes.Clear();
         try
         {
            tv.Nodes.Add(HelpObject["Root"]["Title"].ToString());
            TreeNode root = tv.Nodes[0];
            root.Tag = HelpObject["Root"]["Text"].ToString();
            foreach (dynamic c in HelpObject["Root"]["Children"])
            {
               TreeNode tn = new TreeNode(c["Title"].ToString())
               {
                  Tag = c["Text"].ToString()
               };
               if (c["HasChildren"].ToString().ToLower() == "true")
                  tn.Nodes.AddRange(GetChildNode(c["Children"]).ToArray());
               root.Nodes.Add(tn);
            }
         }
         catch (Exception)
         {
            tv.Nodes.Add("FAILED TO PARSE HELP-FILE");
         }
      }
      private static List<TreeNode> GetChildNode(dynamic c)
      {
         List<TreeNode> treeNodeChildren = new List<TreeNode>();
         foreach (var item in c)
         {
            TreeNode tn = new TreeNode(item["Title"].ToString())
            {
               Tag = item["Text"].ToString()
            };
            if (item["HasChildren"].ToString().ToLower() == "true")
               treeNodeChildren.AddRange(GetChildNode(item["Children"]));
            else
               treeNodeChildren.Add(tn);
         }
         return treeNodeChildren;
      }
   }
}
