using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
internal sealed class RemovePermission : BudgetPermissionTestBase
{
    [Test]
    public void Should_RemovePermission()
    {
        // Arrange
        PersonId participantId = PersonId.New(value: Guid.CreateVersion7());
        TestCandidate = CreateTestCandidate();
        TestCandidate.AddPermission(participantId: participantId, permissionType: PermissionType.SubOwner);
        TestCandidate.GetUncommittedChanges();

        // Act
        TestCandidate.RemovePermission(participantId: participantId);

        // Assert
        TestCandidate.Permissions.ShouldNotContain(elementPredicate: x => x.ParticipantId == participantId);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionWithdrawnEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetCode: TestCandidate.BudgetCode, OwnerId: TestCandidate.OwnerId, ParticipantId: participantId,
                PermissionType: PermissionType.SubOwner)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetDoesNotHavePermissionForParticipant()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.CreateVersion7());

        // Act
        Action action = () => TestCandidate.RemovePermission(participantId: participantId);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Budget with ID {TestCandidate.BudgetId} does not have permission for provided user with ID {participantId}.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsOwner()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action action = () => TestCandidate.RemovePermission(participantId: _defaultOwnerId);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Owner permission cannot be removed from budget {TestCandidate.BudgetId}.");
    }
}