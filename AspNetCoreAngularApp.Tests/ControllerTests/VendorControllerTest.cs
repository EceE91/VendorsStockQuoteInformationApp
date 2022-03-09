using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.Controllers;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;

namespace AspNetCoreAngularApp.Tests.ControllerTests
{
    public class VendorControllerTest
    {
        [Fact]
        public async void GetAllAsync_ReturnsListOfVendors()
        {
            // Arrange
            var mockRepo = new Mock<IVendorRepository>();
            var mockMapper = new Mock<IMapper>();

            var testVendors = GetTestVendors();
            mockRepo.Setup(repo => repo.GetAllAsync())
                    .Returns(Task.FromResult(testVendors));
            
            var viewModel = new List<VendorViewModel>
                            {
                                new()
                                {
                                    Name = "Netflix Inc",
                                    Symbol = "NFLX",
                                    Exchange = "NASDAQ"
                                }
                            };
            mockMapper.Setup(m => m.Map<IEnumerable<Vendor>, IEnumerable<VendorViewModel>>(testVendors))
                      .Returns(viewModel);

            var controller = new VendorController(mockMapper.Object, mockRepo.Object);

            // Act
            var result = await controller.GetVendors();
            
            // assert
            result.Value.Should().BeEquivalentTo(viewModel);
        }
        
        [Fact]
        public async void GetAllAsync_WhenExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IVendorRepository>();
            var mockMapper = new Mock<IMapper>();

            mockRepo.Setup(repo => repo.GetAllAsync())
                    .Throws(new Exception());
            
            var controller = new VendorController(mockMapper.Object, mockRepo.Object);

            // Act
            var result = await controller.GetVendors();
            
            // assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        private static IEnumerable<Vendor> GetTestVendors()
        {
            var vendors = new List<Vendor>
                          {
                              new()
                              {
                                  Name = "Netflix Inc",
                                  Symbol = "NFLX",
                                  Exchange = "NASDAQ"
                              }
                          };
            return vendors;
        }
    }
}