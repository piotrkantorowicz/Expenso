using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.System.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.Api.Tests.E2E.IAM;

internal sealed class FakeIamProxy : IIamProxy
{
    public static readonly string[] ExistingEmails =
        ["MatthewSoto@email.com", "JorgePandey@email.com", "EiIbrahim@email.com"];

    private readonly IReadOnlyCollection<UserRepresentation> _users =
    [
        new()
        {
            Id = UserDataInitializer.UserIds[index: 0].ToString(),
            FirstName = "Sergio",
            LastName = "Huang",
            Username = "SHuang",
            Email = ExistingEmails[0]
        },
        new()
        {
            Id = new Guid(g: "32b61237-4859-4281-8702-6fa3e4c72d67").ToString(),
            FirstName = "Krishna",
            LastName = "Le",
            Username = "KLeee",
            Email = ExistingEmails[1]
        },
        new()
        {
            Id = new Guid(g: "0d53ecf2-cef4-47ca-974a-3f1b395cd2c4").ToString(),
            FirstName = "Vincent",
            LastName = "Ashraf",
            Username = "VAshraf",
            Email = ExistingEmails[2]
        }
    ];

    public async Task<GetUserByIdResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return GetUserByIdResponseMap.MapTo(user: await Task.FromResult(
            result: _users.FirstOrDefault(predicate: x => x.Id == userId) ??
                    throw new NotFoundException(message: $"User with id {userId} not found.")));
    }

    public async Task<GetUserByEmailResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return GetUserByEmailResponseMap.MapTo(user: await Task.FromResult(
            result: _users.FirstOrDefault(predicate: x => x.Email == email) ??
                    throw new NotFoundException(message: $"User with email {email} not found.")));
    }
}