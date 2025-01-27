using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
internal sealed class Unblock : BudgetPermissionTestBase
{
    [Test]
    public void Should_Restore()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Block();
        TestCandidate.GetUncommittedChanges();

        // Act
        TestCandidate.Unblock();

        // Assert
        TestCandidate.Blocker?.ShouldBeNull();

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionUnblockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetCode: TestCandidate.BudgetCode, OwnerId: TestCandidate.OwnerId,
                Permissions: TestCandidate.Permissions.ToList().AsReadOnly())
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_TryToUnblockAlreadyUnblocked()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action action = () => TestCandidate.Unblock();

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Budget permission with ID {TestCandidate.Id} is not blocked.");
    }
}