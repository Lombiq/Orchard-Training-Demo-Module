using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Permissions
{
    // Permissions that are used when authorizing users and can be added to different roles are defined in Permission
    // Providers.
    public class PersonPermissions : IPermissionProvider
    {
        // Define the permissions (can be multiple) that you want to add to roles on the dashboard (or from here as a
        // default). In a PermissionProvider it's a good idea to define the permission as publicly accessible so you
        // can reference them as you've seen it for checking the EditContent permission before.
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


        public Task<IEnumerable<Permission>> GetPermissionsAsync() =>
            Task.FromResult(new[]
            {
                ManagePersons,
                AccessPersonListDashboard,
            }
            .AsEnumerable());

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
            // Giving some defaults: which roles should possess which permissions.
            new[]
            {
                new PermissionStereotype
                {
                    // Administrators will have all the permissions by default.
                    Name = "Administrator",
                    // Since AccessPersonListDashboard is implied by EditPersonList we don't have to list the former here.
                    Permissions = new[] { ManagePersons },
                },
                new PermissionStereotype
                {
                    Name = "Editor",
                    Permissions = new[] { AccessPersonListDashboard },
                },
            };
    }
}

// NEXT STATION: Go back to AuthorizationController and find the CanManagePersons action.
