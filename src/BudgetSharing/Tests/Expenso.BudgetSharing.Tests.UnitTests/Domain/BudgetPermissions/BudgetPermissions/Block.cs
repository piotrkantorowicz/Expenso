using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.ValueObjects;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
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

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BlockDate: DateAndTime.New(
                    value: TestCandidate.Blocker!.BlockDate.GetValueOrDefault(defaultValue: _clockMock.Object.UtcNow)),
                OwnerId: TestCandidate.OwnerId, Permissions: TestCandidate.Permissions.ToList().AsReadOnly())
        ]);
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

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId,
                BlockDate: DateAndTime.New(
                    value: TestCandidate.Blocker!.BlockDate.GetValueOrDefault(defaultValue: _clockMock.Object.UtcNow)),
                Permissions: TestCandidate.Permissions.ToList().AsReadOnly())
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_TryToBlockAlreadyBlocked()
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
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(expectedWildcardPattern: $"Budget permission with ID {TestCandidate.Id} is already blocked.");
    }
}