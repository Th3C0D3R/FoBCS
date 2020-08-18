using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using ForgeOfBots.GameClasses;
using ForgeOfBots.GameClasses.ResponseClasses;
using System.ComponentModel;
using ForgeOfBots.DataHandler;

namespace ForgeOfBots.Utils
{
   public static class Helper
   {
      public static void Log(string message)
      {
         //Invoker.CallMethode(ctrl, () => ctrl.Items.Add(message));
         Console.WriteLine(message);
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
      public static string WorldToPlayable(Tuple<string, string, WorldState> world)
      {
         return world.Item1;
      }
      public static Dictionary<string,List<Good>> GetGoodsEraSorted(List<ResearchEra> eraList, Resources resources,List<ResourceDefinition> resourceDefList)
      {
         Dictionary<string, List<Good>> goodList = new Dictionary<string, List<Good>>();
         foreach (ResearchEra era in eraList)
         {
            foreach (ResourceDefinition resDef in resourceDefList)
            {
               if (goodList.ContainsKey(era.name) && goodList[era.name].Count >= 5) break;
               if(resDef.era == era.era)
               {
                  foreach(PropertyInfo prop in resources.GetType().GetProperties())
                  {
                     int.TryParse(prop.GetValue(resources).ToString(), out int amount);
                     if (resDef.id == prop.Name)
                     {
                        if (goodList.ContainsKey(era.name))
                        {
                           goodList[era.name].Add(new Good() { good_id = resDef.id, name = resDef.name , value = amount });
                           break;
                        }
                        else
                        {
                           List<Good> goods = new List<Good>();
                           goods.Add(new Good() { good_id = resDef.id, name = resDef.name, value = amount });
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
   }
   public class WorldData
   {
      public string player_name { get; set; }
      public Dictionary<string,int> player_worlds { get; set; }
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
    public class TwoTArgs<T,Y>
    {
        public T RequestType { get; set; }
        public Y argument2 { get; set; }
    }
}
