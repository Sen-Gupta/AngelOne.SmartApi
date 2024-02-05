using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Responses
{
    public class LogoutResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public string Data { get; set; } = null!;
    }
}
