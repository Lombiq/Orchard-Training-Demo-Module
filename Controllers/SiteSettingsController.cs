/*
 * If you want to access tenant-level settings for your website you use Site Settings. These are singleton objects
 * stored on the ISite object. If you want to access them you need to use the ISiteService service. Adding a site
 * settings object related to the features in your module will also be demonstrated here.
 */

using System.Threading.Tasks;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Entities;
using OrchardCore.Settings;

namespace Lombiq.TrainingDemo.Controllers
{
    public class SiteSettingsController : Controller
    {
        private readonly ISiteService _siteService;


        public SiteSettingsController(ISiteService siteService)
        {
            _siteService = siteService;
        }


        // Here's a quick a simple demonstration about how to use ISiteService. Orchard Core stores basic settings that
        // are accessible right away in the ISite object. Here you will see how to access the site's name you gave when
        // you set up your website.
        public async Task<string> SiteName() =>
            (await _siteService.GetSiteSettingsAsync()).SiteName;

        // NEXT STATION: Models/DemoSettings.cs

        // Now let's see how we access the newly created site settings! Obviously it won't come with a value by default
        // so give it a value on the Dashboard if you want to see something here.
        public async Task<string> DemoSettings() =>
            // As mentioned the custom settings objects are serialized to the ISite object so use the .As<>() helper to
            // access it as you see below.
            (await _siteService.GetSiteSettingsAsync()).As<DemoSettings>().Message;
    }
}

// END OF TRAINING: Site settings

// NEXT STATION: Filters/ShapeInjectionFilter.cs