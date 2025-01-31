using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonService.Utility
{
    public class Cache
    {

        //using redis cache management system to store data for a particular time
        public async static Task<T> Get<T> (IDistributedCache _cache, string key) where T : class
        {
            try
            {
                var cachedResponse = await _cache.GetStringAsync(key);
                return cachedResponse == null ? null : JsonConvert.DeserializeObject<T>(cachedResponse, new JsonSerializerSettings() { 
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore 
                    // this will ignore error when there is any parent-child relation ship 
                });

            } catch(Exception ex)
            {
                Console.WriteLine("Common: " + ex.Message);
                throw ex;
            }
        }

        public async static Task Set<T>(IDistributedCache cache, string key, T value, int absoluteMinutes, int slidingMinute) where T : class
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(absoluteMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(slidingMinute)
            };

            var response = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            await cache.SetStringAsync(key, response, cacheOptions);
        }

        public async static Task SetBySeconds<T>(IDistributedCache cache, string key, T value, int absoluteSeconds, int slidingSeconds) where T : class
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(absoluteSeconds),
                SlidingExpiration = TimeSpan.FromSeconds(slidingSeconds)
            };

            var response = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            await cache.SetStringAsync(key, response, cacheOptions);
        }

        public async static Task Clear(IDistributedCache _cache, string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
            } catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
