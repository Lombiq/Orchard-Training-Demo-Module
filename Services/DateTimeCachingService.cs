using Orchard;
using Orchard.Caching;
using Orchard.Services;
using System;

/*
 * In this section, we'll implement a small service that will cache the current time and date
 * for a given amount of time. Just for the sake of the example, of course.
 */

namespace OrchardHUN.TrainingDemo.Services
{
    // We're creating an interface derived from IDependency to be able to inject our CachingService
    // to wherever we need it.
    // Normally we place interfaces into separate code files but we tend not to do that if they're very tiny.
    public interface IDateTimeCachingService : IDependency
    {
        DateTime GetCachedDateTime();
        void InvalidateCachedDateTime();
    }

    public class DateTimeCachingService : IDateTimeCachingService
    {
        // For now we'll use ICacheManager but you should know that there is already another caching service,
        // ICacheService, which is implemented in the Contrib.Cache module and will ship with Orchard 1.7:
        // http://gallery.orchardproject.net/List/Modules/Orchard.Module.Contrib.Cache
        // That will most possibly deprecate ICacheManager for most of the use-cases, because they serve differently:
        // *    ICacheManager is an instance-specific cache: everything you store there will be available for that Orchard
        //      instance only.
        // *    ICacheService however can have implementations for distributed caching (like memcached) so items stored
        //      in the cache are available for every instance in a multi-instance (i.e. multi-node, multi-server, web farm)
        //      setup.
        // Both services are tenant-specific, i.e. if you cache something you'll only be able to access it from the
        // same tenant (see http://docs.orchardproject.net/Documentation/Setting-up-a-multi-tenant-orchard-site for
        // multi-tenancy).
        private readonly ICacheManager _cacheManager;

        // IClock is a service to access the system clock in an injected (and thus mockable and testable) way.
        // Also it serves as a cache monitor; you'll see the usage shortly.
        private readonly IClock _clock;

        // We're using ISignals to be able to send a signal to the cache to invalidate the given entry: with 
        // ICacheManager entries are not directly invalidated but through sending invalidating signals.
        private readonly ISignals _signals;

        // This is a unique identifier for the signal.
        private const string InvalidateDateTimeCacheSignal = "OrchardHUN.TrainingDemo.InvalidateCachedDateTime";


        public DateTimeCachingService(ICacheManager cacheManager, IClock clock, ISignals signals)
        {
            _cacheManager = cacheManager;
            _clock = clock;
            _signals = signals;
        }


        // This method will return the date and time from the cache if the stored data is still valid,
        // else it will be regenerated and then returned.
        public DateTime GetCachedDateTime()
        {
            // The first parameter should be a unique identifier for your cache entry.
            return _cacheManager.Get("OrchardHUN.TrainingDemo.CurrentDateTime", ctx =>
                {
                    // Not that here we use two kinds of cache entry invalidation but normally you use only one.

                    // We are "monitoring" the expiration of the cache entry using the Clock service,
                    // which will invalidate the cache entry when a given amount of time has passed.
                    // Use this if the cache entry should be periodically invalidated.
                    ctx.Monitor(_clock.When(TimeSpan.FromSeconds(90)));
                    
                    // One of ISignal's method is "When", just like with IClock. This will trigger the invalidation
                    // of this cache entry when the "Trigger" method is called with the same parameter.
                    // Use this if you want to invalidate the cache entry explicitly.
                    ctx.Monitor(_signals.When(InvalidateDateTimeCacheSignal));

                    return DateTime.Now;
                });
        }

        // We're providing a public method in our service,
        public void InvalidateCachedDateTime()
        {
            // in which we trigger the signal to invalidate the cache entry.
            _signals.Trigger(InvalidateDateTimeCacheSignal);
        }
    }
}

// NEXT STATION: Controllers/CacheController.cs!