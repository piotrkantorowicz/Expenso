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
        PersonId participantId = PersonId.New(Guid.NewGuid());
        PermissionType permissionType = PermissionType.SubOwner;

        // Act
        TestCandidate.AddPermission(participantId, permissionType);

        // Assert
        TestCandidate
            .Permissions.Should()
            .ContainSingle(x => x.ParticipantId == participantId && x.PermissionType == permissionType);

        AssertDomainEventPublished(TestCandidate, new[]
        {
            new BudgetPermissionGrantedEvent(MessageContextFactoryMock.Object.Current(), _defaultBudgetPermissionId,
                _defaultBudgetId, participantId, permissionType)
        });
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ParticipantIdIsAlreadyAdded()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () => TestCandidate.AddPermission(_defaultPersonId, PermissionType.SubOwner);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage($"Budget {TestCandidate.BudgetId} already has permission for participant {_defaultPersonId}.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsUnknown()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(Guid.NewGuid());
        PermissionType permissionType = PermissionType.Unknown;

        // Act
        Action act = () => TestCandidate.AddPermission(participantId, permissionType);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage($"Unknown permission type {permissionType.Value} cannot be processed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetHasOwnerPermission()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        PersonId participantId = PersonId.New(Guid.NewGuid());

        // Act
        Action act = () => TestCandidate.AddPermission(participantId, PermissionType.Owner);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(new StringBuilder()
                .Append("Budget ")
                .Append(TestCandidate.BudgetId)
                .Append(" can have only one owner permission.")
                .ToString());
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_TryAssignOtherUserAsOwner()
    {
        // Arrange
        TestCandidate = CreateTestCandidate(false);
        PersonId participantId = PersonId.New(Guid.NewGuid());

        // Act
        Action act = () => TestCandidate.AddPermission(participantId, PermissionType.Owner);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(new StringBuilder()
                .Append("Budget ")
                .Append(TestCandidate.BudgetId)
                .Append(" cannot have owner permission for other user ")
                .Append(participantId)
                .Append(" that its owner ")
                .Append(_defaultPersonId)
                .Append('.')
                .ToString());
    }
}