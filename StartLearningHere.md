# Hi there! - Learn Orchard Core module development



## Introduction


Good to see you want to learn the ins and outs of Orchard Core module creation. Reading code is always a good way for this!

We'll guide you on your journey to become an Orchard Core developer. Look for "NEXT STATION" comments in the code to see where to head next, otherwise look through the code as you like.

Before you dive deep into this module it'd be best if you made sure that you have done the following: 

* You know how ASP.NET Core works. It's important that you understand how ASP.NET Core works or generally what MVC is about. If you are not familiar with the topic take a look at the tutorials at https://docs.microsoft.com/en-us/aspnet/core/tutorials/?view=aspnetcore-2.1.
* You've read through the documentation at https://orchardcore.readthedocs.io (at least the "About Orchard Core" section, but it would be great if you'd read the whole documentation).
* You know Orchard Core from a user's perspective and understand the concepts underlying the system. 
* If you want to be more familiar with Orchard Core fundamentals you can watch some of the following video about the beta2 capabilities here: https://www.youtube.com/watch?v=6ZaqWmq8Pog or simply pick some of the interesting Orchard CMS podcasts from this YouTube playlist: https://www.youtube.com/watch?v=Xu6S2XawyY4&list=PLuskKJW0FhJfOAN3dL0Y0KBMdG1pKESVn


## Moving forward


We've invested a lot of time creating this module. If you have ideas regarding it or have found mistakes, please let us know on the project page: https://github.com/Lombiq/Orchard-Training-Demo-Module

If you'd like to get trained instead of self-learning or you need help in form of mentoring sessions take a look at Orchard Dojo's  trainings: https://orcharddojo.net/orchard-training

After you complete this tutorial (or even during walking through it) you're encouraged to look at the built-in modules how they solve similar tasks. If you chose to use the simpler way to add this project to Orchard Core then you should try the other way as well to have the whole Orchard source at your hands: let it be your tutor :-).

Later on, you may want to take a look at *Map.cs* (remember, "X marks the spot!") in the project root for reminders regarding specific solutions.


## Let's get started!


**FIRST STATION**: First of all, let's discuss how a .NET Standard library becomes an Orchard Module. If you look into the Dependencies of this project you will find either a NuGet reference for the OrchardCore.Module.Targets package or if you go with the full Orchard Core source code way you can add this particular project as a Project reference.

On the other hand the module manifest file is also required. So...

**NEXT STATION**: Head over to Manifest.cs. That file is the module's manifest; a Manifest.cs is required for Orchard modules.

This demo is heavily inspired by Sipke Schoorstra's Orchard Harvest session (http://www.youtube.com/watch?v=MH9mcodTX-U) and brought to you by the Orchard Hungary team (http://english.orchardproject.hu/) and Lombiq (https://lombiq.com/).