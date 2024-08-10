using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;

internal sealed class GetUserQueryHandler(IUserService userService) : IQueryHandler<GetUserQuery, GetUserResponse>
{
    private readonly IUserService _userService =
        userService ?? throw new ArgumentNullException(paramName: nameof(userService));

    public async Task<GetUserResponse?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        (_, string? userId, string? email) = query;

        if (!string.IsNullOrEmpty(value: userId))
        {
            return await _userService.GetUserByIdAsync(userId: userId);
        }

        if (!string.IsNullOrEmpty(value: email))
        {
            return await _userService.GetUserByEmailAsync(email: email);
        }

        throw new NotFoundException(message: $"{nameof(query.UserId)} or {nameof(query.Email)} must be provided.");
    }
}