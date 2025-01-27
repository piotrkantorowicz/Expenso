using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal sealed class Expire : BudgetPermissionRequestStatusTrackerTestBase
{
    [Test]
    public void Should_SetExpireStatus()
    {
        // Arrange

        // Act
        TestCandidate.Expire();

        // Assert
        TestCandidate.Status.ShouldBe(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Expired);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_AlreadyConfirmed()
    {
        // Arrange
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Only pending budget permission request {_budgetPermissionRequestId} can be made expired.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Cancelled()
    {
        // Arrange
        TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert

        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Only pending budget permission request {_budgetPermissionRequestId} can be made expired.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Expired()
    {
        // Arrange
        TestCandidate.Expire();

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert

        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Only pending budget permission request {_budgetPermissionRequestId} can be made expired.");
    }
}