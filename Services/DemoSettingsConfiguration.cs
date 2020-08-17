using Lombiq.TrainingDemo.Models;
using Microsoft.Extensions.Options;
using OrchardCore.Entities;
using OrchardCore.Settings;

namespace Lombiq.TrainingDemo.Services
{
    // This is a configuration class that'll load the options from site settings.
    public class DemoSettingsConfiguration : IConfigureOptions<DemoSettings>
    {
        private readonly ISiteService _siteService;


        public DemoSettingsConfiguration(ISiteService siteService) => _siteService = siteService;


        public void Configure(DemoSettings options)
        {
            // The method parameter comes from the other configuration options, like the appsettings.json file if it's
            // set, or will be null. You can decide what should happen if both settings are set. Here we'll use the
            // other one if it's set.
            if (!string.IsNullOrEmpty(options.Message)) return;

            // If you'd like to try this out, add an "OrchardCore" section to your web app's appsettings.json file. So
            // in the end it should look something like this:
            ////{
            ////  "Logging": {
            ////    "IncludeScopes": false,
            ////    "LogLevel": {
            ////      "Default": "Warning"
            ////    }
            ////  },
            ////  "OrchardCore": {
            ////    "Lombiq_TrainingDemo": {
            ////      "Message": "This comes from appsettings!"
            ////    }
            ////  }
            ////}

            // Not that for this to work IShellConfiguration needs to be accessed from the Startup class, so quickly
            // check out what we have there related to settings and come back!

            // Unfortunately, no async here so we need to run this synchronously.
            var settings = _siteService.GetSiteSettingsAsync()
                .GetAwaiter().GetResult()
                .As<DemoSettings>();

            options.Message = settings.Message;
        }
    }
}
