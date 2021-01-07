using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.OffScreen;

namespace ForgeOfBots.Forms
{
   public partial class Browser : Form
   {

      public ChromiumWebBrowser cwb;
      public Form Main;
      public Browser(Form _main)
      {
         Main = _main;
         InitializeComponent();
      }

      private void Browser_FormClosing(object sender, FormClosingEventArgs e)
      {
         Cef.Shutdown();
      }
   }
}
