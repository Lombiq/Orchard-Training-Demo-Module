using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.DisplayManagement;
using OrchardCore.ResourceManagement;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Filters;

public class ResourceFromShapeInjectingFilter : IAsyncResultFilter
{
    // We've seen IResourceManager and IShapeFactory before.
    private readonly IResourceManager _resourceManager;

    private readonly IShapeFactory _shapeFactory;

    // IDisplayHelper is new, however. We'll use it to execute a shape into HTML and inject that as a head script!
    private readonly IDisplayHelper _displayHelper;

    public ResourceFromShapeInjectingFilter(
        IResourceManager resourceManager,
        IShapeFactory shapeFactory,
        IDisplayHelper displayHelper)
    {
        _resourceManager = resourceManager;
        _shapeFactory = shapeFactory;
        _displayHelper = displayHelper;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Similar to ResourceInjectionFilter only run this if we're in a full view and the "alert" query string
        // parameter is present. Just open any page and add "?alert" or "&alert" to the URL and see what happens!
        if (context.Result is PartialViewResult || !context.HttpContext.Request.Query.ContainsKey("alert"))
        {
            await next();

            return;
        }

        // We use the shape factory again to instantiate the AlertScriptShape. Check out Views/AlertScriptShape.cshtml
        // because there's something curious there too, then come back! Next, we also use the display helper to execute
        // the shape and generate its HTML content.
        var shapeContent = await _displayHelper.ShapeExecuteAsync(await _shapeFactory.New.AlertScriptShape());

        // Did you know that you can inject inline scripts with resource manager too? You can use this technique not
        // just like this to generate inline scripts but also e.g. to generate HTML output from shapes in background
        // tasks (like when sending e-mails and you want to have proper templates for them).
        _resourceManager.RegisterFootScript(shapeContent);

        await next();
    }
}

// END OF TRAINING SECTION: Utilizing action and result filters

// NEXT STATION: Services/ShapeHidingShapeTableProvider.cs
