/*
 * This is a controller you can find in any ASP.NET Core MVC application; Orchard is an MVC application, although much-much more 
 * than that.
 * 
 * An Orchard module is basically an MVC area. You could create areas that don't interact with Orchard and you'd just use standard 
 * ASP.NET Core MVC skills. Of course we want more than that, so let's take a closer look.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.Notify;

namespace Lombiq.TrainingDemo.Controllers
{
    public class YourFirstOrchardCoreController : Controller
    {
        private readonly INotifier _notifier;
        private readonly IStringLocalizer<YourFirstOrchardCoreController> T;
        private readonly IHtmlLocalizer<YourFirstOrchardCoreController> H;


        public YourFirstOrchardCoreController(
            INotifier notifier,
            IStringLocalizer<YourFirstOrchardCoreController> stringLocalizer,
            IHtmlLocalizer<YourFirstOrchardCoreController> htmlLocalizer)
        {
            _notifier = notifier;

            T = stringLocalizer;
            H = htmlLocalizer;
        }


        public ActionResult Index() =>
            // For now we just return an empty view. This action is accessible from under
            // Lombiq.TrainingDemo/YourFirstOrchardCore/Index route (appended to your site's root path; so using defaults
            // it would look something like this: https://localhost:44300/Lombiq.TrainingDemo/YourFirstOrchardCore/Index).
            // If you don't know how this path gets together take a second look at how ASP.NET Core MVC routing works!
            View(new { Message = T["Hello you!"] });

        // This attribute will override the default route (see above) and use a custom one. This is also something that is an
        // ASP.NET Core MVC feature but this can be used on Orchard Core controllers as well.
        public ActionResult NotifyMe()
        {
            _notifier.Information(H["Congratulations! You have been notified!"]);

            return View();
        }
    }
}

/*
 * If you've finished with both actions (and their .cshtml files as well), then
 * NEXT STATION: Controllers/BasicOrchardCoreServicesController is what's next.
 */
