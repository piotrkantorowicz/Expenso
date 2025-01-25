using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

[TestFixture]
internal sealed class Expire : BudgetPermissionRequestTestBase
{
    [Test]
    public void Should_MarkBudgetPermissionRequestAsExpired()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Expire();

        // Assert
        TestCandidate.StatusTracker.Status.ShouldBe(expected: BudgetPermissionRequestStatus.Expired);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestExpiredEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                BudgetCode: TestCandidate.BudgetCode,
                PermissionType: TestCandidate.PermissionType,
                ExpirationDate: TestCandidate.StatusTracker.ExpirationDate)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenCancelled()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Only pending budget permission request {TestCandidate.Id} can be made expired.");
        

    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenConfirmed()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Only pending budget permission request {TestCandidate.Id} can be made expired.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestHasBeenAlreadyExpired()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Expire();

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Only pending budget permission request {TestCandidate.Id} can be made expired.");
    }
}