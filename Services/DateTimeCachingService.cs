using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using OrchardCore.DynamicCache;
using OrchardCore.Environment.Cache;
using OrchardCore.Modules;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Services;

public class DateTimeCachingService(
    IMemoryCache memoryCache,
    ILocalClock localClock,
    IDynamicCacheService dynamicCacheService,
    ITagCache tagCache,
    ISignal signal) : IDateTimeCachingService
{
    public const string MemoryCacheKey = "Lombiq.TrainingDemo.MemoryCache.DateTime";
    public const string DynamicCacheKey = "Lombiq.TrainingDemo.DynamicCache.DateTime";
    public const string DynamicCacheTag = "Lombiq.TrainingDemo.DynamicCache.DateTime.Tag";

    // This method will get or create the cached DateTime object using the IMemoryCache.
    public async Task<DateTime> GetMemoryCachedDateTimeAsync()
    {
        if (!memoryCache.TryGetValue(MemoryCacheKey, out DateTime cachedDate))
        {
            cachedDate = (await localClock.LocalNowAsync).DateTime;

            memoryCache.Set(MemoryCacheKey, cachedDate, GetMemoryCacheChangeToken());
        }

        return cachedDate;
    }

    // This method a DateTime object will be cached with a 30 second expiration using Orchard Core Dynamic Cache.
    public Task<DateTime> GetDynamicCachedDateTimeWith30SecondsExpiryAsync() =>
        // To cache and object or retrieve them a CacheContext object must be used. It has one mandatory property, the
        // cache key. The caching parameters are also set in this object. To see how it works go inside this method.
#pragma warning disable SA1114 // Parameter list should follow declaration (necessary for the comment)
        GetOrCreateDynamicCachedDateTimeAsync(
            // Notice that the CacheContext object has chainable methods so you can use them to populate the settings.
            new CacheContext(DynamicCacheKey).WithExpiryAfter(TimeSpan.FromSeconds(30)));

#pragma warning restore SA1114 // Parameter list should follow declaration

    // This method will cache the current date similarly then the previous one however instead of setting a 30 second
    // expiration it will be tagged so later it can be invalidated. Also this cache will be differentiated by the route.
    // This means that the DateTime cached here on one route will be unaccessible on other routes so another DateTime
    // will be cached for that other particular route. There are multiple differentiators already implemented in Orchard
    // Core, see: https://docs.orchardcore.net/en/latest/docs/reference/modules/DynamicCache/.
    public Task<DateTime> GetDynamicCachedDateTimeVariedByRoutesAsync() =>
        GetOrCreateDynamicCachedDateTimeAsync(
            new CacheContext(DynamicCacheKey)
                .AddContext("route")
                .AddTag(DynamicCacheTag));

    // Invalidates the memory cache and all the dynamic caches which have been tagged.
    public async Task InvalidateCachedDateTimeAsync()
    {
        // As mentioned ISignal service is used to invalidate the memory cache. This will invalidate all cache entries
        // if there are multiple ones related to the token.
        await signal.SignalTokenAsync(MemoryCacheKey);

        // ITagCache.RemoveTagAsync will invalidate all the dynamic caches which are tagged with the given tag.
        await tagCache.RemoveTagAsync(DynamicCacheTag);
    }

    // This change token is generated based on the cache key using the ISignal service. It is used to invalidate the
    // memory cache. You can use this not just as another way to invalidate specific entries but also a way to
    // invalidate many at the same time: You can use tie multiple cache entries to the same signal too.
    private IChangeToken GetMemoryCacheChangeToken() => signal.GetToken(MemoryCacheKey);

    private async Task<DateTime> GetOrCreateDynamicCachedDateTimeAsync(CacheContext cacheContext)
    {
        // Now that we have a cache context we try to acquire the object. The objects always need to be strings.
        var cachedDateTimeText = await dynamicCacheService.GetCachedValueAsync(cacheContext);

        // If the date time text is not null then parse it to DateTime otherwise use the ILocalClock service to set it
        // to the current date.
        var cachedDateTime = cachedDateTimeText != null ?
            DateTime.Parse(cachedDateTimeText, CultureInfo.InvariantCulture) :
            (await localClock.LocalNowAsync).DateTime;

        // If the date time text is null (meaning it wasn't cached) cache the DateTime object (which in this case is the
        // current date).
        if (cachedDateTimeText == null)
        {
            await dynamicCacheService.SetCachedValueAsync(
                cacheContext,
                cachedDateTime.ToString(CultureInfo.InvariantCulture));
        }

        return cachedDateTime;
    }

    // NEXT STATION: Views/Cache/Index.cshtml
}
