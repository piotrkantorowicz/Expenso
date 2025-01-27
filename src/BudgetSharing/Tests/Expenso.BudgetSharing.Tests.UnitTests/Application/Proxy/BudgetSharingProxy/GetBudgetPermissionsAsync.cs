using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Application.Proxy.BudgetSharingProxy;

[TestFixture]
internal sealed class GetBudgetPermissionsAsync : BudgetSharingProxyTestBase
{
    [Test]
    public async Task Should_ReturnBudgetPermissions_When_RequestIsValid()
    {
        // Arrange
        GetBudgetPermissionsRequest request = new();

        IPagedList<GetBudgetPermissionsResponse> response = PagedList<GetBudgetPermissionsResponse>.Create([
                new GetBudgetPermissionsResponse(Id: Guid.CreateVersion7(), BudgetId: Guid.CreateVersion7(),
                    OwnerId: Guid.CreateVersion7(),
                    BudgetCode: "BDGT/997/12/2024", Permissions: new List<GetBudgetPermissionsResponsePermission>
                    {
                        new(ParticipantId: Guid.CreateVersion7(),
                            PermissionType: GetBudgetPermissionsResponsePermissionType.Reviewer)
                    })
            ], currentPage: PaginationDefaults.Page, resultsPerPage: PaginationDefaults.Limit,
            totalPages: PaginationDefaults.Page, totalResults: 1);

        _queryDispatcherMock
            .Setup(expression: q => q.QueryAsync(It.IsAny<GetBudgetPermissionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: response);

        // Act
        IPagedList<GetBudgetPermissionsResponse>? result =
            await TestCandidate.GetBudgetPermissionsAsync(request: request);

        // Assert
        result.ShouldBeEquivalentTo(expected: response);
    }

    [Test]
    public async Task Should_ReturnEmptyPagedList_When_NoResults()
    {
        // Arrange
        GetBudgetPermissionsRequest request = new();

        IPagedList<GetBudgetPermissionsResponse> response = PagedList<GetBudgetPermissionsResponse>.Create(items: [],
            currentPage: 1, resultsPerPage: 25, totalPages: 0, totalResults: 0);

        _queryDispatcherMock
            .Setup(expression: q => q.QueryAsync(It.IsAny<GetBudgetPermissionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: response);

        // Act
        IPagedList<GetBudgetPermissionsResponse>? result =
            await TestCandidate.GetBudgetPermissionsAsync(request: request);

        // Assert
        result?.ShouldNotBeNull();
        result?.TotalResults.ShouldBe(expected: 0);
        result?.Items.ShouldBeEmpty();
    }
}