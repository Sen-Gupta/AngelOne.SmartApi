using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients
{
    public class AngelOneTokenClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AngelOneTokenClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
    }
}
