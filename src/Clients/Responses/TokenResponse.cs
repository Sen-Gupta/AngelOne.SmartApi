using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Responses
{
    public class TokenResponse : BaseResponse
    {

        [JsonPropertyName("data")]
        public Tokens Tokens { get; set; }
    }

    public class Tokens
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("jwtToken")]
        public string JwtToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("feedToken")]
        public string FeedToken { get; set; }

        [JsonIgnore]
        public DateTime? Expiry { get; set; }
    }
}
