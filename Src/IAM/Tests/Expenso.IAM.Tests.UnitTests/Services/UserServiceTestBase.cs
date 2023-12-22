using Expenso.IAM.Core.DTO;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Core.Services.Interfaces;

using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Services;

internal abstract class UserServiceTestBase : TestBase
{
    protected User _user = null!;
    protected UserDto _userDto = null!;

    protected IUserService TestCandidate { get; private set; } = null!;

    protected Mock<IKeycloakUserClient> KeycloakUserClientMock { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        KeycloakUserClientMock = AutoFixtureProxy.Freeze<Mock<IKeycloakUserClient>>();
        _user = AutoFixtureProxy.Build<User>().WithAutoProperties().Create();

        _userDto = AutoFixtureProxy
            .Build<UserDto>()
            .With(x => x.UserId, _user.Id)
            .With(x => x.Firstname, _user.FirstName)
            .With(x => x.Lastname, _user.LastName)
            .With(x => x.Email, _user.Email)
            .With(x => x.Username, _user.Username)
            .Create();

        TestCandidate = AutoFixtureProxy.Build<UserService>().WithAutoProperties().Create();
    }
}