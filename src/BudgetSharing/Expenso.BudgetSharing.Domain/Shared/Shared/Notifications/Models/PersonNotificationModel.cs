using Expenso.IAM.Shared.DTO.GetUser;

namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;

public sealed record PersonNotificationModel(GetUserResponse? Person, bool CanSendNotification);