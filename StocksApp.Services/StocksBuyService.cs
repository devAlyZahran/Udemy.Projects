using Microsoft.Extensions.Logging;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.Services
{
    public class StocksBuyService : IStocksBuyAdderService, IStocksBuyGetterService
    {

        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksBuyService> _logger;

        public StocksBuyService(IStocksRepository stocksRepository, ILogger<StocksBuyService> logger)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            List<BuyOrderResponse> result = new List<BuyOrderResponse>();
            try
            {
                result = _stocksRepository.GetBuyOrders().Result.Select(b => b.ToBuyOrderResponse()).ToList();
                _logger.LogError($"Buy Orders Count: {result.Count}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"There's an error while reading Buy Orders, Excepton: {ex.Message}");
            }
            return result;
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            _stocksRepository.CreateBuyOrder(buyOrder);

            _logger.LogInformation("Buy Order Created");

            return buyOrder.ToBuyOrderResponse();
        }
    }
}
