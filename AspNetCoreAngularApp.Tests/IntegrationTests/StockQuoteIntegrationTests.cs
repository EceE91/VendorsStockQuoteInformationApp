using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels;
using AspNetCoreAngularApp.Tests.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace AspNetCoreAngularApp.Tests.IntegrationTests
{
    public class StockQuoteIntegrationTests
    {
        private readonly HttpClient _client;

        public StockQuoteIntegrationTests()
        {
            // Arrange
            var server = new TestServer(
                new WebHostBuilder()
                   .UseStartup<Startup>()
            );
            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetStockQuote_FromCsv()
        {
            //Set the current directory.
            Directory.SetCurrentDirectory("/Users/ece.ercan/EcesProjects/NN/AngularAspNetCoreApp/AspNetCoreAngularApp.Tests");
            
            // Act
            var vendorSymbol = "NFLX";
            var response = await _client.GetAsync($"/api/stockquote/{vendorSymbol}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var stockQuote = new StockQuoteViewModel
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

            string expectedJson = JsonConvert.SerializeObject(
                stockQuote,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    // When serializing floats and doubles, Json.NET always adds ".0" at the end if the number doesn't contain any fractional part
                    // I added DecimalJsonConverter to remove the ".0"
                    Converters = new JsonConverter[] { new DecimalJsonConverter() }
                }
            );

            // Assert
            Assert.Equal(expectedJson, responseString);
        }
        
        [Fact]
        public async Task GetStockQuote_FromJson()
        {
            //Set the current directory.
            Directory.SetCurrentDirectory("/Users/ece.ercan/EcesProjects/NN/AngularAspNetCoreApp/AspNetCoreAngularApp.Tests");
            
            // Act
            var vendorSymbol = "APPL";
            var response = await _client.GetAsync($"/api/stockquote/{vendorSymbol}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var stockQuote = new StockQuoteViewModel
                             {
                                 Name = "Apple Inc",
                                 Symbol = "APPL",
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

            string expectedJson = JsonConvert.SerializeObject(
                stockQuote,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    // When serializing floats and doubles, Json.NET always adds ".0" at the end if the number doesn't contain any fractional part
                    // I added DecimalJsonConverter to remove the ".0"
                    Converters = new JsonConverter[] { new DecimalJsonConverter() }
                }
            );

            // Assert
            Assert.Equal(expectedJson, responseString);
        }
    }
}