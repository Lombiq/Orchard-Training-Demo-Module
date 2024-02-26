using Lombiq.TrainingDemo.Activities;
using Lombiq.TrainingDemo.ViewModels;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;

namespace Lombiq.TrainingDemo.Drivers;

// ActivityDisplayDriver is specifically for implementing workflow tasks. It performs a simple mapping of a
// ManagePersonsPermissionCheckerTask to a ManagePersonsPermissionCheckerTaskViewModel and vice versa. Don't forget to
// register this class with the service provider (see: Startup.cs).
public class ManagePersonsPermissionCheckerTaskDisplayDriver :
    ActivityDisplayDriver<ManagePersonsPermissionCheckerTask,
    ManagePersonsPermissionCheckerTaskViewModel>
{
    protected override void EditActivity(
        ManagePersonsPermissionCheckerTask activity,
        ManagePersonsPermissionCheckerTaskViewModel model) =>
        model.UserName = activity.UserName.Expression;

    protected override void UpdateActivity(
        ManagePersonsPermissionCheckerTaskViewModel model,
        ManagePersonsPermissionCheckerTask activity) =>
        activity.UserName = new WorkflowExpression<string>(model.UserName);
}

// NEXT STATION: Check out the following files to see how we make the activity visible on the admin, then come back here.
// Views/Items/ManagePersonsPermissionCheckerTask.Fields.Edit.cshtml,
// ManagePersonsPermissionCheckerTask.Fields.Design.cshtml, ManagePersonsPermissionCheckerTask.Fields.Thumbnail.cshtml

// We created a simple workflow for the implemented activity to demonstrate how it works. It can be found under
// Recipes/TrainingDemo.Workflows.Sample.recipe.json and you can also see how to import it to the setup recipe using
// the recipe step in the TrainingDemo.recipe.json file.

// END OF TRAINING SECTION: Workflows

// This is the end of the training. It is always hard to say goodbye so... don't do it. Let us know your thoughts about
// this module or Orchard Core itself on GitHub (https://github.com/Lombiq/Orchard-Training-Demo-Module) or send us an
// email to crew@lombiq.com instead. If you feel like you need some more training on developing Orchard Core web
// applications, don't hesitate to contact us!
