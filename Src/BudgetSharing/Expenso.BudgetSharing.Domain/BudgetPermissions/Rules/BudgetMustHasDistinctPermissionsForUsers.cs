using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetMustHasDistinctPermissionsForUsers(
    BudgetId budgetId,
    PersonId participantId,
    IEnumerable<Permission> permissions) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));
    private readonly PersonId _participantId = participantId ?? throw new ArgumentNullException(nameof(participantId));

    private readonly IEnumerable<Permission> _permissions =
        permissions ?? throw new ArgumentNullException(nameof(permissions));

    public string Message =>
        $"Budget {_budgetId} already has permission for participant {_participantId}.";

    public bool IsBroken()
    {
        return _permissions.SingleOrDefault(x => x.ParticipantId == _participantId) is not null;
    }
}