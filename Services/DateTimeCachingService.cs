using Orchard;
using Orchard.Caching;
using Orchard.Services;
using System;
using System.Threading;

/*
 * In this section, we'll implement a small service that will cache the current time and date
 * for a given amount of time.
 */

namespace OrchardHUN.TrainingDemo.Services
{
    // We're creating an interface derived from IDependency to be able to inject our Cache Service
    // to wherever we need it.
    public interface IDateTimeCachingService : IDependency
    {
        DateTime GetCachedDateTime();
        void InvalidateCachedDateTime();
    }

    public class DateTimeCachingService : IDateTimeCachingService
    {
        // Most of the time when you want to cache something, you're going to need to use the
        // built-in ICacheManager, which has an authority in the current tenant.
        // For wider availablity, for example to cache data over a webfarm, you should use ICacheService,
        // which is implemented in the Contrib.Cache module:
        // http://gallery.orchardproject.net/List/Modules/Orchard.Module.Contrib.Cache
        private readonly ICacheManager _cacheManager;

        // IClock is a cache-related service, which we'll use to determine if the cached data is expired.
        private readonly IClock _clock;

        // We're using ISignals to be able to send a signal to the cache to invalidate the given entry.
        private readonly ISignals _signals;
        // This is a unique identifier for the signal.
        private const string _invalidateDateTimeCacheSignal = "OrchardHUN.TrainingDemo.InvalidateCachedDateTime";

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
                    // We are "monitoring" the expiration of the cache entry using the Clock service,
                    // which will invalidate the cache entry when a given amount of time has passed.
                    ctx.Monitor(_clock.When(TimeSpan.FromSeconds(90)));
                    
                    // One of ISignal's method is "When", just like with IClock. This will trigger the invalidation
                    // of this cache entry when the "Trigger" function is called with the same parameter.
                    ctx.Monitor(_signals.When(_invalidateDateTimeCacheSignal));

                    // We'll put the thread to sleep for 1 second to be able to easily determine if we're getting
                    // cached data or not.
                    Thread.Sleep(1000);

                    return DateTime.Now;
                });
        }

        // We're providing a public method in our service,
        public void InvalidateCachedDateTime()
        {
            // in which we trigger the signal to invalidate the cache entry.
            _signals.Trigger(_invalidateDateTimeCacheSignal);
        }
    }
}

// NEXT STATION: CacheController.cs!