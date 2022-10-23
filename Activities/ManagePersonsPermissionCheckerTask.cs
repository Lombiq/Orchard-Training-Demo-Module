using Microsoft.Extensions.Localization;
using OrchardCore.Security.Services;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Activities;

public class ManagePersonsPermissionCheckerTask : TaskActivity
{
    private readonly IStringLocalizer S;

    private readonly IWorkflowExpressionEvaluator _expressionEvaluator;

    private readonly IUserService _userService;

    private readonly IRoleService _roleService;


    public ManagePersonsPermissionCheckerTask(
        IUserService userService,
        IWorkflowExpressionEvaluator expressionEvaluator,
        IRoleService roleService,
        IStringLocalizer<ManagePersonsPermissionCheckerTask> localizer)
    {
        _expressionEvaluator = expressionEvaluator;
        S = localizer;
        _userService = userService;
        _roleService = roleService;
    }

    public override string Name => nameof(ManagePersonsPermissionCheckerTask);

    public override LocalizedString DisplayText => S["Manage Persons Permission Checker Task"];

    public override LocalizedString Category => S["User"];
    public WorkflowExpression<string> UserName
    {
        get => GetProperty(() => new WorkflowExpression<string>());
        set => SetProperty(value);
    }

    public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
    {
        return Outcomes(S["HasPermission"], S["NoPermission"]);
    }

    public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
    {
        var userName = await _expressionEvaluator.EvaluateAsync(UserName, workflowContext, null);
        User user = (User)await _userService.GetUserAsync(userName);

        if (user != null)
        {
            var permissions = user.RoleNames
                .Select(async roleName => await _roleService.GetRoleClaimsAsync(roleName))
                .SelectMany(claim => claim.Result)
                .Where(claim => "Permission".Equals(claim.Type))
                .Select(claim => claim.Value)
                .Distinct()
                .ToList();

            return permissions.Contains("ManagePersons") ? Outcomes("HasPermission") : Outcomes("NoPermission");
        }

        return Outcomes("NoPermission");
    }
}
