using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatus;

internal sealed class IsPending : BudgetPermissionRequestStatusTestBase
{
    [Test]
    public void Should_ReturnTrue_When_BudgetPermissionRequestStatusIsPending()
    {
        // Arrange
        TestCandidate testCandidate = TestCandidate.Pending;

        // Act
        bool result = testCandidate.IsPending();

        // Assert
        result.Should().BeTrue();
    }
}