using Microsoft.Extensions.Logging;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.Services
{
    public class StocksSellService : IStocksSellAdderService, IStocksSellGetterService
    {

        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksBuyService> _logger;

        public StocksSellService(IStocksRepository stocksRepository, ILogger<StocksBuyService> logger)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
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
