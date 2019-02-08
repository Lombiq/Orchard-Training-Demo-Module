using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ResourceManagement;

namespace Lombiq.TrainingDemo.Filters
{
    // Don't forget to add this filter to the filter collection in the Startup.cs.
    public class ResourceInjectionFilter : IAsyncResultFilter
    {
        // To register resources you can use the IResourceManager service.
        private readonly IResourceManager _resourceManager;


        public ResourceInjectionFilter(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }


        public async Task OnResultExecutionAsync(ResultExecutingContext filterContext, ResultExecutionDelegate next)
        {
            // Let's decide when the filter should be executed. It wouldn't make sense to inject resources if this is a
            // partial view. Also here's an example about how to check if the request contains a "fadeIn" query string
            // parameter.
            if ((filterContext.Result is PartialViewResult) ||
                !filterContext.HttpContext.Request.Query.ContainsKey("fadeIn"))
            {
                await next();

                return;
            }

            // You can register "stylesheet" or "script" resources. You also can set where to be rendered with the
            // .AtHead() or .AtFoot() methods chained the RegisterResource() method which obviously makes sense if the
            // resource is a script.
            _resourceManager.RegisterResource("stylesheet", "Lombiq.TrainingDemo.Filtered");

            await next();
            return;
        }
    }
}