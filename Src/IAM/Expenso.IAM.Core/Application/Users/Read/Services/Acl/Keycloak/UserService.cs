using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser.DTO.Response.Maps;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;

internal sealed class UserService(
    IKeycloakUserClient keycloakUserClient,
    KeycloakProtectionClientOptions keycloakProtectionClientOptions) : IUserService
{
    private readonly KeycloakProtectionClientOptions _keycloakProtectionClientOptions =
        keycloakProtectionClientOptions ?? throw new ArgumentNullException(nameof(keycloakProtectionClientOptions));

    private readonly IKeycloakUserClient _keycloakUserClient =
        keycloakUserClient ?? throw new ArgumentNullException(nameof(keycloakUserClient));

    public async Task<GetUserResponse> GetUserByIdAsync(string userId)
    {
        User keycloakUser = await _keycloakUserClient.GetUser(_keycloakProtectionClientOptions.Realm, userId);

        if (keycloakUser is null)
        {
            throw new NotFoundException($"User with id {userId} not found.");
        }

        GetUserResponse getUserResponse = GetUserResponseMap.MapTo(keycloakUser);

        return getUserResponse;
    }

    public async Task<GetUserResponse> GetUserByEmailAsync(string email)
    {
        List<User> keycloakUsers = (await _keycloakUserClient.GetUsers(_keycloakProtectionClientOptions.Realm,
            new GetUsersRequestParameters
            {
                Email = email
            })).ToList();

        User? user = keycloakUsers.Count == 0 ? null : keycloakUsers.Single();

        if (user is null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        GetUserResponse getUserResponse = GetUserResponseMap.MapTo(user);

        return getUserResponse;
    }
}