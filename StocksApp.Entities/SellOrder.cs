using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Entities
{
    public class SellOrder
    {
        /*
         Guid SellOrderID

        string StockSymbol [Mandatory]

        string StockName [Mandatory]

        DateTime DateAndTimeOfOrder

        uint Quantity [Range(1, 100000)]

        double Price [Range(1, 10000)]
         */

        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public UInt32 Quantity { get; set; }
        public double Price { get; set; }

    }
}
