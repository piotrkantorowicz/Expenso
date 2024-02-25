using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Proxy;

public interface IIamProxy
{
    Task<GetUserExternalResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

    Task<GetUserExternalResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}