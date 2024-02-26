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
        TestCandidate.Status.Should().Be(BudgetPermissionRequestStatus.Confirmed);

        AssertDomainEventPublished([
            new BudgetPermissionRequestConfirmedEvent(MessageContextFactoryMock.Object.Current(),
                TestCandidate.BudgetId, TestCandidate.ParticipantId, TestCandidate.PermissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionRequestIsAlreadyConfirmed()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Confirm();

        // Act
        Action act = () => TestCandidate.Confirm();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage($"Only pending budget permission request {TestCandidate.Id} can be made confirmed.");
    }
}