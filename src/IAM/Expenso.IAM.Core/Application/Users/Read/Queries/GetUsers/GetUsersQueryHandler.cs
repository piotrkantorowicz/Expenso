using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IPagedList<GetUsersResponse>>
{
    private readonly IUserService _userService;

    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(paramName: nameof(userService));
    }

    public async Task<IPagedList<GetUsersResponse>?> HandleAsync(GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        return await _userService.GetUsersAsync(request: query.Payload, pagination: query.Pagination,
            cancellationToken: cancellationToken);
    }
}