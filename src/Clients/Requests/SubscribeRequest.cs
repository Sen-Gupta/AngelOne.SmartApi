using System.Text.Json.Serialization;

namespace AngelOne.SmartApi.Clients.Requests
{
    public class SubscribeRequest
    {
        [JsonPropertyName("correlationID")]
        public string CorrelationID { get; set; }

        [JsonPropertyName("action")]
        public int Action { get; set; }

        [JsonPropertyName("params")]
        public Params Tokens { get; set; } = new Params();

        // Constructor to generate a random alphanumeric correlation ID
        public SubscribeRequest()
        {
            CorrelationID = GenerateRandomCorrelationID();
        }

        // Method to generate a random alphanumeric string of a given length
        private string GenerateRandomCorrelationID()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class Params
    {
        /// <summary>
        /// Mode is subscription type: 1, 2, 3, 4
        /// </summary>
        [JsonPropertyName("mode")]
        public int Mode { get; set; }

        [JsonPropertyName("tokenList")]
        public List<Token> TokenList { get; set; } = new List<Token>();
    }

    public class Token
    {
        [JsonPropertyName("exchangeType")]
        public int ExchangeType { get; set; }

        [JsonPropertyName("tokens")]
        public List<string> Tokens { get; set; } = new List<string>();
    }
}
