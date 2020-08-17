/*
 * Creating RESTful web APIs as part of an Orchard module is very similar to creating an MVC controller and pretty much
 * the same as creating one in plain ASP.NET Core (see the related tutorial here:
 * https://docs.microsoft.com/en-us/aspnet/core/web-api/). Let's just see a basic example!
 *
 * Note that out of the box Orchard provides a web API too: There are many RESTful API endpoints available e.g. for
 * content and tenant management, as well as a GraphQL API infrastructure that many modules contribute to (see:
 * https://docs.orchardcore.net/en/dev/docs/reference/modules/Apis.GraphQL/) and you can extend too.
 */

using Lombiq.TrainingDemo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Mvc.Utilities;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Controllers
{
    // This controller will retrieve a Person Page content item for us as a simple example that nevertheless showcases
    // the most important things you need to know. If you want to see a more complex example of such a controller do
    // check out the ApiController in the OrchardCore.Content module in the official source.

    // Using attribute routing to have a proper route for all actions in this controller.
    [Route("api/Lombiq.TrainingDemo")]
    // The ApiController attribute is not strictly mandatory but pretty useful, see:
    // https://docs.microsoft.com/en-us/aspnet/core/web-api/#apicontroller-attribute
    [ApiController]
    // We'll handle authorization within the actions (i.e. API endpoints) so nothing else needed here.
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class ApiController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IContentManager _contentManager;


        public ApiController(IAuthorizationService authorizationService, IContentManager contentManager)
        {
            _authorizationService = authorizationService;
            _contentManager = contentManager;
        }


        // Look up the ID of a Person Page that you've created previously (when you open one from the admin content
        // item list the URL will contain it as /Admin/Contents/ContentItems/<content item ID>) and use it to access
        // this action under api/Lombiq.TrainingDemo?contentItemId=<content item ID>.
        public async Task<IActionResult> Get(string contentItemId)
        {
            // Authorization is important in API endpoints as well of course. We're re-using the previously created
            // permission here.
            // To authenticate with the API you can use any ASP.NET Core authentication scheme but Orchard offers
            // various OpenID-based options. If you just want to quickly check out the API then grant the permission
            // for the Anonymous role on the admin.
            if (!await _authorizationService.AuthorizeAsync(User, PersonPermissions.ManagePersons))
            {
                return this.ChallengeOrForbid();
            }

            // Just the usual stuff again.
            var contentItem = await _contentManager.GetAsync(contentItemId);

            // Only allow the retrieval of Person Page items.
            if (contentItem?.ContentType != ContentTypes.PersonPage) contentItem = null;

            // The action will return the JSON representation of the content item automatically. You can then consume
            // that from a web SPA or a mobile app, for example.
            return contentItem == null ? (IActionResult)NotFound() : Ok(contentItem);
        }
    }
}

// END OF TRAINING SECTION: Web API

// NEXT STATION: Middlewares/RequestLoggingMiddleware.cs
