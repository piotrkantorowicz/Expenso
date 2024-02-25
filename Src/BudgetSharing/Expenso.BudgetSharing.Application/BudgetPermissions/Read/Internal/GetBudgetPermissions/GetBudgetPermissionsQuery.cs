using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions.DTO.Requests;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions.DTO.Responses;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    IMessageContext MessageContext,
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;