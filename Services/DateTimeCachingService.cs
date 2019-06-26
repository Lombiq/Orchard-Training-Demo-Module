using Microsoft.Extensions.Caching.Memory;
using OrchardCore.DynamicCache;
using OrchardCore.Environment.Cache;
using OrchardCore.Modules;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Services
{
    public interface IDateTimeCachingService
    {
        Task<DateTime> GetMemoryCachedDateTimeAsync();
        Task<DateTime> GetDynamicCachedDateTimeWith30SecondsExpiryAsync();
        Task<DateTime> GetDynamicCachedDateTimeVariedByRoutesAsync();
        Task InvalidateCachedDateTimeAsync();
    }


    public class DateTimeCachingService : IDateTimeCachingService
    {
        public const string MemoryCacheKey = "Lombiq.TrainingDemo.MemoryCache.DateTime";
        public const string DynamicCacheKey = "Lombiq.TrainingDemo.DynamicCache.DateTime";
        public const string DynamicCacheTag = "Lombiq.TrainingDemo.DynamicCache.DateTime.Tag";


        // You've already seen the IClock service for getting the current UTC date. This service can be used to get the
        // current local date based on the site settings. Also dates can be converted from or to UTC.
        private readonly ILocalClock _localClock;

        // IMemoryCache service is a built-in service in ASP.NET Core. To learn more about this visit
        // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory.
        private readonly IMemoryCache _memoryCache;

        // Dynamic Cache is implemented primarily for caching shapes. It is based on the built-in ASP.NET Core
        // IDistributedCache service which by default is implemented by DistributedMemoryCache. To learn more about
        // distributed caching and IDistributedCache visit
        // https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed.
        private readonly IDynamicCacheService _dynamicCacheService;

        // Tag cache is a service for tagging cached data and invalidating cache by their tags.
        private readonly ITagCache _tagCache;


        public DateTimeCachingService(
            IMemoryCache memoryCache, 
            ILocalClock localClock, 
            IDynamicCacheService dynamicCacheService,
            ITagCache tagCache)
        {
            _memoryCache = memoryCache;
            _localClock = localClock;
            _dynamicCacheService = dynamicCacheService;
            _tagCache = tagCache;
        }


        // This method will get or create the cached DateTime object using the IMemoryCache.
        public async Task<DateTime> GetMemoryCachedDateTimeAsync() =>
            await _memoryCache.GetOrCreateAsync(
                MemoryCacheKey,
                async (entry) => (await _localClock.LocalNowAsync).DateTime);

        // This method a DateTime object will be cached with a 30 second expiration using Orchard Core Dynamic Cache.
        public async Task<DateTime> GetDynamicCachedDateTimeWith30SecondsExpiryAsync() =>
            // To cache and object or retrieve them a CacheContext object must be used. It has one mandatory property,
            // the cache key. The caching parameters are also set in this object. To see how it works go inside this
            // method.
            await GetOrCreateDynamicCachedDateTimeAsync(
                // Notice that the CacheContext object has chainable methods so you can use them to populate the
                // settings.
                new CacheContext(DynamicCacheKey).WithExpiryAfter(TimeSpan.FromSeconds(30)));

        // This method will cache the current date similarly then the previous one however instead of setting a 30
        // second expiration it will be tagged so later it can be invalidated. Also this cache will be differentiated
        // by the route. This means that the DateTime cached here on one route will be unaccessible on other routes so
        // another DateTime will be cached for that other particular route. There are multiple differentiators already
        // implemented in Orchard Core, see:
        // https://orchardcore.readthedocs.io/en/dev/OrchardCore.Modules/OrchardCore.DynamicCache/#available-contexts.
        public async Task<DateTime> GetDynamicCachedDateTimeVariedByRoutesAsync() =>
            await GetOrCreateDynamicCachedDateTimeAsync(
                new CacheContext(DynamicCacheKey)
                    .AddContext("route")
                    .AddTag(DynamicCacheTag));

        // It will invalidate all the DateTime cached which has been tagged. In our example it will invalidate all the
        // route-specific caches.
        public async Task InvalidateCachedDateTimeAsync() =>
            await _tagCache.RemoveTagAsync(DynamicCacheTag);


        private async Task<DateTime> GetOrCreateDynamicCachedDateTimeAsync(CacheContext cacheContext)
        {
            // Now that we have a cache context we try to acquire the object. The objects always need to be strings.
            var cachedDateTimeText = await _dynamicCacheService.GetCachedValueAsync(cacheContext);

            // If the date time text is not null then parse it to DateTime otherwise use the ILocalClock service to set
            // it to the current date.
            var cachedDateTime = cachedDateTimeText != null ?
                DateTime.Parse(cachedDateTimeText, CultureInfo.InvariantCulture) :
                (await _localClock.LocalNowAsync).DateTime;

            // If the date time text is null (meaning it wasn't cached) cache the DateTime object (which in this case
            // is the current date).
            if (cachedDateTimeText == null)
            {
                await _dynamicCacheService.SetCachedValueAsync(
                    cacheContext,
                    cachedDateTime.ToString(CultureInfo.InvariantCulture));
            }

            return cachedDateTime;
        }

        // NEXT STATION: Views/Cache/Index.cshtml
    }
}
