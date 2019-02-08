using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Layout;

namespace Lombiq.TrainingDemo.Filters
{
    public class ShapeInjectionFilter : IAsyncResultFilter
    {
        private readonly ILayoutAccessor _layoutAccessor;
        private readonly IShapeFactory _shapeFactory;


        public ShapeInjectionFilter(ILayoutAccessor layoutAccessor, IShapeFactory shapeFactory)
        {
            _layoutAccessor = layoutAccessor;
            _shapeFactory = shapeFactory;
        }


        public async Task OnResultExecutionAsync(ResultExecutingContext filterContext, ResultExecutionDelegate next)
        {
            if (!(filterContext.Result is ViewResult || filterContext.Result is PageResult))
            {
                await next();

                return;
            }

            dynamic layout = await _layoutAccessor.GetLayoutAsync();

            var contentZone = layout.Zones["Content"];
            contentZone.Add(await _shapeFactory.New.InjectedShape());

            await next();
            return;
        }
    }
}