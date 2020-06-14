using Lombiq.TrainingDemo.Drivers;
using Lombiq.TrainingDemo.Permissions;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Navigation
{
    // To actually see the menu item on the admin menu we need to add a navigation provider to it.
    public class DemoSettingsAdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer T;


        public DemoSettingsAdminMenu(IStringLocalizer<DemoSettingsAdminMenu> stringLocalizer) => T = stringLocalizer;


        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase)) return Task.CompletedTask;

            // If you want to put a menu item to a deeper lever under an existing menu item you just need to build your
            // menu using the menu text of the existing items. Here the Configuration and Settings menu items are
            // already existing items and this is the place you should put your site settings, however, you could use
            // any other place if you want.
            builder.Add(T["Configuration"], configuration => configuration
                .Add(T["Settings"], settings => settings
                    .Add(T["Demo"], T["Demo"], demo => demo
                        // The Action will be the AdminController.Index action in the OrchardCore.Settings module. It
                        // will make sure that the proper editor group will be displayed so give the editor group ID
                        // too using your publicly accessible editor group constant.
                        .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = DemoSettingsDisplayDriver.EditorGroupId })
                        // Authorize so it will be displayed only if the user has permission to access it.
                        .Permission(DemoSettingsPermissions.ManageDemoSettings)
                        // It's a third-level menu item so put it on local navigation if it's supported.
                        .LocalNav()
                    )));

            return Task.CompletedTask;
        }
    }
}

// NEXT STATION: Let's head back to Controllers/SiteSettingsController!
