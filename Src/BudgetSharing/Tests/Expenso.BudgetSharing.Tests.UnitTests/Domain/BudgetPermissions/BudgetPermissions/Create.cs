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
        TestCandidate.Id.Should().Be(_defaultBudgetPermissionId);
        TestCandidate.BudgetId.Should().Be(_defaultBudgetId);
        TestCandidate.OwnerId.Should().Be(_defaultPersonId);

        TestCandidate
            .Permissions.Should()
            .ContainSingle(x => x.ParticipantId == _defaultPersonId && x.PermissionType == PermissionType.Owner);

        TestCandidate.Deletion.Should().BeNull();

        AssertDomainEventPublished(TestCandidate, new[]
        {
            new BudgetPermissionGrantedEvent(MessageContextFactoryMock.Object.Current(), TestCandidate.Id,
                TestCandidate.BudgetId, TestCandidate.OwnerId, PermissionType.Owner)
        });
    }
}