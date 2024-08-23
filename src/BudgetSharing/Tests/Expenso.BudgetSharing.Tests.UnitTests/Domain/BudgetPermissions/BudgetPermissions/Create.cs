using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

internal sealed class Create : BudgetPermissionTestBase
{
    [Test]
    public void Should_Create()
    {
        // Arrange
        // Act
        TestCandidate = CreateTestCandidate(emitDomainEvents: true);

        // Assert
        TestCandidate.Id.Should().Be(expected: _defaultBudgetPermissionId);
        TestCandidate.BudgetId.Should().Be(expected: _defaultBudgetId);
        TestCandidate.OwnerId.Should().Be(expected: _defaultOwnerId);

        TestCandidate
            .Permissions.Should()
            .ContainSingle(predicate: x =>
                x.ParticipantId == _defaultOwnerId && x.PermissionType == PermissionType.Owner);

        TestCandidate.Blocker.Should().BeNull();

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.OwnerId,
                PermissionType: PermissionType.Owner)
        });
    }
}