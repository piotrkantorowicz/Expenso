using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

internal sealed class Block : BudgetPermissionTestBase
{
    [Test]
    public void Should_Delete()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Block();

        // Assert
        TestCandidate.Blocker?.Should().NotBeNull();
        TestCandidate.Blocker?.IsBlocked.Should().BeTrue();

        TestCandidate
            .Blocker?.BlockDate.Should()
            .BeCloseTo(nearbyTime: DateTimeOffset.UtcNow, precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BlockDate: TestCandidate.Blocker!.BlockDate, OwnerId: TestCandidate.OwnerId,
                Permissions: TestCandidate.Permissions.ToList().AsReadOnly())
        });
    }

    [Test]
    public void Should_Delete_When_ClockIsProvided()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Block(clock: _clockMock.Object);

        // Assert
        TestCandidate.Blocker?.Should().NotBeNull();
        TestCandidate.Blocker?.IsBlocked.Should().BeTrue();

        TestCandidate
            .Blocker?.BlockDate.Should()
            .BeCloseTo(nearbyTime: _clockMock.Object.UtcNow, precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, BlockDate: TestCandidate.Blocker!.BlockDate,
                Permissions: TestCandidate.Permissions.ToList().AsReadOnly())
        });
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Deleted()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Block();
        TestCandidate.GetUncommittedChanges();

        // Act
        Action act = () => TestCandidate.Block();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: $"Budget permission with id: {TestCandidate.Id} is already deleted");
    }
}