using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ResourceManagement;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Filters
{
    // Don't forget to add this filter to the filter collection in the Startup.cs file.
    public class ResourceInjectionFilter : IAsyncResultFilter
    {
        // To register resources you can use the IResourceManager service.
        private readonly IResourceManager _resourceManager;


        public ResourceInjectionFilter(IResourceManager resourceManager) => _resourceManager = resourceManager;


        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Let's decide when the filter should be executed. It wouldn't make sense to inject resources if this is a
            // partial view. Also here's an example of how to check if the request contains a "fadeIn" query string
            // parameter.
            if ((context.Result is PartialViewResult) ||
                !context.HttpContext.Request.Query.ContainsKey("fadeIn"))
            {
                await next();

                return;
            }

            // You can register "stylesheet" or "script" resources. You can also set where they'll be rendered with the
            // .AtHead() or .AtFoot() methods chained on the RegisterResource() method which obviously makes sense only
            // if the resource is a script.
            _resourceManager.RegisterResource("stylesheet", "Lombiq.TrainingDemo.Filtered");

            await next();
        }
    }
}

// END OF TRAINING SECTION: Utilizing action and result filters

// NEXT STATION: Controllers/CacheController.cs
