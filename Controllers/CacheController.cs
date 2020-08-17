/*
 * There are multiple ways to cache data in ASP.NET Core which are used in Orchard Core as well. For example the
 * content items are cached using the IMemoryCache service. In-memory caching holds the copy of data in local server’s
 * memory which could be enough in most cases. Some features, however, require some extra functionality and more
 * complex ways of caching such as shapes. In Orchard Core there is a so-called Dynamic Cache which is built upon the
 * Distributed Cache. Distributed cache maintains cache in a centralized location which is accessible by servers in a
 * cluster. In Orchard Core this is also memory cache but opens up the possibility to use Redis for example. In this
 * tutorial we will see a couple of examples of caching a DateTime object and shape caching will also be demonstrated.
 */

using Lombiq.TrainingDemo.Services;
using Lombiq.TrainingDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Controllers
{
    public class CacheController : Controller
    {
        // The actual caching is implemented in a service which we'll soon investigate.
        private readonly IDateTimeCachingService _dateTimeCachingService;


        public CacheController(IDateTimeCachingService dateTimeCachingService) => _dateTimeCachingService = dateTimeCachingService;


        // In this action we'll cache a DateTime three different ways.
        public async Task<ActionResult> Index()
        {
            // This one will be cached using the built-in ASP.NET Core IMemoryCache.
            var memoryCachedDateTime = await _dateTimeCachingService.GetMemoryCachedDateTimeAsync();

            // This one will be using the DynamicCache provided by Orchard Core. It will have a 30 second expiration.
            var dynamicCachedDateTimeWith30SecondsExpiry =
                await _dateTimeCachingService.GetDynamicCachedDateTimeWith30SecondsExpiryAsync();

            // Finally this date will be cached only for this route.
            var dynamicCachedDateTimeVariedByRoutes =
                await _dateTimeCachingService.GetDynamicCachedDateTimeVariedByRoutesAsync();

            // NEXT STATION: Services/DateTimeCachingService.cs

            return View("Index", new CacheViewModel
            {
                MemoryCachedDateTime = memoryCachedDateTime,
                DynamicCachedDateTimeWith30SecondsExpiry = dynamicCachedDateTimeWith30SecondsExpiry,
                DynamicCachedDateTimeVariedByRoutes = dynamicCachedDateTimeVariedByRoutes,
            });
        }

        // This action will result in the same page as Index, however, the route will be different so the
        // route-specific cache can be tested.
        public Task<ActionResult> DifferentRoute() =>
            Index();

        // This action will invalidate the memory cache and all the route-specific caches. Calling this action will
        // redirect back to the Index page and you will notice that the date value is updated.
        public async Task<ActionResult> InvalidateDateTimeCache()
        {
            await _dateTimeCachingService.InvalidateCachedDateTimeAsync();

            return RedirectToAction("Index");
        }

        // Now let's see how Orchard Core utilizes the Dynamic Cache the most. In this action you will see how shapes
        // are cached.
        public ActionResult Shape() =>
            View();

        // NEXT STATION: Views/Cache/Shape.cshtml
    }
}
