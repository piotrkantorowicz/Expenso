using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

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
        TestCandidate
            .Status.Should()
            .Be(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                .Cancelled);

        TestCandidate.CancellationDate.Should().NotBeNull();
        TestCandidate.CancellationDate!.Value.Should().Be(expected: cancellationDate);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Confirmed()
    {
        // Arrange
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
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
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
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
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled.");
    }
}