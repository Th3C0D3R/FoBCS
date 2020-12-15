using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using Windows.Foundation.Collections;
using System.Diagnostics;
using Windows.Foundation;
using System.ComponentModel;
using System.Drawing;
using ForgeOfBots.LanguageFiles;
using System.Net;
using Microsoft.AppCenter.Crashes;
using System.Threading.Tasks;

namespace ForgeOfBots.Utils
{
   public static class Helper
   {
      #region "API"
      [DllImport("kernel32", SetLastError = true)]
      static extern IntPtr LoadLibrary(string lpFileName);
      [DllImport("kernel32", SetLastError = true)]
      static extern bool FreeLibrary(IntPtr hModule);
      #endregion

      private static readonly List<string> msgHistory = new List<string>();
      public static List<string> MSGHistory = new List<string>();
      public static void Log(string message, ListBox listbox = null)
      {
         //Invoker.CallMethode(ctrl, () => ctrl.Items.Add(message));
#if DEBUG
         Console.WriteLine(message);
         if (listbox != null)
         {
            msgHistory.Add(message);
            MSGHistory.Add(message);
            try
            {
               if (msgHistory.Count > 0)
               {
                  Invoker.CallMethode(listbox, () => listbox.Items.AddRange(msgHistory.ToArray()));
                  Invoker.SetProperty(listbox, () => listbox.TopIndex, listbox.Items.Count - 1);
                  msgHistory.Clear();
               }
            }
            catch
            { }
         }
#elif RELEASE
            if (listbox != null)
            {
                msgHistory.Add(message);
                MSGHistory.Add(message);
                try
                {
                    
               if(msgHistory.Count > 0)
               {
                  Invoker.CallMethode(listbox, () => listbox.Items.AddRange(msgHistory.ToArray()));
                  Invoker.SetProperty(listbox, () => listbox.TopIndex, listbox.Items.Count - 1);
                  msgHistory.Clear();
               }
                }
                catch
                { }
            }
            else
            {
                Console.WriteLine(message);
            }
#endif
      }
      public static bool CheckForInternetConnection()
      {
         try
         {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.google.com"))
               return true;
         }
         catch { return false; }

      }
      public static string CalcSig(string data, string userkey, string secret)
      {
         data = data.Replace(" ", "");
         string sig = userkey + secret + data;
         return CalculateMD5Hash(sig).Substring(0, 10);
      }
      public static string CalculateMD5Hash(string input)
      {
         MD5 md5 = MD5.Create();
         byte[] inputBytes = Encoding.ASCII.GetBytes(input);
         byte[] hash = md5.ComputeHash(inputBytes);
         StringBuilder sb = new StringBuilder();
         for (int i = 0; i < hash.Length; i++)
         {
            sb.Append(hash[i].ToString("X2"));
         }
         return sb.ToString().Replace("-", string.Empty).ToLower();
      }
      public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
      {
         DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
         dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
         return dtDateTime;
      }
      public static string WorldToPlayable(Tuple<string, string, WorldState> world)
      {
         return world.Item1;
      }
      public static bool IsOnScreen(Form form)
      {
         Screen[] screens = Screen.AllScreens;
         foreach (Screen screen in screens)
         {
            Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);
            if (screen.WorkingArea.Contains(formRectangle))
               return true;
         }
         return false;
      }
      public static Dictionary<string, List<Good>> GetGoodsEraSorted(List<ResearchEra> eraList, JObject resources, JObject resourceDefList)
      {
         if (resources.Count <= 0 || resourceDefList.Count <= 0 || eraList.Count <= 0) return new Dictionary<string, List<Good>>();
         Dictionary<string, List<Good>> goodList = new Dictionary<string, List<Good>>();
         foreach (ResearchEra era in eraList)
         {
            foreach (JToken resDef in resourceDefList["responseData"].ToList())
            {
               if (goodList.ContainsKey(era.name) && goodList[era.name].Count >= 5) break;
               if (resDef["era"]?.ToString() == era.era)
               {
                  foreach (JToken resource in resources["responseData"]["resources"].ToList())
                  {
                     string propName = resource.ToObject<JProperty>().Name;
                     int amount = resource.First.ToObject<int>();
                     if (resDef["id"]?.ToString() == propName)
                     {
                        if (goodList.ContainsKey(era.name))
                        {
                           goodList[era.name].Add(new Good() { good_id = resDef["id"]?.ToString(), name = resDef["name"]?.ToString(), value = amount });
                           break;
                        }
                        else
                        {
                           List<Good> goods = new List<Good>
                           {
                              new Good() { good_id = resDef["id"]?.ToString(), name = resDef["name"]?.ToString(), value = amount }
                           };
                           goodList.Add(era.name, goods);
                           break;
                        }
                     }
                  }
               }
            }
         }
         return goodList;
      }
      public static List<KeyValuePair<string, List<EntityEx>>> GetGroupedList(List<EntityEx> buildings)
      {
         List<KeyValuePair<string, List<EntityEx>>> newBuildingList = new List<KeyValuePair<string, List<EntityEx>>>();
         foreach (EntityEx item in buildings)
         {
            var tmp = newBuildingList.FindAll(e => e.Key == item.cityentity_id);
            bool added = false;
            foreach (KeyValuePair<string, List<EntityEx>> keyValItem in tmp)
            {
               if (keyValItem.Value.First().state["__class__"].ToString() == item.state["__class__"].ToString())
               {
                  if (item.state["__class__"].ToString().ToLower() == "IdleState".ToLower() || item.state["__class__"].ToString().ToLower() == "ProductionFinishedState".ToLower())
                  {
                     keyValItem.Value.Add(item);
                     added = true;
                  }
                  else if (item.state["__class__"].ToString().ToLower() == "ProducingState".ToLower())
                  {
                     TimeSpan iteminTime = TimeSpan.FromSeconds(double.Parse(item.state["next_state_transition_in"].ToString()));
                     TimeSpan keyValItemInTime = TimeSpan.FromSeconds(double.Parse(keyValItem.Value.First().state["next_state_transition_in"].ToString()));
                     if (keyValItemInTime.TotalSeconds - 2 <= iteminTime.TotalSeconds && iteminTime.TotalSeconds <= keyValItemInTime.TotalSeconds + 2)
                     {
                        keyValItem.Value.Add(item);
                        added = true;
                     }
                     //DateTime atTime = UnixTimeStampToDateTime(double.Parse(keyValItem.Value.First().state["next_state_transition_at"].ToString()));
                  }
               }
            }
            if (!added) newBuildingList.Add(new KeyValuePair<string, List<EntityEx>>(item.cityentity_id, new List<EntityEx>() { item }));
         }
         return newBuildingList;
      }
      public static IEnumerable<string> GetDescriptions(Type type)
      {
         var descs = new List<string>();
         var names = Enum.GetNames(type);
         foreach (var name in names)
         {
            var field = type.GetField(name);
            var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            foreach (DescriptionAttribute fd in fds)
            {
               descs.Add(fd.Description);
            }
         }
         return descs;
      }
      public static bool CheckLibrary(string fileName)
      {
         IntPtr ldLibrary = LoadLibrary(fileName);
         bool retVal = ldLibrary != IntPtr.Zero;
         FreeLibrary(ldLibrary);
         return retVal;
      }
      public static Type GetTypeByName(this Assembly asm, string typeName)
      {
         try
         {
            Type[] assemblyTypes = asm.GetTypes();
            foreach (Type type in assemblyTypes)
               if (type.Name.Equals(typeName))
                  return type;
            return null;
         }
         catch (ReflectionTypeLoadException ex)
         {
            Console.WriteLine(ex.ToString());
            return null;
         }
      }
      public static object ExecuteMethod(Assembly asm, string classtype, string methodename, object[] param)
      {
         var type = asm.GetTypeByName(classtype);
         var methodInfo = type.GetMethod(methodename);
         if (methodInfo != null)
         {
            object instanceType = Activator.CreateInstance(type);
            return methodInfo.Invoke(instanceType, param);
         }
         return null;
      }
      public static async Task setTimeout(Action action, int timeoutInMilliseconds)
      {
         await Task.Delay(timeoutInMilliseconds);
         action();
      }
      public enum Result
      {
         Success,
         Expired,
         Failed
      }
   }
   public class WorldData
   {
      public string player_name { get; set; }
      public Dictionary<string, int> player_worlds { get; set; }
      public List<World> worlds { get; set; }
   }
   public class World
   {
      public string id { get; set; }
      public string num_id { get; set; }
      public string name { get; set; }
      public string url { get; set; }
      public int started_at { get; set; }
      public bool register { get; set; }
      public bool login { get; set; }
      public bool best { get; set; }
      public bool premium_world { get; set; }
      public int rank { get; set; }
      public object update { get; set; } = null;
      public string constraint_worlds { get; set; }
      public string description { get; set; }
      public int user_limit { get; set; }

   }
   public static class Invoker
   {
      private delegate void SetPropertyThreadSafeDelegate<TResult>(Control @this, Expression<Func<TResult>> property, TResult value);
      public static void SetProperty<TResult>(this Control @this, Expression<Func<TResult>> property, TResult value)
      {
         var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;

         if (propertyInfo == null || @this.GetType().GetProperty(propertyInfo.Name, propertyInfo.PropertyType) == null)
            throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
         if (@this.InvokeRequired)
            @this.Invoke(new SetPropertyThreadSafeDelegate<TResult>(SetProperty), new object[] { @this, property, value });
         else
            @this.GetType().InvokeMember(propertyInfo.Name, BindingFlags.SetProperty, null, @this, new object[] { value });
      }
      public static void CallMethode(this Control @this, Action action)
      {
         @this.Invoke(action);
      }
      public static void InvokeAction<TControlType>(this TControlType control, Action<TControlType> del) where TControlType : ToolStripStatusLabel
      {
         if (control.GetCurrentParent().InvokeRequired)
            control.GetCurrentParent().Invoke(new Action(() => del(control)));
         else
            del(control);
      }
   }
   public static class NotificationHelper
   {
      public enum NotifyDuration
      {
         [Description("long")]
         Long,
         [Description("short")]
         Short
      }
      private const string pAPP_ID = "FoBots.Notification.7AC208C23B5B454D88E371DD75C8C2BC";
      private static readonly string NotID = "Forge of Bots";
      public static void ShowNotify(string header, string content, NotifyDuration duration = NotifyDuration.Long,
          TypedEventHandler<ToastNotification, object> activateFunc = null,
          TypedEventHandler<ToastNotification, ToastDismissedEventArgs> dismissFunc = null,
          TypedEventHandler<ToastNotification, ToastFailedEventArgs> failFunc = null, string iconPath = "", string APP_ID = "")
      {
         ComponentResourceManager resources = new ComponentResourceManager(typeof(Main));
         System.Drawing.Icon icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

         string filepath = Path.Combine(Environment.ExpandEnvironmentVariables(@"%AppData%"), "FoBots");
         Directory.CreateDirectory(filepath);
         if (!File.Exists(Path.Combine(filepath, "icon.ico")))
         {
            FileStream outStream = File.Create(Path.Combine(filepath, "icon.ico"));
            icon.Save(outStream);
            outStream.Flush();
            outStream.Close();
         }
         iconPath = Path.Combine(filepath, "icon.ico");
         ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText02;
         XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
         XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
         toastTextElements[0].AppendChild(toastXml.CreateTextNode(header));
         toastTextElements[1].AppendChild(toastXml.CreateTextNode(content));
         XmlNodeList toastImageElements = toastXml.GetElementsByTagName("image");
         ((XmlElement)toastImageElements[0]).SetAttribute("src", iconPath);
         IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
         ((XmlElement)toastNode).SetAttribute("duration", Enum.GetName(typeof(NotifyDuration), duration));
         ToastNotification toast = new ToastNotification(toastXml);
         toast.Activated += activateFunc ?? ToastActivated;
         toast.Dismissed += dismissFunc ?? ToastDismissed;
         toast.Failed += failFunc ?? ToastFailed;
         try
         {
            if (TryCreateShortcut(APP_ID))
               ToastNotificationManager.CreateToastNotifier(APP_ID + NotID).Show(toast);
         }
         catch (Exception ex)
         {
            NLog.LogManager.Flush();
            var attachments = new ErrorAttachmentLog[] { ErrorAttachmentLog.AttachmentWithText(File.ReadAllText("log.foblog"), "log.foblog") };
            var properties = new Dictionary<string, string> { { "ShowNotify", strings.ShowingNotify } };
            Crashes.TrackError(ex, properties, attachments);
         }
      }
      private static void ToastActivated(ToastNotification sender, object e)
      {

      }
      private static void ToastDismissed(ToastNotification sender, ToastDismissedEventArgs e)
      {
         string outputText = "";
         switch (e.Reason)
         {
            case ToastDismissalReason.ApplicationHidden:
               outputText = "The app hide the toast using ToastNotifier.Hide";
               break;
            case ToastDismissalReason.UserCanceled:
               outputText = "The user dismissed the toast";
               break;
            case ToastDismissalReason.TimedOut:
               outputText = "The toast has timed out";
               break;
         }
         Console.WriteLine(outputText);
      }
      private static void ToastFailed(ToastNotification sender, ToastFailedEventArgs e)
      {
         Console.WriteLine(e.ErrorCode.Message);
      }
      public static bool TryCreateShortcut(string APP_ID = "")
      {
         string shortcutPath = Path.Combine(Environment.ExpandEnvironmentVariables(@"%AppData%"), "Microsoft", "Windows", "Start Menu", "Programs", Assembly.GetExecutingAssembly().GetName().Name + ".lnk");
         if (!File.Exists(shortcutPath))
         {
            InstallShortcut(shortcutPath, APP_ID);
            return true;
         }
         return true;
      }
      private static void InstallShortcut(string shortcutPath, string APP_ID = "")
      {
         string _APP_ID = string.IsNullOrEmpty(APP_ID) ? pAPP_ID : APP_ID;
         string exePath = Process.GetCurrentProcess().MainModule.FileName;
         IShellLinkW newShortcut = (IShellLinkW)new CShellLink();
         ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
         ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));
         IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;
         using (PropVariant appId = new PropVariant(_APP_ID + NotID))
         {
            ErrorHelper.VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appId));
            ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
         }
         IPersistFile newShortcutSave = (IPersistFile)newShortcut;
         ErrorHelper.VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
      }
      internal enum STGM : long
      {
         STGM_READ = 0x00000000L,
         STGM_WRITE = 0x00000001L,
         STGM_READWRITE = 0x00000002L,
         STGM_SHARE_DENY_NONE = 0x00000040L,
         STGM_SHARE_DENY_READ = 0x00000030L,
         STGM_SHARE_DENY_WRITE = 0x00000020L,
         STGM_SHARE_EXCLUSIVE = 0x00000010L,
         STGM_PRIORITY = 0x00040000L,
         STGM_CREATE = 0x00001000L,
         STGM_CONVERT = 0x00020000L,
         STGM_FAILIFTHERE = 0x00000000L,
         STGM_DIRECT = 0x00000000L,
         STGM_TRANSACTED = 0x00010000L,
         STGM_NOSCRATCH = 0x00100000L,
         STGM_NOSNAPSHOT = 0x00200000L,
         STGM_SIMPLE = 0x08000000L,
         STGM_DIRECT_SWMR = 0x00400000L,
         STGM_DELETEONRELEASE = 0x04000000L,
      }
      internal static class ShellIIDGuid
      {
         internal const string IShellLinkW = "000214F9-0000-0000-C000-000000000046";
         internal const string CShellLink = "00021401-0000-0000-C000-000000000046";
         internal const string IPersistFile = "0000010b-0000-0000-C000-000000000046";
         internal const string IPropertyStore = "886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99";
      }
      [ComImport, Guid(ShellIIDGuid.IShellLinkW), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
      internal interface IShellLinkW
      {
         uint GetPath([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, IntPtr pfd, uint fFlags);
         uint GetIDList(out IntPtr ppidl);
         uint SetIDList(IntPtr pidl);
         uint GetDescription([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxName);
         uint SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
         uint GetWorkingDirectory([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
         uint SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
         uint GetArguments([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
         uint SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
         uint GetHotKey(out short wHotKey);
         uint SetHotKey(short wHotKey);
         uint GetShowCmd(out uint iShowCmd);
         uint SetShowCmd(uint iShowCmd);
         uint GetIconLocation([Out(), MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszIconPath, int cchIconPath, out int iIcon);
         uint SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
         uint SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
         uint Resolve(IntPtr hwnd, uint fFlags);
         uint SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
      }
      [ComImport, Guid(ShellIIDGuid.IPersistFile), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
      internal interface IPersistFile
      {
         uint GetCurFile([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile);
         uint IsDirty();
         uint Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.U4)] STGM dwMode);
         uint Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, bool fRemember);
         uint SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
      }
      [ComImport]
      [Guid(ShellIIDGuid.IPropertyStore)]
      [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
      interface IPropertyStore
      {
         uint GetCount([Out] out uint propertyCount);
         uint GetAt([In] uint propertyIndex, out PropertySet key);
         uint GetValue([In] ref PropertyKey key, [Out] PropVariant pv);
         uint SetValue([In] ref PropertyKey key, [In] PropVariant pv);
         uint Commit();
      }
      [ComImport, Guid(ShellIIDGuid.CShellLink), ClassInterface(ClassInterfaceType.None)]
      internal class CShellLink { }
      public static class ErrorHelper
      {
         public static void VerifySucceeded(uint hresult)
         {
            if (hresult > 1)
            {
               throw new Exception("Failed with HRESULT: " + hresult.ToString("X"));
            }
         }
      }
   }
   public class TwoStringArgs : EventArgs
   {
      public string s1 { get; set; }
      public string s2 { get; set; }
   }
   public class OneTArgs<T> : EventArgs
   {
      public T t1 { get; set; }
   }
   public class TwoTArgs<T, Y>
   {
      public T RequestType { get; set; }
      public Y argument2 { get; set; }
   }
}
