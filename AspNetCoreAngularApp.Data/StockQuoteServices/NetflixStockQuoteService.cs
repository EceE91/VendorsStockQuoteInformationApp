using System.IO;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Data.StockQuoteServices
{
    public class NetflixStockQuoteService: IStockQuoteService
    {
        public StockQuote FetchStockQuoteInformation()
        {
            var filePath = Directory.GetCurrentDirectory() + "/AspNetCoreAngularApp.Data/CsvFiles/NetflixStockQuote.csv";
            string extension = Path.GetExtension(filePath);
            var strategy = ImportStrategyPicker.Select(extension);
            return strategy.ImportStockQuote(filePath);
        }
    }
}