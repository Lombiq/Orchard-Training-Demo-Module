using Lombiq.TrainingDemo.Activities;
using Lombiq.TrainingDemo.ViewModels;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;

namespace Lombiq.TrainingDemo.Drivers;

// ActivityDisplayDriver is specifically for implementing Workflow Activity Tasks.
// Don't forget to register this class with the service provider (see: Startup.cs).
public class ManagePersonsPermissionCheckerTaskDisplayDriver :
    ActivityDisplayDriver<ManagePersonsPermissionCheckerTask, ManagePersonsPermissionCheckerTaskViewModel>
{
    protected override void EditActivity(
        ManagePersonsPermissionCheckerTask activity,
        ManagePersonsPermissionCheckerTaskViewModel model) => model.UserName = activity.UserName.Expression;

    protected override void UpdateActivity(
        ManagePersonsPermissionCheckerTaskViewModel model,
        ManagePersonsPermissionCheckerTask activity) => activity.UserName = new WorkflowExpression<string>(model.UserName);
}

