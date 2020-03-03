using System.Windows.Forms;
using CefSharp;
//using CefSharp.OffScreen;
using CefSharp.WinForms;
using static CefSharp.LogSeverity;
using ForgeOfBots.CefBrowserHandler;
using System.Collections.Generic;
using System;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Threading.Tasks;

namespace ForgeOfBots
{
   public partial class Main : Form
   {
      public ChromiumWebBrowser cwb;
      public bool CookieLoaded = false;
      public ResourceManager resMgr = new ResourceManager("ForgeOfBots.Properties.Resources", Assembly.GetExecutingAssembly());
      public Dictionary<string, string> AllCookies = new Dictionary<string, string>();
      public int CurrentState = 0;
      public Main()
      {
         InitializeComponent();
         var settings = new CefSettings();
         settings.LogSeverity = Verbose;
         settings.CachePath = "cache";
         settings.PersistSessionCookies = true;
         settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0";
         settings.WindowlessRenderingEnabled = true;
         Cef.Initialize(settings);

         var browserSettings = new BrowserSettings();
         browserSettings.DefaultEncoding = "UTF-8";
         browserSettings.Javascript = CefState.Enabled;
         browserSettings.JavascriptAccessClipboard = CefState.Enabled;
         browserSettings.LocalStorage = CefState.Enabled;
         browserSettings.WebSecurity = CefState.Disabled;

         cwb = new ChromiumWebBrowser();
         cwb.Load("https://de.forgeofempires.com");
         cwb.RequestHandler = new CustomRequestHandler();
         cwb.FrameLoadEnd += Cwb_FrameLoadEnd;
         cwb.MenuHandler = new CustomMenu();
         cwb.Dock = DockStyle.Fill;
         Controls.Add(cwb);
      }

      private void Cwb_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
      {
         cwb.ShowDevTools();
         CookieHandler.CookiesLoaded += OnCookiesLoaded;
         CookieHandler.GetCookies();
      }

      private bool blockedLoginWorld = false;
      private bool blockedLogin = false;
      private void OnCookiesLoaded(object sender, CookieLoadedEventArgs e)
      {
         object PlayableWorlds = null;
         AllCookies = e.AllCookies;
         BeginInvoke(new MethodInvoker(() => lbCookies.Items.Clear()));
         foreach (var item in AllCookies)
         {
            BeginInvoke(new MethodInvoker(() => lbCookies.Items.Add(item.Key + " -> " + item.Value)));
         }
         if (CurrentState == 0 && !blockedLogin)
         {
            blockedLogin = true;
            CurrentState = 1;
            string loginJS = resMgr.GetString("preloadLoginWorld");
            loginJS = loginJS.Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()]).Replace("###USERNAME###", "JohnnyWalker").Replace("###PASSWORD###", "Carlo1509!?");
            cwb.ExecuteScriptAsync(loginJS);
            
         }
         //else if (CurrentState == 1 && !blockedLoginWorld && AllCookies.ContainsKey("CSRF".ToLower()))
         //{
         //   string loginWorld = resMgr.GetString("preloadLoginWorld");
         //   //loginJS = loginJS.Replace("###XSRF-TOKEN###", AllCookies["XSRF-TOKEN".ToLower()]).Replace("###USERNAME###", "JohnnyWalker").Replace("###PASSWORD###", "Carlo1509!?");
         //   var task = cwb.EvaluateScriptAsync(loginWorld);
         //   blockedLoginWorld = true;
         //   task.ContinueWith(t =>
         //   {
         //      if (!t.IsFaulted)
         //      {
         //         var response = t.Result;
         //         PlayableWorlds = response.Success ? (response.Result ?? "null") : response.Message;
         //         blockedLoginWorld = false;
         //         CurrentState = 2;
         //      }
         //   });
         //}
         else if (CurrentState == 2)
         {
            var x = "";
         }
      }

      private void Main_FormClosed(object sender, FormClosedEventArgs e)
      {
         Cef.Shutdown();
      }

   }
   public static class CookieHandler
   {
      public static ICookieManager CookieManager { private set; get; }
      public static Dictionary<string, string> _allCookies = new Dictionary<string, string>();
      public static void GetCookies()
      {
         CookieManager = Cef.GetGlobalCookieManager();
         var visitor = new CookieMonster(allCookies =>
         {
            //BeginInvoke(new MethodInvoker(() => lbCookies.Items.Clear()));
            foreach (var item in allCookies)
            {
               //BeginInvoke(new MethodInvoker(() => lbCookies.Items.Add(item.Item1 + " -> " + item.Item2)));
               if (_allCookies.ContainsKey(item.Item1.ToLower()))
               {
                  if (_allCookies[item.Item1.ToLower()] != item.Item2)
                     _allCookies[item.Item1.ToLower()] = item.Item2;
               }
               else
                  _allCookies.Add(item.Item1.ToLower(), item.Item2);
            }
            OnCookiesLoaded(new CookieLoadedEventArgs() { AllCookies = _allCookies });
         });
         CookieManager.VisitAllCookies(visitor);
      }

      public static void OnCookiesLoaded(CookieLoadedEventArgs e)
      {
         CookiesLoaded?.Invoke("CookieHandler", e);
      }

      public static event CookieLoadedEventHandler CookiesLoaded;
   }
   public delegate void CookieLoadedEventHandler(object sender, CookieLoadedEventArgs e);
   class CookieMonster : ICookieVisitor
   {
      readonly List<Tuple<string, string>> cookies = new List<Tuple<string, string>>();
      readonly Action<IEnumerable<Tuple<string, string>>> useAllCookies;

      public CookieMonster(Action<IEnumerable<Tuple<string, string>>> useAllCookies)
      {
         this.useAllCookies = useAllCookies;
      }

      public void Dispose()
      {

      }

      public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
      {
         cookies.Add(new Tuple<string, string>(cookie.Name, cookie.Value));

         if (count == total - 1)
            useAllCookies(cookies);

         return true;
      }
   }
   public class CustomMenu : IContextMenuHandler
   {
      public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
      {
         return;
      }

      public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
      {
         return false;
      }

      public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
      {
         return;
      }

      public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
      {
         return false;
      }
   }
   public class CookieLoadedEventArgs : EventArgs
   {
      public Dictionary<string, string> AllCookies { get; set; }
   }
}
