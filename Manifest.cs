using OrchardCore.Modules.Manifest;

[assembly: Module(
    // Display name of the module. Will be displayed on the Modules page of the Dashboard if the module has just one
    // feature (see below).
#pragma warning disable SA1114 // Parameter list should follow declaration (necessary for the comment)
    Name = "Lombiq Training Demo",
#pragma warning restore SA1114 // Parameter list should follow declaration
    // Your name, company or any name that identifies the developers working on the project.
    Author = "Lombiq Technologies",
    // Optionally you can add a website URL (e.g. your company's website, GitHub repository URL).
    Website = "https://github.com/Lombiq/Orchard-Training-Demo-Module",
    // Version of the module.
    Version = "2.0.0",
    // Short description of the module.
    Description = "Orchard Core training demo module for teaching Orchard Core fundamentals primarily by going " +
        "through its source code."
)]

// This is a big module so it has multiple features. What are features, you ask? You can divide a module into multiple
// "features"; in the Orchard sense, a feature is a piece of functionality that you can individually turn on or off on
// the admin. It's like a sub-module.
// If you have multiple features in a module then you also have to have one for the base feature (otherwise you can't
// turn that on). If you don't have multiple features then the Category and Dependencies configuration from below can
// just directly go to the Module attribute.
[assembly: Feature(
    // The technical ID of the feature. The base feature should have the same name as the module (i.e. its csproj file).
#pragma warning disable SA1114 // Parameter list should follow declaration (necessary for the comment)
    Id = "Lombiq.TrainingDemo",
#pragma warning restore SA1114 // Parameter list should follow declaration
    // Display name of the feature, displayed on the Modules page of the Dashboard.
    Name = "Lombiq Training Demo",
    // Features are categorized on the Dashboard so it's a good idea to put similar features/modules together into a
    // separate section.
    Category = "Training",
    // Short description of the feature. It will be displayed on the Dashboard (where you can enable/disable it).
    Description = "Orchard Core training demo module for teaching Orchard Core fundamentals primarily by going " +
        "through its source code.",
    // Features can have dependencies which are other module names (name of the project) or if these modules have
    // sub-features too then the name of the feature. If you use any service, taghelper etc. coming from an Orchard Core
    // feature then you need to include them in this list. Orchard Core will make sure to enable all dependent features
    // when you enable a feature that has dependencies. Without this some features would not work even if the assembly
    // is referenced in the project.
    Dependencies = new[]
    {
        "OrchardCore.BackgroundTasks",
        "OrchardCore.Contents",
        "OrchardCore.ContentTypes",
        "OrchardCore.ContentFields",
        "OrchardCore.DynamicCache",
        "OrchardCore.Media",
        "OrchardCore.Navigation",
        "OrchardCore.Apis.GraphQL",
    }
)]

// And we also have a second feature!
[assembly: Feature(
    // We commonly suffix the base feature/module ID and name for sub-features.
#pragma warning disable SA1114 // Parameter list should follow declaration (necessary for the comment)
    Id = "Lombiq.TrainingDemo.Middlewares",
#pragma warning restore SA1114 // Parameter list should follow declaration
    Name = "Lombiq Training Demo - Middlewares",
    Category = "Training",
    Description = "Demonstrates how to write middlewares in a separate feature.",
    // It's usual for sub-features to depend on the base feature but this is not mandatory.
    Dependencies = new[]
    {
        "Lombiq.TrainingDemo",
    }
)]

// How do you distinguish what's activated when turning on a feature? With the same Feature attribute! Check out the
// Startup.cs file for the Startup class corresponding to both features but then follow up here!

// END OF TRAINING SECTION: Manifest
// NEXT STATION: Controllers/YourFirstOrchardCoreController.cs
