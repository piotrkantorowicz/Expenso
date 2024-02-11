using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

public sealed record PermissionType : Enumeration<PermissionType>
{
    public static readonly PermissionType Unknown = new(0, "Unknown");
    public static readonly PermissionType Owner = new(1, "Owner");
    public static readonly PermissionType SubOwner = new(2, "SubOwner");
    public static readonly PermissionType Reviewer = new(3, "Reviewer");

    // Required for EF Core
    // ReSharper disable once UnusedMember.Local
    private PermissionType() : this(default, default!)
    {
    }

    private PermissionType(int value, string displayName) : base(value, displayName)
    {
    }

    private PermissionType(int value) : base(value, FromValue(value).DisplayName)
    {
    }

    public static PermissionType Create(int value)
    {
        return new PermissionType(value);
    }

    public bool IsUnknown()
    {
        return this == Unknown;
    }
}