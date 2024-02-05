using System;
using System.IdentityModel.Tokens.Jwt;

using AngelOne.SmartApi.Clients.Responses;

using Microsoft.IdentityModel.Tokens;

namespace AngelOne.SmartApi.Clients.Managers
{
    public class TokenManager
    {
        private Tokens _loginToken = null!;
        private Tokens _smartAPIToken = null!;



        public bool IsLoginValid()
        {
            return _loginToken?.Expiry > DateTime.UtcNow;
        }

        /// <summary>
        /// We need to validate the access token validity
        /// </summary>
        /// <param name="IsHistorical"></param>
        /// <returns></returns>
        public bool IsTokenValid()
        {
            return _smartAPIToken != null;
        }

        public bool IsAccessTokenValid(string accessToken, string secretKey)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false  // Disable lifetime validation for checking expiration manually
            };

            try
            {
                SecurityToken securityToken;
                var claimsPrincipal = handler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

                // Check the expiration claim manually
                var expirationClaim = claimsPrincipal.FindFirst("exp");
                if (expirationClaim != null && long.TryParse(expirationClaim.Value, out long expirationTime))
                {
                    // Compare with the current time
                    var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    return expirationTime > currentTime;
                }

                return false; // No expiration claim found
            }
            catch (SecurityTokenException)
            {
                return false; // Token validation failed
            }
        }

        //Setters
        public void SetLoginToken(Tokens token)
        {
            //Token is expired in 5:30 AM from the date of login
            token.Expiry = DateTime.UtcNow.Date.AddDays(1).AddHours(5).AddMinutes(30);
            _loginToken = token;

        }

        public void SetAPIToken(Tokens token)
        {
            _smartAPIToken = token;
        }

        //Getters
        public Tokens GetLoginToken()
        {
            return _loginToken;
        }
        public Tokens GetAPIToken()
        {
            return _smartAPIToken;
        }
    }
}
