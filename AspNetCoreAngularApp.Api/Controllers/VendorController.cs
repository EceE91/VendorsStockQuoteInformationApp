using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;

        public VendorController(IMapper mapper, IVendorRepository vendorRepository)
        {
            _mapper = mapper;
            _vendorRepository = vendorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorViewModel>>> GetVendors()
        {
            try
            {
                var vendors = await _vendorRepository.GetAllAsync();
                return new ActionResult<IEnumerable<VendorViewModel>>(_mapper.Map<IEnumerable<Vendor>, IEnumerable<VendorViewModel>>(vendors));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}