using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatus;

[TestFixture]
internal sealed class IsUnknown : BudgetPermissionRequestStatusTestBase
{
    [Test]
    public void Should_ReturnTrue_When_BudgetPermissionRequestStatusIsPending()
    {
        // Arrange
        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus testCandidate =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Unknown;

        // Act
        bool result = testCandidate.IsUnknown();

        // Assert
        result.Should().BeTrue();
    }
}