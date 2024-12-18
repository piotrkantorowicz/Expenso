using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.Queries.Pagination;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;

internal sealed record GetUsersQuery(IMessageContext MessageContext, Paging? Pagination, GetUsersRequest? Payload)
    : IPagedQuery<IPagedList<GetUsersResponse>>;