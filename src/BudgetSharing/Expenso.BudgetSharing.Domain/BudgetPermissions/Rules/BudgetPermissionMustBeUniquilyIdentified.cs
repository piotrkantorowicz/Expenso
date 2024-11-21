using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.System.Tasks;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

public sealed class BudgetPermissionMustBeUniquilyIdentified : IBusinessRule
{
    private readonly BudgetPermissionId _budgetPermissionId;
    private readonly BudgetId _budgetId;
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;

    public BudgetPermissionMustBeUniquilyIdentified(BudgetPermissionId budgetPermissionId, BudgetId budgetId,
        IBudgetPermissionRepository budgetPermissionRepository)
    {
        _budgetPermissionId =
            budgetPermissionId ?? throw new ArgumentNullException(paramName: nameof(budgetPermissionId));

        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));

        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));
    }

    public string Message =>
        $"A budget permission must be uniquely identified by its ID {_budgetPermissionId} and Budget ID {_budgetId}.";

    public bool IsBroken()
    {
        BudgetPermission? budgetPermissionFetchedById = _budgetPermissionRepository
            .GetByIdAsync(budgetPermissionId: _budgetPermissionId, cancellationToken: default)
            .RunAsSync();

        BudgetPermission? budgetPermissionFetchedByBudgetId = _budgetPermissionRepository
            .GetByBudgetIdAsync(budgetId: _budgetId, cancellationToken: default)
            .RunAsSync();

        return budgetPermissionFetchedById is not null || budgetPermissionFetchedByBudgetId is not null;
    }
}