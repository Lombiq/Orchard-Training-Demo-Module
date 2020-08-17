using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Permissions
{
    public class DemoSettingsPermissions : IPermissionProvider
    {
        public static readonly Permission ManageDemoSettings = new Permission(
            nameof(ManageDemoSettings),
            "Manage Person content items.");


        public Task<IEnumerable<Permission>> GetPermissionsAsync() =>
            Task.FromResult(new[]
            {
                ManageDemoSettings,
            }
            .AsEnumerable());

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
            new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageDemoSettings },
                },
            };
    }
}
