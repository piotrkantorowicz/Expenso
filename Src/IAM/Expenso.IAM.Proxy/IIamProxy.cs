using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Proxy;

public interface IIamProxy
{
    Task<GetUserResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

    Task<GetUserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}