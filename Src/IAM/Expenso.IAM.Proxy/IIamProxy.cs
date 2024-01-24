using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Proxy;

public interface IIamProxy
{
    Task<GetUserInternalResponse?> GetUserByIdAsync(string userId);

    Task<GetUserInternalResponse?> GetUserByEmailAsync(string email);
}
