using OrchardCore.ResourceManagement;

namespace Lombiq.TrainingDemo
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest
                .DefineScript("Pickr")
                .SetUrl("/Lombiq.TrainingDemo/Pickr/pickr.min.js");

            manifest
                .DefineStyle("Pickr")
                .SetUrl("/Lombiq.TrainingDemo/Pickr/pickr.min.css");

            manifest
                .DefineStyle("Lombiq.TrainingDemo.ColorPicker")
                .SetUrl("/Lombiq.TrainingDemo/Styles/trainingdemo-colorpicker.min.css",
                    "/Lombiq.TrainingDemo/Styles/trainingdemo-colorpicker.css")
                .SetDependencies("Pickr");
        }
    }
}
