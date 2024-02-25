using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    IMessageContext MessageContext,
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;