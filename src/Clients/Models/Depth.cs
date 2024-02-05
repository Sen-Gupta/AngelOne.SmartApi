using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models
{
    public class Depth
    {
        [JsonPropertyName("buy")]
        public List<DepthItem> Buy { get; set; } = null!;

        [JsonPropertyName("sell")]
        public List<DepthItem> Sell { get; set; } = null!;
    }
}
