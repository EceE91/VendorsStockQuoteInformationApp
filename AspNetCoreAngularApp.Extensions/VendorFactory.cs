using System;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Data.StockQuoteServices;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions
{
    public class VendorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public VendorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    
        public IStockQuoteService GetStockQuoteService(string vendorSymbol)
        {
            if(vendorSymbol == "NFLX")
                return (IStockQuoteService)_serviceProvider.GetRequiredService(typeof(NetflixStockQuoteService));
            return (IStockQuoteService)_serviceProvider.GetRequiredService(typeof(AppleStockQuoteService));
        }
    }
}