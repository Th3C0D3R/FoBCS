using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.Utils
{
   public enum ProductionState
   {
      Idle,
      Producing,
      Finished
   }

   public class EventArgsValued : EventArgs
   {
      public dynamic data = null;
   }
   public static class Extensions
   {
      public static bool HasCityID(this List<Tuple<string, string, WorldState>> list, string ID)
      {
         foreach (Tuple<string, string, WorldState> item in list)
            if (item.Item1 == ID) return true;
         return false;
      }
      public static List<Tuple<string, string, WorldState>> ChangeTuple(this List<Tuple<string, string, WorldState>> list, string id = null, string name = null, WorldState state = WorldState.none)
      {
         List<Tuple<string, string, WorldState>> newList = new List<Tuple<string, string, WorldState>>();
         foreach (Tuple<string, string, WorldState> item in list)
         {
            if (id != null)
            {
               if (id == item.Item1)
               {
                  if (name != null)
                  {
                     if (state != WorldState.none)
                        newList.Add(new Tuple<string, string, WorldState>(id, name, state));
                     else
                        newList.Add(new Tuple<string, string, WorldState>(id, name, item.Item3));
                  }
                  else
                     newList.Add(new Tuple<string, string, WorldState>(id, item.Item2, item.Item3));
               }
               else
                  newList.Add(item);
            }
            else
               newList.Add(item);
         }
         return newList;
      }
      public static void AddDistinctRange<P>(this List<P> list, params List<P>[] lists) where P : Player
      {
         foreach (List<P> item in lists.ToList())
         {
            foreach (P obj in item)
            {
               if (!list.Exists(x => x.player_id == obj.player_id))
                  list.Add(obj);
            }
         }
      }
      public static ProductionOption GetProductionOption(int time = 5)
      {
         List<ProductionOption> prodOption = new List<ProductionOption>
         {
            new ProductionOption { id = 1, text = "5min", time = 5 },
            new ProductionOption { id = 2, text = "15min", time = 15 },
            new ProductionOption { id = 3, text = "1h", time = 60 },
            new ProductionOption { id = 4, text = "4h", time = 240 },
            new ProductionOption { id = 5, text = "8h", time = 480 },
            new ProductionOption { id = 6, text = "1d", time = 1440 }
         };
         return prodOption.Find(e => e.time == time);
      }
      public static ProductionOption GetGoodProductionOption(int time = 240)
      {
         List<ProductionOption> prodOption = new List<ProductionOption>
         {
            new ProductionOption { id = 1, text = "4h", time = 240 },
            new ProductionOption { id = 2, text = "8h", time = 480 },
            new ProductionOption { id = 3, text = "1d", time = 1440 },
            new ProductionOption { id = 4, text = "2d", time = 2880 }
         };
         return prodOption.Find(e => e.time == time);
      }
      public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
      {
         if (assembly == null) throw new ArgumentNullException(nameof(assembly));
         try
         {
            return assembly.GetTypes();
         }
         catch (ReflectionTypeLoadException e)
         {
            return e.Types.Where(t => t != null);
         }
      }
   }
   /// <summary>
   /// Generates a 16 byte Unique Identification code of a computer
   /// Example: 4876-8DB5-EE85-69D3-FE52-8CF7-395D-2EA9
   /// </summary>
   public static class FingerPrint
   {
      private static string fingerPrint = string.Empty;
      public static string Value()
      {
         if (string.IsNullOrEmpty(fingerPrint))
            fingerPrint = GetHash("CPU >> " + cpuId() + "\nBIOS >> " + biosId() + "\nBASE >> " + baseId() +/*"\nDISK >> "+ diskId() + "\nVIDEO >> " + */ videoId() + "\nMAC >> " + macId());
         return fingerPrint;
      }
      private static string GetHash(string s)
      {
         MD5 sec = new MD5CryptoServiceProvider();
         ASCIIEncoding enc = new ASCIIEncoding();
         byte[] bt = enc.GetBytes(s);
         return GetHexString(sec.ComputeHash(bt));
      }
      private static string GetHexString(byte[] bt)
      {
         string s = string.Empty;
         for (int i = 0; i < bt.Length; i++)
         {
            byte b = bt[i];
            int n, n1, n2;
            n = (int)b;
            n1 = n & 15;
            n2 = (n >> 4) & 15;
            if (n2 > 9)
               s += ((char)(n2 - 10 + (int)'A')).ToString();
            else
               s += n2.ToString();
            if (n1 > 9)
               s += ((char)(n1 - 10 + (int)'A')).ToString();
            else
               s += n1.ToString();
            if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "";
         }
         return s;
      }
      #region Original Device ID Getting Code
      //Return a hardware identifier
      private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
      {
         string result = "";
         ManagementClass mc = new ManagementClass(wmiClass);
         ManagementObjectCollection moc = mc.GetInstances();
         foreach (ManagementObject mo in moc)
         {
            if (mo[wmiMustBeTrue].ToString() == "True")
            {
               if (result == "")
               {
                  try
                  {
                     result = mo[wmiProperty].ToString();
                     break;
                  }
                  catch
                  {
                  }
               }
            }
         }
         return result;
      }
      private static string identifier(string wmiClass, string wmiProperty)
      {
         string result = "";
         ManagementClass mc = new ManagementClass(wmiClass);
         ManagementObjectCollection moc = mc.GetInstances();
         foreach (ManagementObject mo in moc)
         {
            if (result == "")
            {
               try
               {
                  result = mo[wmiProperty]?.ToString();
                  break;
               }
               catch
               {
               }
            }
         }
         return result;
      }
      private static string cpuId()
      {
         string retVal = identifier("Win32_Processor", "UniqueId");
         if (retVal == "") //If no UniqueID, use ProcessorID
         {
            retVal = identifier("Win32_Processor", "ProcessorId");
            if (retVal == "") //If no ProcessorId, use Name
            {
               retVal = identifier("Win32_Processor", "Name");
               if (retVal == "") //If no Name, use Manufacturer
                  retVal = identifier("Win32_Processor", "Manufacturer");
               //Add clock speed for extra security
               retVal += identifier("Win32_Processor", "MaxClockSpeed");
            }
         }
         return retVal;
      }
      //BIOS Identifier
      private static string biosId()
      {
         return identifier("Win32_BIOS", "Manufacturer")
         + identifier("Win32_BIOS", "SMBIOSBIOSVersion")
         + identifier("Win32_BIOS", "IdentificationCode")
         + identifier("Win32_BIOS", "SerialNumber")
         + identifier("Win32_BIOS", "ReleaseDate")
         + identifier("Win32_BIOS", "Version");
      }
      //Motherboard ID
      private static string baseId()
      {
         return identifier("Win32_BaseBoard", "Model")
         + identifier("Win32_BaseBoard", "Manufacturer")
         + identifier("Win32_BaseBoard", "Name")
         + identifier("Win32_BaseBoard", "SerialNumber");
      }
      //Primary video controller ID
      private static string videoId()
      {
         return identifier("Win32_VideoController", "DriverVersion")
         + identifier("Win32_VideoController", "Name");
      }
      //First enabled network card ID
      private static string macId()
      {
         return identifier("Win32_NetworkAdapterConfiguration",
             "MACAddress", "IPEnabled");
      }
      #endregion
   }
   public static class Identifier
   {
      public static string GetInfo(string qry, string prop)
      {
         ManagementObjectSearcher searcher;
         try
         {
            searcher = new ManagementObjectSearcher("SELECT * FROM " + qry);
            foreach (ManagementObject mo in searcher.Get())
            {
               PropertyDataCollection searcherProperties = mo.Properties;
               return searcherProperties[prop].Value.ToString() ?? "";
            }
         }
         catch (Exception ex)
         {}
         return "";
      }
   }
   public class ProductionOption
   {
      public int id;
      public int time;
      public string text;
   }

   public class PlayAbleWorldItem
   {
      public string WorldName;
      public string WorldID;
      public override string ToString()
      {
         return WorldName;
      }
   }

   public class LanguageItem
   {
      public int Language;
      public string Code;
      public string Description;

      public override string ToString()
      {
         return $"{Description}";
      }
   }

}
