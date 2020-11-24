using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.Utils.Premium
{
   public class FirewallHelper
   {
      public static void OpenFirewallPort(int port)
      {
         try
         {
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            try
            {
               while (firewallPolicy.Rules.Item("ForgeOfBots").Enabled)
                  firewallPolicy.Rules.Remove("ForgeOfBots");
            }
            catch (System.IO.FileNotFoundException)
            { }
         }
         catch (Exception)
         { }
         finally
         {
            INetFwRule firewallRuleOut = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRuleOut.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRuleOut.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            firewallRuleOut.Description = "Allow Connection for ForgeofBots-Service";
            firewallRuleOut.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            firewallRuleOut.LocalPorts = port.ToString();
            firewallRuleOut.RemotePorts = port.ToString();
            firewallRuleOut.Enabled = true;
            firewallRuleOut.InterfaceTypes = "All";
            firewallRuleOut.Name = "ForgeOfBots";

            INetFwRule firewallRuleIn = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            firewallRuleIn.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            firewallRuleIn.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            firewallRuleIn.Description = "Allow Connection for ForgeofBots-Service";
            firewallRuleIn.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            firewallRuleIn.LocalPorts = port.ToString();
            firewallRuleIn.RemotePorts = port.ToString();
            firewallRuleIn.Enabled = true;
            firewallRuleIn.InterfaceTypes = "All";
            firewallRuleIn.Name = "ForgeOfBots";

            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            firewallPolicy.Rules.Add(firewallRuleOut);
            firewallPolicy.Rules.Add(firewallRuleIn);
         }
      }
   }
}
