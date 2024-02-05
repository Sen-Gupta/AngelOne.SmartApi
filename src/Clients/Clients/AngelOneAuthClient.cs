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
    public class AngelOneAuthClient
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly SmartApiSettings _smartApiSettings;
        private readonly TokenManager _tokenManager;
        public AngelOneAuthClient(IConfiguration configuration,
            HttpClient httpClient,
            TokenManager tokenManager)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
            _tokenManager = tokenManager;
            _smartApiSettings = _configuration.GetSection("SmartApi").Get<SmartApiSettings>()!;
        }

        public async Task<bool> Login(bool IsHistorical = false)
        {
            try
            {
                System.Console.WriteLine($"Making Login Request at {_httpClient.BaseAddress}.");
                var apiKey = _smartApiSettings?.GetAPIKey(IsHistorical);

                if (string.IsNullOrEmpty(apiKey))
                {
                    System.Console.WriteLine("API Key is null or empty. Please check your appsettings.json file.");
                    return false;
                }

                await RequestUtility.ApplyHeaders(_httpClient, apiKey, string.Empty);

                var response = await _httpClient.PostAsJsonAsync("", GetLoginRequest());

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await ResponseUtility.ParseResponse<TokenResponse>(response);

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

        public async Task<bool> EnsureSession(bool IsHistorical = false)
        {
            var IsLoginValid = _tokenManager.IsLoginValid(IsHistorical);
            if (IsLoginValid)
            {
                return true;
            }
            else
            {
                return await Login(IsHistorical);
            }
        }


        public async Task<bool> Logout()
        {
            HttpClient logOutClient = new HttpClient();
            logOutClient.BaseAddress = new Uri(Constants.Endpoints.BaseUrls.API);
            try
            {
                //We need the API Key to make the request
                var apiKey = _smartApiSettings?.GetAPIKey();
                if (string.IsNullOrEmpty(apiKey))
                {
                    System.Console.WriteLine("API Key is null or empty. Please check your appsettings.json file.");
                    return false;
                }

                var loginToken = _tokenManager.GetAPIToken();

                await RequestUtility.ApplyHeaders(logOutClient, apiKey, loginToken.JwtToken);

                var response = await logOutClient.PostAsJsonAsync(Constants.Endpoints.Logout, GetLogoutRequest());

                if (response.IsSuccessStatusCode)
                {
                    var logoutResponse = await ResponseUtility.ParseResponse<LogoutResponse>(response);

                    if (logoutResponse != null &&
                        logoutResponse.Status &&
                        logoutResponse.Message.ToLower() == "success" &&
                        string.IsNullOrEmpty(logoutResponse.ErrorCode))
                    {
                        return true;
                    }
                    else
                    {
                        // Login response indicates failure
                        ResponseUtility.HandleLoginFailure(logoutResponse);
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

        private LoginRequest GetLogoutRequest()
        {
            LoginRequest logoutRequest = new LoginRequest();
            logoutRequest.ClientCode = _smartApiSettings.Credentials.ClientCode;
            return logoutRequest;
        }

    }
}
