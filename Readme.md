# Lombiq Training Demo for Orchard Core



## About

Demo [Orchard Core CMS](https://www.orchardcore.net/) module for training purposes guiding you to become an Orchard developer. Note that this module also has an Orchard 1.x version in the [dev-orchard-1 branch of the repository](https://github.com/Lombiq/Orchard-Training-Demo-Module/tree/dev-orchard-1).

**If you are interested in training materials and Orchard trainings please visit [Orchard Dojo](https://orcharddojo.net/).**


## How to start

You can use this module as part of a vanilla Orchard Core source that includes the full source code - which is the recommended way. You can also use it as part of a solution that uses Orchard Core NuGet packages, however, it's harder to look under the hood of Orchard Core features.

The module depends on **[Lombiq.VueJs](https://github.com/Lombiq/Orchard-Vue.js)** so you need to download it and place it next to it as elaborated below. Be sure to set up tools for building client-side resources as explained under the [VueJs module's docs](https://github.com/Lombiq/Orchard-Vue.js#prerequisites).

The module assumes that you have a good understanding of basic Orchard concepts, and that you can get around the Orchard admin area (the [official documentation](https://docs.orchardcore.net/en/dev/) may help you with that). You should also be familiar with how to use Visual Studio and write C#, as well as the concepts of ASP.NET Core MVC.


### Using a full Orchard Core source

1. Open an Orchard Core solution. You can download or `git clone` Orchard from [GitHub](https://github.com/OrchardCMS/OrchardCore/). Be sure to clone to the `master` branch, i.e. the latest released source, as this is what the module is kept up to date with (not the latest `dev` which serves for ongoing development).
2. Add the Lombiq.VueJs project first, then this one to the solution into folders named exactly "Lombiq.VueJs" and "Lombiq.TrainingDemo" under *src/OrchardCore.Modules* (it's important that you put them into this file system folder - the logical solution folder doesn't matter). Be sure to read the Vue.js module's Readme on what to install for it to build properly!
3. Add these two projects as references to the `OrchardCore.Application.Cms.Targets` project (it's in the *src/Targets* solution folder).
4. Set the `OrchardCore.Cms.Web` project as the startup project if it isn't already and run it (F5 or CTRL+F5).
5. Setup the website using the "Training Demo" recipe.

If you want to learn about unit and integration testing too then also add the test project to the solution: Add it from the module's *Tests* folder to the solution's *test* solution folder.


### Using NuGet packages

The [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) repository showcases such a web app. (And it also contains all of Lombiq's open-source Orchard themes and modules as a bonus! Check it out for what we have already solved for you.) So if you want to skip ahead you can just use that directly. If you feel adventurous then by all means create your own app as explained below!

1. Create a new web project using [the Init-OrchardCore script from the Utility Scripts project](https://github.com/Lombiq/Utility-Scripts). You can also follow the slightly more involved [official documentation](https://docs.orchardcore.net/en/dev/docs/getting-started/templates/#generate-an-orchard-cms-web-application) on how the use the code generation templates. Be sure not to disable logging (it's enabled by default) when using the template otherwise logging won't work. If you're starting an app from scratch it's highly recommended to use the templates instead of creating it manually.
2. Add this project and Lombiq.VueJs to the solution. For Lombiq.VueJs use a folder named exactly "Lombiq.VueJs". Be sure to read the Vue.js module's Readme on what to install for it to build properly!
3. Add these projects as references to the web project.
4. Set the web project as the startup project and run it (F5 or CTRL+F5).
5. Setup the website using the "Training Demo" recipe.

If you want to learn about unit and integration testing too then also add the test project to the solution: Add it from the module's *Tests* folder to a new *test* solution folder.


## Using this module for training purposes

If your module compiles and you are able to enable this module on the dashboard then head over to the **[StartLearningHere.md](StartLearningHere.md)** file and start exploring all the great things you can do in Orchard Core.

Also if you are brave enough to not follow any guide or you want to start the guide from somewhere else then go to the **Map.cs** file and jump to any class you are interested in. [StartLearningHere.md](StartLearningHere.md) also has training sections linked so you can go to a broader section.

Be sure to check out the [Orchard Dojo Library for Orchard Core](https://orcharddojo.net/orchard-resources/CoreLibrary/) for a wealth of Orchard Core guidelines, best practices, development utilities (like scripts and snippets), and more as well!


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

When adding new tutorials please keep in mind the following:

- Insert tutorial steps into the existing flow, either at the end or between two existing ones.
- If it's a new training section then indicate as such by an "END OF TRAINING SECTION" comment at the end and add it to the list under [StartLearningHere.md](StartLearningHere.md).
- Add pointers to its classes/files in *Map.cs*.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
