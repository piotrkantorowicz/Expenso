using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;

namespace Expenso.IAM.Core.Users.Services;

public interface IUserService
{
    Task<GetUserResponse> GetUserByIdAsync(string userId);

    Task<GetUserResponse> GetUserByEmailAsync(string email);
}