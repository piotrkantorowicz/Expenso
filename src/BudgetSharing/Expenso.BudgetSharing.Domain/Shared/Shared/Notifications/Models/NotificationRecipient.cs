namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;

public sealed record NotificationRecipient(string? UserId, string? Email, string? Fullname)
{
    public static NotificationRecipient Empty => new(UserId: null, Email: null, Fullname: null);

    public bool CanSendNotification => HasValue;

    public bool HasValue => !string.IsNullOrWhiteSpace(value: UserId) && !string.IsNullOrWhiteSpace(value: Email) &&
                            !string.IsNullOrWhiteSpace(value: Fullname);
}