using Expenso.IAM.Proxy.DTO;

namespace Expenso.IAM.Core.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserById(string userId);
    Task<UserDto?> GetUserByEmail(string email);
}