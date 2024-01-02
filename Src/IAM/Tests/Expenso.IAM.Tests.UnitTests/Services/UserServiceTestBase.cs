using Expenso.IAM.Core.DTO;
using Expenso.IAM.Core.Mappings;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Core.Services.Interfaces;

using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Services;

internal abstract class UserServiceTestBase : TestBase
{
    protected IUserService TestCandidate { get; private set; } = null!;

    protected Mock<IKeycloakUserClient> KeycloakUserClientMock { get; private set; } = null!;

    protected User User { get; private set; } = null!;

    protected UserDto UserDto { get; private set; } = null!;

    protected string UserId { get; private set; } = null!;

    protected string UserEmail { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        KeycloakProtectionClientOptions keycloakProtectionClientOptions = new KeycloakProtectionClientOptions();
        KeycloakUserClientMock = new Mock<IKeycloakUserClient>();
        UserId = Guid.NewGuid().ToString();
        UserEmail = "email@email.com";

        User = new User
        {
            Id = UserId,
            FirstName = "Valentina",
            LastName = "Long",
            Username = "vLong",
            Email = UserEmail
        };

        UserDto = UserMap.MapToDto(User)!;
        TestCandidate = new UserService(KeycloakUserClientMock.Object, keycloakProtectionClientOptions);
    }
}