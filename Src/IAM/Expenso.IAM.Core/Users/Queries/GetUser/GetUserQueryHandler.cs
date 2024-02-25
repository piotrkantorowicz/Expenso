using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;
using Expenso.IAM.Core.Users.Services;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Core.Users.Queries.GetUser;

internal sealed class GetUserQueryHandler(IUserService userService) : IQueryHandler<GetUserQuery, GetUserResponse>
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<GetUserResponse?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        (_, string? userId, string? email) = query;

        if (!string.IsNullOrEmpty(userId))
        {
            return await _userService.GetUserByIdAsync(userId);
        }

        if (!string.IsNullOrEmpty(email))
        {
            return await _userService.GetUserByEmailAsync(email);
        }

        throw new NotFoundException($"{nameof(query.UserId)} or {nameof(query.Email)} must be provided.");
    }
}