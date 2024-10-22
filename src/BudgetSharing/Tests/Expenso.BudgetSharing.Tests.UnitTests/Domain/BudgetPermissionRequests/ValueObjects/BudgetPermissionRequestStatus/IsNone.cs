using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatus;

[TestFixture]
internal sealed class IsNone : BudgetPermissionRequestStatusTestBase
{
    [Test]
    public void Should_ReturnTrue_When_BudgetPermissionRequestStatusIsPending()
    {
        // Arrange
        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus testCandidate =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.None;

        // Act
        bool result = testCandidate.IsNone();

        // Assert
        result.Should().BeTrue();
    }
}