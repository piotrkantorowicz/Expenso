using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

public sealed record PermissionType : Enumeration<PermissionType>
{
    public static readonly PermissionType Unknown = new(0, "Unknown");
    public static readonly PermissionType Owner = new(1, "Owner");
    public static readonly PermissionType SubOwner = new(2, "SubOwner");
    public static readonly PermissionType Reviewer = new(3, "Reviewer");

    private PermissionType(int value, string displayName) : base(value, displayName)
    {
    }

    public bool IsReviewerOrSubOwner()
    {
        return this == Reviewer || this == SubOwner;
    }

    public bool IsOwner()
    {
        return this == Owner;
    }
}
