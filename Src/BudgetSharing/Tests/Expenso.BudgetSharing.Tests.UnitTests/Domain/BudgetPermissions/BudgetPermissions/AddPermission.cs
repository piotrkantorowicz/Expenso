using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

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

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetPermissionId: _defaultBudgetPermissionId, BudgetId: _defaultBudgetId,
                ParticipantId: participantId, PermissionType: permissionType)
        });
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ParticipantIdIsAlreadyAdded()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () =>
            TestCandidate.AddPermission(participantId: _defaultPersonId, permissionType: PermissionType.SubOwner);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Budget {TestCandidate.BudgetId} already has permission for participant {_defaultPersonId}.");
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
            .WithMessage(
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
            .WithMessage(expectedWildcardPattern: new StringBuilder()
                .Append(value: "Budget ")
                .Append(value: TestCandidate.BudgetId)
                .Append(value: " can have only one owner permission.")
                .ToString());
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
            .WithMessage(expectedWildcardPattern: new StringBuilder()
                .Append(value: "Budget ")
                .Append(value: TestCandidate.BudgetId)
                .Append(value: " cannot have owner permission for other user ")
                .Append(value: participantId)
                .Append(value: " that its owner ")
                .Append(value: _defaultPersonId)
                .Append(value: '.')
                .ToString());
    }
}