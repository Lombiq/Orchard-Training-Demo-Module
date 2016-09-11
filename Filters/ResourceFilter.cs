using System.Web.Mvc;
using Orchard.Mvc.Filters;
using Orchard.UI.Resources;

/* What you're now looking at is an extension point for the available filters in this Orchard application.
 * Filters in Orchard work based on MVC's filters, so please read the short tutorial available at
 * http://www.asp.net/mvc/tutorials/older-versions/controllers-and-routing/understanding-action-filters-cs if you're not 
 * yet familiar with the basic concept.
 * 
 * "Fast forwarding Memory to a more recent one."
 * 
 * As you now know, filters in MVC are implementations of one of the four basic filter interfaces (IActionFilter, 
 * IAuthorizationFilter, IResultFilter, IExceptionFilter) and can be applied to actions using attributes. You can stick 
 * with that, but Orchard provides a little addition to that. If your filter class inherits from FilterProvider, your 
 * filter will be applied to every request (so you don't have to add attributes to any action for the filter to be 
 * applied), but you have to perform a check in the filter's method(s) to determine whether it's necessary to run the 
 * code in the given method, e.g. by checking a condition related to the context.
 */

namespace OrchardHUN.TrainingDemo.Filters
{
    // In our example, we'll create a ResultFilter spiced with Orchard stuff. Notice that you have to derive
    // from FilterProvider unless you want to implement IFilterProvider directly.
    public class ResourceFilter : FilterProvider, IResultFilter
    {
        // Let's inject a ResourceManager to be able to manage static resources like scripts or stylesheets.
        private readonly IResourceManager _resourceManager;


        public ResourceFilter(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }


        // This method will be called after the action result is executed.
        public void OnResultExecuted(ResultExecutedContext filterContext) { }

        // This method will be called before the action result is executed.
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // This is a simple and elegant way to check if we're on Admin UI or not.
            if (Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext)) return;
            // This way we can check if the current request renders a PartialView.
            if (filterContext.Result is PartialViewResult) return;

            // We'll just add a script resource (which is already declared in the ResourceManifest.cs)
            // to the result being generated if we're not on the admin UI.
            _resourceManager.Require("script", "OrchardHUN.TrainingDemo.Filtered").AtHead();
            // Notice that we don't added the script in OnResultExecuted(): after the execution of the result
            // static resources are already processed so we wouldn't be able to include new ones.
        }
    }
}

// NEXT STATION: To see a simple example for caching data in an Orchard-y way, please go to
// Services/DateTimeCachingService.cs!