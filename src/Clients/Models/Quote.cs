using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models
{
    public class Quote
    {
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("tradingSymbol")]
        public string TradingSymbol { get; set; }

        [JsonPropertyName("symbolToken")]
        public string SymbolToken { get; set; }

        [JsonPropertyName("ltp")]
        public double LTP { get; set; }

        [JsonPropertyName("open")]
        public double Open { get; set; }

        [JsonPropertyName("high")]
        public double High { get; set; }

        [JsonPropertyName("low")]
        public double Low { get; set; }

        [JsonPropertyName("close")]
        public double Close { get; set; }

        [JsonPropertyName("lastTradeQty")]
        public int LastTradeQty { get; set; }

        [JsonPropertyName("exchFeedTime")]
        public string ExchFeedTime { get; set; }

        [JsonPropertyName("exchTradeTime")]
        public string ExchTradeTime { get; set; }

        [JsonPropertyName("netChange")]
        public double NetChange { get; set; }

        [JsonPropertyName("percentChange")]
        public double PercentChange { get; set; }

        [JsonPropertyName("avgPrice")]
        public double AvgPrice { get; set; }

        [JsonPropertyName("tradeVolume")]
        public int TradeVolume { get; set; }

        [JsonPropertyName("opnInterest")]
        public int OpnInterest { get; set; }

        [JsonPropertyName("lowerCircuit")]
        public double LowerCircuit { get; set; }

        [JsonPropertyName("upperCircuit")]
        public double UpperCircuit { get; set; }

        [JsonPropertyName("totBuyQuan")]
        public int TotBuyQuan { get; set; }

        [JsonPropertyName("totSellQuan")]
        public int TotSellQuan { get; set; }

        [JsonPropertyName("52WeekLow")]
        public double Week52Low { get; set; }

        [JsonPropertyName("52WeekHigh")]
        public double Week52High { get; set; }

        [JsonPropertyName("depth")]
        public DepthItem Depth { get; set; }
    }
}
