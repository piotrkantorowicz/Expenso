using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetMustHasDistinctPermissionsForUsers : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly PersonId _participantId;
    private readonly IEnumerable<Permission> _permissions;

    public BudgetMustHasDistinctPermissionsForUsers(BudgetId budgetId, PersonId participantId,
        IEnumerable<Permission> permissions)
    {
        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));
        _participantId = participantId ?? throw new ArgumentNullException(paramName: nameof(participantId));
        _permissions = permissions ?? throw new ArgumentNullException(paramName: nameof(permissions));
    }

    public string Message =>
        $"Budget {_budgetId} already has permission for participant {_participantId}.";

    public bool IsBroken()
    {
        return _permissions.SingleOrDefault(predicate: x => x.ParticipantId == _participantId) is not null;
    }
}