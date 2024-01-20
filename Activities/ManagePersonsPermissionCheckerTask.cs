using Lombiq.TrainingDemo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Activities;

// A simple workflow task that accepts a username as a TextField input and checks whether the user has ManagePersons
// Permission or not.
public class ManagePersonsPermissionCheckerTask(
    IAuthorizationService authorizationService,
    IUserService userService,
    IWorkflowExpressionEvaluator expressionEvaluator,
    IStringLocalizer<ManagePersonsPermissionCheckerTask> localizer) : TaskActivity
{
    private readonly IStringLocalizer S = localizer;

    // The technical name of the activity. Activities in a workflow definition reference this name.
    public override string Name => nameof(ManagePersonsPermissionCheckerTask);

    // The displayed name of the activity, so it can use localization.
    public override LocalizedString DisplayText => S["Manage Persons Permission Checker Task"];

    // The category to which this activity belongs. The activity picker groups activities by this category.
    public override LocalizedString Category => S["User"];

    // The username to evaluate for ManagePersons permission.
    public WorkflowExpression<string> UserName
    {
        get => GetProperty(() => new WorkflowExpression<string>());
        set => SetProperty(value);
    }

    // Returns the possible outcomes of this activity.
    public override IEnumerable<Outcome> GetPossibleOutcomes(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext) =>
        Outcomes(S["HasPermission"], S["NoPermission"]);

    // This is the heart of the activity and actually performs the work to be done.
    public override async Task<ActivityExecutionResult> ExecuteAsync(
        WorkflowExecutionContext workflowContext,
        ActivityContext activityContext)
    {
        var userName = await expressionEvaluator.EvaluateAsync(UserName, workflowContext, encoder: null);
        var user = (User)await userService.GetUserAsync(userName);

        if (user != null)
        {
            var userClaim = await userService.CreatePrincipalAsync(user);

            if (await authorizationService.AuthorizeAsync(userClaim, PersonPermissions.ManagePersons))
            {
                return Outcomes("HasPermission");
            }
        }

        return Outcomes("NoPermission");
    }
}

// NEXT STATION: Now you have to create a ViewModel for the ActivityDisplayDriver. Check out the following file
// and come back here.
// ViewModels/ManagePersonsPermissionCheckerTaskViewModel.cs

// NEXT STATION: Drivers/ManagePersonsPermissionCheckerTaskDisplayDriver.cs
