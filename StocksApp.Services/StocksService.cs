using Microsoft.Extensions.Logging;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.Services
{
    public class StocksService : IStocksService
    {

        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksService> _logger;

        public StocksService(IStocksRepository stocksRepository, ILogger<StocksService> logger)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
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

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            _stocksRepository.CreateSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
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

        public List<SellOrderResponse> GetSellOrders()
        {
            List<SellOrderResponse> result = new List<SellOrderResponse>();
            try
            {
                result = _stocksRepository.GetSellOrders().Result.Select(b => b.ToSellOrderResponse()).ToList();
                _logger.LogError($"Sell Orders Count: {result.Count}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"There's an error while reading Sell Orders, Excepton: {ex.Message}");
            }
            return result;
        }
    }
}
