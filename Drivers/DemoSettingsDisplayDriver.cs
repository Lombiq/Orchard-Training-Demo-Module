using System.Threading.Tasks;
using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.Permissions;
using Lombiq.TrainingDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;

namespace Lombiq.TrainingDemo.Drivers
{
    public class DemoSettingsDisplayDriver : SectionDisplayDriver<ISite, DemoSettings>
    {
        public const string GroupId = "Demo";

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _hca;


        public DemoSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor hca)
        {
            _authorizationService = authorizationService;
            _hca = hca;
        }


        public override async Task<IDisplayResult> EditAsync(DemoSettings settings, BuildEditorContext context)
        {
            if (!await IsAuthorizedToManageDemoSettingsAsync())
            {
                return null;
            }

            return Initialize<DemoSettingsViewModel>("DemoSettings_Edit", model =>
                {
                    model.Message = settings.Message;
                }).Location("Content:3").OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(DemoSettings settings, BuildEditorContext context)
        {
            if (context.GroupId == GroupId)
            {
                if (!await IsAuthorizedToManageDemoSettingsAsync())
                {
                    return null;
                }

                var model = new DemoSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                settings.Message = model.Message;
            }

            return await EditAsync(settings, context);
        }


        private async Task<bool> IsAuthorizedToManageDemoSettingsAsync()
        {
            var user = _hca.HttpContext?.User;

            return user != null && await _authorizationService.AuthorizeAsync(user, DemoSettingsPermissions.ManageDemoSettings);
        }
    }
}