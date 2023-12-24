﻿using AngelOne.SmartApi.Clients.Managers;
using AngelOne.SmartApi.Clients.Requests;
using AngelOne.SmartApi.Clients.Responses;
using AngelOne.SmartApi.Clients.Settings;
using AngelOne.SmartApi.Clients.Utilities;

using Microsoft.Extensions.Configuration;

using OtpNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients
{
    public class AngelOneTokenClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly SmartApiSettings _smartApiSettings;
        private readonly TokenManager _tokenManager;
        private readonly AngelOneLoginClient _angelOneloginClient;
        public AngelOneTokenClient(IConfiguration configuration, 
            HttpClient httpClient,
            TokenManager tokenManager,
            AngelOneLoginClient angelOneLoginClient
            )
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
            _tokenManager = tokenManager;
            _angelOneloginClient = angelOneLoginClient;
            _smartApiSettings = _configuration.GetSection("SmartApi").Get<SmartApiSettings>();
        }

        public async Task<bool> EnsureSession(bool IsHistorical = false)
        {
            var IsLoginValid = _tokenManager.IsTokenValid(IsHistorical);
            if (IsLoginValid)
            {
                return true;
            }
            else
            {
                return await GetToken(IsHistorical);
            }
        }

        //We need to Login to get the Token
        public async Task<bool> GetToken(bool IsHistorical = false)
        {
            try
            {
                System.Console.WriteLine($"Making Token Request at {_httpClient.BaseAddress}.");

                //We need the API Key to make the request
                var apiKey = _smartApiSettings?.GetAPIKey(IsHistorical);    
                if (string.IsNullOrEmpty(apiKey))
                {
                    System.Console.WriteLine("API Key is null or empty. Please check your appsettings.json file.");
                    return false;
                }


                //Ensure Login Session is valid
                var IsLoginValid = await _angelOneloginClient.EnsureSession(IsHistorical);
                if(!IsLoginValid)
                {
                    return false;
                }

                var loginToken = _tokenManager.GetLoginToken(IsHistorical);

                await RequestUtility.ApplyHeaders(_httpClient, apiKey, loginToken.JwtToken);

                var response = await _httpClient.PostAsJsonAsync("", GetTokenRequest(loginToken.RefreshToken));

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await ResponseUtility.ParseResponse<TokenResponse>(response);

                    if (tokenResponse != null &&
                        tokenResponse.Status &&
                        tokenResponse.Message.ToLower() == "success" &&
                        string.IsNullOrEmpty(tokenResponse.ErrorCode))
                    {
                        // Successfully logged in
                        _tokenManager.SetAPIToken(tokenResponse.Tokens, IsHistorical);
                        return true;
                    }
                    else
                    {
                        // Login response indicates failure
                        ResponseUtility.HandleLoginFailure(tokenResponse);
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
        private Tokens GetTokenRequest(string token)
        {
            Tokens tokenRequest = new Tokens();
            tokenRequest.RefreshToken = token;
            return tokenRequest;
        }

    }
}
