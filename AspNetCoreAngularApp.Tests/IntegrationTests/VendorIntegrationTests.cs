using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace AspNetCoreAngularApp.Tests.IntegrationTests
{
    public class VendorIntegrationTests
    {
        private readonly HttpClient _client;

        public VendorIntegrationTests()
        {
            // Arrange
            var server = new TestServer(
                new WebHostBuilder()
                   .UseStartup<Startup>()
            );
            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetAllVendors()
        {
            // Act
            var response = await _client.GetAsync("/api/vendor");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var vendors = new List<VendorViewModel>
                          {
                              new()
                              {
                                  Name = "Netflix Inc",
                                  Symbol = "NFLX",
                                  Exchange = "NASDAQ"
                              },
                              new()
                              {
                                  Name = "Apple Inc",
                                  Symbol = "APPL",
                                  Exchange = "NASDAQGS"
                              }
                          };

            string expectedJson = JsonConvert.SerializeObject(
                vendors,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );

            // Assert
            Assert.Equal(expectedJson, responseString);
        }
    }
}