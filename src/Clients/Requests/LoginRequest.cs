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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull )]
        public string ClientCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("password")]
        public string PIN { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("totp")]
        public string TOTP { get; set; }
    }
    
}
