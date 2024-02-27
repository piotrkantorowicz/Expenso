

using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;

namespace Expenso.IAM.Proxy;

public interface IIamProxy
{
    Task<GetUserResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

    Task<GetUserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}