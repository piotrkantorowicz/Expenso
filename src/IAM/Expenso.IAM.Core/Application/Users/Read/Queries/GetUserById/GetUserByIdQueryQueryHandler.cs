using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;

internal sealed class GetUserByIdQueryQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryQueryHandler(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(paramName: nameof(userService));
    }

    public async Task<GetUserByIdResponse?> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        return await _userService.GetUserByIdAsync(userId: query.Payload?.UserId, cancellationToken: cancellationToken);
    }
}