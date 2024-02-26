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
        TestCandidate.Status.Should().Be(BudgetPermissionRequestStatus.Expired);

        AssertDomainEventPublished([
            new BudgetPermissionRequestExpiredEvent(MessageContextFactoryMock.Object.Current(), TestCandidate.BudgetId,
                TestCandidate.ParticipantId, TestCandidate.PermissionType, TestCandidate.ExpirationDate)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestIsAlreadyCancel()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Expire();

        // Act
        Action act = () => TestCandidate.Expire();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage($"Only pending budget permission request {TestCandidate.Id} can be made expired.");
    }
}