using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
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
        private readonly IFixture _fixture;

        private readonly IStocksRepository _stocksRepository;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;

        public StocksServiceTest(ITestOutputHelper outputHelper)
        {
            _fixture = new Fixture();

            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            _stocksService = new StocksService(_stocksRepository);
            _outputHelper = outputHelper;
        }

        #region CreateBuyOrder

        // 1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateBuyOrder_NullObject()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;

            Func<Task> action = async () =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        // 2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_ZeroBuyOrderQuantity()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(p => p.Quantity, (uint)0)
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stocksRepositoryMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        // 3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_BuyOrderQuantityEquals100001()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(p => p.Quantity, (uint)100001)
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stocksRepositoryMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_ZeroBuyOrderPrice()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(p => p.Price, 0)
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stocksRepositoryMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_BuyOrderPriceEquals100001()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(p => p.Price, 100001)
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stocksRepositoryMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_NullableStockSymbol()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(p => p.StockSymbol, null as string)
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stocksRepositoryMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_InvalidDateAndTimeOfOrder()
        {

            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(p => p.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stocksRepositoryMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            Func<Task> action = async () =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID(guid).
        [Fact]
        public async Task CreateBuyOrder_ValidValues()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(p => p.StockSymbol, "MSFT")
                .Create();

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            _stocksRepositoryMock.Setup(s => s.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            BuyOrderResponse buyOrderResponse_actual = _stocksService.CreateBuyOrder(buyOrderRequest);

            buyOrderResponse_actual.BuyOrderID.Should().NotBeEmpty();
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
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(p => p.StockSymbol, "MSFT")
                .Create();

            SellOrder sellOrder_expected = sellOrderRequest.ToSellOrder();

            _stocksRepositoryMock.Setup(s => s.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder_expected);

            SellOrderResponse sellOrderResponse_actual = _stocksService.CreateSellOrder(sellOrderRequest);

            sellOrderResponse_actual.SellOrderID.Should().NotBeEmpty();
        }

        #endregion

        #region GetAllBuyOrders
        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllBuyOrders_EmptyList()
        {
            var buyOrders = new List<BuyOrder>();
            _stocksRepositoryMock.Setup(s => s.GetBuyOrders()).ReturnsAsync(buyOrders);

            List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders();

            buyOrderResponses.Should().BeEmpty();
        }


        // 2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public void GetAllBuyOrders_AddFewBuyOrders()
        {
            // Arrange countries to get their names and pass it to persons objects
            // that we'll use to add them to a list and try to retrieve this list

            List<BuyOrder> buyOrders = new List<BuyOrder>
            {
                _fixture.Build<BuyOrder>()
                .With(p => p.StockSymbol, "MSFT")
                .Create(),

                _fixture.Build<BuyOrder>()
                .With(p => p.StockSymbol, "MSFT")
                .Create(),

                _fixture.Build<BuyOrder>()
                .With(p => p.StockSymbol, "MSFT")
                .Create()
            };

            List<BuyOrderResponse> buyOrderResponses_expected = buyOrders.Select(b => b.ToBuyOrderResponse()).ToList();

            _outputHelper.WriteLine("Expected:");
            foreach (BuyOrderResponse item in buyOrderResponses_expected)
            {
                _outputHelper.WriteLine(item.ToString());
            }
            _stocksRepositoryMock.Setup(s => s.GetBuyOrders()).ReturnsAsync(buyOrders);


            // Act
            List<BuyOrderResponse> buyOrderResponseList_from_actual = _stocksService.GetBuyOrders();

            _outputHelper.WriteLine("Actual:");
            foreach (BuyOrderResponse item in buyOrderResponseList_from_actual)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            // Assert
            buyOrderResponseList_from_actual.Should().BeEquivalentTo(buyOrderResponses_expected);
        }
        #endregion

        #region GetAllSellOrders
        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public void GetAllSellOrders_EmptyList()
        {
            var sellOrders = new List<SellOrder>();
            _stocksRepositoryMock.Setup(s => s.GetSellOrders()).ReturnsAsync(sellOrders);
            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();

            sellOrderResponses.Should().BeEmpty();
        }

        // When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public void GetAllSellOrders_AddFewBuyOrders()
        {
            // Arrange countries to get their names and pass it to persons objects
            // that we'll use to add them to a list and try to retrieve this list
            List<SellOrder> sellOrders = new List<SellOrder>
            {
                _fixture.Build<SellOrder>()
                .With(p => p.StockSymbol, "MSFT")
                .Create(),

                _fixture.Build<SellOrder>()
                .With(p => p.StockSymbol, "MSFT")
                .Create(),

                _fixture.Build<SellOrder>()
                .With(p => p.StockSymbol, "MSFT")
                .Create()
            };

            List<SellOrderResponse> sellOrderResponses_expected = sellOrders.Select(b => b.ToSellOrderResponse()).ToList();

            _outputHelper.WriteLine("Expected:");
            foreach (SellOrderResponse item in sellOrderResponses_expected)
            {
                _outputHelper.WriteLine(item.ToString());
            }
            _stocksRepositoryMock.Setup(s => s.GetSellOrders()).ReturnsAsync(sellOrders);


            // Act
            List<SellOrderResponse> sellOrderResponseList_from_actual = _stocksService.GetSellOrders();

            _outputHelper.WriteLine("Actual:");
            foreach (SellOrderResponse item in sellOrderResponseList_from_actual)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            // Assert
            sellOrderResponseList_from_actual.Should().BeEquivalentTo(sellOrderResponses_expected);

        }
        #endregion

    }
}