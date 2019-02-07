using System;
using System.Threading.Tasks;
using Lombiq.TrainingDemo.Drivers;
using Lombiq.TrainingDemo.Permissions;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Lombiq.TrainingDemo.Navigation
{
    public class DemoSettingsAdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer T;


        public DemoSettingsAdminMenu(IStringLocalizer<PersonsAdminMenu> stringLocalizer)
        {
            T = stringLocalizer;
        }


        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder.Add(T["Configuration"], configuration => configuration
                .Add(T["Settings"], settings => settings
                    .Add(T["Demo"], T["Demo"], demo => demo
                        .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = DemoSettingsDisplayDriver.GroupId })
                        .Permission(DemoSettingsPermissions.ManageDemoSettings)
                        .LocalNav()
                    )));

            return Task.CompletedTask;
        }
    }
}

// NEXT STATION: Let's head back to Controllers/AdminController!
