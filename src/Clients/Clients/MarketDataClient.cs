using AngelOne.SmartApi.Clients.Managers;
using AngelOne.SmartApi.Clients.Models;
using AngelOne.SmartApi.Clients.Requests;
using AngelOne.SmartApi.Clients.Responses;
using AngelOne.SmartApi.Clients.Settings;
using AngelOne.SmartApi.Clients.Utilities;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients
{
    public class MarketDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly SmartApiSettings _smartApiSettings;
        private readonly TokenManager _tokenManager;
        private readonly AngelOneTokenClient _angelOneTokenClient;

        public MarketDataClient(IConfiguration configuration, 
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
        /// Get's the Profile
        /// </summary>
        /// <returns>Return<ref>Profile</ref> or null</returns>
        public async Task<Profile> GetProfile()
        {
            try
            {
                System.Console.WriteLine($"Making Profile Request at {_httpClient.BaseAddress}.");

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

                var response = await _httpClient.GetAsync(_smartApiSettings.Endpoints.Profile);

                if (response.IsSuccessStatusCode)
                {
                    var profileResponse = await ResponseUtility.ParseResponse<ProfileResponse>(response);

                    if (profileResponse != null &&
                        profileResponse.Status &&
                        profileResponse.Message.ToLower() == "success" &&
                        string.IsNullOrEmpty(profileResponse.ErrorCode))
                    {
                        return profileResponse.Profile;
                    }
                    else
                    {
                        // Login response indicates failure
                        ResponseUtility.HandleLoginFailure(profileResponse);
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


        public async Task<List<Quote>> GetQuotes(QuoteRequest quoteRequest)
        {
            List<Quote> quotes = new List<Quote>();
            try
            {
                System.Console.WriteLine($"Making Quotes Request at {_httpClient.BaseAddress}{_smartApiSettings.Endpoints.Quote}.");
                var quoteRequestJson = JsonSerializer.Serialize(quoteRequest);
                System.Console.WriteLine($"Posted Data: {quoteRequestJson}");

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

                var response = await _httpClient.PostAsJsonAsync(_smartApiSettings.Endpoints.Quote, quoteRequest);

                if (response.IsSuccessStatusCode)
                {
                    var quoteResponse = await ResponseUtility.ParseResponse<QuoteResponse>(response);

                    if (quoteResponse != null &&
                        quoteResponse.Status &&
                        quoteResponse.Message.ToLower() == "success" &&
                        string.IsNullOrEmpty(quoteResponse.ErrorCode))
                    {
                        return quoteResponse.Results.Quotes;
                    }
                    else
                    {
                        // Login response indicates failure
                        ResponseUtility.HandleLoginFailure(quoteResponse);
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

            return quotes;
        }
    }
}
