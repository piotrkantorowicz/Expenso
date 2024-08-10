using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Rules;

internal sealed class UnknownPermissionTypeCannotBeProcessed(PermissionType permissionType) : IBusinessRule
{
    private readonly PermissionType _permissionType =
        permissionType ?? throw new ArgumentNullException(paramName: nameof(permissionType));

    public string Message => $"Unknown permission type {_permissionType.Value} cannot be processed.";

    public bool IsBroken()
    {
        return _permissionType.IsUnknown();
    }
}