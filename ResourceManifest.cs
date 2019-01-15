/*
 * If you want to include scripts and stylesheets in the pages you can register resources here and then reference them
 * in any shape and let the resource manager know where these needs to be included (head or foot).
 */

using OrchardCore.ResourceManagement;

namespace Lombiq.TrainingDemo
{
    // ResourceManifest classes implement IResourceManifestProvider and possess the BuildManifests method. Don't forget
    // to register this class with the service provider (see: Startup.cs). If you want to learn more about resources
    // see: https://orchardcore.readthedocs.io/en/latest/OrchardCore.Modules/OrchardCore.Resources/README/
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
                // We're registering a script with DefineStyle and defining it's name. It's a global name, so choose
                // wisely. There is no strict naming convention, but we can give you an advice how to choose a unique
                // name: it should contain the module's full namespace followed by a meaningful name. Although if it's
                // a third-party library you can give a more general name like you see below. This script will be the
                // javascript plugin for the color picker.
                .DefineScript("Pickr")
                // This is the actual script file that will be assigned to the resource name. Please note that the
                // naming of the file itself is following similar rules as the resource name, with some modifications
                // applied as it's a file name.
                .SetUrl("/Lombiq.TrainingDemo/Pickr/pickr.min.js");

            manifest
                // With the DefineStyle method you can define a stylesheet. The way of doing this is very similar to
                // defining scripts. Since stylesheets are actually shapes, it's possible to override them, so you
                // should try to avoid name collisions: file names (not the full path, just the name!) should be
                // globally unique.
                .DefineStyle("Pickr")
                .SetUrl("/Lombiq.TrainingDemo/Pickr/pickr.min.css");

            manifest
                // Finally let's see an example for defining a resource for our custom code. You can see the naming is
                // more specific and contains our namespace.
                .DefineStyle("Lombiq.TrainingDemo.ColorPicker")
                .SetUrl("/Lombiq.TrainingDemo/Styles/trainingdemo-colorpicker.min.css",
                    "/Lombiq.TrainingDemo/Styles/trainingdemo-colorpicker.css")
                // You can give a list of resource names to SetDependencies to force the loading of other resources
                // when a given resource is used. Here Pickr is a dependency.
                .SetDependencies("Pickr");

            // If you go back to the Views/ColorField-ColorPicker.Edit.cshtml you will understand why all these three
            // resources will be loaded using those style and script tag helpers.
        }
    }
}

// NEXT STATION: Gulpfile.js