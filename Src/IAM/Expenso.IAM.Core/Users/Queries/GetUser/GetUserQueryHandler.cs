using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Core.Users.Services;
using Expenso.Shared.Queries;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Core.Users.Queries.GetUser;

internal sealed class GetUserQueryHandler(IUserService userService) : IQueryHandler<GetUserQuery, GetUserResponse>
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<GetUserResponse?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken = default)
    {
        (string? id, string? email) = query;

        if (!string.IsNullOrEmpty(id))
        {
            return await _userService.GetUserByIdAsync(id);
        }

        if (!string.IsNullOrEmpty(email))
        {
            return await _userService.GetUserByEmailAsync(email);
        }

        throw new NotFoundException($"{nameof(query.Id)} or {nameof(query.Email)} must be provided.");
    }
}