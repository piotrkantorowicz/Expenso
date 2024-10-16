using FluentAssertions;

using TestCandidate = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatus;

[TestFixture]
internal sealed class IsUnknown : BudgetPermissionRequestStatusTestBase
{
    [Test]
    public void Should_ReturnTrue_When_BudgetPermissionRequestStatusIsPending()
    {
        // Arrange
        TestCandidate testCandidate = TestCandidate.Unknown;

        // Act
        bool result = testCandidate.IsUnknown();

        // Assert
        result.Should().BeTrue();
    }
}