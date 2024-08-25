using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

internal sealed class Cancel : BudgetPermissionRequestTestBase
{
    [Test]
    public void Should_MarkBudgetPermissionRequestAsCancelled()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Cancel();

        // Assert
        TestCandidate.Status.Should().Be(expected: BudgetPermissionRequestStatus.Cancelled);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestCancelledEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                PermissionType: TestCandidate.PermissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestIsAlreadyCancel()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Cancel();

        // Act
        DomainRuleValidationException? exception =
            Assert.Throws<DomainRuleValidationException>(code: () => TestCandidate.Cancel());

        // Assert
        string expectedExceptionMessage =
            $"Only pending budget permission request {TestCandidate.Id} can be made cancelled";

        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}