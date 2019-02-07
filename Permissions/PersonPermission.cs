using System.Collections.Generic;
using OrchardCore.Security.Permissions;

namespace Lombiq.TrainingDemo.Permissions
{
    public class PersonPermission : IPermissionProvider
    {
        // Define the permissions (can be multiple) that you want to set to roles on the dashboard (or from here as a
        // default). In a PermissionProvider it's a good idea to define the permission publicly accessible so you can
        // reference it as you've seen it for checking EditContent permission before.
        public static readonly Permission ManagePersons = new Permission(
            nameof(ManagePersons),
            "Manage Person content items.");

        // Here's another permission that has a third parameter which is called "ImpliedBy". It means that everybody
        // who has the ManagePersons permission also automatically possesses the AccessPersonListDashboard permission
        // as well. Be aware that because of this AccessPersonListDashboard should be written after ManagePersons.
        public static readonly Permission AccessPersonListDashboard = new Permission(
            nameof(AccessPersonListDashboard),
            "Access the Person List dashboard",
            new[] { ManagePersons });

        
        public IEnumerable<Permission> GetPermissions() =>
            new[]
            {
                ManagePersons,
                AccessPersonListDashboard
            };

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
            // Giving some defaults: which user groups should possess which permissions.
            new[]
            {
                new PermissionStereotype
                {
                    // Administrators will have all the permissions by default.
                    Name = "Administrator",
                    // Since AccessPersonListDashboard is implied by EditPersonList we don't have to list the former here.
                    Permissions = new[] { ManagePersons }
                },
                new PermissionStereotype
                {
                    Name = "Editor",
                    Permissions = new[] { AccessPersonListDashboard }
                }
            };
    }
}

// NEXT STATION: Go back to AuthorizationController and find the CanManagePersons action.