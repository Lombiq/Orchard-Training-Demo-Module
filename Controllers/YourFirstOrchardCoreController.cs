/*
 * This is a controller you can find in any ASP.NET Core MVC application; Orchard is an MVC application, although
 * much-much more than that.
 *
 * An Orchard module is basically an MVC area. You could create areas that don't interact with Orchard and you'd just
 * use standard ASP.NET Core MVC skills. Of course we want more than that, so let's take a closer look.
 *
 * Here you will see how to use some simple ASP.NET Core and Orchard Core services.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement.Notify;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Controllers;

public class YourFirstOrchardCoreController : Controller
{
    private readonly INotifier _notifier;
    private readonly IStringLocalizer T;
    private readonly IHtmlLocalizer H;

    // You can use the non-generic counterpart of ILogger once injected just be sure to inject the generic one otherwise
    // the log entries won't contain the name of the class.
    private readonly ILogger _logger;

    // Orchard Core uses the built in dependency injection feature coming with ASP.NET Core. You can use the module's
    // Startup class to register your own services with the service provider. To learn more see:
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
    public YourFirstOrchardCoreController(
        INotifier notifier,
        IStringLocalizer<YourFirstOrchardCoreController> stringLocalizer,
        IHtmlLocalizer<YourFirstOrchardCoreController> htmlLocalizer,
        ILogger<YourFirstOrchardCoreController> logger)
    {
        _notifier = notifier;
        _logger = logger;

        T = stringLocalizer;
        H = htmlLocalizer;
    }

    // Here's a simple action that will return some message. Nothing special here just demonstrates that this will work
    // in Orchard Core right after enabling the module. The route for this action will be
    // /Lombiq.TrainingDemo/YourFirstOrchardCore/Index.
    public ActionResult Index()
    {
        // Simple texts can be localized using the IStringLocalizer service as you can see below. To learn more about
        // how localization works in Orchard check out the docs:
        // https://docs.orchardcore.net/en/latest/docs/reference/modules/Localize/
        ViewData["Message"] = T["Hello you!"];

        // Nothing special happens in the view but you can check it in the Views/YourFirstOrchardController/Index.cshtml
        // file.
        return View();
    }

    // Let's see some custom routing here. This attribute will override the default route and use this one.
    [Route("TrainingDemo/NotifyMe")]
    public async Task<IActionResult> NotifyMe()
    {
        // ILogger is an ASP.NET Core service that will write something into the specific log files. In Orchard Core
        // NLog is used for logging and the error level is "Error" by default. You can find the error log in the
        // /App_Data/logs/orchard-log-[date].log file. Logging can be configured in the NLog.config file in the web
        // project (e.g. OrchardCore.Cms.Web). Oh, and one more thing: if you install the Lombiq Orchard Visual Studio
        // Extension it'll provide you a handy Error Log Watcher which lights up if there's a new error! Check it out
        // here:
        // https://marketplace.visualstudio.com/items?itemName=LombiqVisualStudioExtension.LombiqOrchardVisualStudioExtension
        _logger.LogError("You have been notified about some error!");

        // INotifier is an Orchard Core service to send messages to the user. This service can be used almost everywhere
        // in the code base not only in Controllers. This service requires a LocalizedHtmlString object so the
        // IHtmlLocalizer service needs to be used for localization.
        await _notifier.InformationAsync(H["Congratulations! You have been notified! Check the error log too!"]);

        return View();

        // NEXT STATION: Views/YourFirstOrchardCore/NotifyMe.cshtml
    }
}
