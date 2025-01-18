using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.Queries.Pagination;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;

internal sealed record GetUsersQuery(IMessageContext MessageContext, Pagination? Pagination, GetUsersRequest? Payload)
    : IPagedQuery<IPagedList<GetUsersResponse>>;