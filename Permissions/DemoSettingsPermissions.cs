using System.Collections.Generic;
using OrchardCore.Security.Permissions;

namespace Lombiq.TrainingDemo.Permissions
{
    public class DemoSettingsPermissions : IPermissionProvider
    {
        public static readonly Permission ManageDemoSettings = new Permission(
            nameof(ManageDemoSettings),
            "Manage Person content items.");

        
        public IEnumerable<Permission> GetPermissions() =>
            new[]
            {
                ManageDemoSettings
            };

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
            new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageDemoSettings }
                }
            };
    }
}