using AngelOne.SmartApi.Clients.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Utilities
{
    public static class MachineInfoUtility
    {
        private static MachineInfo _machineInformation = null!;

        #region Private Methods
        public static async Task<MachineInfo> GetMachineInfo()
        {
            if(_machineInformation == null)
            {
                MachineInfo machineInfo = new MachineInfo();

                // Local IP Address
                machineInfo.LocalIp = GetLocalIPAddress();

                // Public IP Address
                machineInfo.PublicIp = await GetPublicIPAddressAsync();

                // MAC Address
                machineInfo.MacAddress = GetMacAddress();
                _machineInformation = machineInfo;
            }
            return _machineInformation;
        }
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }
        private static async Task<string> GetPublicIPAddressAsync()
        {
            string publicIp = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    string publicIpJson = await client.GetStringAsync("https://api64.ipify.org?format=json");
                    var ipObject = JsonSerializer.Deserialize<Dictionary<string, string>>(publicIpJson);
                    publicIp = ipObject?["ip"]!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving public IP address: {ex.Message}");
            }
            //Return a hard coded IP address if the public IP address cannot be retrieved
            if (string.IsNullOrEmpty(publicIp))
            {
                publicIp = "103.196.222.91";
            }
            return publicIp;
        }
        private static string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    PhysicalAddress mac = nic.GetPhysicalAddress();
                    return mac.ToString();
                }
            }
            return "fe80::216e:6507:4b90:3719";
        }
        #endregion
    }
}
