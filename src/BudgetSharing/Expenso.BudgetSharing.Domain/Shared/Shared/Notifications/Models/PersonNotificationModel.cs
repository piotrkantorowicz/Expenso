using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;

public sealed record PersonNotificationModel(GetUserResponse? Person, bool CanSendNotification);