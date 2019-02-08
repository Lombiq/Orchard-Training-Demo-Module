namespace Lombiq.TrainingDemo.Models
{
    // If you want to create your own site settings that needs to be stored you need to use a simple POCO class for
    // that which will be serialized to the ISite object.
    public class DemoSettings
    {
        // We will store a simple message for demonstration purposes.
        public string Message { get; set; }
    }
}

// NEXT STATION: Drivers/DemoSettingsDisplayDriver.cs