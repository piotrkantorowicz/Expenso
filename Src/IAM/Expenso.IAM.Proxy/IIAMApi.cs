using Expenso.IAM.Proxy.DTO;

namespace Expenso.IAM.Proxy;

public interface IIamApi
{
    Task<UserDto?> GetUserById(string userId);

    Task<UserDto?> GetUserByEmail(string email);
}