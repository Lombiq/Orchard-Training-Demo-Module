/*
 * You can use ASP.NET Core MVC filters to do some Orchard-specific operations such as injecting shapes or resources
 * into your pages. Here you will see two examples for them.
 *
 * This one will be a filter where you can inject an ad-hoc shape into a zone. A common scenario would be to inject a
 * banner into every page to notify the user about something important.
 *
 * The second filter (ResourceInjectionFilter) will be an example of injecting a resource (script or stylesheet) into a
 * page or pages. In the example a stylesheet will be injected if the URL contains a "fadeIn" query string parameter
 * which will make the page fade in on load.
 *
 * The third filter, ResourceFromShapeInjectingFilter will combine the two and totally blow your mind!
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Layout;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Filters;

// Don't forget to add this filter to the filter collection in the Startup.cs file.
// To access the layout which contains the zones you need to use the ILayoutAccessor service.
// To generate ad-hoc shapes the IShapeFactory can be used. This is the same which is behind the New property in
// templates that you have previously seen in AdHocShape.cshtml.
public class ShapeInjectionFilter(ILayoutAccessor layoutAccessor, IShapeFactory shapeFactory) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // You can decide when the filter should be executed here. If this is not a ViewResult or PageResult the shape
        // injection wouldn't make any sense since there wouldn't be any zones.
        if (context.Result is not (ViewResult or PageResult))
        {
            await next();

            return;
        }

        // We first retrieve the layout.
        var layout = await layoutAccessor.GetLayoutAsync();

        // The Layout object will contain a Zones dictionary that you can use to access a zone. The Content zone is
        // usually available in all themes and is the main zone in the middle of each page.
        var contentZone = layout.Zones["Content"];
        // Here you can add an ad-hoc generated shape to the Content zone. This works in the same way as we've seen
        // previously when we talked about display management. You can find the template that'll render this shape under
        // Views/InjectedShape.cshtml.
        await contentZone.AddAsync(await shapeFactory.CreateAsync("InjectedShape"));

        await next();
    }
}

// NEXT STATION: Filters/ResourceInjectionFilter.cs
