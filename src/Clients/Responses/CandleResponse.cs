using AngelOne.SmartApi.Clients.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Responses
{
    public class CandleResponse: BaseResponse
    {
        [JsonPropertyName("data")]
        public List<JsonDocument> Results { get; set; }

        public List<CandleData> GetCandle()
        {
            return Results?.Select(c => new CandleData(c)).ToList();
        }
    }
}
