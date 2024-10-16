using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
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

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionWithdrawnEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: participantId, PermissionType: PermissionType.SubOwner)
        ]);
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
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Budget with ID {TestCandidate.BudgetId} does not have permission for provided user with ID {participantId}.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsOwner()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () => TestCandidate.RemovePermission(participantId: _defaultOwnerId);

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern: $"Owner permission cannot be removed from budget {TestCandidate.BudgetId}.");
    }
}