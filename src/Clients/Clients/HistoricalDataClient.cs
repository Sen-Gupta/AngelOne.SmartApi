using Microsoft.Extensions.Configuration;
using AngelOne.SmartApi.Clients.Managers;
using AngelOne.SmartApi.Clients.Requests;
using AngelOne.SmartApi.Clients.Responses;
using AngelOne.SmartApi.Clients.Settings;
using AngelOne.SmartApi.Clients.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;

namespace AngelOne.SmartApi.Clients
{
    public class HistoricalDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly SmartApiSettings _smartApiSettings;
        private readonly TokenManager _tokenManager;
        private readonly AngelOneTokenClient _angelOneTokenClient;

        public HistoricalDataClient(IConfiguration configuration, 
            HttpClient httpClient,
            TokenManager tokenManager,
            AngelOneTokenClient angelOneTokenClient
            )
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
            _tokenManager = tokenManager;
            _angelOneTokenClient = angelOneTokenClient;
            _smartApiSettings = _configuration.GetSection("SmartApi").Get<SmartApiSettings>();
        }

        /// <summary>
        /// Quote Request
        /// </summary>
        /// <param name="candleRequest">Contains Mode, Exchanges and list of Angel Tokens</param>
        /// <returns>QuoteResult List containing failed and succecced results</returns>
        public async Task<CandleResponse> GetCandle(CandleRequest candleRequest)
        {
            try
            {
                System.Console.WriteLine($"Making Candle Request at {_httpClient.BaseAddress}{Constants.Endpoints.Candle}.");
                var candleRequestJson = JsonSerializer.Serialize(candleRequest);
                System.Console.WriteLine($"Posted Data: {candleRequestJson}");

                //We need the API Key to make the request
                var apiKey = _smartApiSettings?.GetAPIKey();
                if (string.IsNullOrEmpty(apiKey))
                {
                    System.Console.WriteLine("API Key is null or empty. Please check your appsettings.json file.");
                    return null;
                }

                //Ensure Token is valid
                var IsSessionValid = await _angelOneTokenClient.EnsureSession();
                if (!IsSessionValid)
                {
                    return null;
                }

                var apiToken = _tokenManager.GetAPIToken();

                await RequestUtility.ApplyHeaders(_httpClient, apiKey, apiToken.JwtToken);

                var response = await _httpClient.PostAsJsonAsync(Constants.Endpoints.Candle, candleRequest);

                if (response.IsSuccessStatusCode)
                {
                    var candleResponse = await ResponseUtility.ParseResponse<CandleResponse>(response);

                    if (candleResponse != null &&
                        candleResponse.Status &&
                        candleResponse.Message.ToLower() == "success" &&
                        string.IsNullOrEmpty(candleResponse.ErrorCode))
                    {
                        return candleResponse;
                    }
                    else
                    {
                        // Login response indicates failure
                        ResponseUtility.HandleLoginFailure(candleResponse);
                    }
                }
                else
                {
                    // HTTP request to login endpoint failed
                    ResponseUtility.HandleHttpRequestFailure(response);
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                ResponseUtility.HandleUnexpectedException(ex);
            }

            return null;
        }
    }
}
