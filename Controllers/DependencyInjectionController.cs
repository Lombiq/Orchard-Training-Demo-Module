/*
 * Another pretty simple controller, but here we get to know how dependency injection works within Orchard. If you're
 * not yet familiar with DI make sure to study the concept first: http://en.wikipedia.org/wiki/Dependency_injection.
 * 
 * Orchard gives us built-in constructor injection for dependencies. This means that if you request a dependency by its 
 * interface in your class's constructor, Orchard will automatically inject an active implementation for you.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Orchard.DisplayManagement.Notify;

namespace OrchardHUN.TrainingDemo.Controllers
{
    public class DependencyInjectionController : Controller
    {
        /* 
         * INotifier is one of Orchard's most basic services. It is used to display notifications on the UI.
         * As this field will be set only in the ctor it can be read-only. Using an underscore for private fields is 
         * conventional.
         */
        private readonly INotifier _notifier;

        
        /*
         * Use IHttpContextAccessor if you want to only access the HttpContext.
         * 
         * We could use the standard ASP.NET Core statics to access the HttpContext; using this wrapper dependency instead 
         * makes unit testing possible.
         * 
         * "hca" is not a wide-spread convention, but let's start it, OK?
         */
        private readonly IHttpContextAccessor _hca;


        /*
         * We run strings through the IStringLocalizer delegate to translate them (i.e. fetch a translation if present). 
         * Conventional name for it is T.
         * We use a public property here. This is generally a good practice for IStringLocalizer and also for Logger. This way 
         * when writing unit tests the instances can be overwritten from the outside and thus mocked if necessary.
         */
        public IStringLocalizer T { get; }


        /* 
         * Use the IHtmlLocalizer implementation for resources that contain HTML.
         * IHtmlLocalizer HTML encodes arguments that are formatted in the resource string, but not the resource string.
         * Conventional name for it is H.
         */
        public IHtmlLocalizer H { get; }


        // Not surprisingly, a service used for logging...
        public ILogger Logger { get; }


        public DependencyInjectionController(INotifier notifier, 
            IHttpContextAccessor hca,
            ILogger<DependencyInjectionController> logger,
            IStringLocalizer<DependencyInjectionController> t,
            IHtmlLocalizer<DependencyInjectionController> h)
        {
            _notifier = notifier;
            _hca = hca;

            Logger = logger;
            T = t;
            H = h;
        }


        public ActionResult NotifyMe()
        {
            var httpContext = _hca.HttpContext;

            var currentUser = httpContext.User;
            var currentUserName = currentUser?.Identity.Name;
            var currentHost = httpContext.Request.Host;

            // The notifier requires a localized HTML string.
            // Also, this is the way to have localized HTML strings with dynamically injected content.
            _notifier.Information(H[$"Please continue testing. -- Subject name: <em>{currentUserName}</em>; Subject location: <em>{currentHost}</em>"]);

            // This will get into the logs, really. Check the latest log in App_Data/Logs!
            // TODO: Orchard Core does not support logging yet!
            // https://github.com/OrchardCMS/Orchard2/issues/300
            Logger.LogInformation("Test subject notified.");

            // We redirect to the first controller here but the notification will still be displayed. This is because
            // notifications are meant to provide a way to interact with the user even after a redirect. Of course after
            // they're shown once notifications will be dismissed.
            return RedirectToAction(nameof(DependencyInjectionController.NotifyMe), "DependencyInjection");
            
            // NEXT STATION: Models/Person
        }
    }
}
