using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;

internal sealed class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, GetUserByEmailResponse>
{
    private readonly IUserService _userService;

    public GetUserByEmailQueryHandler(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(paramName: nameof(userService));
    }

    public async Task<GetUserByEmailResponse?> HandleAsync(GetUserByEmailQuery query,
        CancellationToken cancellationToken)
    {
        return await _userService.GetUserByEmailAsync(email: query.Payload?.Email,
            cancellationToken: cancellationToken);
    }
}