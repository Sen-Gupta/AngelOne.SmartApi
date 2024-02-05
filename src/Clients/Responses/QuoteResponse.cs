using System.Text.Json.Serialization;
using AngelOne.SmartApi.Clients.Models;

namespace AngelOne.SmartApi.Clients.Responses
{
    public class QuoteResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public QuoteResult? Results { get; set; } 
         
    }

    public class QuoteResult
    {
        [JsonPropertyName("fetched")]
        public List<Quote> Quotes { get; set; } = new List<Quote>();

        [JsonPropertyName("unfetched")]
        public List<FailedQuote> FailedQuotes { get; set; } = new List<FailedQuote>();
        public bool HasFailedQuotes => FailedQuotes?.Count > 0;
    }
}
