using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
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

    public async Task<GetUserByIdResponse?> GetUserByIdAsync(GetUserByIdRequest request,
        CancellationToken cancellationToken)
    {
        return GetUserByIdResponseMap.MapTo(user: await Task.FromResult(
            result: _users.FirstOrDefault(predicate: x => x.Id == request.UserId) ??
                    throw new NotFoundException(message: $"User with id {request.UserId} not found.")));
    }

    public async Task<GetUserByEmailResponse?> GetUserByEmailAsync(GetUserByEmailRequest request,
        CancellationToken cancellationToken)
    {
        return GetUserByEmailResponseMap.MapTo(user: await Task.FromResult(
            result: _users.FirstOrDefault(predicate: x => x.Email == request.Email) ??
                    throw new NotFoundException(message: $"User with email {request.Email} not found.")));
    }

    public async Task<IReadOnlyCollection<GetUsersResponse>?> GetUsersAsync(GetUsersRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Limit <= 0)
        {
            throw new ArgumentException(message: "Limit cannot be negative or equal to 0",
                paramName: nameof(request.Limit));
        }

        return GetUsersResponseMap.MapTo(user: await Task.FromResult(result: _users
            .Where(predicate: request.Exact ? PredicateExact : PredicateRelative)
            .Take(count: request.Limit ?? int.MaxValue)
            .ToList()));

        bool PredicateRelative(UserRepresentation x)
        {
            return (string.IsNullOrWhiteSpace(value: request.Email) ||
                    x.Email?.Contains(value: request.Email, comparisonType: StringComparison.OrdinalIgnoreCase) ==
                    true) &&
                   (string.IsNullOrWhiteSpace(value: request.Firstname) || x.FirstName?.Contains(
                       value: request.Firstname,
                       comparisonType: StringComparison.OrdinalIgnoreCase) == true) &&
                   (string.IsNullOrWhiteSpace(value: request.Lastname) || x.LastName?.Contains(value: request.Lastname,
                       comparisonType: StringComparison.OrdinalIgnoreCase) == true) &&
                   (string.IsNullOrWhiteSpace(value: request.Username) || x.Username?.Contains(value: request.Username,
                       comparisonType: StringComparison.OrdinalIgnoreCase) == true);
        }

        bool PredicateExact(UserRepresentation x)
        {
            return (string.IsNullOrWhiteSpace(value: request.Email) || string.Equals(a: x.Email, b: request.Email,
                       comparisonType: StringComparison.OrdinalIgnoreCase)) &&
                   (string.IsNullOrWhiteSpace(value: request.Firstname) || string.Equals(a: x.FirstName,
                       b: request.Firstname,
                       comparisonType: StringComparison.OrdinalIgnoreCase)) &&
                   (string.IsNullOrWhiteSpace(value: request.Lastname) || string.Equals(a: x.LastName,
                       b: request.Lastname,
                       comparisonType: StringComparison.OrdinalIgnoreCase)) &&
                   (string.IsNullOrWhiteSpace(value: request.Username) || string.Equals(a: x.Username,
                       b: request.Username,
                       comparisonType: StringComparison.OrdinalIgnoreCase));
        }
    }
}