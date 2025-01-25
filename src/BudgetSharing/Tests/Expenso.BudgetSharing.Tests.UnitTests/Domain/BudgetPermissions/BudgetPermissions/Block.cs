using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

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
        TestCandidate.Blocker?.ShouldNotBeNull();
        TestCandidate.Blocker?.IsBlocked.ShouldBeTrue();

        TestCandidate.Blocker?.BlockDate!.Value.ShouldBeCloseTo(expected: DateTimeOffset.UtcNow,
            precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BlockDate: DateAndTime.New(
                    value: TestCandidate.Blocker!.BlockDate.GetValueOrDefault(defaultValue: _clockMock.Object.UtcNow)),
                BudgetCode: TestCandidate.BudgetCode,
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
        TestCandidate.Blocker?.ShouldNotBeNull();
        TestCandidate.Blocker?.IsBlocked.ShouldBeTrue();

        TestCandidate.Blocker?.BlockDate!.Value.ShouldBeCloseTo(expected: _clockMock.Object.UtcNow,
            precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionBlockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId,
                BlockDate: DateAndTime.New(
                    value: TestCandidate.Blocker!.BlockDate.GetValueOrDefault(defaultValue: _clockMock.Object.UtcNow)),
                BudgetCode: TestCandidate.BudgetCode,
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
        Action action = () => TestCandidate.Block();

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Budget permission with ID {TestCandidate.Id} is already blocked.");
    }
}