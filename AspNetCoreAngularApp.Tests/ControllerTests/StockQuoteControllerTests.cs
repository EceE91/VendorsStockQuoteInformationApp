using System;
using System.IO;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.Controllers;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Data.StockQuoteServices;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace AspNetCoreAngularApp.Tests.ControllerTests
{
    public class StockQuoteControllerTests
    {
        [Fact]
        public async void GetStockQuote_ReturnsStockQuoteOfSelectedVendor()
        {
            // Arrange
            var netflixSymbol = "NFLX";
            var mockMapper = new Mock<IMapper>();
            var mockQuoteService = new Mock<IStockQuoteService>();
            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceProvider.Setup(x => x.GetService(typeof(NetflixStockQuoteService)))
                                            .Returns(mockQuoteService.Object);
            
            var testStockQuote = CreateStockQuote();
            mockQuoteService.Setup(service => service.FetchStockQuoteInformation()).Returns(testStockQuote);

            var factory = new VendorFactory(mockServiceProvider.Object);
            factory.GetStockQuoteService(netflixSymbol);
            
            var viewModel = new StockQuoteViewModel
                            {
                                Name = "Netflix Inc",
                                Symbol = "NFLX",
                                LastPrice = 524.49,
                                Change = 15.6,
                                ChangePercent = 3.06549549018453,
                                Timestamp = "Wed Oct 23 13:39:19 UTC-06:00 2013",
                                MSDate = 41570.568969907,
                                MarketCap = 476497591530,
                                Volume = 397562,
                                ChangeYTD = 532.1729,
                                ChangePercentYTD = -1.44368493773359,
                                High = 52499,
                                Low = 519.175,
                                Open = 519.175
                            };
            mockMapper.Setup(m => m.Map<StockQuote, StockQuoteViewModel>(testStockQuote))
                      .Returns(viewModel);
            
            var controller = new StockQuoteController(mockMapper.Object, factory);

            // Act
            var result = await controller.GetStockQuote(netflixSymbol);
            
            // assert
            result.Value.Should().BeEquivalentTo(viewModel);
        }

        [Fact]
        public async void GetStockQuote_WhenJsonSerializationExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var appleSymbol = "APPL";
            var mockMapper = new Mock<IMapper>();
            var mockQuoteService = new Mock<IStockQuoteService>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            
            var mockStrategy = new Mock<IImportStrategy>();
            mockStrategy.Setup(x => x.ImportStockQuote(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData.json")))
                        .Throws(new JsonSerializationException());

            mockServiceProvider.Setup(x => x.GetService(typeof(AppleStockQuoteService)))
                                            .Returns(mockQuoteService.Object);

            mockQuoteService.Setup(service => service.FetchStockQuoteInformation()).Throws(new JsonSerializationException());

            var factory = new VendorFactory(mockServiceProvider.Object);
            factory.GetStockQuoteService(appleSymbol);

            var controller = new StockQuoteController(mockMapper.Object, factory);

            // Act
            var result = await controller.GetStockQuote(appleSymbol);
            
            // assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        
        [Fact]
        public async void GetStockQuote_WhenFileNotFoundExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var appleSymbol = "APPL";
            var mockMapper = new Mock<IMapper>();
            var mockQuoteService = new Mock<IStockQuoteService>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            
            var mockStrategy = new Mock<IImportStrategy>();
            mockStrategy.Setup(x => x.ImportStockQuote(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData.json")))
                        .Throws(new FileNotFoundException());

            mockServiceProvider.Setup(x => x.GetService(typeof(AppleStockQuoteService)))
                               .Returns(mockQuoteService.Object);

            mockQuoteService.Setup(service => service.FetchStockQuoteInformation()).Throws(new JsonSerializationException());

            var factory = new VendorFactory(mockServiceProvider.Object);
            factory.GetStockQuoteService(appleSymbol);

            var controller = new StockQuoteController(mockMapper.Object, factory);

            // Act
            var result = await controller.GetStockQuote(appleSymbol);
            
            // assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        private StockQuote CreateStockQuote()
        {
            return new StockQuote
                   {
                       Name = "Netflix Inc",
                       Symbol = "NFLX",
                       LastPrice = 524.49,
                       Change = 15.6,
                       ChangePercent = 3.06549549018453,
                       Timestamp = "Wed Oct 23 13:39:19 UTC-06:00 2013",
                       MSDate = 41570.568969907,
                       MarketCap = 476497591530,
                       Volume = 397562,
                       ChangeYTD = 532.1729,
                       ChangePercentYTD = -1.44368493773359,
                       High = 52499,
                       Low = 519.175,
                       Open = 519.175
                   };
        }
    }
}