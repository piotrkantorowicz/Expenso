using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal sealed class Confirm : BudgetPermissionRequestStatusTrackerTestBase
{
    [Test]
    public void Should_SetConfirmedStatus()
    {
        // Arrange
        DateTimeOffset confirmationDate = _clockMock.Object.UtcNow;


        // Act
        TestCandidate.Confirm(confirmationDate: confirmationDate);

        // Assert
        TestCandidate
            .Status.Should()
            .Be(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                .Confirmed);

        TestCandidate.ConfirmationDate.Should().NotBeNull();
        TestCandidate.ConfirmationDate!.Value.Should().Be(expected: confirmationDate);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_AlreadyConfirmed()
    {
        // Arrange
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made confirmed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Cancelled()
    {
        // Arrange
        TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made confirmed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Expired()
    {
        // Arrange
        TestCandidate.Expire();

        // Act
        Action action = () => TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made confirmed.");
    }
}