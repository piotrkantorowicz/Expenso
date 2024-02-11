using System.Text;

using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.Rules;

internal sealed class UnknownPermissionTypeCannotBeProcessed(PermissionType permissionType) : IBusinessRule
{
    private readonly PermissionType _permissionType =
        permissionType ?? throw new ArgumentNullException(nameof(permissionType));

    public string Message => new StringBuilder()
        .Append("Unknown permission type - ")
        .Append(_permissionType)
        .Append(" cannot be processed")
        .ToString();

    public bool IsBroken()
    {
        return _permissionType.IsUnknown();
    }
}