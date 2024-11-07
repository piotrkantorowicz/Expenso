namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;

public sealed record NotificationRecipients(
    NotificationRecipient? Owner,
    IReadOnlyCollection<NotificationRecipient> Participants);