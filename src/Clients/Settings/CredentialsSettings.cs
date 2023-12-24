using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Settings
{
    public class CredentialsSettings
    {
        public string ClientCode { get; set; }
        public string ClientPIN { get; set; }
        public string TOTPCode { get; set; }
        public string HistoricalDataKey { get; set; }
        public string MarketDataKey { get; set; }
    }
}
