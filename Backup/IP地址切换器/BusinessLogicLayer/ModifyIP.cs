using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows.Forms;

using IP地址切换器.BusinessObjectLayer;

namespace IP地址切换器.BusinessLogicLayer
{
    class ModifyIP 
    {
        public bool ModIP(IPAddress myIP)
        {
            bool isSuccess = false;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    myIP.Mac = mo["MacAddress"].ToString();
                    try
                    {
                        //设置IP地址和子网掩码
                        ManagementBaseObject newIPSubMask = mo.GetMethodParameters("EnableStatic");
                        newIPSubMask["IPAddress"] = new string[] { myIP.Ip };
                        newIPSubMask["SubnetMask"] = new string[] { myIP.Subnetmask };
                        ManagementBaseObject setIPSubMask = mo.InvokeMethod("EnableStatic", newIPSubMask, null);
                        //设置
                        //ManagementBaseObject newSubMask = mo.GetMethodParameters("EnableStatic");
                        //newSubMask["SubnetMask"] = new string[] { myIP.Subnetmask };
                        //ManagementBaseObject setSubMask = mo.InvokeMethod("EnableStatic", newSubMask, null);

                        //设置网关
                        ManagementBaseObject newGate = mo.GetMethodParameters("SetGateways");
                        newGate["DefaultIPGateway"] = new string[] { myIP.Gateway };
                        ManagementBaseObject setGateway = mo.InvokeMethod("SetGateways", newGate, null);

                        //设置DNS
                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = new string[] { myIP.Dns1, myIP.Dns2 };
                        ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        isSuccess = true;                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("原因：" + ex);
                        isSuccess = false;
                    }
                }
            }
            return isSuccess;
        }
        /// <summary>
        /// 获取本机MAC地址
        /// </summary>
        /// <returns></returns>
        public  string GetMAC()
        {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    mac = mo["MacAddress"].ToString();
                }
            }
            return mac;
        }
    }
}
