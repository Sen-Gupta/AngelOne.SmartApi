using AngelOne.SmartApi.Clients.Sockets.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Sockets
{
    public class Ticker
    {
        private readonly IWebSocketV2 _webSocketV2;
        public Ticker(IWebSocketV2 webSocketV2)
        {
            _webSocketV2 = webSocketV2;
        }
    }
}
