using Expenso.IAM.Core.Users.Services;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Core.Users.Internal.Queries.GetUser;

internal sealed class GetUserInternalQueryHandler(IUserService userService)
    : IQueryHandler<GetUserQuery, GetUserInternalResponse>
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<GetUserInternalResponse?> HandleAsync(GetUserQuery query,
        CancellationToken cancellationToken = default)
    {
        (string? id, string? email) = query;

        if (!string.IsNullOrEmpty(id))
        {
            return await _userService.GetUserByIdInternalAsync(id);
        }

        if (!string.IsNullOrEmpty(email))
        {
            return await _userService.GetUserByEmailInternalAsync(email);
        }

        throw new NotFoundException($"{nameof(query.Id)} or {nameof(query.Email)} must be provided.");
    }
}
