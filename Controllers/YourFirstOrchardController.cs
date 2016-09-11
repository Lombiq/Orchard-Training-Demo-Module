/*
 * This is a controller you can find in any MVC application; Orchard is an MVC application, although much-much more than 
 * that.
 * 
 * An Orchard module is basically an MVC area. You could create areas that don't interact with Orchard and you'd just 
 * use standard ASP.NET MVC skills. Of course we want more than that, so let's take a closer look.
 */

using System.Web.Mvc;
using Orchard.Themes;

namespace OrchardHUN.TrainingDemo.Controllers
{
    // Nothing special here
    public class YourFirstOrchardController : Controller
    {
        // Notice the attribute. This is the most basic piece of Orchard's API. This tells Orchard to place the action's
        // rendered view into the layout. I.e. the view returned by this action will get wrapped into the layout provided
        // by the current theme.
        [Themed]
        public ActionResult Index()
        {
            // For now we just return an empty view. This action is accessible from under
            // OrchardHUN.TrainingDemo/YourFirstOrchard route (appended to your site's root path; so using defaults it
            // would look something like this:
            // http://localhost:30320/OrchardLocal/OrchardHUN.TrainingDemo/YourFirstOrchard) If you don't know how this
            // path gets together take a second look at how ASP.NET MVC routing works!

            return View();

            // NEXT STEP: take a look at the view we're using at Views/YourFirstOrchard/Index
        }
    }
}