using Expenso.IAM.Shared.DTO.GetUser;

namespace Expenso.IAM.Core.Application.Users.Read.Services;

public interface IUserService
{
    Task<GetUserResponse> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

    Task<GetUserResponse> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}