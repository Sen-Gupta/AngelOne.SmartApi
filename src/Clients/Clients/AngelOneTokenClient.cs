using AngelOne.SmartApi.Clients.Settings;

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
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
        }

        //We need to Login to get the Token
        public async Task<string> GetToken()
        {
            var smartApiSettings = _configuration.GetSection("SmartApi").Get<SmartApiSettings>();
            System.Console.WriteLine($"Making Login Request at {_httpClient.BaseAddress}.");
            return await Task.FromResult(string.Empty);
        }

       
    }
}
