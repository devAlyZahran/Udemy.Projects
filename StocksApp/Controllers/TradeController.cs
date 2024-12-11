using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.IServices;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;
using StocksApp.ViewModels;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {

        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;
        private readonly TradingOptions _options;

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration, IOptions<TradingOptions> options, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
            _options = options.Value;
            _stocksService = stocksService;
        }

        [Route("/")]
        [Route("~/Trade/Index")]
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

        [Route("~/Trade/Orders")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();

            OrdersViewModel ordersViewModel = new OrdersViewModel()
            {
                BuyOrders = buyOrderResponses,
                SellOrders = sellOrderResponses
            };

            //ViewBag.TradingOptions = _options;

            return View(ordersViewModel);
        }

        [Route("~/Trade/BuyOrder")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest? buyOrderRequest)
        {

            if (!ModelState.IsValid)
                return View();

            BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest);
            Guid id = buyOrderResponse.BuyOrderID;

            return RedirectToAction(nameof(Index));
        }

        [Route("~/Trade/SellOrder")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest? sellOrderRequest)
        {
            //update date of order
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest);
            Guid id = sellOrderResponse.SellOrderID;

            return RedirectToAction(nameof(Index));
        }
    }
}
