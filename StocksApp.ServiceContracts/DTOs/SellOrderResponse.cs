using StocksApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts.DTOs
{
    public class SellOrderResponse
    {

        public Guid SellOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public UInt32 Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(SellOrderResponse)) return false;

            SellOrderResponse coming = (SellOrderResponse)obj;

            return (
                SellOrderID == coming.SellOrderID
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
                DateAndTimeOfOrder: {DateAndTimeOfOrder.ToString("dd MMM yyyy")}";
        }

    }

    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse
            {
                SellOrderID = sellOrder.SellOrderID,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity,
                StockName = sellOrder.StockName,
                StockSymbol = sellOrder.StockSymbol
            };
        }
    }
}
