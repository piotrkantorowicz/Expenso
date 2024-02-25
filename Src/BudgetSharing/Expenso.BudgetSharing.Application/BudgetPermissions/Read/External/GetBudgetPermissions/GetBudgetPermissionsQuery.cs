using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.External.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    IMessageContext MessageContext,
    Guid? BudgetPermissionId = null,
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null) : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;