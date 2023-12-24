using AngelOne.SmartApi.Clients.Managers;
using AngelOne.SmartApi.Clients.Requests;
using AngelOne.SmartApi.Clients.Responses;
using AngelOne.SmartApi.Clients.Settings;
using AngelOne.SmartApi.Clients.Utilities;

using Microsoft.Extensions.Configuration;

using OtpNet;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AngelOne.SmartApi.Clients
{
    public class AngelOneLoginClient
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly SmartApiSettings _smartApiSettings;
        private readonly TokenManager _tokenManager;
        public AngelOneLoginClient(IConfiguration configuration, 
            HttpClient httpClient,
            TokenManager tokenManager)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
            _tokenManager = tokenManager;
            _smartApiSettings = _configuration.GetSection("SmartApi").Get<SmartApiSettings>();
        }

        public async Task<bool> Login(bool loginToHistoricalDataApi = false)
        {
            try
            {
                System.Console.WriteLine($"Making Login Request at {_httpClient.BaseAddress}.");
                var apiKey = string.Empty;
                
                if (loginToHistoricalDataApi)
                {
                    apiKey = _smartApiSettings.Credentials.HistoricalDataKey;                                       
                }
                else
                {
                    apiKey = _smartApiSettings.Credentials.MarketDataKey;  
                }
                
                if(string.IsNullOrEmpty(apiKey))
                {
                    System.Console.WriteLine("API Key is null or empty. Please check your appsettings.json file.");
                    return false;
                }
                
                await RequestUtility.ApplyHeaders(_httpClient, apiKey, string.Empty);

                var response = await _httpClient.PostAsJsonAsync("", GetLoginRequest());

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await ResponseUtility.ParseTokenResponse(response);

                    if (loginResponse != null &&
                        loginResponse.Status &&
                        loginResponse.Message.ToLower() == "success" &&
                        string.IsNullOrEmpty(loginResponse.ErrorCode))
                    {
                        // Successfully logged in
                        _tokenManager.SetLoginToken(loginResponse.Tokens);
                        return true;
                    }
                    else
                    {
                        // Login response indicates failure
                        ResponseUtility.HandleLoginFailure(loginResponse);
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

            return false;
        }
        private LoginRequest GetLoginRequest()
        {
            LoginRequest loginRequest = new LoginRequest();
            loginRequest.ClientCode = _smartApiSettings.Credentials.ClientCode;
            loginRequest.PIN = _smartApiSettings.Credentials.ClientPIN;
            loginRequest.TOTP = new Totp(Base32Encoding.ToBytes(_smartApiSettings.Credentials.TOTPCode)).ComputeTotp();
            return loginRequest;
        }
    }
}
