using Expenso.IAM.Core.DTO;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Core.Services;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(string userId);

    Task<UserContract> GetUserByIdInternalAsync(string userId);

    Task<UserDto> GetUserByEmailAsync(string email);

    Task<UserContract> GetUserByEmailInternalAsync(string email);
}