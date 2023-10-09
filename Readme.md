# Lombiq Training Demo for Orchard Core

## About

Demo [Orchard Core](https://orchardcore.net/) module for training purposes guiding you to become an Orchard developer. Note that this module also has an Orchard 1.x version in the [dev-orchard-1 branch of the repository](https://github.com/Lombiq/Orchard-Training-Demo-Module/tree/dev-orchard-1).

If you prefer tutorial videos more then check out the [Dojo Course 3, the complete Orchard Core tutorial series](https://orcharddojo.net/orchard-training/dojo-course-3-the-full-orchard-core-tutorial).

**If you are interested in training materials and personalized Orchard trainings please visit [Orchard Dojo](https://orcharddojo.net/).**

## How to start

The module assumes that you have a good understanding of basic Orchard concepts, and that you can get around the Orchard admin area (the [official documentation](https://docs.orchardcore.net/) and the [Dojo Course 3 tutorial series](https://orcharddojo.net/orchard-training/dojo-course-3-the-full-orchard-core-tutorial) may help you with that). You should also be familiar with how to use Visual Studio (or any other C# IDE) and write C#, as well as the concepts of ASP.NET Core MVC.

Before you dive deep into this module it'd be best if you make sure that you have done the following:

- You know how ASP.NET Core MVC works. It's important that you understand how ASP.NET Core MVC works or generally what MVC is about. If you are not familiar with the topic take a look at the tutorials at <https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/?view=aspnetcore-3.1>.
- You've read through the documentation under <https://docs.orchardcore.net/en/latest/> (at least the "About Orchard Core" section, but it would be great if you'd skim the whole documentation).
- You know Orchard Core from a user's perspective and understand the fundamental concepts underlying the system. (The [Dojo Course 3 tutorial series](https://orcharddojo.net/orchard-training/dojo-course-3-the-full-orchard-core-tutorial) may help you with that.)

The [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) repository showcases a web app using Orchard Core from NuGet packages, and it includes this module too. Furthermore, it also contains all of Lombiq's open-source Orchard themes and modules as a bonus! (Check them out for what we have already solved for you.) So, just use that to work with this module:

1. Complete [the repository's prerequisites](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions#prerequisites-and-getting-started) and clone it to the latest `dev` branch.
2. Make sure the `Lombiq.OSOCE.Web` project is the startup project (it should be).
3. Start the app with <kbd>Ctrl</kbd> + <kbd>F5</kbd>.
4. Set up the website using the "Training Demo" recipe.

## Learning Orchard Core with this module

Once the app is running, head over to the **[StartLearningHere.md](StartLearningHere.md)** file and start exploring all the great things you can do in Orchard Core.

Also if you are brave enough to not follow any guide or you want to start the guide from somewhere else then go to the **Map.cs** file and jump to any class you are interested in. [StartLearningHere.md](StartLearningHere.md) also has training sections linked so you can go to a broader section.

Be sure to check out the [Orchard Dojo Library for Orchard Core](https://orcharddojo.net/orchard-resources/CoreLibrary/) for a wealth of Orchard Core guidelines, best practices, development utilities (like scripts and snippets), and more as well!

You can also follow the [Dojo Course 3 tutorial series](https://orcharddojo.net/orchard-training/dojo-course-3-the-full-orchard-core-tutorial) if you like to learn from videos.

Keep in mind that your best living reference of how to do something in Orchard is [the official repo](https://github.com/OrchardCMS/OrchardCore) and our Open-Source Orchard Core Extensions solution. Clone both and keep the solutions open when you’re working on something so you can quickly look up anything.

If you'd like to clean out comments from code files so you can just see the essence, then use the [Comment Remover VS extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.CommentRemover) to quickly do it.

Take a look at the Recipes menu in the Admin screen to load in additional sample content. This is not included by default to reduce clutter.

## Dependencies

This module has the following dependencies:

- [Lombiq Helpful Libraries for Orchard Core](https://github.com/Lombiq/Helpful-Libraries)
- [Lombiq Node.js Extensions](https://gihub.com/Lombiq/NodeJs-Extensions)

## Contributing and support

Bug reports, feature requests, comments, questions, code contributions and love letters are warmly welcome. You can send them to us via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

When adding new tutorials please keep in mind the following:

- Insert tutorial steps into the existing flow, either at the end or between two existing ones. Use "NEXT STATION" comments to indicate the next file the reader should check out.
- If it's a new training section then indicate as such by an "END OF TRAINING SECTION" comment at the end and add it to the list under [StartLearningHere.md](StartLearningHere.md).
- Add pointers to its classes/files in _Map.cs_.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
