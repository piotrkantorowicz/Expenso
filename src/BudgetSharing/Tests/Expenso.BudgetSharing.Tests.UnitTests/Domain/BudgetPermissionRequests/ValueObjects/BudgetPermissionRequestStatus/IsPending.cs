using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatus;

[TestFixture]
internal sealed class IsPending : BudgetPermissionRequestStatusTestBase
{
    [Test]
    public void Should_ReturnTrue_When_BudgetPermissionRequestStatusIsPending()
    {
        // Arrange
        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus testCandidate =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Pending;

        // Act
        bool result = testCandidate.IsPending();

        // Assert
        result.Should().BeTrue();
    }
}