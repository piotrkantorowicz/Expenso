using Expenso.Shared.Types.Domain;

namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model.ValueObjects;

internal sealed record PermissionType(int Value, string DisplayName) : Enumeration<PermissionType>(Value, DisplayName)
{
    public static readonly PermissionType Unknown = new(0, "Unknown");
    public static readonly PermissionType Owner = new(1, "Owner");
    public static readonly PermissionType SubOwner = new(2, "SubOwner");
    public static readonly PermissionType Reviewer = new(3, "Reviewer");

    public bool IsReviewerOrSubOwner()
    {
        return this == Reviewer || this == SubOwner;
    }

    public bool IsOwner()
    {
        return this == Owner;
    }
}