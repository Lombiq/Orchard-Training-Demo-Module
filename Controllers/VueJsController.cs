/*
 * Vue.js is a progressive framework for building user interfaces, mostly single-page applications. What if you want to
 * build an SPA only for one feature or build multiple SPAs in one Orchard Core module? Here's an example of how to
 * organize your Vue apps and components to keep them reusable and also have all the features we like in Orchard Core
 * such as shape overrides and localization.
 *
 * If you are not familiar with Vue.js you can skip this tutorial or give it a try and see how cool of a framework it
 * is. See: https://vuejs.org/v2/guide/
 */

using Microsoft.AspNetCore.Mvc;

namespace Lombiq.TrainingDemo.Controllers
{
    // This controller is for returning a Vue.js application in an MVC view, however, you could use any other ways of
    // doing it such as injecting as a shape or using widgets.
    public class VueJsController : Controller
    {
        [HttpGet]
        // Open this from under /Lombiq.TrainingDemo/VueJs/DemoApp
        public ActionResult DemoApp() => View();

        // NEXT STATION: Views/VueJs/DemoApp.cshtml
    }
}
