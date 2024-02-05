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
        public string SymbolToken { get; set; } = null!;

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; } = null!;

        [JsonPropertyName("interval")]
        public string Interval { get; set; } = null!;

        [JsonPropertyName("fromdate")]
        public string FromDate { get; set; } = null!;

        [JsonPropertyName("todate")]
        public string ToDate { get; set; } = null!;
    }
}
