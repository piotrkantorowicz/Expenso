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
        PersonId participantId = PersonId.New(value: Guid.NewGuid());
        TestCandidate = CreateTestCandidate();
        TestCandidate.AddPermission(participantId: participantId, permissionType: PermissionType.SubOwner);
        TestCandidate.GetUncommittedChanges();

        // Act
        TestCandidate.RemovePermission(participantId: participantId);

        // Assert
        TestCandidate.Permissions.Should().NotContain(predicate: x => x.ParticipantId == participantId);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionWithdrawnEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetPermissionId: TestCandidate.Id, BudgetId: TestCandidate.BudgetId, ParticipantId: participantId,
                PermissionType: PermissionType.SubOwner)
        });
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetDoesNotHavePermissionForParticipant()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.NewGuid());

        // Act
        Action act = () => TestCandidate.RemovePermission(participantId: participantId);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Budget with id: {TestCandidate.BudgetId} does not have permission for provided user with id: {participantId}");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsOwner()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () => TestCandidate.RemovePermission(participantId: _defaultPersonId);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern: $"Owner permission cannot be removed from budget {TestCandidate.BudgetId}");
    }
}