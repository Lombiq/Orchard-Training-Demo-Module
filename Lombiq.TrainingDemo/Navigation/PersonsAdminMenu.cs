using Lombiq.HelpfulLibraries.OrchardCore.Navigation;
using Lombiq.TrainingDemo.Controllers;
using Lombiq.TrainingDemo.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;

namespace Lombiq.TrainingDemo.Navigation;

// INavigationProvider is used for building different kind of navigations (not just admin menus).
// Here we implement it for the admin menu using AdminMenuNavigationProviderBase.
public sealed class PersonsAdminMenu : AdminMenuNavigationProviderBase
{
    public PersonsAdminMenu(IHttpContextAccessor hca, IStringLocalizer<PersonsAdminMenu> stringLocalizer)
        : base(hca, stringLocalizer)
    {
    }

    protected override void Build(NavigationBuilder builder) =>
        // The name parameter differentiates different menu types. In our case it is "admin" so let's build that.
        builder.Add(T["Person Pages"], "5", menu => menu
            // The first-level item should be a nice looking menu item so let's add a class name and an ID. It can be
            // used if you want to override the menu item shape in order to add a nice looking icon.
            .AddClass("persons").Id("persons")
            // This means that the top-level menu item also will point to the action where its first child item points.
            .LinkToFirstChild(value: true)

            // Now let's add the sub menu items.
            .Add(T["Test"], subitem => subitem
                .Action(
                    nameof(AdminController.Index),
                    typeof(AdminController).ControllerName(),
                    new { area = $"{nameof(Lombiq)}.{nameof(TrainingDemo)}" })
            )

            .Add(T["Person List"], subitem => subitem
                .LinkToFirstChild(value: true)

                .Add(T["Newest Items"], thirdLevelItem => thirdLevelItem
                    .Action(
                        nameof(AdminController.PersonListNewest),
                        typeof(AdminController).ControllerName(),
                        new { area = $"{nameof(Lombiq)}.{nameof(TrainingDemo)}" })
                    .Permission(PersonPermissions.AccessPersonListDashboard)
                    .LocalNav())

                .Add(T["Oldest Items"], thirdLevelItem => thirdLevelItem
                    .Action(
                        nameof(AdminController.PersonListOldest),
                        typeof(AdminController).ControllerName(),
                        new { area = $"{nameof(Lombiq)}.{nameof(TrainingDemo)}" })
                    .Permission(PersonPermissions.AccessPersonListDashboard)
                    .LocalNav())));
}

// NEXT STATION: Let's head back to Controllers/AdminController!
