using OrchardHUN.TrainingDemo.Services;
using System;
using System.Web.Mvc;

namespace OrchardHUN.TrainingDemo.Controllers
{
    // Nothing new here.
    public class CacheController : Controller
    {
        private readonly IDateCachingService _dateCachingService;

        public CacheController(IDateCachingService dateCachingService)
        {
            _dateCachingService = dateCachingService;
        }

        // Go to ~/OrchardHUN.TrainingDemo/Cache/DateTime to see the result.
        public DateTime DateTime()
        {
            // Nothing fancy, just the date and time displayed with plain text.
            return _dateCachingService.GetCachedDateTime();
        }
    }
}

// NEXT STATION: City 17