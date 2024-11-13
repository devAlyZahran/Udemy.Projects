using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.IServices;
using StocksApp.ViewModels;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {

        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly TradingOptions _options;

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration, IOptions<TradingOptions> options)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
            _options = options.Value;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            StockTrade stockTrade = new StockTrade();

            string stockSymbol = !string.IsNullOrEmpty(_options.DefaultStockSymbol) ? _options.DefaultStockSymbol : "MSFT";

            // Get Quote Service
            Task<Dictionary<string, object>?> stockPriceQuote = _finnhubService.GetStockPriceQuote(stockSymbol);
            Task<Dictionary<string, object>?> companyProfile = _finnhubService.GetCompanyProfile(stockSymbol);

            //load data from finnHubService into model object
            if (companyProfile != null && stockPriceQuote != null)
            {
                stockTrade = new StockTrade() { 
                    StockSymbol = Convert.ToString(companyProfile.Result["ticker"]), 
                    StockName = Convert.ToString(companyProfile.Result["name"]), 
                    Price = Convert.ToDouble(stockPriceQuote.Result["c"].ToString()) 
                };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }
    }
}
