using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using AutoMapper;
namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Api.Mappers
{
    public class MainProfile: Profile
    {
        public MainProfile()
        {
            //Vendor to VendorViewModel, and vice-versa
            CreateMap<Vendor, VendorViewModel>().ReverseMap();

            //StockQuote to StockQuoteViewModel, and vice-versa
            CreateMap<StockQuote, StockQuoteViewModel>().ReverseMap();
        }
    }
}