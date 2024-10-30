using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Rules;

internal sealed class ParticipantPermissionTypeMustHaveValue : IBusinessRule
{
    private readonly PersonId? _participantId;
    private readonly PermissionType? _permissionType;

    public ParticipantPermissionTypeMustHaveValue(PermissionType? permissionType, PersonId? participantId = null)
    {
        _permissionType = permissionType;
        _participantId = participantId;
    }

    public string Message => $"Permission type cannot be empty for participant {_participantId}.";

    public bool IsBroken()
    {
        return _permissionType is null || _permissionType.IsNone();
    }
}