using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Proxy;

public interface IIamProxy
{
    Task<UserContract?> GetUserByIdAsync(string userId);

    Task<UserContract?> GetUserByEmailAsync(string email);
}