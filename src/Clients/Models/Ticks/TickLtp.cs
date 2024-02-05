using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models.Ticks
{
    public class TickLtp
    {
        public string Mode { get; set; } = null!;
        public int SubscriptionMode { get; set; }
        public int ExchangeType { get; set; }
        public string Token { get; set; } = null!;
        public long SequenceNumber { get; set; }
        public long ExchangeTimestamp { get; set; }
        public double LastTradedPrice { get; set; }
    }
}
