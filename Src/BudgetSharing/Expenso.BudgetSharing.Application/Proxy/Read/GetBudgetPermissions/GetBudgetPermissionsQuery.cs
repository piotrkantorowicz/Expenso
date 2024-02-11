using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Proxy.Read.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    Guid? BudgetPermissionId = null,
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null) : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;