using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatus;

internal sealed class Create : BudgetPermissionRequestStatusTestBase
{
    [Test, TestCase(0, "Unknown"), TestCase(1, "Pending"), TestCase(2, "Confirmed"), TestCase(3, "Cancelled"),
     TestCase(4, "Expired")]
    public void Should_CreateBudgetPermissionRequestStatus(int value, string displayName)
    {
        // Arrange
        // Act
        TestCandidate result = TestCandidate.Create(value);

        // Assert
        result.Value.Should().Be(value);
        result.DisplayName.Should().Be(displayName);
    }
}