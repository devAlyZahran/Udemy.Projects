using StocksApp.IServices;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _clientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($@"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader reader = new StreamReader(stream);

                string result = await reader.ReadToEndAsync();

                Dictionary<string, object>? DataDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(result);

                if (DataDictionary == null)
                    throw new InvalidOperationException("Ther's no data for this quote!");

                return DataDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _clientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($@"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader reader = new StreamReader(stream);

                string result = await reader.ReadToEndAsync();

                Dictionary<string, object>? DataDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(result);

                if (DataDictionary == null)
                    throw new InvalidOperationException("Ther's no data for this quote!");

                return DataDictionary;
            }
        }
    }
}
