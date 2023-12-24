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
    }
}
