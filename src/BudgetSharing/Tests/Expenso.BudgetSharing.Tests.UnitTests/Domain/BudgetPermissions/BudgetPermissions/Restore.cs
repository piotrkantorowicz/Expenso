using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

internal sealed class Restore : BudgetPermissionTestBase
{
    [Test]
    public void Should_Restore()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Delete();
        TestCandidate.GetUncommittedChanges();

        // Act
        TestCandidate.Restore();

        // Assert
        TestCandidate.Deletion?.Should().BeNull();

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionRestoredEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetPermissionId: TestCandidate.Id, BudgetId: TestCandidate.BudgetId,
                ParticipantIds: TestCandidate.Permissions.Select(selector: x => x.ParticipantId).ToList().AsReadOnly())
        });
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Deleted()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        Action act = () => TestCandidate.Restore();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: $"Budget permission with id: {TestCandidate.Id} is not deleted");
    }
}