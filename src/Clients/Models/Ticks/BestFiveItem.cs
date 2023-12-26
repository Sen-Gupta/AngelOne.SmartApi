using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models.Ticks
{
    public struct BestFiveItem
    {
        public int BuySellFlag { get; set; }
        public long Quantity { get; set; }
        public double Price { get; set; }
        public int Orders { get; set; }
    }
}
