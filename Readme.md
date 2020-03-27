# Orchard Core Training Demo module



## Project description

Demo Orchard Core module for training purposes guiding you to become an Orchard developer. Note that this module also has an Orchard 1.x version in the [dev branch of the repository](https://github.com/Lombiq/Orchard-Training-Demo-Module/tree/dev).

**If you are interested in training materials and Orchard trainings please visit [Orchard Dojo](https://orcharddojo.net/).**


## How to start

You can use this module as part of a vanilla Orchard Core source that includes the full source code - which is the recommended way. You can also use it as part of a solution that uses Orchard Core NuGet packages, however, it's harder to look under the hood of Orchard Core features.

The module depends on **[Lombiq.VueJs](https://github.com/Lombiq/Orchard-Vue.js)** so you need to download it and place it next to it.

The module assumes that you have a good understanding of basic Orchard concepts, and that you can get around the Orchard admin area (the [official documentation](https://docs.orchardcore.net/en/dev/) may help you with that). You should also be familiar with how to use Visual Studio and write C#, as well as the concepts of ASP.NET Core MVC.


### Using a full Orchard Core source

1. Open an Orchard Core solution. You can download or `git clone` Orchard from [GitHub](https://github.com/OrchardCMS/OrchardCore/).
2. Add this project and the Lombiq.VueJs project to the solution (it doesn't matter which solution folder you use but place them next to each other).
3. Add these two projects as references to the `OrchardCore.Application.Cms.Targets` project (it's in the *src/Targets* solution folder).
4. Set the `OrchardCore.Cms.Web` project as the startup project if it isn't already and run it (F5 or CTRL+F5).
5. Setup the website using the "Training Demo" recipe.


### Using NuGet packages

1. Open a web project that uses the Orchard Core NuGet packages or [create one using the code generation template](https://docs.orchardcore.net/en/dev/docs/getting-started/templates/#generate-an-orchard-cms-web-application). Be sure not to disable logging (it's enabled by default) when using the template otherwise logging won't work. If you're starting an app from scratch it's highly recommended to use the templates instead of creating it manually.
2. Add this project and Lombiq.VueJs to the solution.
3. Add these projects as references to the web project.
4. Run the web project (F5 or CTRL+F5).
5. Setup the website using the "Training Demo" recipe.

The [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) repository showcases such a web app (and it also contains all of Lombiq's open-source Orchard themes and modules as a bonus!).


## Using this module for training purposes

If your module compiles and you are able to enable this module on the dashboard then head over to the **[StartLearningHere.md](StartLearningHere.md)** file and start exploring all the great things you can do in Orchard Core.

Also if you are brave enough to not follow any guide or you want to start the guide from somewhere else then go to the **Map.cs** file and jump to any class you are interested in.


## Contribution and feedback

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-training-demo-module](https://bitbucket.org/Lombiq/orchard-training-demo-module) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Training-Demo-Module](https://github.com/Lombiq/Orchard-Training-Demo-Module) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**. Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](https://lombiq.com/). Commercial-grade support is available through Lombiq.