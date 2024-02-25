using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.Rules;

internal sealed class UnknownPermissionTypeCannotBeProcessed(PermissionType permissionType) : IBusinessRule
{
    private readonly PermissionType _permissionType =
        permissionType ?? throw new ArgumentNullException(nameof(permissionType));

    public string Message => $"Unknown permission type {_permissionType} cannot be processed.";

    public bool IsBroken()
    {
        return _permissionType.IsUnknown();
    }
}