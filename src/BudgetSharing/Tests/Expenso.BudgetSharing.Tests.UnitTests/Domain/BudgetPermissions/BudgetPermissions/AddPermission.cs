using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
internal sealed class AddPermission : BudgetPermissionTestBase
{
    [Test]
    public void Should_AddPermission()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.NewGuid());
        PermissionType permissionType = PermissionType.SubOwner;

        // Act
        TestCandidate.AddPermission(participantId: participantId, permissionType: permissionType);

        // Assert
        TestCandidate.Permissions.ShouldContainSingle(predicate: x =>
            x.ParticipantId == participantId && x.PermissionType == permissionType);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetCode: TestCandidate.BudgetCode, OwnerId: TestCandidate.OwnerId, ParticipantId: participantId,
                PermissionType: permissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ParticipantIdIsAlreadyAdded()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action action = () =>
            TestCandidate.AddPermission(participantId: _defaultOwnerId, permissionType: PermissionType.SubOwner);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Budget {TestCandidate.BudgetId} already has permission for participant {_defaultOwnerId}.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsNone()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.NewGuid());
        PermissionType permissionType = PermissionType.None;

        // Act
        Action action = () => TestCandidate.AddPermission(participantId: participantId, permissionType: permissionType);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Permission type cannot be empty for participant {participantId}.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsNull()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.NewGuid());
        PermissionType? permissionType = null;

        // Act
        Action action = () => TestCandidate.AddPermission(participantId: participantId, permissionType: permissionType);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Permission type cannot be empty for participant {participantId}.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetHasOwnerPermission()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.NewGuid());

        // Act
        Action action = () =>
            TestCandidate.AddPermission(participantId: participantId, permissionType: PermissionType.Owner);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Budget {TestCandidate.BudgetId} can have only one owner permission.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_TryAssignOtherUserAsOwner()
    {
        // Arrange
        TestCandidate = CreateTestCandidate(createDefaultPermission: false);
        PersonId participantId = PersonId.New(value: Guid.NewGuid());

        // Act
        Action action = () =>
            TestCandidate.AddPermission(participantId: participantId, permissionType: PermissionType.Owner);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"Budget {TestCandidate.BudgetId} cannot have owner permission for other user {participantId} that its owner {_defaultOwnerId}.");
    }
}