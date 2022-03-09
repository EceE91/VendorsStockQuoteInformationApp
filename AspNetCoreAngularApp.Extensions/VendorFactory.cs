using System;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Data.StockQuoteServices;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions
{
    public class VendorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public VendorFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
    
        public IStockQuoteService GetStockQuoteService(string vendorSymbol)
        {
            if(vendorSymbol == "NFLX")
                return (IStockQuoteService)_serviceProvider.GetService(typeof(NetflixStockQuoteService));
            return (IStockQuoteService)_serviceProvider.GetService(typeof(AppleStockQuoteService));
        }
    }
}