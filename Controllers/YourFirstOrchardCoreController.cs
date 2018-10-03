/*
 * This is a controller you can find in any ASP.NET Core MVC application; Orchard is an MVC application, although much-much more 
 * than that.
 * 
 * An Orchard module is basically an MVC area. You could create areas that don't interact with Orchard and you'd just 
 * use standard ASP.NET Core MVC skills. Of course we want more than that, so let's take a closer look.
 */

using Microsoft.AspNetCore.Mvc;

namespace Lombiq.TrainingDemo.Controllers
{
    public class YourFirstOrchardCoreController : Controller
    {
        public ActionResult Index() =>
            // For now we just return an empty view. This action is accessible from under
            // Lombiq.TraningDemo/YourFirstOrchard/Index route (appended to your site's root path; so using defaults it
            // would look something like this:
            // http://localhost:44300/Lombiq.TraningDemo/YourFirstOrchard/Index) If you don't know how this
            // path gets together take a second look at how ASP.NET Core MVC routing works!
            View();

        // This attribute will override the default route (see above) and use a custom one. This is also something that is an
        // ASP.NET Core MVC feature but this can be used on Orchard Core controllers as well.
        [Route("TrainingDemo/ActionWithRoute")]
        public ActionResult ActionWithRoute() =>
            View();
    }
}

/*
 * If you've finished with both actions (and their .cshtml files as well), then
 * NEXT STATION: Controllers/BasicOrchardCoreServicesController is what's next.
 */
