using ForgeOfBots.Forms;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using Microsoft.AppCenter.Crashes;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;

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
      public static void Log(string message, params ListBox[] listbox)
      {
         //Invoker.CallMethode(ctrl, () => ctrl.Items.Add(message));
         if (listbox != null)
         {
#if DEBUG
            Console.WriteLine(message);
#endif
            msgHistory.Add(message);
            MSGHistory.Add(message);
            try
            {

               foreach (ListBox item in listbox)
               {
                  Invoker.CallMethode(item, () => item.Items.AddRange(msgHistory.ToArray()));
                  Invoker.SetProperty(item, () => item.TopIndex, item.Items.Count - 1);
               }
               msgHistory.Clear();
            }
            catch
            { }
         }
         else
         {
            Console.WriteLine(message);
         }
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
         return $"{world.Item1}|{world.Item2}";
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
      public static Dictionary<string, List<Unit>> GetUnitEraSorted(List<ResearchEra> eraList, List<UnitType> unitTypes, Army responseData)
      {
         if (unitTypes.Count <= 0 || responseData == null) return new Dictionary<string, List<Unit>>();
         Dictionary<string, List<Unit>> armyList = new Dictionary<string, List<Unit>>();
         foreach (ResearchEra era in eraList)
         {
            foreach (var unittype in unitTypes)
            {
               if (unittype.minEra == era.era)
               {
                  foreach (var army in responseData.units)
                  {
                     Unit unit = new Unit
                     {
                        unit = army,
                        name = unittype.name,
                        count = responseData.counts.ToList().Find(c => c.unitTypeId == army.unitTypeId).unattached,
                        ids = responseData.units.ToList().FindAll(e => e.unitTypeId == army.unitTypeId).Select(e => e.unitId).ToList()
                     };
                     if (unittype.unitTypeId == army.unitTypeId)
                     {
                        if (armyList.ContainsKey(era.name))
                        {
                           armyList[era.name].Add(unit);
                           break;
                        }
                        else
                        {
                           List<Unit> unitlist = new List<Unit>
                        {
                           unit
                        };
                           armyList.Add(era.name, unitlist);
                           break;
                        }
                     }
                  }
               }
            }
         }
         return armyList;
      }
      public static List<Unit> GetUnitSorted(List<UnitType> unitTypes, Army responseData)
      {
         if (responseData == null) return new List<Unit>();
         var tmpList = new List<Unit>();
         foreach (var unittype in unitTypes)
         {
            foreach (var army in responseData.units)
            {
               Unit unit = new Unit
               {
                  unit = army,
                  name = unittype.name,
                  count = responseData.counts.ToList().Find(c => c.unitTypeId == army.unitTypeId).unattached,
                  ids = responseData.units.ToList().FindAll(e => e.unitTypeId == army.unitTypeId).Select(e => e.unitId).ToList()
               };
               if (unittype.unitTypeId == army.unitTypeId)
               {
                  tmpList.Add(unit);
                  break;
               }
            }
         }
         return tmpList;
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
      public static string ReadResource(string name)
      {
         var assembly = Assembly.GetExecutingAssembly();
         string resourcePath = name;
         if (!name.StartsWith(nameof(Helper)))
         {
            resourcePath = assembly.GetManifestResourceNames()
                .Single(str => str.EndsWith(name));
         }

         using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
         using (StreamReader reader = new StreamReader(stream))
         {
            return reader.ReadToEnd();
         }
      }
      public enum Result
      {
         Success,
         Expired,
         Failed
      }


      public static int GetP1(string AgeString, int Level)
      {
         int[] BronzeAge = { 5, 10, 10, 15, 25, 30, 35, 40, 45, 55, 60, 65, 75, 80, 85, 95, 100, 110, 115, 125, 130, 140, 145, 155, 160, 170, 180, 185, 195, 200, 210, 220, 225, 235, 245, 250, 260, 270, 275, 285, 295, 300, 310, 320, 330, 340, 345, 355, 365, 375, 380, 390, 400, 410, 420, 430, 440, 445, 455, 465, 475, 485, 495, 505, 510, 520, 530, 540, 550, 560, 570, 580, 590, 600, 610, 620, 630, 640, 650, 660, 670, 680, 690, 700, 710, 720, 730, 740, 750, 760, 770, 780, 790, 800, 810, 820, 830, 840, 850, 860, 870, 880, 890, 905, 915, 925, 935, 945, 955, 965, 975, 985, 995, 1010, 1020, 1030, 1040, 1050, 1060, 1070, 1085, 1095, 1105, 1115, 1125, 1135, 1150, 1160, 1170, 1180, 1190, 1200, 1215, 1225, 1235, 1245, 1255, 1270, 1280, 1290, 1300, 1310, 1325, 1335, 1345, 1355, 1370, 1380, 1390, 1400, 1415, 1425, 1435, 1445, 1460, 1470, 1480 };
         int[] IronAge = { 5, 10, 15, 20, 25, 30, 40, 45, 50, 60, 65, 70, 80, 85, 95, 105, 110, 120, 125, 135, 145, 150, 160, 170, 175, 185, 195, 200, 210, 220, 230, 240, 245, 255, 265, 275, 285, 290, 300, 310, 320, 330, 340, 350, 360, 370, 380, 390, 400, 405, 415, 425, 435, 450, 455, 465, 475, 485, 495, 510, 520, 530, 540, 550, 560, 570, 580, 590, 600, 610, 620, 630, 645, 655, 665, 675, 685, 695, 705, 720, 730, 740, 750, 760, 775, 785, 795, 805, 815, 825, 840, 850, 860, 870, 885, 895, 905, 915, 930, 940, 950, 960, 975, 985, 995, 1010, 1020, 1030, 1040, 1055, 1065 };
         int[] EarlyMiddleAge = { 5, 10, 15, 20, 25, 35, 40, 50, 55, 65, 70, 80, 85, 95, 100, 110, 120, 130, 135, 145, 155, 165, 175, 180, 190, 200, 210, 220, 230, 240, 250, 255, 265, 275, 285, 295, 305, 315, 325, 335, 345, 360, 370, 380, 390, 400, 410, 420, 430, 440, 450, 465, 475, 485, 495, 505, 515, 525, 540, 550, 560, 570, 585, 595, 605, 615, 625, 640, 650, 660, 675, 685, 695, 705, 720, 730, 740, 755, 765, 775, 790, 800, 810, 825, 835, 850, 860, 875, 885, 895, 910, 920, 930, 945, 955, 970, 980, 995, 1005, 1015, 1030, 1040, 1055, 1065, 1080, 1090, 1105, 1115, 1130, 1140, 1155, 1165, 1180, 1190, 1205, 1215, 1230, 1240, 1255, 1265, 1280, 1290, 1305, 1320, 1330, 1345, 1355, 1370, 1380, 1395, 1405, 1420, 1435 };
         int[] HighMiddleAge = { 5, 10, 15, 20, 30, 35, 45, 50, 60, 65, 75, 85, 95, 100, 110, 120, 130, 140, 150, 155, 165, 175, 185, 195, 205, 215, 225, 235, 245, 255, 265, 275, 285, 300, 310, 320, 330, 340, 350, 365, 375, 385, 395, 405, 420, 430, 440, 450, 465, 475, 485, 500, 510, 520, 535, 545, 555, 570, 580, 590, 605, 615, 630, 640, 650, 665, 675, 690, 700, 715, 725, 735, 750, 760, 775, 785, 800, 810, 825, 835, 850, 860, 875, 890, 900, 915, 925, 940, 950, 965, 975, 990, 1005, 1015, 1030, 1040, 1055, 1070, 1080, 1095, 1110, 1120, 1135, 1150, 1160, 1175, 1190, 1200, 1215, 1230, 1240, 1255 };
         int[] LateMiddleAge = { 5, 10, 15, 25, 30, 40, 45, 55, 65, 70, 80, 90, 100, 110, 120, 125, 140, 150, 155, 170, 180, 190, 200, 210, 220, 230, 240, 250, 265, 275, 285, 295, 310, 320, 330, 340, 355, 365, 375, 390, 400, 410, 425, 435, 450, 460, 470, 485, 495, 510, 520, 535, 545, 560, 570, 585, 595, 610, 620, 635, 645, 660, 670, 685, 700, 710, 725, 735, 750, 765, 775, 790, 805, 815, 830, 845, 855, 870, 885, 895, 910, 925, 935, 950, 965, 980, 990, 1005, 1020, 1035, 1045, 1060, 1075, 1090, 1105, 1115, 1130, 1145, 1160, 1175, 1185, 1200, 1215, 1230, 1245, 1260, 1275, 1285, 1300, 1315, 1330, 1345, 1360, 1375, 1390, 1405, 1415, 1430, 1445, 1460, 1475, 1490, 1505, 1520, 1535, 1550, 1565, 1580, 1595, 1610, 1625, 1640, 1655, 1670, 1685, 1700, 1715, 1730, 1745, 1760, 1775, 1790 };
         int[] ColonialAge = { 5, 10, 15, 25, 35, 40, 50, 60, 65, 75, 85, 95, 105, 115, 125, 135, 145, 155, 170, 180, 190, 200, 210, 225, 235, 245, 260, 270, 280, 295, 305, 315, 330, 340, 350, 365, 375, 390, 400, 415, 425, 440, 450, 465, 480, 490, 505, 515, 530, 540, 555, 570, 580, 595, 610, 620, 635, 650, 665, 675, 690, 705, 715, 730, 745, 760, 775, 785, 800, 815, 830, 840, 855, 870, 885, 900, 915, 930, 940, 955, 970, 985, 1000, 1015, 1030, 1045, 1060, 1075, 1090, 1100, 1115, 1130, 1145, 1160, 1175, 1190, 1205, 1220, 1235, 1250, 1265, 1280, 1295, 1310, 1325 };
         int[] IndustrialAge = { 10, 10, 20, 25, 35, 45, 50, 60, 70, 80, 90, 100, 115, 120, 135, 145, 155, 165, 180, 190, 200, 215, 225, 235, 250, 260, 275, 285, 300, 310, 325, 335, 350, 360, 375, 390, 400, 415, 425, 440, 455, 465, 480, 495, 505, 520, 535, 550, 560, 575, 590, 605, 620, 635, 645, 660, 675, 690, 705, 720, 735, 745, 760, 775, 790, 805, 820, 835, 850, 865, 880, 895, 910, 925, 940, 955, 970, 985, 1000, 1015, 1030, 1045, 1065, 1075, 1095, 1110, 1125, 1140, 1155, 1170, 1185, 1200, 1220, 1235, 1250, 1265, 1280, 1300, 1315, 1330, 1345, 1360, 1375, 1395, 1410, 1425 };
         int[] ProgressiveEra = { 10, 10, 20, 30, 35, 45, 55, 65, 75, 85, 95, 105, 120, 130, 140, 155, 165, 175, 190, 200, 215, 225, 240, 250, 265, 275, 290, 300, 315, 330, 340, 355, 370, 385, 395, 410, 425, 440, 450, 465, 480, 495, 510, 525, 535, 550, 565, 580, 595, 610, 625, 640, 655, 670, 685, 700, 715, 730, 745, 760, 775, 790, 805, 820, 835, 855, 870, 885, 900, 915, 930, 945, 965, 980, 995, 1010, 1025, 1045, 1060, 1075, 1090, 1110, 1125, 1140, 1160, 1175, 1190, 1205, 1225, 1240, 1255, 1275, 1290, 1305, 1325, 1340, 1355, 1375, 1390, 1410, 1425, 1440, 1460, 1475, 1490, 1510, 1525, 1545, 1560, 1580, 1595, 1615, 1630, 1650, 1665, 1685, 1700, 1715, 1735, 1755, 1770, 1790, 1805, 1825, 1840, 1860, 1875, 1895, 1915, 1930, 1950, 1965, 1985, 2000, 2020, 2040, 2055, 2075, 2095, 2110, 2130, 2145, 2165, 2185, 2200, 2220, 2240, 2255, 2275, 2295, 2310, 2330, 2350, 2365, 2385, 2405, 2420, 2440, 2460, 2480, 2495, 2515, 2535, 2555, 2570, 2590, 2610, 2630, 2645, 2665, 2685, 2705, 2720 };
         int[] ModernEra = { 10, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 115, 125, 135, 150, 160, 175, 185, 200, 210, 225, 240, 250, 265, 280, 290, 305, 320, 335, 345, 360, 375, 390, 405, 420, 430, 450, 460, 475, 490, 505, 520, 535, 550, 565, 580, 600, 615, 630, 645, 660, 675, 690, 705, 725, 740, 755, 770, 785, 800, 820, 835, 850, 870, 885, 900, 915, 935, 950, 965, 985, 1000, 1015, 1035, 1050, 1065, 1085, 1100, 1120, 1135, 1150, 1170, 1185, 1205, 1220, 1240, 1255, 1275, 1290, 1310, 1325, 1345, 1360, 1380, 1395, 1415, 1430, 1450, 1470, 1485, 1505 };
         int[] PostModernEra = { 10, 10, 20, 30, 40, 50, 60, 75, 85, 95, 110, 120, 130, 145, 155, 170, 185, 195, 210, 225, 235, 250, 265, 280, 295, 305, 320, 335, 350, 365, 380, 395, 410, 425, 440, 455, 470, 485, 500, 515, 535, 550, 565, 580, 595, 615, 630, 645, 660, 675, 695, 710, 725, 745, 760, 775, 795, 810, 830, 845, 860, 880, 895, 915, 930, 945, 965, 985, 1000, 1020, 1035, 1050, 1070, 1090, 1105, 1125, 1140, 1160, 1175, 1195, 1215, 1230, 1250, 1265, 1285, 1305, 1320, 1340, 1360, 1375, 1395, 1415, 1435, 1450, 1470, 1490, 1510, 1525, 1545, 1565, 1585, 1600, 1620, 1640, 1660, 1680, 1695, 1715, 1735, 1755, 1775, 1790, 1810, 1830, 1850, 1870, 1890, 1910, 1930, 1950, 1965, 1985, 2005, 2025, 2045, 2065, 2085, 2105, 2125, 2145, 2165, 2185, 2205, 2225, 2245, 2265 };
         int[] ContemporaryEra = { 10, 15, 20, 30, 40, 55, 65, 75, 85, 100, 115, 125, 140, 150, 165, 180, 190, 205, 220, 235, 250, 265, 280, 290, 305, 320, 335, 355, 365, 385, 400, 415, 430, 445, 460, 480, 495, 510, 525, 545, 560, 575, 590, 610, 625, 645, 660, 675, 695, 710, 730, 745, 765, 780, 800, 815, 835, 850, 870, 885, 905, 920, 940, 960, 975, 995, 1015, 1030, 1050, 1070, 1085, 1105, 1125, 1140, 1160, 1180, 1200, 1215, 1235, 1255, 1275, 1290, 1310, 1330, 1350, 1370, 1390, 1410, 1425, 1445, 1465, 1485, 1505, 1525, 1545, 1565, 1580, 1600, 1625, 1640, 1660, 1680, 1700, 1720, 1740, 1760, 1780, 1800, 1820, 1840, 1860, 1880, 1900, 1920, 1945, 1965, 1985, 2005, 2025, 2045, 2065, 2085, 2105, 2125 };
         int[] TomorrowEra = { 10, 15, 20, 35, 45, 55, 65, 80, 90, 105, 120, 130, 145, 160, 175, 185, 200, 215, 230, 245, 260, 275, 290, 305, 320, 335, 355, 370, 385, 400, 420, 435, 450, 465, 485, 500, 515, 535, 550, 570, 585, 605, 620, 640, 655, 675, 690, 710, 730, 745, 765, 780, 800, 820, 835, 855, 875, 890, 910, 930, 945, 965, 985, 1005, 1025, 1040, 1060, 1080, 1100, 1120, 1140, 1155, 1175, 1195, 1215, 1235, 1255, 1275, 1295, 1315, 1335, 1355, 1375, 1395, 1415, 1435, 1455, 1475, 1495, 1515, 1535, 1555, 1575, 1595, 1615, 1640, 1660, 1680, 1700, 1720, 1740, 1760, 1780, 1805 };
         int[] FutureEra = { 10, 15, 25, 35, 45, 60, 70, 85, 95, 110, 120, 135, 150, 165, 180, 195, 210, 225, 240, 255, 270, 290, 305, 320, 335, 355, 370, 385, 405, 420, 435, 455, 470, 490, 505, 525, 540, 560, 575, 595, 615, 630, 650, 670, 685, 705, 725, 740, 760, 780, 800, 815, 835, 855, 875, 895, 915, 930, 950, 970, 990, 1010, 1030, 1050, 1070, 1090, 1110, 1130, 1150, 1170, 1190, 1210, 1230, 1250, 1270, 1290, 1310, 1335, 1355, 1375, 1395, 1415, 1435, 1455, 1480, 1500, 1520, 1540, 1560, 1585, 1605, 1625, 1645, 1670, 1690, 1710, 1735, 1755, 1775, 1800, 1820, 1840, 1865, 1885, 1905, 1930, 1950, 1975, 1995, 2015, 2040, 2060, 2085, 2105, 2130, 2150, 2170, 2195, 2215, 2240, 2260, 2285, 2305, 2330, 2350, 2375, 2395, 2420, 2445, 2465, 2490, 2510, 2535, 2555, 2580, 2605, 2625, 2650, 2675, 2695, 2720, 2740, 2765, 2790, 2810, 2835, 2860, 2880, 2905, 2930, 2950, 2975, 3000, 3025, 3050, 3070, 3095, 3120, 3140, 3165, 3190, 3215, 3235, 3260, 3285, 3310, 3335, 3355, 3380, 3405, 3430, 3455, 3480, 3500, 3525, 3550, 3575, 3600, 3625, 3650, 3670, 3695, 3720 };
         int[] ArcticFuture = { 10, 15, 25, 35, 45, 60, 75, 85, 100, 115, 130, 145, 160, 170, 190, 205, 220, 235, 250, 265, 285, 300, 315, 335, 350, 370, 385, 400, 420, 440, 455, 475, 490, 510, 525, 545, 565, 585, 600, 620, 640, 660, 675, 695, 715, 735, 755, 775, 795, 815, 830, 850, 870, 895, 910, 930, 950, 970, 995, 1015, 1035, 1055, 1075, 1095, 1115, 1135, 1155, 1180, 1200, 1220, 1240, 1260, 1285, 1305, 1325, 1350, 1370, 1390, 1410, 1435, 1455, 1475, 1500, 1520, 1545, 1565, 1585, 1610, 1630, 1650, 1675, 1695, 1720, 1740, 1765, 1785, 1810, 1830, 1855, 1875, 1900, 1920, 1945, 1965, 1990, 2015, 2035, 2060, 2080, 2105, 2125, 2150, 2175, 2195, 2220, 2245, 2265, 2290, 2315, 2335, 2360, 2385, 2405, 2430, 2455, 2480, 2500, 2525, 2550, 2575, 2595, 2620, 2645, 2670, 2690, 2715, 2740 };
         int[] OceanicFuture = { 10, 15, 25, 35, 50, 65, 75, 90, 105, 120, 135, 150, 165, 180, 195, 210, 230, 245, 260, 280, 295, 310, 330, 350, 365, 385, 400, 420, 440, 455, 475, 495, 510, 530, 550, 570, 590, 605, 625, 645, 665, 685, 705, 725, 745, 765, 785, 805, 825, 845, 865, 890, 910, 930, 950, 970, 990, 1015, 1035, 1055, 1075, 1100, 1120, 1140, 1160, 1185, 1205, 1225, 1250, 1270, 1295, 1315, 1335, 1360, 1380, 1405, 1425, 1450, 1470, 1495, 1515, 1540, 1560, 1585, 1605, 1630, 1650, 1675, 1700, 1720, 1745, 1770, 1790, 1815, 1835, 1860, 1885, 1905, 1930, 1955, 1980, 2000, 2025, 2050, 2070, 2095, 2120, 2145, 2170, 2190, 2215, 2240, 2265, 2290, 2310 };
         int[] VirtualFuture = { 10, 15, 25, 40, 50, 65, 80, 95, 110, 125, 140, 155, 170, 185, 205, 220, 235, 255, 270, 290, 305, 325, 345, 360, 380, 400, 415, 435, 455, 475, 495, 510, 530, 550, 570, 590, 610, 630, 650, 670, 690, 715, 735, 755, 775, 795, 815, 840, 860, 880, 900, 925, 945, 965, 990, 1010, 1030, 1055, 1075, 1095, 1120, 1140, 1165, 1185, 1210, 1230, 1255, 1275, 1300, 1320, 1345, 1365, 1390, 1415, 1435, 1460, 1485, 1505, 1530, 1555, 1575, 1600, 1625, 1645, 1670, 1695, 1720, 1745, 1765, 1790, 1815, 1840, 1860, 1885, 1910, 1935, 1960, 1985, 2010, 2030, 2055, 2080, 2105, 2130, 2155, 2180, 2205, 2230, 2255, 2280, 2305, 2330, 2355, 2380, 2405, 2430, 2455, 2480, 2505, 2530 };
         int[] SpaceAgeMars = { 10, 15, 25, 40, 55, 70, 80, 95, 115, 125, 145, 160, 175, 195, 210, 230, 245, 265, 280, 300, 320, 335, 355, 375, 395, 415, 435, 455, 470, 490, 510, 535, 550, 575, 595, 615, 635, 655, 675, 700, 720, 740, 760, 785, 805, 825, 850, 870, 890, 915, 935, 960, 980, 1005, 1025, 1050, 1070, 1095, 1115, 1140, 1160, 1185, 1210, 1230, 1255, 1280, 1300, 1325, 1350, 1370, 1395, 1420, 1445, 1470, 1490, 1515, 1540, 1565, 1590, 1615, 1635, 1660, 1685, 1710, 1735, 1760, 1785, 1810, 1835, 1860, 1885, 1910, 1935, 1960, 1985, 2010, 2035, 2060, 2085, 2110, 2135, 2160 };
         int[] SpaceAgeAsteroidBelt = { 10, 15, 30, 40, 55, 70, 85, 100, 115, 130, 150, 165, 185, 200, 220, 235, 255, 275, 295, 310, 330, 350, 370, 390, 410, 430, 450, 470, 490, 510, 530, 550, 575, 595, 615, 635, 660, 680, 700, 725, 745, 770, 790, 810, 835, 855, 880, 905, 925, 950, 970, 995, 1015, 1040, 1065, 1085, 1110, 1135, 1160, 1180, 1205, 1230, 1255, 1275, 1300, 1325, 1350, 1375, 1400, 1425, 1450, 1470, 1500, 1520, 1545, 1570, 1595, 1620, 1650, 1670, 1685 };
         int[] AllAge = { 5, 10, 15, 20, 30, 35, 45, 50, 60, 65, 75, 85, 95, 100, 110, 120, 130, 140, 150, 155, 165, 175, 185, 195, 205, 215, 225, 235, 245, 255, 265, 275, 285, 300, 310, 320, 330, 340, 350, 365, 375, 385, 395, 405, 420, 430, 440, 450, 465, 475, 485, 500, 510, 520, 535, 545, 555, 570, 580, 590, 605, 615, 630, 640, 650, 665, 675, 690, 700, 715, 725, 735, 750, 760, 775, 785, 800, 810, 825, 835, 850, 860, 875, 890, 900, 915, 925, 940, 950, 965, 975, 990, 1005, 1015, 1030, 1040, 1055, 1070, 1080, 1095, 1110, 1120, 1135, 1150, 1160, 1175, 1190, 1200, 1215, 1230, 1240, 1255 };
         if (AgeString == "BronzeAge")
         {
            if (BronzeAge.Length < Level) return 0; else return BronzeAge[Level];
         }
         else if (AgeString == "IronAge")
         {
            if (IronAge.Length < Level) return 0; else return IronAge[Level];
         }
         else if (AgeString == "EarlyMiddleAge")
         {
            if (EarlyMiddleAge.Length < Level) return 0; else return EarlyMiddleAge[Level];
         }
         else if (AgeString == "HighMiddleAge")
         {
            if (HighMiddleAge.Length < Level) return 0; else return HighMiddleAge[Level];
         }
         else if (AgeString == "LateMiddleAge")
         {
            if (LateMiddleAge.Length < Level) return 0; else return LateMiddleAge[Level];
         }
         else if (AgeString == "ColonialAge")
         {
            if (ColonialAge.Length < Level) return 0; else return ColonialAge[Level];
         }
         else if (AgeString == "IndustrialAge")
         {
            if (IndustrialAge.Length < Level) return 0; else return IndustrialAge[Level];
         }
         else if (AgeString == "ProgressiveEra")
         {
            if (ProgressiveEra.Length < Level) return 0; else return ProgressiveEra[Level];
         }
         else if (AgeString == "ModernEra")
         {
            if (ModernEra.Length < Level) return 0; else return ModernEra[Level];
         }
         else if (AgeString == "PostModernEra")
         {
            if (PostModernEra.Length < Level) return 0; else return PostModernEra[Level];
         }
         else if (AgeString == "ContemporaryEra")
         {
            if (ContemporaryEra.Length < Level) return 0; else return ContemporaryEra[Level];
         }
         else if (AgeString == "TomorrowEra")
         {
            if (TomorrowEra.Length < Level) return 0; else return TomorrowEra[Level];
         }
         else if (AgeString == "FutureEra")
         {
            if (FutureEra.Length < Level) return 0; else return FutureEra[Level];
         }
         else if (AgeString == "ArcticFuture")
         {
            if (ArcticFuture.Length < Level) return 0; else return ArcticFuture[Level];
         }
         else if (AgeString == "OceanicFuture")
         {
            if (BronzeAge.Length < Level) return 0; else return OceanicFuture[Level];
         }
         else if (AgeString == "VirtualFuture")
         {
            if (VirtualFuture.Length < Level) return 0; else return VirtualFuture[Level];
         }
         else if (AgeString == "SpaceAgeMars")
         {
            if (SpaceAgeMars.Length < Level) return 0; else return SpaceAgeMars[Level];
         }
         else if (AgeString == "SpaceAgeAsteroidBelt")
         {
            if (SpaceAgeAsteroidBelt.Length < Level) return 0; else return SpaceAgeAsteroidBelt[Level];
         }
         else if (AgeString == "AllAge")
         {
            if (AllAge.Length < Level) return 0; else return AllAge[Level];
         }
         else
         {
            return 0;
         }
      }
      public static Bitmap Resize(this Image image, int width, int height)
      {

         var destRect = new Rectangle(0, 0, width, height);
         var destImage = new Bitmap(width, height);

         destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

         using (var graphics = Graphics.FromImage(destImage))
         {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
               wrapMode.SetWrapMode(WrapMode.TileFlipXY);
               graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
         }

         return destImage;
      }
      public async static Task GetPortraitsXML()
      {
         string url = "https://foe##server##.innogamescdn.com/assets/shared/avatars/Portraits.xml";
         string jsonURL = url.Replace("##server##", StaticData.UserData.WorldServer);
         string JSON_FilePath = Path.Combine(StaticData.ProgramPath, Path.GetFileName(jsonURL));
         if (!Directory.Exists(StaticData.ProgramPath)) Directory.CreateDirectory(StaticData.ProgramPath);
         FileInfo fi = new FileInfo(JSON_FilePath);
         Console.Write($"Downloading {Path.GetFileName(jsonURL)} [");
         using (var client = new WebClient())
         {
            Uri uri = new Uri(jsonURL.Replace("'", ""));
            client.Headers.Add("User-Agent", StaticData.UserData.CustomUserAgent);
            client.QueryString.Add("file", JSON_FilePath);
            if (fi.Exists) fi.Delete();
            await client.DownloadFileTaskAsync(uri, JSON_FilePath).ConfigureAwait(false);
         }
      }
      public static void ProcessPortraits()
      {

         GetPortraitsXML().Wait(1000 * 3);
         string JSON_FilePath = Path.Combine(StaticData.ProgramPath, "Portraits.xml");
         Console.WriteLine($"Downloading {JSON_FilePath} complete");
         if (Path.GetExtension(JSON_FilePath) == ".xml")
         {
            var xml = XDocument.Load(JSON_FilePath);
            foreach (var item in xml.Root.Elements("portrait").ToList())
            {
               if (!ListClass.PortraitList.ContainsKey(item.Attribute("name").Value))
                  ListClass.PortraitList.Add(item.Attribute("name").Value, $"https://foe{StaticData.UserData.WorldServer}.innogamescdn.com/assets/shared/avatars/{item.Attribute("src").Value + ".jpg"}");
            }
         }
      }
      public static bool IsChromeInstalled()
      {
         try
         {
            string path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null).ToString();
            if (path != null)
            {
               if (File.Exists("chromedriver.exe")) return true;
               string Version = FileVersionInfo.GetVersionInfo(path).FileVersion;
               Version = Version.Substring(0, Version.LastIndexOf('.'));
               string ChromeDriverVersionURL = $"https://chromedriver.storage.googleapis.com/LATEST_RELEASE_{Version}";
               using (var webClient = new WebClient())
               {
                  webClient.Encoding = Encoding.GetEncoding(1252);
                  string resultStrings = webClient.DownloadString(ChromeDriverVersionURL);
                  string ChromeDriverURL = $"https://chromedriver.storage.googleapis.com/{resultStrings}/chromedriver_win32.zip";
                  File.Delete("chromedriver.zip");
                  webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                  webClient.DownloadFile(ChromeDriverURL, "chromedriver.zip");
                  File.Delete("chromedriver.exe");
                  ZipFile.ExtractToDirectory("chromedriver.zip", ".");
                  File.Delete("chromedriver.zip");
               }

               return true;
            }
            return false;
         }
         catch (Exception)
         {
            return false;
         }
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
         ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMain));
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
            var properties = new Dictionary<string, string> { { "ShowNotify", i18n.getString("ShowingNotify") } };
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
   public enum Eras
   {
      AllAge = 0,
      NoAge = 0,
      StoneAge = 1,
      BronzeAge = 2,
      IronAge = 3,
      EarlyMiddleAge = 4,
      HighMiddleAge = 5,
      LateMiddleAge = 6,
      ColonialAge = 7,
      IndustrialAge = 8,
      ProgressiveEra = 9,
      ModernEra = 10,
      PostModernEra = 11,
      ContemporaryEra = 12,
      TomorrowEra = 13,
      FutureEra = 14,
      ArcticFuture = 15,
      OceanicFuture = 16,
      VirtualFuture = 17,
      SpaceAgeMars = 18,
      SpaceAgeAsteroidBelt = 19,
      SpaceAgeVenus = 20
   };

   public delegate void CustomEvent(object sender, dynamic data = null);
}
