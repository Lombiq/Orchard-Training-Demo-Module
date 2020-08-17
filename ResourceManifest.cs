/*
 * If you want to include scripts and stylesheets in the pages you can register resources here and then reference them
 * in any shape and let the resource manager know where these need to be included (head or foot).
 */

using OrchardCore.ResourceManagement;

namespace Lombiq.TrainingDemo
{
    // ResourceManifest classes implement IResourceManifestProvider and possess the BuildManifests method. Don't forget
    // to register this class with the service provider (see: Startup.cs). If you want to learn more about resources
    // see: https://docs.orchardcore.net/en/dev/docs/reference/modules/Resources/
    public class ResourceManifest : IResourceManifestProvider
    {
        // This is the only method to implement. Using it we're going to register some static resources
        // to be able to use them in our templates.
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            // We add a new instance of ResourceManifest to the ResourceManifestBuilder,
            // instantiated by the Add method.
            var manifest = builder.Add();

            manifest
                // We're registering a script with DefineScript and defining its name. It's a global name, so choose
                // wisely. There is no strict naming convention, but we can give you an advice how to choose a unique
                // name: it should contain the module's full namespace followed by a meaningful name. Although if it's
                // a third-party library you can give a more general name like you see below. This script will be the
                // javascript plugin for the color picker.
                .DefineScript("Pickr")
                // This is the actual script file that will be assigned to the resource name. Please note that the
                // naming of the file itself is following similar rules as the resource name, with some modifications
                // applied as it's a file name.
                // If you add two arguments then the first one can be the production variant of the resource (the one
                // commonly minified) and the other one the debug variant (non-minified, for local development). In
                // this case be sure to use one variant for both the local and CDN URL, or two for both so there is no
                // confusion.
                .SetUrl("~/Lombiq.TrainingDemo/Pickr/pickr.min.js")
                // You can also use a CDN (or just a CDN) if you want to optimize static resource loading. If a
                // resource has both a local and CDN version then you can decide when including it which one to use or
                // you can set this globally under General Settings from the admin.
                .SetCdn("https://cdn.jsdelivr.net/npm/pickr-widget@0.3.6/dist/pickr.min.js")
                // In case of a CDN make sure to also utilize Subresource Integrity so the script can't be changed on
                // the CDN and potentially harm your site! For more info on SRI see:
                // https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity.
                // You can create such hashes with e.g. this tool: https://www.srihash.org/.
                .SetCdnIntegrity("sha384-9QkVz27WSgTpBfZqt9HJh4LIH88MjcPx4wGafm3SZOHXnje8A5mIeWwQ332WZxS/")
                // You can also define a version for a resource. Multiple resources with the same name but different
                // version can exist and when including the resource you can decide which one to use.
                .SetVersion("0.3.6");

            manifest
                // With the DefineStyle method you can define a stylesheet. The way of doing this is very similar to
                // defining scripts.
                .DefineStyle("Pickr")
                .SetUrl("~/Lombiq.TrainingDemo/Pickr/pickr.min.css");

            manifest
                // Finally let's see an example for defining a resource for our custom code. You can see the naming is
                // more specific and contains our namespace.
                .DefineStyle("Lombiq.TrainingDemo.ColorPicker")
                .SetUrl(
                    "~/Lombiq.TrainingDemo/Styles/trainingdemo-colorpicker.min.css",
                    "~/Lombiq.TrainingDemo/Styles/trainingdemo-colorpicker.css")
                // You can give a list of resource names to SetDependencies to force the loading of other resources
                // when a given resource is used. Here Pickr is a dependency.
                .SetDependencies("Pickr");

            // If you go back to the Views/ColorField-ColorPicker.Edit.cshtml you will understand why all these three
            // resources will be loaded using those style and script tag helpers.

            manifest
                .DefineStyle("Lombiq.TrainingDemo.Filtered")
                .SetUrl(
                    "~/Lombiq.TrainingDemo/Styles/trainingdemo-filtered.min.css",
                    "~/Lombiq.TrainingDemo/Styles/trainingdemo-filtered.css");

            // This resource will be required for our demo Vue.js application.
            manifest
                .DefineScript("Lombiq.TrainingDemo.DemoApp")
                .SetUrl("~/Lombiq.TrainingDemo/Apps/demo.min.js", "~/Lombiq.TrainingDemo/Apps/demo.js");
        }
    }
}

// END OF TRAINING SECTION: Resource management

// NEXT STATION: Controllers/AuthorizationController.cs
