/*
 * If you want to access tenant-level settings for your website you use Site Settings. These are singleton objects
 * stored on the ISite object. If you want to access them you need to use the ISiteService service. Adding a site
 * settings object related to the features in your module will also be demonstrated here.
 */

using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.Entities;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Controllers
{
    public class SiteSettingsController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly DemoSettings _demoSettings;


        public SiteSettingsController(ISiteService siteService, IOptionsSnapshot<DemoSettings> demoOptions)
        {
            _siteService = siteService;
            _demoSettings = demoOptions.Value;
        }


        // Here's a quick simple demonstration about how to use ISiteService. Orchard Core stores basic settings that
        // are accessible right away in the ISite object. Here you will see how to access the site's name you gave when
        // you set up your website.
        public async Task<string> SiteName() =>
            (await _siteService.GetSiteSettingsAsync()).SiteName;

        // NEXT STATION: Models/DemoSettings.cs

        // Now let's see how we access the newly created site settings! Obviously it won't come with a value by default
        // so give it a value on the Dashboard if you want to see something here.
        public async Task<string> DemoSettings()
        {
            // As mentioned the custom settings objects are serialized into the ISite object so use the .As<>() helper
            // to access it as you see below.
            var messageFromSiteSettings = (await _siteService.GetSiteSettingsAsync()).As<DemoSettings>().Message;

            // But as you've seen in DemoSettings.cs our site settings are also Options so we can use it as such too:
            var messageFromOptions = _demoSettings.Message;
            // This, however, will of course be the same, since it's also produced from site settings... Or will it be?
            // Check it out!
            // NEXT STATION: Services/DemoSettingsConfiguration.cs

            // Note that we injected IOptionsSnapshot<DemoSettings>: This will always fetch the latest options, so e.g.
            // reload them from the DB. If you instead want the settings to be cached and only refresh when the app is
            // restarted you can inject IOptions<DemoSettings>.

            return $"Message from site settings: \"{messageFromSiteSettings}\". From options: \"{messageFromOptions}\"";
        }
    }
}

// END OF TRAINING SECTION: Site settings and IConfiguration

// NEXT STATION: Filters/ShapeInjectionFilter.cs