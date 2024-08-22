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
        TestCandidate.Confirm();

        // Assert
        TestCandidate.Status.Should().Be(expected: BudgetPermissionRequestStatus.Confirmed);

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
        TestCandidate.Confirm();

        // Act
        DomainRuleValidationException? exception =
            Assert.Throws<DomainRuleValidationException>(code: () => TestCandidate.Confirm());

        // Assert
        string expectedExceptionMessage =
            $"Only pending budget permission request {TestCandidate.Id} can be made confirmed";

        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}