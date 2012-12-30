using Orchard.Caching;
using Orchard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrchardHUN.TrainingDemo.Services
{
    public class DateCachingService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;

        public DateCachingService(ICacheManager cacheManager, IClock clock)
        {
            _cacheManager = cacheManager;
            _clock = clock;
        }

        private DateTime GetDateTime()
        {
            return _cacheManager.Get("OrchardHUN.TrainingDemo.CurrentDateTime", ctx =>
                {
                    ctx.Monitor(_clock.When(TimeSpan.FromMinutes(5)));
                    return DateTime.Now;
                });
        }
    }
}