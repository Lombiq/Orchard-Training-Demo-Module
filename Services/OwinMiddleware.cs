using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Environment;
using Orchard.Logging;
using Orchard.Owin;
using Orchard.Services;
using Orchard.Settings;
using Owin;

namespace OrchardHUN.TrainingDemo.Services
{
    // Here we'll write a so called Owin middleware with which you can extend the Orchard request processing pipeline.
    // If you haven't heard of Owin before check out http://owin.org/.
    public class OwinMiddleware : IOwinMiddlewareProvider
    {
        // Mostly you'll only need the WCA, see below why.
        private readonly IWorkContextAccessor _wca;
        // Or use Work<T> injections, also see below for the explanation.
        private readonly Work<ISiteService> _siteServiceWork;

        public ILogger Logger { get; set; }


        public OwinMiddleware(
            IWorkContextAccessor wca,
            Work<ISiteService> siteServiceWork)
        {
            _wca = wca;
            _siteServiceWork = siteServiceWork;

            Logger = NullLogger.Instance;
        }


        public IEnumerable<OwinMiddlewareRegistration> GetOwinMiddlewares()
        {
            return new[]
            {
                // Although we only construct a single OwinMiddlewareRegistration here, you could return multiple ones of
                // course.
                new OwinMiddlewareRegistration
                {
                    // The priority value decides the order of OwinMiddlewareRegistrations. I.e. "0" will run before
                    // "10", but registrations without a priority value will run before the ones that have it set. Note
                    // that this priority notation is the same as the one for shape placement (so you can e.g. use
                    // ":before").
                    Priority = "50",

                    // This is the delegate that sets up middlewares.
                    Configure = app =>
                        // This delegate is the actual middleware. Make sure to add using Owin; otherwise you won't get
                        // why the following line won't compile. The context is the Owin context, something similar to
                        // HttpContext; the next delegate is the next middleware in the pipeline. Note that you could
                        // write multiple configuration steps here, not just this one.
                        app.Use(async (context, next) =>
                        {
                            // Note that although your IOwinMiddlewareProvider behaves like an ordinary Orchard
                            // dependency, the middleware delegate lives on its own and will run detached from the
                            // provider! Because of this you'll need to either access the Work Context as we do here, or
                            // inject your dependencies as Work<TDependency> objects. If you build multiple middlewares
                            // with many dependencies here, doing the following is a better choice.
                            var workContext = _wca.GetContext();

                            // But this would be an alternative:
                            var siteSettings = _siteServiceWork.Value.GetSiteSettings();

                            var clock = workContext.Resolve<IClock>();

                            var requestStart = clock.UtcNow;


                            // We let the next middleware run, but this is not mandatory: if this middleware would return
                            // a cached page for example then we could just leave this out.
                            await next.Invoke();
                            // Think twice when wrapping this call into a try-catch: here you'd catch all exceptions that
                            // would normally result in a 404 or an 503, so it's maybe better to always let them bubble
                            // up. But keep in mind that any uncaught exception here in your code will result in a YSOD.


                            var requestDuration = clock.UtcNow - requestStart;

                            // No need to use the ugly HttpContext, because we have OwinContext!
                            var url = context.Request.Uri;

                            // OK, but what if we _really_ need something from HttpContext?
                            // This is Orchard, so should be true...
                            if (context.Environment.ContainsKey("System.Web.HttpContextBase"))
                            {
                                var httpContext = context.Environment["System.Web.HttpContextBase"] as HttpContextBase;
                                if (httpContext != null)
                                {
                                    // Voila, we have the ugly HttpContext again! Like RouteData:
                                    var routeDataValues = httpContext.Request.RequestContext.RouteData.Values;
                                    // ...you know what to do.
                                }
                            }

                            Logger.Information(
                                "The request to " + url + " on the site " + siteSettings.SiteName + " had taken " + requestDuration + "time.");

                            // You see, we've done something useful!

                            // NEXT STATION: let's add some tokens of our own! Head over to UtcNowTokens.
                        })
                }
            };
        }
    }
}