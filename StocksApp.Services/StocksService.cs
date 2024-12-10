using StocksApp.Entities;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrderes;
        private readonly List<SellOrder> _sellOrderes;
        public StocksService()
        {
            _buyOrderes = new List<BuyOrder>();
            _sellOrderes = new List<SellOrder>();
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            _buyOrderes.Add(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            _sellOrderes.Add(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            return _buyOrderes.Select(b => b.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return _sellOrderes.Select(b => b.ToSellOrderResponse()).ToList();
        }
    }
}
