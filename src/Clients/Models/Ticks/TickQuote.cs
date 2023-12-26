using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models.Ticks
{
    public class TickQuote : TickLtp
    {
        public double LastTradedQuantity { get; set; }
        public double AvgTradedPrice { get; set; }
        public double VolTraded { get; set; }
        public double TotalBuyQuantity { get; set; }
        public double TotalSellQuantity { get; set; }
        public double OpenPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double ClosePrice { get; set; }
    }
}
