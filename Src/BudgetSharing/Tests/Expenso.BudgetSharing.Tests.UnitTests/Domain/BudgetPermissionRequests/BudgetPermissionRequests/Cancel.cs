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
        TestCandidate.Status.Should().Be(BudgetPermissionRequestStatus.Cancelled);

        AssertDomainEventPublished([
            new BudgetPermissionRequestCancelledEvent(MessageContextFactoryMock.Object.Current(),
                TestCandidate.BudgetId, TestCandidate.ParticipantId, TestCandidate.PermissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestIsAlreadyCancel()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Cancel();

        // Act
        Action act = () => TestCandidate.Cancel();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage($"Only pending budget permission request {TestCandidate.Id} can be made cancelled.");
    }
}