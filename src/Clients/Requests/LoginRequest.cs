using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Requests
{
    public class LoginRequest
    {
        [JsonPropertyName("clientcode")]
        public string ClientCode { get; set; }

        [JsonPropertyName("password")]
        public string PIN { get; set; }

        [JsonPropertyName("totp")]
        public string TOTP { get; set; }
    }
    
}
