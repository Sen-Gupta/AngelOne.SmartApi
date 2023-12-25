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
            public const string FULL = "SnapQuote";
            public const string QUOTE = "Quote";
            public const string LTP = "LTP";
            public const string DEPTH = "Depth";

            public static readonly Dictionary<string, int> Codes = new Dictionary<string, int>
            {
                {DEPTH, 4 },
                {FULL, 3},
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
            public static readonly Dictionary<string, int> Codes = new Dictionary<string, int>
            {
                {NSE, 1 },
                {BSE, 3},
            };
        }

        public static class Sockets
        {

            public const string TickerSocketUrl = "wss://smartapisocket.angelone.in/smart-stream";
            public const string  PING = "ping";
            public const int HeartBeatInterval = 25;
            

            //Retry Strategies
            public static class Retry
            {
                public const int MAX_ATTEMPTS = 1;
                public const int STRATEGY = 0;
                public const int DELAY = 10;
                public const int MULTIPLIER = 2;
                public const int DURATION = 60;
            }

            //Headers
            public static class Headers
            {
                public const string AUTHORIZATION = "Authorization";
                public const string APIKEY = "x-api-key";
                public const string CLIENTCODE = "x-client-code";
                public const string FEEDTOKEN = "x-feed-token";
            }

            public static class Actions
            {
                public const int SUBSCRIBE_ACTION = 1;
                public const int UNSUBSCRIBE_ACTION = 0;
            }
        }
    }
}
