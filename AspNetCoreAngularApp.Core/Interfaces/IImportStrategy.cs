using System.Collections.Generic;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces
{
    public interface IImportStrategy
    {
        StockQuote ImportStockQuote(string filePath);
    }
}