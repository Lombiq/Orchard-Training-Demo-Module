using OrchardHUN.TrainingDemo.Services;
using System;
using System.Web.Mvc;

namespace OrchardHUN.TrainingDemo.Controllers
{
    // Nothing new here.
    public class CacheController : Controller
    {
        private readonly IDateTimeCachingService _dateTimeCachingService;


        public CacheController(IDateTimeCachingService dateTimeCachingService) => 
            _dateTimeCachingService = dateTimeCachingService;


        // Go to ~/OrchardHUN.TrainingDemo/Cache/GetDateTime to see the result.
        public DateTime GetDateTime(string service = "CacheService") =>
            // Nothing fancy, just the date and time displayed with plain text.
            _dateTimeCachingService.GetCachedDateTime(service);

        // We're exposing the trigger for the signal in a normal controller action, reachable at 
        // ~/OrchardHUN.TrainingDemo/Cache/InvalidateDateTime.
        public ActionResult InvalidateDateTime()
        {
            _dateTimeCachingService.InvalidateCachedDateTime();

            // After invalidating the cache entry, we'll show the regenerated data.
            return RedirectToAction(nameof(CacheController.GetDateTime));
        }
    }
}

// NEXT STATION: Controllers/FileManagementController