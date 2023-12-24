using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AngelOne.SmartApi.Clients
{
    public class AngelOneLoginClient
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AngelOneLoginClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
    }
}
