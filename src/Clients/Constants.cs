using AngelOne.SmartApi.Clients.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients
{
    public static class Constants
    {
        public static class CANDLE_INTERVAL
        {
            public const string ONE_MINUTE = "ONE_MINUTE";
            public const string TWO_MINUTE = "TWO_MINUTE";
            public const string THREE_MINUTE = "THREE_MINUTE";
            public const string FIVE_MINUTE = "FIVE_MINUTE";
            public const string TEN_MINUTE = "TEN_MINUTE";
            public const string FIFTEEN_MINUTE = "FIFTEEN_MINUTE";
            public const string THIRTY_MINUTE = "THIRTY_MINUTE";
            public const string ONE_HOUR = "ONE_HOUR";
            public const string ONE_DAY = "ONE_DAY";

            public static readonly Dictionary<string, int> TimeIntervals = new Dictionary<string, int>
            {
                {ONE_MINUTE, 30},
                {TWO_MINUTE, 60},
                {THREE_MINUTE, 60},
                {FIVE_MINUTE, 100},
                {TEN_MINUTE, 100},
                {FIFTEEN_MINUTE, 200},
                {THIRTY_MINUTE, 200},
                {ONE_HOUR, 400},
                {ONE_DAY, 2000}
            };
        }
        public static class TickerModes
        {
            public const string SNAP_QUOTE = "SnapQuote";
            public const string QUOTE = "Quote";
            public const string LTP = "LTP";

            public static readonly Dictionary<string, int> TimeIntervals = new Dictionary<string, int>
            {
                {SNAP_QUOTE, 3},
                {QUOTE, 2},
                {LTP, 1}
            };
        }
        public static class Endpoints
        {
            public const string Login = "user/v1/loginByPassword";
            public const string Token = "jwt/v1/generateTokens";
            public const string Quote = "market/v1/quote/";
            public const string Candle = "historical/v1/getCandleData";
            public const string Profile = "user/v1/getProfile";
            public const string Logout = "user/v1/logout";
            public const string RMSLimit = "user/v1/getRMS";

            public static class BaseUrls
            {
                public const string Auth = "https://apiconnect.angelbroking.com/rest/auth/angelbroking/";
                public const string API = "https://apiconnect.angelbroking.com/rest/secure/angelbroking/";
            }

        }

        public static class Modes
        {
            public const string LTP = "LTP";
            public const string OHLC = "OHLC";
            public const string FULL = "FULL";
        }

        public static class Exchanges
        {
            public const string NSE = "NSE";
            public const string BSE = "BSE";
        }
    }
}
