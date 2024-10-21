using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Response;

namespace Expenso.IAM.Core.Application.Users.Read.Services;

public interface IUserService
{
    Task<GetUserByIdResponse> GetUserByIdAsync(string? userId, CancellationToken cancellationToken);

    Task<GetUserByEmailResponse> GetUserByEmailAsync(string? email, CancellationToken cancellationToken);
}