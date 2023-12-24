using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients
{
    public class MarketDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public MarketDataClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
    }
}
