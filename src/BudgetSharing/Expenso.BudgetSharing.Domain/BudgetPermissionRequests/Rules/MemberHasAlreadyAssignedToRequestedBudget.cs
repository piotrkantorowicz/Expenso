using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class MemberHasAlreadyAssignedToRequestedBudget : IBusinessRule
{
    private readonly BudgetPermission? _budgetPermission;
    private readonly PersonId _participantId;

    public MemberHasAlreadyAssignedToRequestedBudget(PersonId participantId, BudgetPermission? budgetPermission)
    {
        _participantId = participantId ?? throw new ArgumentNullException(paramName: nameof(participantId));
        _budgetPermission = budgetPermission;
    }

    public string Message =>
        $"Participant {_participantId} has already budget permission for budget {_budgetPermission?.BudgetId}.";

    public bool IsBroken()
    {
        return _budgetPermission is not null && _budgetPermission
            .Permissions.Select(selector: x => x.ParticipantId)
            .Contains(value: _participantId);
    }
}