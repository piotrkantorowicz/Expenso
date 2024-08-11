using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.Shared.ValueObjects;

public sealed record PermissionType : Enumeration<PermissionType>
{
    public static readonly PermissionType Unknown = new(value: 0, displayName: "Unknown");
    public static readonly PermissionType Owner = new(value: 1, displayName: "Owner");
    public static readonly PermissionType SubOwner = new(value: 2, displayName: "SubOwner");
    public static readonly PermissionType Reviewer = new(value: 3, displayName: "Reviewer");

    private PermissionType(int value, string displayName) : base(Value: value, DisplayName: displayName)
    {
    }

    public static PermissionType Create(int value)
    {
        return FromValue(value: value);
    }

    public bool IsUnknown()
    {
        return this == Unknown;
    }
}