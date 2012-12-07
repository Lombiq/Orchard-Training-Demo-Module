/*
 * Another pretty simple controller, but here we get to know how dependency injection works within Orchard. If you're not yet
 * familiar with DI make sure to study the concept first.
 * 
 * Orchard gives us built-in constructor injection for dependencies. This means that you request a dependency by its interface
 * in your class's constructor. Orchard will automatically inject an active implementation for you.
 * 
 * In the form of ILogger and Localizer we also get to know to exceptions.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace OrchardHUN.TrainingDemo.Controllers
{
    public class DependencyInjectionController : Controller
    {
        // As this field will be set only in the ctor it can be read-only. Using an underscore for private fields is conventional.
        private readonly INotifier _notifier;

        /*
         * We run strings through the Localizer delegate to translate them (i.e. fetch a translation if present). Conventional name
         * for it is T.
         * We use a public property here. This is generally a good practice for Localizer and also for Logger. This way when writing
         * unit tests the instances can be overwritten from the outside and thus mocked if necessary. (We don't need this for ctor-
         * injected dependencies as they can be mocked in a very straightforward way, through the DI container.)
         */
        public Localizer T { get; set; }

        public ILogger Logger { get; set; }

        /* INotifier is one of Orchard's most basic services. It is used to display notifications on the UI.
         * 
         * Take a look at the interface (just press F12 when the cursor is on the type). What's important for us that it derives
         * from IDependency. IDependency is a marker interface used for the auto-discovery of dependencies. This page:
         * http://docs.orchardproject.net/Documentation/How-Orchard-works (look for the "Dependency Injection" title) describes
         * such marker interfaces in detail.
         */
        public DependencyInjectionController(INotifier notifier)
        {
            _notifier = notifier;

            // We can get a Localizer "instance" from the NullLocalizer. So the T method will create a new LocalizedString object
            // for us every time we call it (this is in the NullLocalizer's implementation). It's a shortcut.
            T = NullLocalizer.Instance;

            // The Logger is a special dependency: we can't inject ILogger in the ctor! Thus we fill the property here with some
            // dummy value just not to have a null. But really the property will be filled through property injection with a real
            // logger.
            Logger = NullLogger.Instance;
        }

        public ActionResult NotifyMe()
        {
            // The notifier requires a localized string. This means if we'd have a corresponding entry for this string in a .po file
            // then it would appear in e.g. Hungarian if we would have the locale set to hu-HU. See: http://docs.orchardproject.net/Documentation/Creating-global-ready-applications
            _notifier.Information(T("Please continue testing."));

            // This will get into the logs, really. Check the latest log in App_Data/Logs!
            Logger.Error("Test subject notified.");

            // We redirect to the first controller here but the notification will still be displayed. This is because notifications
            // are meant to provide a way to interact with the user even after a redirect. Of course after they're shown once 
            // notifications will be dismissed.
            return RedirectToAction("Index", "YourFirstOrchard");

            // NEXT STATION:
        }
    }
}