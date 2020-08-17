using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.Permissions;
using Lombiq.TrainingDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Drivers
{
    // Now this display driver abstraction is different from the one you've seen before. In Orchard Core you can
    // connect different objects to a master object that will be connected in the database when storing them. Site
    // settings are handled this way.
    public class DemoSettingsDisplayDriver : SectionDisplayDriver<ISite, DemoSettings>
    {
        // Since technically we have only one SiteSettings we have separate the editors using editor groups. It's a
        // good idea to store the editor group ID in a publicly accessibly constant (would be much better to store it
        // in a static class placed in a Constants folder).
        public const string EditorGroupId = "Demo";

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _hca;


        public DemoSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor hca)
        {
            _authorizationService = authorizationService;
            _hca = hca;
        }


        // Here's the EditAsync override to display editor for our site settings on the Dashboard. Note that it has a
        // sync version too.
        public override async Task<IDisplayResult> EditAsync(DemoSettings section, BuildEditorContext context)
        {
            // What you really don't want to is to let unauthorized users update site-level settings of your site so
            // it's really advisable to create a separate permission for managing the settings or the feature related
            // to this settings and use it here. We've created one that you can see in the
            // Permissions/DemoSettingsPermissions.cs file.
            if (!await IsAuthorizedToManageDemoSettingsAsync())
            {
                // If not authorized then return null which means that nothing will be displayed that would've been
                // displayed by this DisplayDriver otherwise.
                return null;
            }

            // Use the Initialize helper with a view model as usual for editors.
            return Initialize<DemoSettingsViewModel>(
                $"{nameof(DemoSettings)}_Edit",
                viewModel => viewModel.Message = section.Message)
            .Location("Content:1")
            // The OnGroup helper will make sure that the shape will be displayed on the desired editor group.
            .OnGroup(EditorGroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(DemoSettings section, BuildEditorContext context)
        {
            // Since this DisplayDriver is for the ISite object this UpdateAsync will be called every time if a site
            // settings editor is being updated. To make sure that this is for our editor group check it here.
            if (context.GroupId == EditorGroupId)
            {
                // Authorize here too.
                if (!await IsAuthorizedToManageDemoSettingsAsync())
                {
                    return null;
                }

                // Update the view model and the settings model as usual.
                var viewModel = new DemoSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(viewModel, Prefix);

                section.Message = viewModel.Message;
            }

            return await EditAsync(section, context);
        }


        private async Task<bool> IsAuthorizedToManageDemoSettingsAsync()
        {
            // Since the User object is not accessible here (as it was accessible in the Controller) we need to grab it
            // from the HttpContext.
            var user = _hca.HttpContext?.User;

            return user != null && await _authorizationService.AuthorizeAsync(user, DemoSettingsPermissions.ManageDemoSettings);
        }
    }
}

// NEXT STATION: Navigations/DemoSettingsAdminMenu.cs
