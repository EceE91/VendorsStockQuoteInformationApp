using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AspNetCoreAngularApp.Controllers;
using AspNetCoreAngularApp.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace AspNetCoreAngularApp.Tests
{
    public class HotelControllerTest
    {
        private readonly List<Hotel> _hotels;
        private string _cacheKey = "HotelList";
        public HotelControllerTest()
        {
            _hotels = new List<Hotel>();
            _hotels = GetAllTestData();
        }

        [Fact]
        public async void GetHotel_WhenReadsFromAValidJsonFile_FetchesAllHotelsSuccessfully()
        {
            //arrange
            var expected = _hotels;
            var cache = SetupCache();
            HotelController hotelController = new HotelController(cache);

            //act
            var result = await hotelController.GetHotel();

            //assert
            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void  GetHotel_WhenCalledSecondTime_FetchesAllHotelsFromCacheSuccessfully()
        {
            //arrange
            var cache = SetupCache();
            HotelController hotelController = new HotelController(cache);
            
            //act
            await hotelController.GetHotel();
            var result2 = await hotelController.GetHotel();
            
            //assert
            cache.TryGetValue(_cacheKey, out var cachedResult);
            result2.Value.Should().BeEquivalentTo(cachedResult as List<Hotel>);
        }
        
        [Fact]
        public async void  PostHotel_WhenNewHotelIsAdded_NewHotelIsCreatedSuccessfully()
        {
            //arrange
            HotelController hotelController = new HotelController(SetupCache());
            
            //act
            var newHotel = new Hotel
                           {
                               Name = "hotel 21",
                               Description = "hotel 21 desc",
                               Location = "Hotel 21 location",
                               Rating = 4
                           };
            
            //act
            var result =  await hotelController.PostHotel(newHotel);
            var item = (Hotel)((CreatedAtActionResult)result.Result).Value;

            // Assert
            item.Id.Should().NotBe(null);
        }
        
        [Fact]
        public async void  DeleteHotel_WhenAHotelIsDeleted_ReturnsUpdatedListSuccessfully()
        {
            //arrange
            HotelController hotelController = new HotelController(SetupCache());
            
            //act
            var hotelToBeDeleted = _hotels.First();
            var result =  await hotelController.DeleteHotel(hotelToBeDeleted.Id);
            
            // Assert
            result.Value.Id.Should().Be(hotelToBeDeleted.Id);
        }
        
        [Fact]
        public async void  DeleteHotel_WhenHotelIdNotFound_ReturnsNotFound()
        {
            //arrange
            HotelController hotelController = new HotelController(SetupCache());
            
            //act
            var hotelIdToBeDeleted = _hotels.Max(x=>x.Id) + 10 ; // make up an id
            var result =  await hotelController.DeleteHotel(hotelIdToBeDeleted);
            
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public async void  PutHotel_WhenAHotelIsUpdated_ReturnsUpdatedListSuccessfully()
        {
            //arrange
            HotelController hotelController = new HotelController(SetupCache());
            
            //act
            var hotelToBeUpdated = _hotels.First();
            hotelToBeUpdated.Name = "this name is changed";
            hotelToBeUpdated.Description = "this description is changed";
            var result =  await hotelController.PutHotel(hotelToBeUpdated.Id, hotelToBeUpdated);
            
            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async void  PutHotel_WhenAHotelIsNotFoundInCache_ReturnsNotFound()
        {
            //arrange
            HotelController hotelController = new HotelController(SetupCache());

            //act
            var nonExistingHotel = new Hotel
                                   {
                                       Name = "hotel 21",
                                       Description = "hotel 21 desc",
                                       Location = "Hotel 21 location",
                                       Rating = 4
                                   };
            var result =  await hotelController.PutHotel(nonExistingHotel.Id, nonExistingHotel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        private IMemoryCache SetupCache()
        {
            //arrange
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            if(memoryCache.TryGetValue(_cacheKey, out List<Hotel> cached)) memoryCache.Remove(_cacheKey);
            memoryCache.Set(_cacheKey, GetAllTestData());
            return memoryCache;
        }
        
        private List<Hotel> GetAllTestData()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData.json");
            var json = File.ReadAllText(filePath);
            var deserializedHotels = JsonConvert.DeserializeObject<List<Hotel>>(json);

            return deserializedHotels.ToList();
        }
    }
}