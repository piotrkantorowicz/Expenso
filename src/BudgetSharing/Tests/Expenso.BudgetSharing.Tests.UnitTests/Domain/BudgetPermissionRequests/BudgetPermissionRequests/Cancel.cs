using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.ValueObjects;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

[TestFixture]
internal sealed class Cancel : BudgetPermissionRequestTestBase
{
    [Test]
    public void Should_MarkBudgetPermissionRequestAsCancelled()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        TestCandidate.StatusTracker.Status.Should().Be(expected: BudgetPermissionRequestStatus.Cancelled);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestCancelledEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                PermissionType: TestCandidate.PermissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenAlreadyCancelled()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {TestCandidate.Id} can be made cancelled.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenConfirmed()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
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
                $"Only pending budget permission request {TestCandidate.Id} can be made cancelled.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenExpired()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
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
                $"Only pending budget permission request {TestCandidate.Id} can be made cancelled.");
    }

    [Test]
    public void
        Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestCancelledDateIsLessOrEqualThanSubmitted()
    {
        // Arrange
        TestCandidate = CreateTestCandidate(delay: 10);
        DateAndTime cancellationDate = _clockMock.Object.UtcNow;

        // Act
        Action action = () => TestCandidate.Cancel(cancellationDate: cancellationDate);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Cancellation date {cancellationDate} must be greater than submission date: {TestCandidate.StatusTracker.SubmissionDate}.");
    }
}