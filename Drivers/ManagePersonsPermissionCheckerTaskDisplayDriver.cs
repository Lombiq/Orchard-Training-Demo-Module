using Lombiq.TrainingDemo.Activities;
using Lombiq.TrainingDemo.ViewModels;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;

namespace Lombiq.TrainingDemo.Drivers;

// ActivityDisplayDriver is specifically for implementing workflow tasks. It performs a simple mapping of a ManagePersonsPermissionCheckerTask
// to a ManagePersonsPermissionCheckerTaskViewModel and vice versa. Don't forget to register this class with the service provider (see: Startup.cs).
public class ManagePersonsPermissionCheckerTaskDisplayDriver :
    ActivityDisplayDriver<ManagePersonsPermissionCheckerTask, ManagePersonsPermissionCheckerTaskViewModel>
{
    protected override void EditActivity(ManagePersonsPermissionCheckerTask activity, ManagePersonsPermissionCheckerTaskViewModel model) =>
        model.UserName = activity.UserName.Expression;

    protected override void UpdateActivity(ManagePersonsPermissionCheckerTaskViewModel model, ManagePersonsPermissionCheckerTask activity) =>
        activity.UserName = new WorkflowExpression<string>(model.UserName);
}

// NEXT STATION: Check out the following files to see how we make the activity visible on the admin, the come back here.
// Views/Items/ManagePersonsPermissionCheckerTask.Fields.Edit.cshtml,
// ManagePersonsPermissionCheckerTask.Fields.Design.cshtml, ManagePersonsPermissionCheckerTask.Fields.Thumbnail.cshtml

// END OF TRAINING SECTION: Workflows
