using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.ClientOptions
{
    public class MarketDataClientOptions
    {
        public string BaseEndpoint { get; set; } = null!;
        public string ClientId { get; set; } = null!;    
        public string ClientPin { get; set; } = null!;
    }
}
