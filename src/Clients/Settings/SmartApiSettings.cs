using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Settings
{
    public class SmartApiSettings
    {
        public CredentialsSettings Credentials { get; set; }
        public EndpointsSettings Endpoints { get; set; }

        public string GetAPIKey(bool IsHistorical = false)
        {
            if(IsHistorical)
            {
                return Credentials.HistoricalDataKey;
            }
            else
            {
                return Credentials.MarketDataKey;
            }
        }
    }
}
