using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatus;

[TestFixture]
internal sealed class Create : BudgetPermissionRequestStatusTestBase
{
    [Test, TestCase(arg1: 0, arg2: "Unknown"), TestCase(arg1: 1, arg2: "Pending"), TestCase(arg1: 2, arg2: "Confirmed"),
     TestCase(arg1: 3, arg2: "Cancelled"), TestCase(arg1: 4, arg2: "Expired")]
    public void Should_CreateBudgetPermissionRequestStatus(int value, string displayName)
    {
        // Arrange
        // Act
        TestCandidate result = TestCandidate.Create(value: value);

        // Assert
        result.Value.Should().Be(expected: value);
        result.DisplayName.Should().Be(expected: displayName);
    }
}