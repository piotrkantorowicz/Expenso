using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUser.Request;
using Expenso.IAM.Shared.DTO.GetUser.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserResponse>
{
    private readonly IUserService _userService;

    public GetUserQueryHandler(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(paramName: nameof(userService));
    }

    public async Task<GetUserResponse?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        return query.Payload switch
        {
            GetUserByIdRequest request => await _userService.GetUserByIdAsync(userId: request.UserId,
                cancellationToken: cancellationToken),
            GetUserByEmailRequest request => await _userService.GetUserByEmailAsync(email: request.Email,
                cancellationToken: cancellationToken),
            _ => throw new NotFoundException(message: $"User not found. Payload: {query.Payload?.GetType().Name}")
        };
    }
}