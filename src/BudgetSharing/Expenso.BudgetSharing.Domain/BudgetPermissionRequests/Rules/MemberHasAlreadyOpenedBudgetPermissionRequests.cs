using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class MemberHasAlreadyOpenedBudgetPermissionRequests : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly IReadOnlyCollection<BudgetPermissionRequest>? _budgetPermissionRequests;
    private readonly PersonId _participantId;
    private readonly PermissionType _permissionType;
    private IEnumerable<BudgetPermissionRequestId> _budgetPermissionRequestIds = [];

    public MemberHasAlreadyOpenedBudgetPermissionRequests(PersonId participantId, BudgetId budgetId,
        PermissionType permissionType, IReadOnlyCollection<BudgetPermissionRequest> budgetPermissionRequests)
    {
        _participantId = participantId;
        _budgetId = budgetId;
        _permissionType = permissionType;
        _budgetPermissionRequests = budgetPermissionRequests;
    }

    public string Message =>
        $"Member has already opened requests {string.Join(separator: ", ", values: _budgetPermissionRequestIds)} for this budget {_budgetId} with same permission {_permissionType}.";

    public bool IsBroken()
    {
        if (_budgetPermissionRequests is null || _budgetPermissionRequests.Count == 0)
        {
            return false;
        }

        if (_permissionType == PermissionType.Owner)
        {
            List<BudgetPermissionRequestId> budgetPermissionRequests = _budgetPermissionRequests
                .Where(predicate: x => x.PermissionType == PermissionType.Owner && x.BudgetId == _budgetId &&
                                       x.ParticipantId == _participantId)
                .Select(selector: x => x.Id)
                .ToList();

            _budgetPermissionRequestIds = budgetPermissionRequests;

            return budgetPermissionRequests.Count > 0;
        }

        if (_permissionType == PermissionType.Reviewer || _permissionType == PermissionType.SubOwner)
        {
            List<BudgetPermissionRequestId> budgetPermissionRequests = _budgetPermissionRequests
                .Where(predicate: x =>
                    (x.PermissionType == PermissionType.Reviewer || x.PermissionType == PermissionType.SubOwner) &&
                    x.BudgetId == _budgetId && x.ParticipantId == _participantId)
                .Select(selector: x => x.Id)
                .ToList();

            _budgetPermissionRequestIds = budgetPermissionRequests;

            return budgetPermissionRequests.Count > 0;
        }

        return false;
    }
}