/*
 * You can create pages to the Orchard Core dashboard really simply by naming your controller to AdminController or
 * putting an [Admin] attribute to the controller. In this section you will learn how to add pages to the dashboard and
 * how to create menu items as well.
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombiq.TrainingDemo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using YesSql;

namespace Lombiq.TrainingDemo.Controllers
{
    // If you have multiple admin controllers then name it whatever you want but put an [Admin] attribute on the
    // controller.
    public class AdminController : Controller, IUpdateModel
    {
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly ISession _session;
        private readonly IAuthorizationService _authorizationService;


        public AdminController(
            IContentItemDisplayManager contentItemDisplayManager,
            ISession session,
            IAuthorizationService authorizationService)
        {
            _contentItemDisplayManager = contentItemDisplayManager;
            _session = session;
            _authorizationService = authorizationService;
        }


        // Let's see how it will be displayed, just type the default URL into the browser with an administrator account
        // (or at least a user who is in a role that has AccessAdmin permission). If you are anonymous then a login
        // page will automatically appear. The permission check (i.e. has AccessAdmin permission) will be automatic as
        // well.
        public ActionResult Index() => View();

        // NEXT STATION: AdminMenu.cs

        public async Task<ActionResult> PersonListNewest()
        {
            // If the user needs to have a specific permission to access a page on the admin panel (besides the
            // AccessAdmin permission) you need to check it here.
            if (!await _authorizationService.AuthorizeAsync(User, PersonPermissions.AccessPersonListDashboard))
            {
                return Unauthorized();
            }

            // Nothing special here just display the last 10 Person content items.
            var persons = await _session
                .Query<ContentItem, ContentItemIndex>()
                .OrderByDescending(index => index.CreatedUtc)
                .Take(10)
                .ListAsync();

            return View("PersonList", await GetShapes(persons));
        }

        public async Task<ActionResult> PersonListOldest()
        {
            if (!await _authorizationService.AuthorizeAsync(User, PersonPermissions.AccessPersonListDashboard))
            {
                return Unauthorized();
            }

            // Display the first 10 Person content items.
            var persons = await _session
                .Query<ContentItem, ContentItemIndex>()
                .OrderBy(index => index.CreatedUtc)
                .Take(10)
                .ListAsync();

            return View("PersonList", await GetShapes(persons));
        }


        private async Task<IEnumerable<IShape>> GetShapes(IEnumerable<ContentItem> persons) =>
            // Notice the "SummaryAdmin" display type which is a built in display type specifically for listing items
            // on the dashboard.
            await Task.WhenAll(persons.Select(async person =>
                await _contentItemDisplayManager.BuildDisplayAsync(person, this, "SummaryAdmin")));
    }
}