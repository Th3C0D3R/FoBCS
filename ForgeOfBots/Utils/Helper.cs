using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace ForgeOfBots.Utils
{
   public static class Helper
   {
      public static void Log(ListBox ctrl, string message)
      {
         Invoker.CallMethode(ctrl, () => ctrl.Items.Add(message));
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
}
