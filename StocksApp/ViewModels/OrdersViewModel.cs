using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.ViewModels
{
    public class OrdersViewModel
    {
        
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();

        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();

    }
}
