using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Pagination;

using FluentAssertions;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

using Moq;

using NUnit.Framework;

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
        IPagedList<GetUsersResponse> getUsers = await TestCandidate.GetUsersAsync(
            request: new GetUsersRequest(UserId: _userId), pagination: Paging.Default,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsers.Should().NotBeNull();
        getUsers.CurrentPage.Should().Be(expected: Paging.Default.Page);
        getUsers.TotalPages.Should().Be(expected: Paging.Default.Page);
        getUsers.ResultsPerPage.Should().Be(expected: Paging.Default.Limit);
        getUsers.TotalResults.Should().Be(expected: 2);
        getUsers.Items.Should().HaveCount(expected: 2);

        IReadOnlyCollection<GetUsersResponse> expectedUsers = new List<GetUsersResponse>
        {
            GetUsersResponseMap.MapTo(user: _user),
            GetUsersResponseMap.MapTo(user: secondUser)
        };

        getUsers.Items.Should().BeEquivalentTo(expectation: expectedUsers);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>(),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }
}