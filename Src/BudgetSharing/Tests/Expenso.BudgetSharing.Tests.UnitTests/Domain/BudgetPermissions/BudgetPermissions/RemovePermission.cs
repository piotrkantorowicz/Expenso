using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

internal sealed class RemovePermission : BudgetPermissionTestBase
{
    [Test]
    public void Should_RemovePermission()
    {
        // Arrange
        PersonId participantId = PersonId.New(Guid.NewGuid());
        TestCandidate = CreateTestCandidate();
        TestCandidate.AddPermission(participantId, PermissionType.SubOwner);
        TestCandidate.GetUncommittedChanges();

        // Act
        TestCandidate.RemovePermission(participantId);

        // Assert
        TestCandidate.Permissions.Should().NotContain(x => x.ParticipantId == participantId);

        AssertDomainEventPublished(TestCandidate, new[]
        {
            new BudgetPermissionWithdrawnEvent(MessageContextFactoryMock.Object.Current(), TestCandidate.Id,
                TestCandidate.BudgetId, participantId, PermissionType.SubOwner)
        });
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetDoesNotHavePermissionForParticipant()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(Guid.NewGuid());

        // Act
        Action act = () => TestCandidate.RemovePermission(participantId);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                $"Budget with id: {TestCandidate.BudgetId} does not have permission for provided user with id: {participantId}");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsOwner()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () => TestCandidate.RemovePermission(_defaultPersonId);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage($"Owner permission cannot be removed from budget {TestCandidate.BudgetId}.");
    }
}