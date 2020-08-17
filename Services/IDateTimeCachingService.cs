using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Services
{
    /// <summary>
    /// Service to demonstrate caching by caching the current <see cref="DateTime"/>. Not much to see here apart from
    /// some docs (this is how we recommend documenting interfaces), for the implementation check out
    /// <see cref="DateTimeCachingService"/>.
    /// </summary>
    public interface IDateTimeCachingService
    {
        /// <summary>
        /// Retrieves the current local <see cref="DateTime"/> and caches it with <see cref="IMemoryCache"/>.
        /// </summary>
        /// <returns>The current local <see cref="DateTime"/> or the cached value.</returns>
        Task<DateTime> GetMemoryCachedDateTimeAsync();

        /// <summary>
        /// Retrieves the current local <see cref="DateTime"/> and caches it with the Orchard Core Dynamic Cache for
        /// 30s.
        /// </summary>
        /// <returns>The current local <see cref="DateTime"/> or the cached value.</returns>
        Task<DateTime> GetDynamicCachedDateTimeWith30SecondsExpiryAsync();

        /// <summary>
        /// Retrieves the current local <see cref="DateTime"/> and caches it with the Orchard Core Dynamic Cache,
        /// specific to the current route.
        /// </summary>
        /// <returns>The current local <see cref="DateTime"/> or the cached value.</returns>
        Task<DateTime> GetDynamicCachedDateTimeVariedByRoutesAsync();

        /// <summary>
        /// Invalidates all the entries cached via this services.
        /// </summary>
        Task InvalidateCachedDateTimeAsync();
    }
}
