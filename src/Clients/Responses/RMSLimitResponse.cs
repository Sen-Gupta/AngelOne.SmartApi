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
        public RMSResult Data { get; set; } = null!;
    }

    public class RMSResult
    {
        [JsonPropertyName("net")]
        public string Net { get; set; } = null!;

        [JsonPropertyName("availablecash")]
        public string AvailableCash { get; set; } = null!;

        [JsonPropertyName("availableintradaypayin")]
        public string AvailableIntradayPayin { get; set; } = null!;

        [JsonPropertyName("availablelimitmargin")]
        public string AvailableLimitMargin { get; set; } = null!;

        [JsonPropertyName("collateral")]
        public string Collateral { get; set; } = null!;

        [JsonPropertyName("m2munrealized")]
        public string M2MUnrealized { get; set; } = null!;

        [JsonPropertyName("m2mrealized")]
        public string M2MRealized { get; set; } = null!;

        [JsonPropertyName("utiliseddebits")]
        public string UtilisedDebits { get; set; } = null!;

        [JsonPropertyName("utilisedspan")]
        public string UtilisedSpan { get; set; } = null!;

        [JsonPropertyName("utilisedoptionpremium")]
        public string UtilisedOptionPremium { get; set; } = null!;

        [JsonPropertyName("utilisedholdingsales")]
        public string UtilisedHoldingSales { get; set; } = null!;

        [JsonPropertyName("utilisedexposure")]
        public string UtilisedExposure { get; set; } = null!;

        [JsonPropertyName("utilisedturnover")]
        public string UtilisedTurnover { get; set; } = null!;

        [JsonPropertyName("utilisedpayout")]
        public string UtilisedPayout { get; set; } = null!;
    }
}
