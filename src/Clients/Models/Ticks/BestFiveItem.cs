using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models.Ticks
{
    public struct BestFiveItem
    {
        public Int16 buy_sell_flag { get; set; }
        public Int64 quantity { get; set; }
        public Double price { get; set; }
        public Int16 orders { get; set; }
    }
}
