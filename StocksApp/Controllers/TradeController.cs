using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StocksApp.Filters.ActionFilter;
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
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration, IOptions<TradingOptions> options, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
            _options = options.Value;
            _stocksService = stocksService;
            _logger = logger;
        }

        #region Commented
        //[Route("/")]
        //[Route("~/Trade/Index")]
        //public async Task<IActionResult> Index()
        //{
        //    StockTrade stockTrade = new StockTrade();

        //    string stockSymbol = !string.IsNullOrEmpty(_options.DefaultStockSymbol) ? _options.DefaultStockSymbol : "MSFT";

        //    // Get Quote Service
        //    Task<Dictionary<string, object>?> stockPriceQuote = _finnhubService.GetStockPriceQuote(stockSymbol);
        //    Task<Dictionary<string, object>?> companyProfile = _finnhubService.GetCompanyProfile(stockSymbol);

        //    //load data from finnHubService into model object
        //    if (companyProfile != null && stockPriceQuote != null)
        //    {
        //        stockTrade = new StockTrade() { 
        //            StockSymbol = Convert.ToString(companyProfile.Result["ticker"]), 
        //            StockName = Convert.ToString(companyProfile.Result["name"]), 
        //            Price = Convert.ToDouble(stockPriceQuote.Result["c"].ToString()) 
        //        };
        //    }

        //    //Send Finnhub token to view
        //    ViewBag.FinnhubToken = _configuration["FinnhubToken"];

        //    return View(stockTrade);
        //} 
        #endregion

        [Route("[action]/{stockSymbol}")]
        [Route("~/Trade/Index/{stockSymbol}")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";


            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);

            //get stock price quotes from API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);


            //create model object
            StockTrade stockTrade = new StockTrade() { StockSymbol = stockSymbol };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade() { StockSymbol = companyProfileDictionary["ticker"].ToString(), StockName = companyProfileDictionary["name"].ToString(), Quantity = _options.DefaultOrderQuantity ?? 0, Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
            }

            //Send Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("~/Trade/Orders")]
        [Route("/")]
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
            _logger.LogInformation($"Buy Orders count: {buyOrderResponses.Count} && Sell Orders Count: {sellOrderResponses.Count}");
            //ViewBag.TradingOptions = _options;

            return View(ordersViewModel);
        }

        [Route("~/Trade/BuyOrder")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest? orderRequest)
        {

            if (!ModelState.IsValid)
                return View();

            BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(orderRequest);
            Guid id = buyOrderResponse.BuyOrderID;

            return RedirectToAction(nameof(Index));
        }

        [Route("~/Trade/SellOrder")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest? orderRequest)
        {
            //update date of order
            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(orderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(orderRequest);
            Guid id = sellOrderResponse.SellOrderID;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OrdersPDF()
        {

            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();
            List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders();

            OrdersViewModel viewModel = new OrdersViewModel()
            {
                BuyOrders = buyOrderResponses,
                SellOrders = sellOrderResponses
            };

            return new ViewAsPdf("OrdersPDF", viewModel, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Bottom = 20, Left = 20, Right = 20, Top = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
