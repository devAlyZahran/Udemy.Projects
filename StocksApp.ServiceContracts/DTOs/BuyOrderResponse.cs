using StocksApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts.DTOs
{
    public class BuyOrderResponse
    {

        public Guid BuyOrderID { get; set; }

        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public UInt32 Quantity { get; set; }
        public double? Price { get; set; }
        public double? TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(BuyOrderResponse)) return false;

            BuyOrderResponse coming = (BuyOrderResponse)obj;

            return (
                BuyOrderID ==  coming.BuyOrderID 
                && StockSymbol == coming.StockSymbol 
                && StockName == coming.StockName 
                && Price == coming.Price
                && DateAndTimeOfOrder == coming.DateAndTimeOfOrder
                && Quantity == coming.Quantity);

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return @$"
                StockSymbol: {StockSymbol}, 
                StockName: {StockName}, 
                Price: {Price}, 
                Quantity: {Quantity}, 
                DateAndTimeOfOrder: {DateAndTimeOfOrder.Value.ToString("dd MMM yyyy")}";
        }

    }

    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity,
                StockName = buyOrder.StockName,
                StockSymbol = buyOrder.StockSymbol
            };
        }

        public static async Task<BuyOrderResponse> ToBuyOrderResponseAsync(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity,
                StockName = buyOrder.StockName,
                StockSymbol = buyOrder.StockSymbol
            };
        }
    }

}
