using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models.Ticks
{
    public struct TickLtp
    {
        public String mode { get; set; }
        public UInt16 subscription_mode { get; set; }
        public UInt16 exchange_type { get; set; }
        public String token { get; set; }
        public Int64 sequence_number { get; set; }
        public Int64 ExchangeTimestam { get; set; }
        public Double last_traded_price { get; set; }
    }
}
