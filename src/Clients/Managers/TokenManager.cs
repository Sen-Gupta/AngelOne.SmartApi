using System;
using System.IdentityModel.Tokens.Jwt;

using AngelOne.SmartApi.Clients.Responses;

using Microsoft.IdentityModel.Tokens;

namespace AngelOne.SmartApi.Clients.Managers
{
    public class TokenManager
    {

        private Tokens _loginToken = new Tokens();
        private Tokens _MarketDataAPIToken = new Tokens();
        private Tokens _HistoricalDataAPIToken = new Tokens();


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
            _loginToken = token;
            //Token is expired in 5:30 AM from the date of login
            _loginToken.Expiry = DateTime.UtcNow.Date.AddDays(1).AddHours(5).AddMinutes(30);
        }
        public void SetMarketDataAPIToken(Tokens token)
        {
            _MarketDataAPIToken = token;
        }

        public void SetHistoricalDataAPIToken(Tokens token)
        {
            _HistoricalDataAPIToken = token;
        }

        //Getters
        public Tokens GetLoginToken()
        {
            return _loginToken;
        }
        public Tokens GetMarketDataAPIToken()
        {
            return _MarketDataAPIToken;
        }
        public Tokens GetHistoricalDataAPIToken()
        {
            return _HistoricalDataAPIToken;
        }
    }
}
