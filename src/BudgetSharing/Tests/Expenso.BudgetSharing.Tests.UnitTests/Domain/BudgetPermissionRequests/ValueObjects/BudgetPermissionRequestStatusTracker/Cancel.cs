using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal sealed class Cancel : BudgetPermissionRequestStatusTrackerTestBase
{
    [Test]
    public void Should_SetCancelledStatus()
    {
        // Arrange
        DateTimeOffset cancellationDate = _clockMock.Object.UtcNow;

        // Act
        TestCandidate.Cancel(cancellationDate: cancellationDate);

        // Assert
        TestCandidate.Status.ShouldBe(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Cancelled);

        TestCandidate.CancellationDate.ShouldNotBeNull();
        TestCandidate.CancellationDate!.Value.ShouldBe(expected: cancellationDate);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Confirmed()
    {
        // Arrange
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_AlreadyCancelled()
    {
        // Arrange
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Expired()
    {
        // Arrange
        TestCandidate.Expire();

        // Act
        Action action = () => TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled.");
    }
}