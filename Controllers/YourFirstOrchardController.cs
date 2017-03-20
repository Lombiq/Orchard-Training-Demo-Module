using Microsoft.AspNetCore.Mvc;

namespace OrchardHUN.TrainingDemo.Controllers
{
    public class YourFirstOrchardController : Controller
    {
        public ActionResult Index()
        {
            // For now we just return an empty view. This action is accessible from under
            // OrchardHUN.TrainingDemo/YourFirstOrchard route (appended to your site's root path; so using defaults it
            // would look something like this:
            // http://localhost:2918/OrchardHUN.TrainingDemo/YourFirstOrchard/Index) If you don't know how this
            // path gets together take a second look at how ASP.NET MVC routing works!

            return View();

            // NEXT STEP: take a look at the view we're using at Views/YourFirstOrchard/Index
        }
    }
}