﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models.Ticks
{
    public struct TickQuote
    {
        public String mode { get; set; }
        public UInt16 subscription_mode { get; set; }
        public UInt16 exchange_type { get; set; }
        public String token { get; set; }
        public Int64 sequence_number { get; set; }
        public Int64 ExchangeTimestam { get; set; }
        public Double last_traded_price { get; set; }
        public Double last_traded_quantity { get; set; }
        public Double avg_traded_price { get; set; }
        public Double vol_traded { get; set; }
        public Double total_buy_quantity { get; set; }
        public Double total_sell_quantity { get; set; }
        public Double open_price_day { get; set; }
        public Double high_price_day { get; set; }
        public Double low_price_day { get; set; }
        public Double close_price { get; set; }
    }
}
