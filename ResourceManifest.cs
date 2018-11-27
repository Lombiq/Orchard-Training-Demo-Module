using System;
using System.Collections.Generic;
using System.Text;
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
                .SetUrl("/Lombiq.TrainingDemo/pickr/pickr.min.js");

            manifest
                .DefineStyle("Pickr")
                .SetUrl("/Lombiq.TrainingDemo/pickr/pickr.min.css");
        }
    }
}
