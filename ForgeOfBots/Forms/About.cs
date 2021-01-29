using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Forms
{
   public partial class About : Form
   {
      public About()
      {
         InitializeComponent();
         lblTitle.Text += $"{StaticData.Version.Major}.{StaticData.Version.Minor}";
         lblVersion.Text = GetVersionNumber(typeof(frmMain), false);
         lblInternalVersion.Text = GetVersionNumber(typeof(frmMain), true);
      }
      public string GetVersionNumber(Type className, bool withInternal)
      {
         if (withInternal)
            return className.Assembly.GetName().Version.Major + "." + className.Assembly.GetName().Version.Minor + "." + className.Assembly.GetName().Version.Build + ((className.Assembly.GetName().Version.Revision) == 0 ? ".0" : "." + className.Assembly.GetName().Version.Revision.ToString());
         else
            return className.Assembly.GetName().Version.Major + "." + className.Assembly.GetName().Version.Minor + "." + className.Assembly.GetName().Version.Build;
      }

   }
}
