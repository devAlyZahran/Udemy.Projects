using StocksApp.Entities;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.Services
{
    public class StocksService : IStocksService
    {

        private readonly StocksDbContext _dbContext;

        public StocksService(StocksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            _dbContext.BuyOrders.Add(buyOrder);
            _dbContext.SaveChanges();

            return buyOrder.ToBuyOrderResponse();
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            _dbContext.SellOrders.Add(sellOrder);
            _dbContext.SaveChanges();

            return sellOrder.ToSellOrderResponse();
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            return _dbContext.BuyOrders.Select(b => b.ToBuyOrderResponse()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return _dbContext.SellOrders.Select(b => b.ToSellOrderResponse()).ToList();
        }
    }
}
