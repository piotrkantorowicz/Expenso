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
        if (!string.IsNullOrEmpty(query.Id))
        {
            return await _userService.GetUserByIdAsync(query.Id);
        }

        if (!string.IsNullOrEmpty(query.Email))
        {
            return await _userService.GetUserByEmailAsync(query.Email);
        }

        throw new NotFoundException($"{nameof(query.Id)} or {nameof(query.Email)} must be provided.");
    }
}