using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreAngularApp.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace AspNetCoreAngularApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheKey = "HotelList";
        private readonly MemoryCacheEntryOptions _cacheExpiryOptions;

        private List<Hotel> HotelList { get; set; }
        public HotelController(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;
            _cacheExpiryOptions = new MemoryCacheEntryOptions
                                  {
                                      AbsoluteExpiration = DateTime.Now.AddMinutes(20),
                                      Priority = CacheItemPriority.High,
                                      SlidingExpiration = TimeSpan.FromMinutes(20)
                                  };
        }
        
        private void ReadHotelsFromFile()
        {
            HotelList = new List<Hotel>();
         
            var filePath = Directory.GetCurrentDirectory() + "/Data/HotelList.json";
            StreamReader streamReader = new StreamReader(filePath);
            string jsonString = streamReader.ReadToEnd();
            if (IsValidJson(jsonString))
            {
                var deserializedHotels = JsonConvert.DeserializeObject<List<Hotel>>(jsonString);
                foreach (var hotel in deserializedHotels)
                {
                    HotelList.Add(hotel);
                }
            }
        }
        
        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false;}
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") || //For object
                strInput.StartsWith("[") && strInput.EndsWith("]")) //For array if any
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    throw new JsonReaderException();
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    throw new Exception(ex.Message);
                }
            }
            return false;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotel()
        {
            if (!_memoryCache.TryGetValue(_cacheKey, out List<Hotel> cachedHotels))
            {
                try
                {
                    ReadHotelsFromFile();
                    cachedHotels = HotelList;
                    SetCache(_cacheKey, cachedHotels, _cacheExpiryOptions);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return cachedHotels;
        }
        
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
        {
            var cachedItems = _memoryCache.Get<List<Hotel>>(_cacheKey);
            // create new Id
            var maxId = cachedItems.OrderByDescending(t => t.Id).First().Id;
            hotel.Id = ++maxId;
            cachedItems.Add(hotel);
            UpdateCache(cachedItems);
            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }
        
        // DELETE: api/Hotel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hotel>> DeleteHotel(int id)
        {
            //if (_memoryCache != null && ((MemoryCache)_memoryCache).Count > 0)
            var cachedItems = _memoryCache.Get<List<Hotel>>(_cacheKey);
            if (cachedItems.Any(x => x.Id == id))
            {
                var hotel = cachedItems.FirstOrDefault(x => x.Id == id);
                cachedItems.Remove(hotel);
                UpdateCache(cachedItems);
                return hotel;
            }
            return NotFound();
        }
        
        // PUT: api/Hotel/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            } 
            var cachedItems = _memoryCache.Get<List<Hotel>>(_cacheKey);
            if (cachedItems.All(x => x.Id != id))
            {
                return NotFound();
            }

            cachedItems.Where(x => x.Id == id).Select(
                h =>
                {
                    h = hotel;
                    return h;
                }
            );

            UpdateCache(cachedItems);
            return NoContent();
        }
        
        private void SetCache(string key,List<Hotel> itemsToBeCached, MemoryCacheEntryOptions expiryOptions)
        {
            _memoryCache.Set(key, itemsToBeCached, expiryOptions);
        }
        
        private void UpdateCache(List<Hotel> itemsToBeCached)
        {
            _memoryCache.Set(_cacheKey, itemsToBeCached, _cacheExpiryOptions);
        }
    }
}