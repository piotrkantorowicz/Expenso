using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

internal sealed class Delete : BudgetPermissionTestBase
{
    [Test]
    public void Should_Delete()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Delete();

        // Assert
        TestCandidate.Deletion?.Should().NotBeNull();
        TestCandidate.Deletion?.IsDeleted.Should().BeTrue();

        TestCandidate
            .Deletion?.RemovalDate.Should()
            .BeCloseTo(nearbyTime: DateTimeOffset.UtcNow, precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionDeletedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetPermissionId: TestCandidate.Id, BudgetId: TestCandidate.BudgetId,
                ParticipantIds: TestCandidate.Permissions.Select(selector: x => x.ParticipantId).ToList().AsReadOnly())
        });
    }

    [Test]
    public void Should_Delete_When_ClockIsProvided()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Delete(clock: _clockMock.Object);

        // Assert
        TestCandidate.Deletion?.Should().NotBeNull();
        TestCandidate.Deletion?.IsDeleted.Should().BeTrue();

        TestCandidate
            .Deletion?.RemovalDate.Should()
            .BeCloseTo(nearbyTime: _clockMock.Object.UtcNow, precision: TimeSpan.FromMilliseconds(value: 500));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents: new[]
        {
            new BudgetPermissionDeletedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetPermissionId: TestCandidate.Id, BudgetId: TestCandidate.BudgetId,
                ParticipantIds: TestCandidate.Permissions.Select(selector: x => x.ParticipantId).ToList().AsReadOnly())
        });
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Deleted()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();
        TestCandidate.Delete();
        TestCandidate.GetUncommittedChanges();

        // Act
        Action act = () => TestCandidate.Delete();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: $"Budget permission with id: {TestCandidate.Id} is already deleted.");
    }
}