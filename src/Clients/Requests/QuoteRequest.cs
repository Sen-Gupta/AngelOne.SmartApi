using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Requests
{
    public class QuoteRequest
    {
        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        [JsonPropertyName("exchangeTokens")]
        public Dictionary<string, List<string>> ExchangeNameTokens { get; set; } = new Dictionary<string, List<string>>();
    }
}
