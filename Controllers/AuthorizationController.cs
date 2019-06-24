/*
 * There is a role-based access control implemented in Orchard Core that is recommended to use for authorization. There
 * are different Roles each with a set of Permissions (e.g. view a specific content, edit a content or access admin
 * panel). When you want to authorize a user you have to use the IAuthorizationService to check if the current user has
 * a permission you give as a parameter (i.e. is the user in a role that constants the given permission). There are
 * permissions that can be used alone and there are ones that can be checked with an object (e.g. ViewContent
 * permission with the ContentItem).
 *
 * Here you will see examples of authorization and also learn how to create your own permissions.
 */

using System.Threading.Tasks;
using Lombiq.TrainingDemo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Notify;

namespace Lombiq.TrainingDemo.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IContentManager _contentManager;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;

        public AuthorizationController(
            IAuthorizationService authorizationService,
            IContentManager contentManager,
            INotifier notifier,
            IHtmlLocalizer<AuthorizationController> htmlLocalizer)
        {
            _authorizationService = authorizationService;
            _contentManager = contentManager;
            _notifier = notifier;
            H = htmlLocalizer;
        }

        // Here we will create a Person content item and check if the user has permission to edit it. It's very common
        // to check if you can view or edit a specific item - it also happens if you use the built in URLs like
        // /Contents/Item/Display/{id} to view a content item.
        public async Task<ActionResult> CanEditPerson()
        {
            // Creating a content item for testing (won't be persisted).
            var person = await _contentManager.NewAsync("Person");

            // Check if the user has permission to edit the content item. When you check content-related permissions
            // (ViewContent, EditContent, PublishContent etc.) there is a difference between checking these for your
            // content items (i.e. the owner is you) and other's content items. When you are the owner of the content
            // item then the ViewOwnContent, EditOwnContent, PublishOwnContent etc. permissions will be checks. This is
            // automatic so you don't need to use them directly. For this newly created Person item the owner is null
            // so the EditContent permission will be used.
            if (!await _authorizationService.AuthorizeAsync(User, OrchardCore.Contents.Permissions.EditContent, person))
            {
                // Return 401 status code using this helper.
                return Unauthorized();
            }

            // To keep the demonstration short, return only a notification about the successful authorization and
            // return to the home page.
            _notifier.Information(H["You are authorized to edit Person content items."]);

            return Redirect("~/");
        }

        // NEXT STATION: Permissions/PersonPermissions

        public async Task<ActionResult> CanManagePersons()
        {
            // We've defined a ManagePersons earlier which is added to the Administrator user by default. If the
            // currently user is not part of the Administrator role then you can add it on the dashboard. Since this
            // permission can be checked without any object as a context the third parameter is left out.
            if (!await _authorizationService.AuthorizeAsync(User, PersonPermissions.ManagePersons))
            {
                return Unauthorized();
            }

            _notifier.Information(H["You are authorized to manage persons."]);

            return Redirect("~/");
        }
    }
}

// END OF TRAINING: Permissions and authorization

// NEXT STATION: Controllers/AdminController