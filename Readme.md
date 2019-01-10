# Orchard Core Training Demo module



## Project Description

Demo Orchard Core module for training purposes.

**If you are interested in training materials and Orchard trainings please visit [Orchard Dojo](https://orcharddojo.net/).**


## How to Start

You can use this module as part of a vanilla Orchard Core source that including the full source code - which is the recommended way. You can use it as part of a solution the uses Orchard Core Nuget packages, however, it's harder to look under the hood of Orchard Core features.


### Using a full Orchard Core source

1. Open an Orchard Core solution
2. Add this project to the solution
3. Add this project as a reference to OrchardCore.Application.Cms.Targets project (it's in the src/Targets solution folder)
4. Run the OrchardCore.Cms.Web project (F5 or CTRL+F5)
5. Setup the website using any recipe except "Blank", log in and enable this module on the Dashboard (~/OrchardCore.Features/Admin/Features)


### Using Nuget packages

1. Open a web project that uses Orchard Core Nuget packages
2. Add this project to the solution
3. Add this project as a reference to web project
4. Replace project references with Orchard Core Nuget package references with the same name as the project references
5. Run the web project (F5 or CTRL+F5)
6. Setup the website using any recipe except "Blank", log in and enable this module on the Dashboard (~/OrchardCore.Features/Admin/Features)


## Using this module for training purposes

If your module compiles and you are able to enable this module on the dashboard then head over to the **StartHere.txt** file and start exploring all the goodies you can do in Orchard Core.

Also if you are brave enough to not follow any guide or you want to start the guide from somewhere else then go to the **Map.cs** file and jump to any class you are interested in.


## Contribution and Feedback

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-training-demo-module](https://bitbucket.org/Lombiq/orchard-training-demo-module) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Training-Demo-Module](https://github.com/Lombiq/Orchard-Training-Demo-Module) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](https://lombiq.com/). Commercial-grade support is available through Lombiq.