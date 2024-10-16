using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

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
        TestCandidate
            .Permissions.Should()
            .ContainSingle(predicate: x => x.ParticipantId == participantId && x.PermissionType == permissionType);

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: participantId, PermissionType: permissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ParticipantIdIsAlreadyAdded()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () =>
            TestCandidate.AddPermission(participantId: _defaultOwnerId, permissionType: PermissionType.SubOwner);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Budget {TestCandidate.BudgetId} already has permission for participant {_defaultOwnerId}.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsUnknown()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.NewGuid());
        PermissionType permissionType = PermissionType.Unknown;

        // Act
        Action act = () => TestCandidate.AddPermission(participantId: participantId, permissionType: permissionType);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern: $"Unknown permission type {permissionType.Value} cannot be processed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetHasOwnerPermission()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(value: Guid.NewGuid());

        // Act
        Action act = () =>
            TestCandidate.AddPermission(participantId: participantId, permissionType: PermissionType.Owner);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern: $"Budget {TestCandidate.BudgetId} can have only one owner permission.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_TryAssignOtherUserAsOwner()
    {
        // Arrange
        TestCandidate = CreateTestCandidate(createDefaultPermission: false);
        PersonId participantId = PersonId.New(value: Guid.NewGuid());

        // Act
        Action act = () =>
            TestCandidate.AddPermission(participantId: participantId, permissionType: PermissionType.Owner);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Budget {TestCandidate.BudgetId} cannot have owner permission for other user {participantId} that its owner {_defaultOwnerId}.");
    }
}