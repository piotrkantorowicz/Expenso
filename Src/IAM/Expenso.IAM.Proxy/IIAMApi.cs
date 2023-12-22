using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Proxy;

public interface IIamApi
{
    Task<UserContract?> GetUserById(string userId);

    Task<UserContract?> GetUserByEmail(string email);
}