using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AngelOne.SmartApi.Clients.Models;

namespace AngelOne.SmartApi.Clients.Utilities
{
    public static class RequestUtility
    {
        public static async Task<HttpClient> ApplyHeaders(HttpClient httpClient, string privateKey, string token)
        {
            var machineInfo = await MachineInfoUtility.GetMachineInfo();
            var headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                    { "X-UserType", "USER" },
                    { "X-SourceID", "WEB" },
                    { "X-ClientLocalIP", machineInfo.LocalIp },
                    { "X-ClientPublicIP", machineInfo.PublicIp },
                    { "X-MACAddress", machineInfo.MacAddress }
                };

            if (!string.IsNullOrEmpty(privateKey))
            {
                headers.Add("X-PrivateKey", privateKey);
            }

            if (!string.IsNullOrEmpty(token))
            {
                headers.Add("Authorization", $"Bearer {token}");
            }

            // Assign the modified request to the existing HttpClient
            httpClient.DefaultRequestHeaders.Clear();
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            return httpClient;
        }
    }
}
