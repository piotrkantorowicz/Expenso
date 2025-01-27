using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers.DTO.Maps;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

using Moq;

using NUnit.Framework;

using Shouldly;

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
            Id = Guid.CreateVersion7().ToString(),
            FirstName = "Brenda",
            LastName = "Mai",
            Username = "BrendaMai",
            Email = "mai@email.com"
        };

        UserRepresentation[] users =
        [
            _user, secondUser
        ];

        _keycloakUserClientMock
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: users);

        // Act
        IPagedList<GetUsersResponse> getUsers = await TestCandidate.GetUsersAsync(
            request: new GetUsersRequest(UserId: _userId),
            pagination: new Pagination(Page: PaginationDefaults.Page, Limit: PaginationDefaults.Limit),
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsers.ShouldNotBeNull();
        getUsers.CurrentPage.ShouldBe(expected: PaginationDefaults.Page);

        getUsers.TotalPages.ShouldBe(expected: (int)Math.Ceiling(a: users.Length / (double)PaginationDefaults.Limit));
        getUsers.ResultsPerPage.ShouldBe(expected: PaginationDefaults.Limit);
        getUsers.TotalResults.ShouldBe(expected: 2);
        getUsers.Items.Count.ShouldBe(expected: 2);

        IReadOnlyCollection<GetUsersResponse> expectedUsers = new List<GetUsersResponse>
        {
            GetUsersResponseMap.MapTo(user: _user),
            GetUsersResponseMap.MapTo(user: secondUser)
        };

        getUsers.Items.ShouldBeEquivalentTo(expected: expectedUsers);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>(),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }
}