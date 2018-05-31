using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using StateCache.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StateCache.RequestPipeline.Middleware
{
    public class CachingLastTuples
    {
        readonly RequestDelegate _next;

        public CachingLastTuples(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, CarServiceContext carContext, IMemoryCache cache)
        {
            CheckExistance<Car>("CarsTuple", carContext, cache);
            CheckExistance<Inspector>("InspectorsTuple", carContext, cache);
            CheckExistance<CarTechState>("StatesTuple", carContext, cache);

            await _next(context);
        }

        void CheckExistance<T>(string key, CarServiceContext context, IMemoryCache cache) where T: class
        {
            if (!cache.TryGetValue(key, out T obj))
            {
                obj = context.Set<T>().Last();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(252));

                cache.Set(key, obj, cacheEntryOptions);
            }
        }
    }
}
