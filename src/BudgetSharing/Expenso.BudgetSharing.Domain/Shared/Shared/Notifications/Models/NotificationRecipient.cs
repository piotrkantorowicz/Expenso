using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;

public sealed record NotificationRecipient(string? UserId, string? Email, string? Fullname)
{
    public static NotificationRecipient Empty => new(UserId: null, Email: null, Fullname: null);

    public bool CanSendNotifications => HasRequiredFields;

    public bool CanBeIncludedInNotifications =>
        Fullname.IsAlphaNumericAndSpecialCharactersString(minLength: 3, maxLength: 100, specialCharacters: " ");

    private bool HasRequiredFields => !string.IsNullOrWhiteSpace(value: UserId) && Email.IsValidEmail() &&
                                      CanBeIncludedInNotifications;
}