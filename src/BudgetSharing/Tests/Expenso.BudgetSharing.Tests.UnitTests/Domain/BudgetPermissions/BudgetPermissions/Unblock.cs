using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

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
        TestCandidate.Blocker?.Should().BeNull();

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionUnblockedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, Permissions: TestCandidate.Permissions.ToList().AsReadOnly())
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Deleted()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () => TestCandidate.Unblock();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(expectedWildcardPattern: $"Budget permission with id: {TestCandidate.Id} is not deleted.");
    }
}