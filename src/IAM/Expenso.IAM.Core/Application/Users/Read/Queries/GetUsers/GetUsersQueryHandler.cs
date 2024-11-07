using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IReadOnlyCollection<GetUsersResponse>>
{
    private readonly IUserService _userService;

    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(paramName: nameof(userService));
    }

    public async Task<IReadOnlyCollection<GetUsersResponse>?> HandleAsync(GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        return await _userService.GetUsersAsync(request: query.Payload, cancellationToken: cancellationToken);
    }
}