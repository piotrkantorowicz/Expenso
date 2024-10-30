using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

[TestFixture]
internal sealed class Confirm : BudgetPermissionRequestTestBase
{
    [Test]
    public void Should_MarkConfirmBudgetPermissionRequestAsConfirmed()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Assert
        TestCandidate.StatusTracker.Status.Should().Be(expected: BudgetPermissionRequestStatus.Confirmed);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestConfirmedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                PermissionType: TestCandidate.PermissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenAlreadyConfirmed()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
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
                $"Only pending budget permission request {TestCandidate.Id} can be made confirmed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenCancelled()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
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
                $"Only pending budget permission request {TestCandidate.Id} can be made confirmed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenExpired()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
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
                $"Only pending budget permission request {TestCandidate.Id} can be made confirmed.");
    }

    [Test]
    public void
        Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestConfirmationDateIsLessOrEqualThanSubmitted()
    {
        // Arrange
        TestCandidate = CreateTestCandidate(delay: 3);
        DateTimeOffset confirmationDate = _clockMock.Object.UtcNow;

        // Act
        Action action = () => TestCandidate.Confirm(confirmationDate: confirmationDate);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Confirmation date {confirmationDate} must be greater than submission date: {TestCandidate.StatusTracker.SubmissionDate}.");
    }
}