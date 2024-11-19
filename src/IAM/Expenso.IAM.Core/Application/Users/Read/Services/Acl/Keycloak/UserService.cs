using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;

internal sealed class UserService : IUserService
{
    private readonly KeycloakSettings _keycloakSettings;
    private readonly IKeycloakUserClient _keycloakUserClient;

    public UserService(IKeycloakUserClient keycloakUserClient, KeycloakSettings keycloakSettings)
    {
        _keycloakSettings = keycloakSettings ?? throw new ArgumentNullException(paramName: nameof(keycloakSettings));

        _keycloakUserClient =
            keycloakUserClient ?? throw new ArgumentNullException(paramName: nameof(keycloakUserClient));
    }

    public async Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest? request,
        CancellationToken cancellationToken)
    {
        UserRepresentation keycloakUser = await _keycloakUserClient.GetUserAsync(realm: _keycloakSettings.Realm,
            userId: request?.UserId ?? string.Empty, cancellationToken: cancellationToken);

        if (keycloakUser is null)
        {
            throw new NotFoundException(resourceName: "User", identifierType: IdentifierType.PrimaryId(),
                identifier: request?.UserId);
        }

        GetUserByIdResponse getUserResponse = GetUserByIdResponseMap.MapTo(user: keycloakUser);

        return getUserResponse;
    }

    public async Task<GetUserByEmailResponse> GetUserByEmailAsync(GetUserByEmailRequest? request,
        CancellationToken cancellationToken)
    {
        List<UserRepresentation> keycloakUsers = (await _keycloakUserClient.GetUsersAsync(
            realm: _keycloakSettings.Realm, parameters: new GetUsersRequestParameters
            {
                Email = request?.Email
            }, cancellationToken: cancellationToken)).ToList();

        if (keycloakUsers.Count > 1)
        {
            throw ConflictException.MultipleRecordsFound(resourceName: "User", identifierType: IdentifierType.Email(),
                identifier: request?.Email);
        }
        
        UserRepresentation? user = keycloakUsers.Count is 0 ? null : keycloakUsers.Single();
        
        if (user is null)
        {
            throw new NotFoundException(resourceName: "User", identifierType: IdentifierType.Email(),
                identifier: request?.Email);
        }

        GetUserByEmailResponse getUserResponse = GetUserByEmailResponseMap.MapTo(user: user);

        return getUserResponse;
    }

    public async Task<IReadOnlyCollection<GetUsersResponse>> GetUsersAsync(GetUsersRequest? request,
        CancellationToken cancellationToken)
    {
        List<UserRepresentation> keycloakUsers = (await _keycloakUserClient.GetUsersAsync(
            realm: _keycloakSettings.Realm, parameters: GetUsersRequestMap.MapTo(request: request),
            cancellationToken: cancellationToken)).ToList();

        return GetUsersResponseMap.MapTo(users: keycloakUsers);
    }
}