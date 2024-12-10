using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;
using StocksApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using Xunit.Abstractions;

namespace StocksApp.Test
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _outputHelper;
        public StocksServiceTest(ITestOutputHelper outputHelper)
        {
            _stocksService = new StocksService();
            _outputHelper = outputHelper;
        }

        #region CreateBuyOrder

        // 1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullObject()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_ZeroBuyOrderQuantity()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest {Quantity = 0};

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityEquals100001()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest { Quantity = 100001 };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_ZeroBuyOrderPrice()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest { Price = 0 };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderPriceEquals100001()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest { Price = 100001 };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_NullableStockSymbol()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest { StockSymbol = null };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_InvalidDateAndTimeOfOrder()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest { DateAndTimeOfOrder = DateTime.Parse("1999-12-31") };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID(guid).
        [Fact]
        public void CreateBuyOrder_ValidValues()
        {

            // Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01"),
                Price = 150,
                Quantity = 10,
                StockName = "Head Phone",
                StockSymbol = "MSFT"
            };

            // Act
            BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest);
            List<BuyOrderResponse> allBuyOrderResponses = _stocksService.GetBuyOrders();

            // Assert
            Assert.True(buyOrderResponse?.BuyOrderID != null);
            Assert.Contains(buyOrderResponse, allBuyOrderResponses);
        }
        #endregion

        #region CreateSellOrder

        //1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateSellOrder_NullObject()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_ZeroBuyOrderQuantity()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest { Quantity = 0 };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_BuyOrderQuantityEquals100001()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest { Quantity = 100001 };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_ZeroBuyOrderPrice()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest { Price = 0 };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_BuyOrderPriceEquals100001()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest { Price = 100001 };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_NullableStockSymbol()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest { StockSymbol = null };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_InvalidDateAndTimeOfOrder()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest { DateAndTimeOfOrder = DateTime.Parse("1999-12-31") };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID(guid).
        [Fact]
        public void CreateSellOrder_ValidValues()
        {

            // Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01"),
                Price = 150,
                Quantity = 10,
                StockName = "Head Phone",
                StockSymbol = "MSFT"
            };

            // Act
            SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest);
            List<SellOrderResponse> allSellOrderResponses = _stocksService.GetSellOrders();

            // Assert
            Assert.True(sellOrderResponse?.SellOrderID != null);
            Assert.Contains(sellOrderResponse, allSellOrderResponses);
        }

        #endregion

        #region GetAllBuyOrders
        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public void GetAllBuyOrders_EmptyList()
        {
            List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders();

            Assert.Empty(buyOrderResponses);
        }


        // 2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public void GetAllBuyOrders_AddFewBuyOrders()
        {
            // Arrange countries to get their names and pass it to persons objects
            // that we'll use to add them to a list and try to retrieve this list

            BuyOrderRequest buyOrderRequest1 = new BuyOrderRequest()
            {
                Quantity = 1,
                Price = 1,
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01"),
                StockName = "Name 1",
                StockSymbol = "SMFT"
            };
            BuyOrderRequest buyOrderRequest2 = new BuyOrderRequest()
            {
                Quantity = 10,
                Price = 22,
                DateAndTimeOfOrder = DateTime.Parse("2003-01-01"),
                StockName = "Name 2",
                StockSymbol = "SMFT"
            };
            BuyOrderRequest buyOrderRequest3 = new BuyOrderRequest()
            {
                Quantity = 12,
                Price = 34,
                DateAndTimeOfOrder = DateTime.Parse("2004-01-01"),
                StockName = "Name 3",
                StockSymbol = "SMFT"
            };

            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>() { buyOrderRequest1, buyOrderRequest2, buyOrderRequest3 };
            List<BuyOrderResponse> buyOrderResponseList_from_add = new List<BuyOrderResponse>();

            foreach (BuyOrderRequest buyOrderAddRequest in buyOrderRequests)
            {
                BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderAddRequest);
                buyOrderResponseList_from_add.Add(buyOrderResponse);
            }

            _outputHelper.WriteLine("Expected:");
            foreach (BuyOrderResponse item in buyOrderResponseList_from_add)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            // Act
            List<BuyOrderResponse> buyOrderResponseList_from_get = _stocksService.GetBuyOrders();

            _outputHelper.WriteLine("Actual:");
            foreach (BuyOrderResponse item in buyOrderResponseList_from_get)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            // Assert
            foreach (BuyOrderResponse buyOrderResponse_from_add in buyOrderResponseList_from_add)
            {
                Assert.Contains(buyOrderResponse_from_add, buyOrderResponseList_from_get);
            }
        }
        #endregion

        #region GetAllSellOrders
        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public void GetAllSellOrders_EmptyList()
        {
            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();

            Assert.Empty(sellOrderResponses);
        }

        // When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public void GetAllSellOrders_AddFewBuyOrders()
        {
            // Arrange countries to get their names and pass it to persons objects
            // that we'll use to add them to a list and try to retrieve this list

            SellOrderRequest sellOrderRequest1 = new SellOrderRequest()
            {
                Quantity = 1,
                Price = 1,
                DateAndTimeOfOrder = DateTime.Parse("2002-01-01"),
                StockName = "Name 1",
                StockSymbol = "SMFT"
            };
            SellOrderRequest sellOrderRequest2 = new SellOrderRequest()
            {
                Quantity = 10,
                Price = 22,
                DateAndTimeOfOrder = DateTime.Parse("2003-01-01"),
                StockName = "Name 2",
                StockSymbol = "SMFT"
            };
            SellOrderRequest sellOrderRequest3 = new SellOrderRequest()
            {
                Quantity = 12,
                Price = 34,
                DateAndTimeOfOrder = DateTime.Parse("2004-01-01"),
                StockName = "Name 3",
                StockSymbol = "SMFT"
            };

            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>() { sellOrderRequest1, sellOrderRequest2, sellOrderRequest3 };
            List<SellOrderResponse> sellOrderResponseList_from_add = new List<SellOrderResponse>();

            foreach (SellOrderRequest sellOrderAddRequest in sellOrderRequests)
            {
                SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderAddRequest);
                sellOrderResponseList_from_add.Add(sellOrderResponse);
            }

            _outputHelper.WriteLine("Expected:");
            foreach (SellOrderResponse item in sellOrderResponseList_from_add)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            // Act
            List<SellOrderResponse> sellOrderResponseList_from_get = _stocksService.GetSellOrders();

            _outputHelper.WriteLine("Actual:");
            foreach (SellOrderResponse item in sellOrderResponseList_from_get)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            // Assert
            foreach (SellOrderResponse sellOrderResponse_from_add in sellOrderResponseList_from_add)
            {
                Assert.Contains(sellOrderResponse_from_add, sellOrderResponseList_from_get);
            }
        }
        #endregion

    }
}