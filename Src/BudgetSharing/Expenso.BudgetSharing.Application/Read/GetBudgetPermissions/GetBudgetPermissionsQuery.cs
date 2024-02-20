using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Requests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Responses;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    IMessageContext MessageContext,
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    bool? IncludePermissions = null,
    GetBudgetPermissionsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;