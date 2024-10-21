using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail.Maps;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById.DTO.Maps;
using Expenso.IAM.Core.Application.Users.Read.Services;
using Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Response;

using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

[TestFixture]
internal abstract class UserServiceTestBase : TestBase<IUserService>
{
    [SetUp]
    public void SetUp()
    {
        _keycloakUserClientMock = new Mock<IKeycloakUserClient>();
        _userId = Guid.NewGuid().ToString();
        _userEmail = "email@email.com";

        _user = new UserRepresentation
        {
            Id = _userId,
            FirstName = "Valentina",
            LastName = "Long",
            Username = "vLong",
            Email = _userEmail
        };

        _getUserByIdResponse = GetUserByIdResponseMap.MapTo(user: _user);
        _getUserByEmailResponse = GetUserByEmailResponseMap.MapTo(user: _user);

        TestCandidate = new UserService(keycloakUserClient: _keycloakUserClientMock.Object,
            keycloakSettings: new KeycloakSettings());
    }

    protected GetUserByIdResponse _getUserByIdResponse = null!;
    protected GetUserByEmailResponse _getUserByEmailResponse = null!;
    protected Mock<IKeycloakUserClient> _keycloakUserClientMock = null!;
    protected UserRepresentation _user = null!;
    protected string _userEmail = null!;
    protected string _userId = null!;
}