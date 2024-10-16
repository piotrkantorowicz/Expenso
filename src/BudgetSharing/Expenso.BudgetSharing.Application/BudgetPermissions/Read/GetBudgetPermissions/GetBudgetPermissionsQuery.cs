using Expenso.BudgetSharing.Shared.DTO.API.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.GetBudgetPermissions.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    IMessageContext MessageContext,
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionsRequest_PermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;