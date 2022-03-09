using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Data.Repositories
{
    public class VendorRepository: IVendorRepository
    {
        public Task<IEnumerable<Vendor>> GetAllAsync()
        {
            try
            {
                var vendors = new List<Vendor>
                              {
                                  new Vendor
                                  {
                                      Name = "Netflix Inc",
                                      Symbol = "NFLX",
                                      Exchange = "NASDAQ"
                                  },
                                  new Vendor
                                  {
                                      Name = "Apple Inc",
                                      Symbol = "APPL",
                                      Exchange = "NASDAQGS"
                                  }
                              };
                return Task.FromResult<IEnumerable<Vendor>>(vendors);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}