using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ForgeOfBots.Utils.Helper;

namespace ForgeOfBots.Utils.Premium
{
   public static class TcpConnection
   {
      static readonly int port = 1337;
      static bool done = false;
      static object ret = null;
#if DEBUG
      static readonly string IP = "134.255.216.102";
#elif RELEASE
      static string IP = "134.255.216.102";
#endif
      public static object SendAuthData(string serial, string hwid, bool CheckOnly = false)
      {
         FirewallHelper.OpenFirewallPort(port);
         IPAddress ip = IPAddress.Parse(IP);
         TcpClient client = new TcpClient();
         try
         {
            client.Connect(ip, port);
            NetworkStream ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o, serial, hwid, CheckOnly));
            thread.Start(client);
            byte[] buffer = Encoding.ASCII.GetBytes($"serial:{serial}|hwid:{hwid}|checkOnly:{CheckOnly}");
            ns.Write(buffer, 0, buffer.Length);
            thread.Join();
            while (!done)
               Application.DoEvents();
            try
            {
               ns.Close();
               client.Client.Shutdown(SocketShutdown.Receive);
               client.Client.Close();
               client.Close();
            }
            catch (Exception)
            {
               ns.Dispose();
               client.Dispose();
            }
            if (ret != null)
               return ret;
            else
               return false;
         }
         catch (Exception)
         {
            return false;
         }
      }
      private static void ReceiveData(TcpClient client,string serial,string hwid,bool CheckOnly = false)
      {
         using (BinaryReader reader = new BinaryReader(client.GetStream()))
         {
            int length = reader.ReadInt32();
            int received = 0;
            byte[] buffer = new byte[length];
            while (received < length)
            {
               int read = reader.Read(buffer, received, length - received);
               received += read;
            };
            string dataRes = Encoding.ASCII.GetString(buffer, 0, length);
            if (!CheckOnly && 
               dataRes != "4728u u2051 0u4y4 0z9yx 471vw 2531z y4".ToLower() &&
               dataRes != "8y4w3 3zv22 5x430 8yv3v x14w5 v6638 24".ToLower())
            {
               length = reader.ReadInt32();
               received = 0;
               buffer = new byte[length];
               while (received < length)
               {
                  int read = reader.Read(buffer, received, length - received);
                  received += read;
               };
            }
            if (dataRes == "xv2v3 3uv1y 004y8 9z5xz w3xuy xzvw3 z0".ToLower() || dataRes == "0yy92 y780v y3476 uz2v7 w9847 z45y6 uv".ToLower())
            {
               if(!CheckOnly && buffer.Length == length)
               {
                  var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                  if (domainAssemblies.Find(da => da.GetName().Name == "FoBPremiumNET") == null)
                  {
                     Main.PremAssembly = Assembly.Load(buffer);
                     ExecuteMethod(Main.PremAssembly, "EntryPoint", "LoadPremium", new object[] { serial, hwid });
                     ret = Result.Success;
                     done = true;
                  }
               }else if (CheckOnly)
               {
                  ret = Result.Success;
                  done = true;
               }
            }
            else if(dataRes == "4728u u2051 0u4y4 0z9yx 471vw 2531z y4".ToLower())
            {
               ret = Result.Expired;
               done = true;
            }
            else if (dataRes == "8y4w3 3zv22 5x430 8yv3v x14w5 v6638 24".ToLower())
            {
               ret = Result.Failed;
               done = true;
            }
         };
      }
   }
}
