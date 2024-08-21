using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class BudgetPermissionMustExistsToBeAbleToCreateBudgetPermissionRequest : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly BudgetPermission? _budgetPermission;

    public BudgetPermissionMustExistsToBeAbleToCreateBudgetPermissionRequest(BudgetPermission? budgetPermission,
        BudgetId budgetId)
    {
        _budgetPermission = budgetPermission;
        _budgetId = budgetId;
    }

    public string Message =>
        $"Unable to create budget permission request for not existant budget permission. Budget {_budgetId}";

    public bool IsBroken()
    {
        return _budgetPermission is null;
    }
}