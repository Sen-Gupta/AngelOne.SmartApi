using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Responses
{
    public class RMSLimitResponse : BaseResponse
    {
        [JsonPropertyName("data")]
        public RMSResult Data { get; set; }
    }

    public class RMSResult
    {
        [JsonPropertyName("net")]
        public string Net { get; set; }

        [JsonPropertyName("availablecash")]
        public string AvailableCash { get; set; }

        [JsonPropertyName("availableintradaypayin")]
        public string AvailableIntradayPayin { get; set; }

        [JsonPropertyName("availablelimitmargin")]
        public string AvailableLimitMargin { get; set; }

        [JsonPropertyName("collateral")]
        public string Collateral { get; set; }

        [JsonPropertyName("m2munrealized")]
        public string M2MUnrealized { get; set; }

        [JsonPropertyName("m2mrealized")]
        public string M2MRealized { get; set; }

        [JsonPropertyName("utiliseddebits")]
        public string UtilisedDebits { get; set; }

        [JsonPropertyName("utilisedspan")]
        public string UtilisedSpan { get; set; }

        [JsonPropertyName("utilisedoptionpremium")]
        public string UtilisedOptionPremium { get; set; }

        [JsonPropertyName("utilisedholdingsales")]
        public string UtilisedHoldingSales { get; set; }

        [JsonPropertyName("utilisedexposure")]
        public string UtilisedExposure { get; set; }

        [JsonPropertyName("utilisedturnover")]
        public string UtilisedTurnover { get; set; }

        [JsonPropertyName("utilisedpayout")]
        public string UtilisedPayout { get; set; }
    }
}
