using System;
using System.Threading.Tasks;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockQuoteController: ControllerBase
    {
        private readonly VendorFactory _vendorFactory;
        private readonly IMapper _mapper;

        public StockQuoteController(IMapper mapper, VendorFactory vendorFactory)
        {
            _mapper = mapper;
            _vendorFactory = vendorFactory;
        }

        [HttpGet("{vendorSymbol}")]
        public ActionResult<StockQuoteViewModel> GetStockQuote(string vendorSymbol)
        {
            try
            {
                var stockQuote = _vendorFactory.GetStockQuoteService(vendorSymbol).FetchStockQuoteInformation();
                return _mapper.Map<StockQuote, StockQuoteViewModel>(stockQuote);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}