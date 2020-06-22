using Lombiq.TrainingDemo.Controllers;
using Lombiq.TrainingDemo.Permissions;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Navigation
{
    // INavigationProvider is used for building different kind of navigations (not just admin menus). Don't forget to
    // register this class with the service provider (see: Startup.cs).
    public class PersonsAdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer T;


        public PersonsAdminMenu(IStringLocalizer<PersonsAdminMenu> stringLocalizer)
        {
            T = stringLocalizer;
        }


        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            // The name parameter differentiates different menu types. In our case it is "admin" so let's check if the
            // menu being built is for admin.
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            // The builder will add different menu items on different levels. Here we'll create a 3-level menu. The
            // first-level menu is what you will see first when you go to the Dashboard and the sub menu items when you
            // click on this. It should have a position parameter as well which not surprisingly will be the position
            // in the admin menu.
            builder.Add(T["Person Pages"], "5", menu => menu
                // The first-level item should be a nice looking menu item so let's add a class name and an ID. It can
                // be used if you want to override the menu item shape (because these are shapes too) in order to add a
                // nice looking icon to it. If you want to override it then use the NavigationItemText-[id].Id
                // alternate when creating the cshtml file.
                // NEXT STATION: Go check how it's done in Views/NavigationItemText-persons.Id.cshtml and come back
                // here!
                .AddClass("persons").Id("persons")
                // This means that the top-level menu item also will point to the action where its first child item
                // points. Now that is conventional to do, however, it really depends on the Admin theme. The TheAdmin
                // theme will never link it to the first child, it will always be a dropdown without a link.
                .LinkToFirstChild(true)
                // Now let's add the sub menu items with the same Add() method we used for the first-level item but
                // chained to this one.
                .Add(T["Test"], subitem => subitem
                    // The Action method will bind the menu item to the action. This is the test action that we've
                    // added to the AdminController to see if this is working automatically. Note the use of Orchard's
                    // ControllerName() helper method.
                    .Action(nameof(AdminController.Index), typeof(AdminController).ControllerName(), new { area = $"{nameof(Lombiq)}.{nameof(TrainingDemo)}" })
                )
                // Add another menu item that will display multiple Person items. However, branch this item to two
                // different third-level items!
                .Add(T["Person List"], subitem => subitem
                    .LinkToFirstChild(true)

                    .Add(T["Newest Items"], thirdLevelItem => thirdLevelItem
                        .Action(nameof(AdminController.PersonListNewest), typeof(AdminController).ControllerName(), new { area = $"{nameof(Lombiq)}.{nameof(TrainingDemo)}" })
                        // This means that the first child menu item will point to our Person List dashboard and be
                        // shown only to users having the AccessPersonListDashboard permission. WARNING: this doesn't
                        // mean others won't be able to access it directly: we have to check in the controller too!
                        .Permission(PersonPermissions.AccessPersonListDashboard)
                        // Optionally mark your menu item to be displayed on a local navigation shape placed outside
                        // the menu. Most probably it will only happen if the theme doesn't support the third-level
                        // menu items and it will display them somewhere else. In the TheAdmin theme it won't make any
                        // difference since the three-level menu is supported.
                        .LocalNav())

                    .Add(T["Oldest Items"], thirdLevelItem => thirdLevelItem
                        .Action(nameof(AdminController.PersonListOldest), typeof(AdminController).ControllerName(), new { area = $"{nameof(Lombiq)}.{nameof(TrainingDemo)}" })
                        .Permission(PersonPermissions.AccessPersonListDashboard)
                        .LocalNav())));

            return Task.CompletedTask;
        }
    }
}

// NEXT STATION: Let's head back to Controllers/AdminController!
