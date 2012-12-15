using Orchard.UI.Resources;

namespace OrchardHUN.TrainingDemo
{
    // ResourceManifest classes implement IResourceManifestProvider and possess the BuildManifests method.
    public class ResourceManifest : IResourceManifestProvider
    {
        // This is the only method to implement. Using it we're going to register some static resources
        // to be able to use them in our templates.
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            // We add a new instance of ResourceManifest to the ResourceManifestBuilder,
            // instantiated by the Add method.
            var manifest = builder.Add();

            manifest
                // We're registering a stylesheet with DefineStyle and defining it's name. Since stylesheets and
                // script are shapes, it's possible to override them, so you should try to avoid name collisions.
                // There is no strict naming convention, but we can give you and advice how to choose a unique name:
                // It should contain the module's full namespace followed by a meaningful name.
                .DefineStyle("OrchardHUN.TrainingDemo.Dependency")
                // This is the actual stylesheet that will be assigned to the resource name. By default, Orchard
                // will look for this file in the Styles folder, but you can override it using SetBasePath:
                // manifest.BasePath points to the root of the project, so setting
                // BasePath(manifest.BasePath + "Bedsheets") will cause that Orchard will look for the given
                // resource in the Bedsheets folder in the project root.
                // Please note that the naming of the file itself is following similiar rules as the resource name,
                // with some modifications applied.
                .SetUrl("orchardhun-trainingdemo-dependency.css");

            manifest
                .DefineStyle("OrchardHUN.TrainingDemo.Other")
                .SetUrl("orchardhun-trainingdemo-other.css")
                // You can give a list of resource names to SetDependencies to force the loading of other resources
                // when a given resource is used.
                .SetDependencies("OrchardHUN.TrainingDemo.Dependency");

            manifest
                // We'll look at the declaration of script resources too.
                .DefineScript("OrchardHUN.TrainingDemo.jQuery")
                // It is possible to include CDN-hosted resources too. Please note that Orchard contains a module
                // which carries jQuery and registers it with the resource name "jQuery", this is only for the
                // sake of demonstration.
                .SetCdn("http://code.jquery.com/jquery-1.8.3.min.js");

            // You can also assign cultures and versions to the resources using SetCultures and SetVersion.
        }
    }
}