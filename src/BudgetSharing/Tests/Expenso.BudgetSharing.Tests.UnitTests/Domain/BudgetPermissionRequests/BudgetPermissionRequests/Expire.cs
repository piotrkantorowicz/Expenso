using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

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
        TestCandidate.StatusTracker.Status.Should().Be(expected: BudgetPermissionRequestStatus.Expired);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestExpiredEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                PermissionType: TestCandidate.PermissionType,
                ExpirationDate: TestCandidate.StatusTracker.ExpirationDate)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestIsAlreadyCancel()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Expire();

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Only pending budget permission request {TestCandidate.Id} can be made expired");
    }
}