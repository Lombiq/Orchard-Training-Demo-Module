# Orchard Core Training Demo module



## About

Demo [Orchard Core CMS](https://www.orchardcore.net/) module for training purposes guiding you to become an Orchard developer. Note that this module also has an Orchard 1.x version in the [dev branch of the repository](https://github.com/Lombiq/Orchard-Training-Demo-Module/tree/dev).

**If you are interested in training materials and Orchard trainings please visit [Orchard Dojo](https://orcharddojo.net/).**


## How to start

You can use this module as part of a vanilla Orchard Core source that includes the full source code - which is the recommended way. You can also use it as part of a solution that uses Orchard Core NuGet packages, however, it's harder to look under the hood of Orchard Core features.

The module depends on **[Lombiq.VueJs](https://github.com/Lombiq/Orchard-Vue.js)** so you need to download it and place it next to it.

The module assumes that you have a good understanding of basic Orchard concepts, and that you can get around the Orchard admin area (the [official documentation](https://docs.orchardcore.net/en/dev/) may help you with that). You should also be familiar with how to use Visual Studio and write C#, as well as the concepts of ASP.NET Core MVC.


### Using a full Orchard Core source

**Warning:** At the moment the latest released Orchard Core version is RC1 and it targets the unsupported 3.0 version of .NET Core. Since we updated our extensions to 3.1 they won't be compatible anymore. Thus for the time being please use the other approach with NuGet packages.

1. Open an Orchard Core solution. You can download or `git clone` Orchard from [GitHub](https://github.com/OrchardCMS/OrchardCore/). Be sure to clone to the `master` branch, i.e. the latest released source, as this is what the module is kept up to date with (not the latest `dev` which serves for ongoing development).
2. Add the Lombiq.VueJs project first, then this one to the solution into folders named exactly Lombiq.VueJs" and "Lombiq.TrainingDemo" (it doesn't matter which folder in the solution' directory you use for them but place them into the same file system folder - the logical solution folder doesn't matter).
3. Add these two projects as references to the `OrchardCore.Application.Cms.Targets` project (it's in the *src/Targets* solution folder).
4. Set the `OrchardCore.Cms.Web` project as the startup project if it isn't already and run it (F5 or CTRL+F5).
5. Setup the website using the "Training Demo" recipe.


### Using NuGet packages

The [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) repository showcases such a web app (and it also contains all of Lombiq's open-source Orchard themes and modules as a bonus!). So if you want to skip ahead you can just use that directly. If you feel adventurous then by all means create your own app as explained below!

1. Open a web project that uses the Orchard Core NuGet packages or [create one using the code generation template](https://docs.orchardcore.net/en/dev/docs/getting-started/templates/#generate-an-orchard-cms-web-application). Be sure not to disable logging (it's enabled by default) when using the template otherwise logging won't work. If you're starting an app from scratch it's highly recommended to use the templates instead of creating it manually.
2. Add this project and Lombiq.VueJs to the solution.
3. Add these projects as references to the web project.
4. Run the web project (F5 or CTRL+F5).
5. Setup the website using the "Training Demo" recipe.


## Using this module for training purposes

If your module compiles and you are able to enable this module on the dashboard then head over to the **[StartLearningHere.md](StartLearningHere.md)** file and start exploring all the great things you can do in Orchard Core.

Also if you are brave enough to not follow any guide or you want to start the guide from somewhere else then go to the **Map.cs** file and jump to any class you are interested in.


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

When adding new tutorials please keep in mind the following:

- Insert tutorial steps into the existing flow, either at the end or between two existing ones.
- If it's a new training section then indicate as such by an "END OF TRAINING SECTION" comment at the end and add it to the list under [StartLearningHere.md](StartLearningHere.md).
- Add pointers to its classes/files in *Map.cs*.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.