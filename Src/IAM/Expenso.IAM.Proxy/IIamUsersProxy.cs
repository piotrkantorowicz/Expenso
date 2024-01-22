using Expenso.IAM.Proxy.DTO.GetUser;

namespace Expenso.IAM.Proxy;

public interface IIamUsersProxy
{
    Task<GetUserInternalResponse?> GetUserByIdAsync(string userId);

    Task<GetUserInternalResponse?> GetUserByEmailAsync(string email);
}