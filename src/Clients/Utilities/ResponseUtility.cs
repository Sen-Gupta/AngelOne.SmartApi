using AngelOne.SmartApi.Clients.Responses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Utilities
{
    public static class ResponseUtility
    {
        public static async Task<TokenResponse> ParseTokenResponse(HttpResponseMessage response)
        {
            try
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                if (!string.IsNullOrEmpty(responseString))
                {
                    return JsonSerializer.Deserialize<TokenResponse>(responseString);
                }
                return null;
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing errors
                System.Console.WriteLine($"Error parsing TokenResponse JSON: {ex.Message}");
                // Log the exception, report it, or perform any necessary actions
                return null;
            }
        }

        public static void HandleLoginFailure(TokenResponse loginResponse)
        {
            // Handle specific failure scenarios based on the content of loginResponse
            System.Console.WriteLine($"Login failed. Status: {loginResponse?.Status}, Message: {loginResponse?.Message}, ErrorCode: {loginResponse?.ErrorCode}");
            // Additional error handling logic if needed
        }

        public static void HandleHttpRequestFailure(HttpResponseMessage response)
        {
            // Handle failure scenarios related to the HTTP request
            System.Console.WriteLine($"HTTP request to login endpoint failed. Status Code: {response?.StatusCode}");
            // Additional error handling logic if needed
        }

        public static void HandleUnexpectedException(Exception ex)
        {
            // Handle unexpected exceptions
            System.Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            // Log the exception, report it, or perform any necessary actions
        }
    }
}
