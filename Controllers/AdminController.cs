/*
 * You can create pages to the Orchard Core dashboard really simply by naming your controller to AdminController. In
 * this section you will learn how to add pages to the dashboard and how to create menu items as well.
 */

using Microsoft.AspNetCore.Mvc;

namespace Lombiq.TrainingDemo.Controllers
{
    public class AdminController : Controller
    {
        // Let's see how it will be displayed, just type the default URL into the browser with an administrator account
        // (or at least a user who is in a role that has AccessAdmin permission). If you are anonymous then a login
        // page will automatically appear. The permission check (i.e. has AccessAdmin permission) will be automatic as
        // well.
        public ActionResult Index() => View();
    }
}