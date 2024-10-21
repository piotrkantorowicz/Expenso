using Expenso.IAM.Shared.DTO.GetUserById.Response;

namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;

// TODO: use other type than proxy response
public sealed record PersonNotificationModel(GetUserByIdResponse? Person, bool CanSendNotification);