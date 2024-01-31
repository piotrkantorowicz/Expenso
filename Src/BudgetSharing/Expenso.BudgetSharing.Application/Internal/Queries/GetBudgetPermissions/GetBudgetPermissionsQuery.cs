using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Internal.Queries.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(Guid? Id = null, Guid? BudgetId = null, Guid? ParticipantId = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;