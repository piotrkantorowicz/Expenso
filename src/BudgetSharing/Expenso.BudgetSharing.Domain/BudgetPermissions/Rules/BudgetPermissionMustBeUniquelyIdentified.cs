using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.System.Tasks;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

public sealed class BudgetPermissionMustBeUniquelyIdentified : IBusinessRule
{
    private readonly BudgetPermissionId _budgetPermissionId;
    private readonly BudgetId _budgetId;
    private readonly PersonId _ownerId;
    private readonly BudgetCode _budgetCode;
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;

    public BudgetPermissionMustBeUniquelyIdentified(BudgetPermissionId budgetPermissionId, BudgetId budgetId,
        PersonId ownerId, BudgetCode budgetCode, IBudgetPermissionRepository budgetPermissionRepository)
    {
        _budgetPermissionId =
            budgetPermissionId ?? throw new ArgumentNullException(paramName: nameof(budgetPermissionId));

        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));

        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));

        _ownerId = ownerId ?? throw new ArgumentNullException(paramName: nameof(ownerId));
        _budgetCode = budgetCode ?? throw new ArgumentNullException(paramName: nameof(budgetCode));
    }

    public string Message =>
        $"A budget permission must be uniquely identified by its ID {_budgetPermissionId} and Budget ID {_budgetId} and combination of Owner ID {_ownerId} and Budget Code {_budgetCode}.";

    public bool IsBroken()
    {
        return _budgetPermissionRepository
            .IsUnique(budgetPermissionId: _budgetPermissionId, budgetId: _budgetId, ownerId: _ownerId,
                budgetCode: _budgetCode, cancellationToken: default)
            .RunAsSync() is false;
    }
}