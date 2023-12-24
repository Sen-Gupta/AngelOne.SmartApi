using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Requests
{
    public class CandleRequest
    {
        [JsonPropertyName("symboltoken")]
        public string SymbolToken { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("interval")]
        public string Interval { get; set; }

        [JsonPropertyName("fromdate")]
        public string FromDate { get; set; }

        [JsonPropertyName("todate")]
        public string ToDate { get; set; }
    }
}
