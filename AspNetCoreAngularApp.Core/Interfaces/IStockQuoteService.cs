using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces
{
    public interface IStockQuoteService
    {
        StockQuote FetchStockQuoteInformation();
    }
}