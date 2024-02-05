using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Settings
{
    public class CredentialsSettings
    {
        public string ClientCode { get; set; } = null!;
        public string ClientPIN { get; set; } = null!;
        public string TOTPCode { get; set; } = null!;
        public string APIKey { get; set; } = null!;
    }
}
