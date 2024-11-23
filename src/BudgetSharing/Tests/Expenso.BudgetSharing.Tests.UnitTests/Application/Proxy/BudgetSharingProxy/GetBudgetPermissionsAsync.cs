using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;

using FluentAssertions;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Application.Proxy.BudgetSharingProxy;

[TestFixture]
internal sealed class GetBudgetPermissionsAsync : BudgetSharingProxyTestBase
{
    [Test]
    public async Task Should_ReturnBudgetPermissions_When_RequestIsValid()
    {
        // Arrange
        GetBudgetPermissionsRequest request = new();

        List<GetBudgetPermissionsResponse> response =
        [
            new(Id: Guid.NewGuid(), BudgetId: Guid.NewGuid(), OwnerId: Guid.NewGuid(),
                Permissions: new List<GetBudgetPermissionsResponsePermission>
                {
                    new(ParticipantId: Guid.NewGuid(),
                        PermissionType: GetBudgetPermissionsResponsePermissionType.Reviewer)
                })
        ];

        _queryDispatcherMock
            .Setup(expression: q => q.QueryAsync(It.IsAny<GetBudgetPermissionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: response);

        // Act
        IReadOnlyCollection<GetBudgetPermissionsResponse>? result =
            await TestCandidate.GetBudgetPermissionsAsync(request: request);

        // Assert
        result.Should().BeEquivalentTo(expectation: response);
    }
}