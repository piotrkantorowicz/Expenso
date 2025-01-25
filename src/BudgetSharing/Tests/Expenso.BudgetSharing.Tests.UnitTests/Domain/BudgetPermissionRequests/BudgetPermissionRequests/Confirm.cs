using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

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
        TestCandidate.StatusTracker.Status.ShouldBe(expected: BudgetPermissionRequestStatus.Confirmed);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestConfirmedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                BudgetCode: TestCandidate.BudgetCode,
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
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Only pending budget permission request {TestCandidate.Id} can be made confirmed.");

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
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Only pending budget permission request {TestCandidate.Id} can be made confirmed.");
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
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Only pending budget permission request {TestCandidate.Id} can be made confirmed.");
    }

    [Test]
    public void
        Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestConfirmationDateIsLessOrEqualThanSubmitted()
    {
        // Arrange
        TestCandidate = CreateTestCandidate(delay: 3);
        DateAndTime confirmationDate = _clockMock.Object.UtcNow;

        // Act
        Action action = () => TestCandidate.Confirm(confirmationDate: confirmationDate);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Confirmation date {confirmationDate} must be greater than submission date: {TestCandidate.StatusTracker.SubmissionDate}.");

    }
}