/*
 * Another pretty simple controller, but here we get to know how dependency injection works within Orchard. If you're
 * not yet familiar with DI make sure to study the concept first: http://en.wikipedia.org/wiki/Dependency_injection.
 * 
 * Orchard gives us built-in constructor injection for dependencies. This means that if you request a dependency by its 
 * interface in your class's constructor, Orchard will automatically inject an active implementation for you.
 * 
 * In the form of ILogger and Localizer we also get to know two exceptions.
 */

using Orchard;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.UI.Notify;
using System.Web.Mvc;

namespace OrchardHUN.TrainingDemo.Controllers
{
    public class DependencyInjectionController : Controller
    {
        /* 
         * INotifier is one of Orchard's most basic services. It is used to display notifications on the UI.
         * 
         * Take a look at the interface (just press F12 when the cursor is on the type). What's important for us that it 
         * derives from IDependency. IDependency is a marker interface used for the auto-discovery of dependencies. This 
         * page: http://docs.orchardproject.net/Documentation/How-Orchard-works (look for the "Dependency Injection" 
         * title) describes such marker interfaces in detail.
         * 
         * As this field will be set only in the ctor it can be read-only. Using an underscore for private fields is 
         * conventional.
         */
        private readonly INotifier _notifier;

        /*
         * The WorkContext is a big object containing all kinds of contextual information including the HttpContext or 
         * data of the current user (if any). Since it's best to access the WorkContext as late as possible we use 
         * IWorkContextAccessor to fetch it just when we need it.
         * 
         * "wca" is a conventional abbreviation.
         */
        private readonly IWorkContextAccessor _wca;

        /*
         * IHttpContextAccessor follows the similar pattern as IWorkContextAccessor. Since the HttpContext is included 
         * in the WorkContext having an IHttpContextAccessor too is superfluous; we have it here just for the sake of 
         * demonstration.
         * 
         * Use IHttpContextAccessor if you want to only access the HttpContext but nothing else from WorkContext so when 
         * writing unit tests it's easier to mock it.
         * 
         * We could use the standard ASP.NET statics to access the HttpContext; using this wrapper dependency instead 
         * makes unit testing possible.
         * 
         * "hca" is not a wide-spread convention, but let's start it, OK?
         */
        private readonly IHttpContextAccessor _hca;

        /*
         * We run strings through the Localizer delegate to translate them (i.e. fetch a translation if present). 
         * Conventional name for it is T.
         * We use a public property here. This is generally a good practice for Localizer and also for Logger. This way 
         * when writing unit tests the instances can be overwritten from the outside and thus mocked if necessary. (We 
         * don't need this for ctor-injected dependencies as they can be mocked in a very straightforward way, through 
         * the DI container.)
         */
        public Localizer T { get; set; }

        // Not surprisingly, a service used for logging...
        public ILogger Logger { get; set; }


        public DependencyInjectionController(INotifier notifier, IWorkContextAccessor wca, IHttpContextAccessor hca)
        {
            _notifier = notifier;
            _wca = wca;
            _hca = hca;

            // We can get a Localizer "instance" from the NullLocalizer. So the T method will create a new
            // LocalizedString object for us every time we call it (this is in the NullLocalizer's implementation). It's
            // a shortcut.
            T = NullLocalizer.Instance;

            // The Logger is a special dependency: we can't inject ILogger in the ctor! Thus we fill the property here
            // with some dummy value just not to have a null. But really the property will be filled through property
            // injection with a real logger.
            Logger = NullLogger.Instance;
        }


        public ActionResult NotifyMe()
        {
            // Note that CurrentUser is an IUser instance which is an IContent derivation. Thus users are content items
            // too!
            var currentUser = _wca.GetContext().CurrentUser;
            var currentUserName = currentUser == null ? "&lt;subject name here&gt;" : currentUser.UserName;

            // The below is the same as _wca.GetContext().HttpContext.Request.Url.ToString(). Note that if you use the
            // WorkContext multiple times in the same method of course you can save it to a variable instead of calling
            // GetContext() each time.
            var currentUrl = _hca.Current().Request.Url.ToString();

            // The notifier requires a localized string. This means if we'd have a corresponding entry for this string in
            // a .po file then it would appear in e.g. Hungarian if we would have the locale set to hu-HU.
            // See: http://docs.orchardproject.net/Documentation/Creating-global-ready-applications Also, this is the way
            // to have localized strings with dynamically injected content.
            _notifier.Information(T("Please continue testing. -- Subject name: {0}; Subject location: {1}", currentUserName, currentUrl));

            // This will get into the logs, really. Check the latest log in App_Data/Logs!
            Logger.Error("Test subject notified.");

            // We redirect to the first controller here but the notification will still be displayed. This is because
            // notifications are meant to provide a way to interact with the user even after a redirect. Of course after
            // they're shown once notifications will be dismissed.
            return RedirectToAction(nameof(YourFirstOrchardController.Index), "YourFirstOrchard");

            // NEXT STATION: Models/PersonRecord
        }
    }
}