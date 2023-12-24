using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Settings
{
    public class CredentialsSettings
    {
        public string ClientId { get; set; }
        public string ClientPIN { get; set; }
        public string TOTPCode { get; set; }
        public string HistoricalDataAPI { get; set; }
        public string MarketDataAPI { get; set; }
    }
}
