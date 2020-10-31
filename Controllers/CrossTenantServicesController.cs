/*
 * Multi-tenancy is in Orchard's very core, utilized everywhere even if you don't notice it. A tenant is basically a
 * subsite with its own independent content and configuration, under its own domain or URL prefix. You can use tenants
 * to e.g. host websites for multiple customers of yours from a single Orchard Core app. The sites won't know anything
 * about each other but they'll run from the same app built from the same codebase, and have access to the same modules
 * and themes. This makes maintaining such sites very efficient, both for hosting and for development. For more details
 * check out the documentation: https://docs.orchardcore.net/en/dev/docs/glossary/#tenant
 *
 * What if you want tenants to be not that isolated though? What if there is certain content or configuration that you
 * actually want to share among tenants or some functionality that you want to centralize on one tenant? You can use
 * the APIs we show below to cross tenant boundaries and use any service from another tenant!
 *
 * Now we're going into pretty advanced territory. But don't worry, we'll explain everything!
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Environment.Shell;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Controllers
{
    // This is a controller just for the sake of easy demonstration, you can do the same thing anywhere. In the Index
    // action, we'll fetch content items from another tenant with the IContentManager service that you already know.
    // This is just an example though, really you can access any other service as well.
    public class CrossTenantServicesController : Controller
    {
        private readonly IShellHost _shellHost;


        // We'll need IShellHost to access services from a currently running shell's dependency injection container
        // (Service Provider).
        public CrossTenantServicesController(IShellHost shellHost) => _shellHost = shellHost;


        // A simple route for convenience. You can access this from under /CrossTenantServices?contentItemId=ID. Here
        // ID needs to be a content item ID that you can get e.g. from the URL when you open an item to edit from the
        // admin (it looks something like "4da2sme18cc2k2rpdgwx3d4cwj" which is NOT made by a cat walking across the
        // keyboard!).
        [Route("CrossTenantServices")]
        public async Task<string> Index(string contentItemId)
        {
            // Even if you don't create tenants, there will still be a single tenant in an Orchard app, the Default
            // tenant. For all other tenants you create you can provide the technical name.

            // In this example, we'll access content items from the Default tenant but this works for any tenant of
            // course. Create a tenant in your app (enable the Tenants feature and then create it from under
            // Configuration / Tenants), enable the Training Demo on it too and check out how this works there!

            // First you have to retrieve the tenant's shell scope that contains the shell's Service Provider. Note
            // that there is also an IShellSettingsManager service that you can use to access the just shell settings
            // for all tenants (shell settings are a tenant's basic settings, like its technical name and its URL).
            var shellScope = await _shellHost.GetScopeAsync("Default");

            // We'll just return the title of the content item from this action but you can do anything else with the
            // item too, like displaying it.
            string title = null;

            // With UsingAsync() we'll create a block where everything is executed within the context of that other
            // tenant. It's a bit similar to being inside a controller action, but remember that all of this is running
            // on the Default tenant, even if you're looking at it from another tenant you've created.
            await shellScope.UsingAsync(async scope =>
            {
                // You can resolve any service from the shell's Service Provider. This serves instead of injecting
                // services in the constructor.
                var contentManager = scope.ServiceProvider.GetRequiredService<IContentManager>();

                // We can use IContentManager as usual, it'll just work.
                // Note that for the sake of simplicity there is no error handling for missing content items here, or
                // any authorization. It's up to you to add those :).
                var contentItem = await contentManager.GetAsync(contentItemId);

                // DisplayText is what you've already learned about in PersonPartHandler.
                title = contentItem.DisplayText;
            });

            return title;
        }
    }
}

// END OF TRAINING SECTION: Accessing services from other tenants

// NEXT STATION: Services/TestedService.cs
