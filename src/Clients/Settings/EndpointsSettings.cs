using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Settings
{
    public class EndpointsSettings
    {
        public class BaseUrlsSettings
        {
            public string Auth { get; set; }
            public string API { get; set; }
        }
        public BaseUrlsSettings BaseUrls { get; set; }
        public string Login { get; set; }
        public string Token { get; set; }
        public string Quote { get; set; }
        public string Candle { get; set; }
    }
}
