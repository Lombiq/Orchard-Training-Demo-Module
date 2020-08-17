/*
 * A middleware in ASP.NET Core is, to quote the official documentation, "software that's assembled into an app
 * pipeline to handle requests and responses". Well said! Middlewares work pretty much the same in Orchard as they do
 * in any ASP.NET Core app but nevertheless, let's see a simple example, though a bit spiced up with Orchard services.
 * For more info on middlewares in general check out
 * https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/. Check out this tutorial too:
 * https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Middlewares
{
    // This middleware will serve as a simple logger for requests and log each request with the site's name
    // Note that while this middleware is in its own class we could just write it as a delegate in the Startup class
    // too. This way Startup won't get cluttered.
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;


        // You need to inject a RequestDelegate instance here.
        public RequestLoggingMiddleware(RequestDelegate next) => _next = next;


        // This method is the actual middleware. Note that apart from the first parameter obligatorily being
        // HttpContext further parameters can be injected Orchard services.
        public async Task InvokeAsync(
            HttpContext context,
            ISiteService siteService,
            ILogger<RequestLoggingMiddleware> logger)
        {
            // We let the next middleware run, but this is not mandatory: if this middleware would return a cached page
            // for example then we would write the cached response to the HttpContext and leave this out.
            await _next(context);
            // Think twice when wrapping this call into a try-catch: here you'd catch all exceptions from the next
            // middleware that would normally result in a 404 or an 503, so it's maybe better to always let them bubble
            // up. But keep in mind that any uncaught exception here in your code will result in an error page.

            // We use LogError() not because we're logging an error just so the message shows up in the log even with
            // log levels ignoring e.g. info or debug entries. Use the logging methods appropriately otherwise!
            logger.LogError(
                "The url {url} was just hit on the site {name}.",
                UriHelper.GetDisplayUrl(context.Request),
                (await siteService.GetSiteSettingsAsync()).SiteName);
        }
    }
}

// END OF TRAINING SECTION: Middlewares

// NEXT STATION: Controllers/CrossTenantServicesController.cs
