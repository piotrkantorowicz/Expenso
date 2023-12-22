using Expenso.IAM.Core.DTO;
using Expenso.IAM.Core.Mappings;
using Expenso.IAM.Core.Services.Interfaces;

using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Services;

internal sealed class UserService(
    IKeycloakUserClient keycloakUserClient,
    KeycloakProtectionClientOptions keycloakProtectionClientOptions) : IUserService
{
    private readonly KeycloakProtectionClientOptions _keycloakProtectionClientOptions =
        keycloakProtectionClientOptions ?? throw new ArgumentNullException(nameof(keycloakProtectionClientOptions));

    private readonly IKeycloakUserClient _keycloakUserClient =
        keycloakUserClient ?? throw new ArgumentNullException(nameof(keycloakUserClient));

    public async Task<UserDto?> GetUserById(string userId)
    {
        User keycloakUser = await _keycloakUserClient.GetUser(_keycloakProtectionClientOptions.Realm, userId);
        UserDto? userDto = UserToUserDto.Map(keycloakUser);

        return userDto;
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        List<User> keycloakUsers = (await _keycloakUserClient.GetUsers(_keycloakProtectionClientOptions.Realm,
            new GetUsersRequestParameters
            {
                Email = email
            })).ToList();

        if (keycloakUsers.Count == 0)
        {
            return null;
        }

        UserDto? userDto = UserToUserDto.Map(keycloakUsers.Single());

        return userDto;
    }
}