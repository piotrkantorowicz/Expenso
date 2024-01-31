using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequest.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermissionRequest;

public sealed record GetBudgetPermissionRequestQuery(Guid Id) : IQuery<GetBudgetPermissionRequestResponse>;