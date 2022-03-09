using System.IO;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Data.StockQuoteServices
{
    public class AppleStockQuoteService: IStockQuoteService
    {
        public StockQuote FetchStockQuoteInformation()
        {
            var filePath = Directory.GetCurrentDirectory() + "/AspNetCoreAngularApp.Data/JsonFiles/AppleStockQuote.json";
            string extension = Path.GetExtension(filePath);
            var strategy = ImportStrategyPicker.Select(extension);
            return strategy.ImportStockQuote(filePath);
        }
    }
}