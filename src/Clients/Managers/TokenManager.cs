using System;
using System.IdentityModel.Tokens.Jwt;

using AngelOne.SmartApi.Clients.Responses;

using Microsoft.IdentityModel.Tokens;

namespace AngelOne.SmartApi.Clients.Managers
{
    public class TokenManager
    {
        private Tokens _marketLoginToken = null!;
        private Tokens _historicalLoginToken = null!;

        private Tokens _MarketDataAPIToken = null!;
        private Tokens _HistoricalDataAPIToken = null!;


        public bool IsLoginValid(bool IsHistorical = false)
        {
            DateTime? validTill;
            if(IsHistorical)
            {
               validTill = _historicalLoginToken?.Expiry;
            }
            else
            {
                validTill = _marketLoginToken?.Expiry;
            }
            return validTill > DateTime.UtcNow;
        }

        /// <summary>
        /// We need to validate the access token validity
        /// </summary>
        /// <param name="IsHistorical"></param>
        /// <returns></returns>
        public bool IsTokenValid(bool IsHistorical = false)
        {
            Tokens tokens;
            if (IsHistorical)
            {
                tokens = _HistoricalDataAPIToken;
            }
            else
            {
                tokens = _MarketDataAPIToken;
            }
            return tokens != null;
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
        public void SetLoginToken(Tokens token, bool IsHistorical = false)
        {
            //Token is expired in 5:30 AM from the date of login
            token.Expiry = DateTime.UtcNow.Date.AddDays(1).AddHours(5).AddMinutes(30);
            if(IsHistorical)
            {
                _historicalLoginToken = token;
            }
            else
            {
                _marketLoginToken = token;
            }
        }

        public void SetAPIToken(Tokens token, bool IsHistorical = false)
        {
            if (IsHistorical)
            {
                _HistoricalDataAPIToken = token;
            }
            else
            {
                _MarketDataAPIToken = token;
            }
        }

        //Getters
        public Tokens GetLoginToken(bool IsHistorical = false)
        {
            if(IsHistorical)
            {
                return _historicalLoginToken;
            }
            else
            {
                return _marketLoginToken;
            }
        }
        public Tokens GetAPIToken(bool IsHistorical = false)
        {
            if (IsHistorical)
            {
                return _HistoricalDataAPIToken;
            }
            else
            {
                return _MarketDataAPIToken;
            }
        }
    }
}
