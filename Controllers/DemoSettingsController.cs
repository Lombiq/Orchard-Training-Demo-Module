/*
 * We'll only use this controller to display the configuration we saved through our site settings part.
 */

using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Settings;
using OrchardHUN.TrainingDemo.Models;
using System.Web.Mvc;

namespace OrchardHUN.TrainingDemo.Controllers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class DemoSettingsController : Controller
    {
        private readonly ISiteService _siteService;


        public DemoSettingsController(ISiteService siteService) => _siteService = siteService;


        // You can open this action by visiting ~/OrchardHUN.TrainingDemo/DemoSettings
        public string Index() =>
            // Simply fetching site settings and handling it as normal content items.
            // By using As<>() you can access a part that's attached to that content item.
            _siteService.GetSiteSettings().As<DemoSettingsPart>().Message;
        
        // Go ahead, try to modify the settings and see if it really works :-).
        // NEXT STATION: how about modifying these settings from the command line? Jump over to 
        // Commands /DemoSettingsCommands!
    }
}