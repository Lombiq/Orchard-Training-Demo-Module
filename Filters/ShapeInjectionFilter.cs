/*
 * You can use ASP.NET Core MVC filters to do some Orchard-specific operations such as injecting shapes or resources to
 * your pages. Here you will see two examples for the.
 *
 * This one will be a filter where you can inject an ad-hoc shape to a zone. A common scenario would be to inject a
 * banner to every page to notify the user about something important.
 *
 * The second filter (ResourceInjectionFilter) will be an example of injecting a resource (script or stylesheet) to a
 * page or pages. In the example a stylesheet will be injected if the URL contains a "fadeIn" query string parameter
 * which will make the page to fade in on load.
 */

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Layout;

namespace Lombiq.TrainingDemo.Filters
{
    // Don't forget to add this filter to the filter collection in the Startup.cs.
    public class ShapeInjectionFilter : IAsyncResultFilter
    {
        // To access layout which contains the zones you need to use ILayoutAccessor service.
        private readonly ILayoutAccessor _layoutAccessor;
        // To generate ad-hoc shapes the IShapeFactory can be used.
        private readonly IShapeFactory _shapeFactory;


        public ShapeInjectionFilter(ILayoutAccessor layoutAccessor, IShapeFactory shapeFactory)
        {
            _layoutAccessor = layoutAccessor;
            _shapeFactory = shapeFactory;
        }


        public async Task OnResultExecutionAsync(ResultExecutingContext filterContext, ResultExecutionDelegate next)
        {
            // You can make exceptions of when the filter should be executed here. If this is a ViewResult or
            // PageResult the shape injection wouldn't make any sense since there wouldn't be any zones.
            if (!(filterContext.Result is ViewResult || filterContext.Result is PageResult))
            {
                await next();

                return;
            }

            // The layout can be handled easily if this is dynamic.
            dynamic layout = await _layoutAccessor.GetLayoutAsync();

            // The dynamic Layout object will contain a Zones dictionary that you can use to access a zone.
            var contentZone = layout.Zones["Content"];
            // Here you can add an ad-hoc generated shape to the content zone.
            contentZone.Add(await _shapeFactory.New.InjectedShape());

            await next();
            return;
        }
    }
}

// NEXT STATION: Filters/ResourceInjectionFilter.cs