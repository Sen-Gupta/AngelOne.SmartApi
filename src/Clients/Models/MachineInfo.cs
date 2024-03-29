﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models
{
    public class MachineInfo
    {
        [JsonPropertyName("localIp")]
        public string LocalIp { get; set; } = null!;

        [JsonPropertyName("publicIp")]
        public string PublicIp { get; set; } = null!;

        [JsonPropertyName("macAddress")]
        public string MacAddress { get; set; } = null!;
    }
}
