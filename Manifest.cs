using OrchardCore.Modules.Manifest;

[assembly: Module(
    // Name of the module to be displayed on the Modules page of the Dashboard.
    Name = "Orchard Core Training Demo",
    // Your name, company or any name that identifies the developers working on the project.
    Author = "Lombiq",
    // Optionally you can add a website URL (e.g. your company's website, GitHub reporitory URL).
    Website = "http://orchardproject.net",
    // Version of the module.
    Version = "2.0",
    // Short description of the module. It will be displayed on the the Dashboard.
    Description = "Orchard Core training demo module for teaching Orchard Core fundamentals primarily by going through its source code.",
    // Modules are categorized on the Dashboard so it's a good idea to put similar modules together to a separate section.
    Category = "Training"
)]

// If you're done reading throught this file go to Controllers/YourFirstOrchardCoreController.