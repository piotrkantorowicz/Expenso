using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;

using Moq;

using NUnit.Framework;

using Shouldly;

using It = Moq.It;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

[TestFixture]
internal sealed class GetUsersAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: PagedList<GetUsersResponse>.Create(items: _getUsersResponse,
                currentPage: PaginationDefaults.Page, resultsPerPage: PaginationDefaults.Limit,
                totalPages: PaginationDefaults.Page, totalResults: _getUsersResponse.Count));

        // Act
        IPagedList<GetUsersResponse>? getUsersResponse =
            await TestCandidate.GetUsersAsync(request: new GetUsersRequest(),
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsersResponse?.ShouldNotBeNull();
        getUsersResponse?.CurrentPage.ShouldBe(expected: PaginationDefaults.Page);

        getUsersResponse?.TotalPages.ShouldBe(
            expected: (int)Math.Ceiling(a: _getUsersResponse.Count / (double)PaginationDefaults.Limit));

        getUsersResponse?.ResultsPerPage.ShouldBe(expected: PaginationDefaults.Limit);
        getUsersResponse?.TotalResults.ShouldBe(expected: _getUsersResponse.Count);
        getUsersResponse?.Items.ShouldBeEquivalentTo(expected: _getUsersResponse);

        _queryDispatcherMock.Verify(
            expression: x => x.QueryAsync(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        IPagedList<GetUsersResponse>? getUsersResponse =
            await TestCandidate.GetUsersAsync(request: new GetUsersRequest(),
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsersResponse.ShouldBeNull();
    }
}