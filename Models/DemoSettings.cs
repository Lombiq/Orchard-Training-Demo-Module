namespace Lombiq.TrainingDemo.Models
{
    // If you want to create your own site settings that need to be stored you need to use a simple POCO class for that
    // which will be serialized to the ISite object. However, here we actually go beyond that: We utilize the ASP.NET
    // Core Options pattern to also register it as an option. This will allow us to not only store these settings in
    // site settings but provide them from a huge variety of other means too, like from an appsettings.json file or
    // environment variables. For all these see:
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/ If you're not familiar with Options
    // check out the official docs here: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options
    // Orchard actually builds on top of all of this, see: https://docs.orchardcore.net/en/dev/docs/reference/core/Configuration/
    public class DemoSettings
    {
        // We will store a simple message for demonstration purposes.
        public string Message { get; set; }
    }
}

// NEXT STATION: Drivers/DemoSettingsDisplayDriver.cs