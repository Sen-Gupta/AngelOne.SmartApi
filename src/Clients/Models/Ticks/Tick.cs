using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models.Ticks
{
    public class Tick: TickQuote
    {
        public long LastTradedTimestamp { get; set; }
        public long OpenInterest { get; set; }
        public double OpenInterestChange { get; set; }
        public double UpperCircuit { get; set; }
        public double LowerCircuit { get; set; }
        public double High52Week { get; set; }
        public double Low52Week { get; set; }
        public BestFiveItem[] Best5 { get; set; }
    }
}
