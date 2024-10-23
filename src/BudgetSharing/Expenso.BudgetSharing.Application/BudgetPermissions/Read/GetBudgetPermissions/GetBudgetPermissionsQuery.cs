using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(IMessageContext MessageContext, GetBudgetPermissionsRequest? Payload)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;