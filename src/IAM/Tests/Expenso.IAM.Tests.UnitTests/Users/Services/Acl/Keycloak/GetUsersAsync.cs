using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

[TestFixture]
internal sealed class GetUsersAsync : UserServiceTestBase
{
    [Test]
    public async Task Should_ReturnUsers_When_UsersExists()
    {
        // Arrange
        UserRepresentation secondUser = new()
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Brenda",
            LastName = "Mai",
            Username = "BrendaMai",
            Email = "mai@email.com"
        };

        _keycloakUserClientMock
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value:
            [
                _user, secondUser
            ]);

        // Act
        IReadOnlyCollection<GetUsersResponse> getUsers = await TestCandidate.GetUsersAsync(
            request: new GetUsersRequest(UserId: _userId), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsers.Should().NotBeNull();
        getUsers.Should().HaveCount(expected: 2);

        IReadOnlyCollection<GetUsersResponse> expectedUsers = new List<GetUsersResponse>
        {
            GetUsersResponseMap.MapTo(user: _user),
            GetUsersResponseMap.MapTo(user: secondUser)
        };

        getUsers.Should().BeEquivalentTo(expectation: expectedUsers);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>(),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }
}