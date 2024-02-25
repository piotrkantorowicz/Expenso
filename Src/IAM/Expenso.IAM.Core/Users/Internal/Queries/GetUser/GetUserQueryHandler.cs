using Expenso.IAM.Core.Users.Services;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Core.Users.Internal.Queries.GetUser;

internal sealed class GetUserInternalQueryHandler(IUserService userService)
    : IQueryHandler<GetUserQuery, GetUserInternalResponse>
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<GetUserInternalResponse?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        (_, string? userId, string? email) = query;

        if (!string.IsNullOrEmpty(userId))
        {
            return await _userService.GetUserByIdInternalAsync(userId);
        }

        if (!string.IsNullOrEmpty(email))
        {
            return await _userService.GetUserByEmailInternalAsync(email);
        }

        throw new NotFoundException($"{nameof(query.UserId)} or {nameof(query.Email)} must be provided.");
    }
}