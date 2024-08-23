namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;

public sealed record UserNotificationModel(
    PersonNotificationModel? Owner,
    IReadOnlyCollection<PersonNotificationModel> Participants);