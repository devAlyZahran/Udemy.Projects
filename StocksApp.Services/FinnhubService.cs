using StocksApp.IServices;
using StocksApp.RepositoryContracts;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;


        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }


        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            //invoke repository
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);

            //return response dictionary back to the caller
            return responseDictionary;
        }


        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            //invoke repository
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            //return response dictionary back to the caller
            return responseDictionary;
        }


        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            //invoke repository
            List<Dictionary<string, string>>? responseDictionary = await _finnhubRepository.GetStocks();

            //return response dictionary back to the caller
            return responseDictionary;
        }


        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            //invoke repository
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

            //return response dictionary back to the caller
            return responseDictionary;
        }
    }
}
