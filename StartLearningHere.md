# Hi there! - Learn Orchard Core module development



## Introduction

Good to see you want to learn the ins and outs of Orchard Core module creation. Reading code is always a good way for this!

We'll guide you on your journey to become an Orchard Core developer. Look for "NEXT STATION" comments in the code to see where to head next, otherwise look through the code as you like.


## Moving forward

We've invested a lot of time creating this module. If you have ideas regarding it or have found mistakes, please let us know on [the project page on GitHub](https://github.com/Lombiq/Orchard-Training-Demo-Module).

If you'd like to get trained instead of self-learning or you need help in form of mentoring sessions take a look at [Orchard Dojo's trainings](https://orcharddojo.net/orchard-training).

After you complete this tutorial (or even during walking through it) you're encouraged to look at Orchard's built-in modules on how they solve similar tasks. Clone [the official repo](https://github.com/OrchardCMS/OrchardCore) and let it be your tutor :-).

Later on, you may want to take a look at *Map.cs* (remember, "X marks the spot!") in the project root for reminders regarding specific solutions.


## Let's get started!

**FIRST STATION**: First of all, let's discuss how a .NET library becomes an Orchard Module. If you look into the Dependencies of this project you will find either the NuGet reference for the `OrchardCore.Module.Targets` package.

Furthermore, the module manifest file is also required. So...

**NEXT STATION**: Head over to *Manifest.cs*. That file is the module's manifest; a *Manifest.cs* is required for Orchard modules.

Note that the module's [recipe file](https://docs.orchardcore.net/en/latest/docs/reference/modules/Recipes/) that you used during setup is in the *Recipes* folder. A lot of features included in this module are configured from there; you can check it out too.

This demo is heavily inspired by [Sipke Schoorstra's Orchard Harvest session](http://www.youtube.com/watch?v=MH9mcodTX-U) and brought to you by [Lombiq Technologies](https://lombiq.com/).


## Training sections

- [Module manifest](Manifest.cs)
- [Map.cs for reminders regarding specific solutions](Map.cs)
- [Your first OrchardCore Controller](Controllers/YourFirstOrchardCoreController.cs)
- [Display management](Controllers/DisplayManagementController.cs)
- [Storing data in document database and index records](Controllers/DatabaseStorageController.cs)
- [Content Item and Content Part development](Models/PersonPart.cs)
- [Content Item display management and querying/modifying content items from code](Controllers/PersonListController.cs)
- [Content Field development](Fields/ColorField.cs)
- [Indexing Content Fields in Lucene](Indexing/ColorFieldIndexHandler.cs)
- [Content Field display and editor options](Views/ColorField.Option.cshtml)
- [Resource management](ResourceManagementOptionsConfiguration.cs)
- [Permissions and authorization](Controllers/AuthorizationController.cs)
- [Navigation menus](Controllers/AdminController.cs) ([See here for for a similar tutorial about theming with Lombiq.BaseTheme.](https://github.com/Lombiq/Orchard-Base-Theme/tree/dev/Lombiq.BaseTheme.Samples))
- [Site settings and `IConfiguration`](Controllers/SiteSettingsController.cs)
- [Utilizing action and result filters](Filters/ShapeInjectionFilter.cs)
- [Shape tables](Services/ShapeHidigingShapeTableProvider.cs)
- [Caching objects and shapes](Controllers/CacheController.cs)
- [File management](Controllers/FileManagementController.cs)
- [Background tasks](Services/DemoBackgroundTask.cs)
- [Event handlers](Events/LoginGreeting.cs)
- [Web API](Controllers/ApiController.cs)
- [Middlewares](Middlewares/RequestLoggingMiddleware.cs)
- [Accessing services from other tenants](Controllers/CrossTenantServicesController.cs)
- [Unit and integration testing](Services/TestedService.cs)
- [Compiling resources using Gulp](Gulpfile.js)
- [GraphQL](GraphQL/Startup.cs)
