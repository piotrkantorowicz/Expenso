using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

internal sealed class Confirm : BudgetPermissionRequestTestBase
{
    [Test]
    public void Should_MarkConfirmBudgetPermissionRequestAsConfirmed()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Confirm(clock: _clockMock.Object);

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
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestIsAlreadyConfirmed()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Confirm(clock: _clockMock.Object);

        // Act
        Action action = () => TestCandidate.Confirm(clock: _clockMock.Object);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {TestCandidate.Id} can be made confirmed");
    }
}