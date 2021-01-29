using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Forms
{
   public partial class Loading : Form
   {
      public Loading()
      {
         InitializeComponent();
         mlVersion.Text = mlVersion.Tag.ToString() + $"{StaticData.Version.Major}.{StaticData.Version.Minor} | by TH3C0D3R";
         if (!i18n.initialized)
         {
            if (Utils.Settings.SettingsExists())
            {
               Utils.Settings temp = Utils.Settings.ReadSettings();
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(temp.Language.Code);
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(temp.Language.Code);
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(temp.Language.Code);
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(temp.Language.Code);
               i18n.Initialize(temp.Language.Code,this);
            }
            else
            {
               Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
               Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en");
               CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en");
               i18n.Initialize("en",this);
            }
         }
         i18n.TranslateForm();
         //TopMost = true;
      }
   }
}
