using StocksApp.ServiceContracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts
{
    public interface IStocksBuyGetterService
    {
        List<BuyOrderResponse> GetBuyOrders();

    }
}
