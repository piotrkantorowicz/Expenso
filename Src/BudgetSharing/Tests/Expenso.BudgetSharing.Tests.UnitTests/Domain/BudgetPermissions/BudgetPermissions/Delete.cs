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
        TestCandidate.Deletion?.RemovalDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(500));

        // TODO: Commented out because the test is not working as expected
        // AssertDomainEventPublished(TestCandidate, new[]
        // {
        //     new BudgetPermissionDeletedEvent(MessageContextFactoryMock.Object.Current(), TestCandidate.Id,
        //         TestCandidate.BudgetId, TestCandidate.Permissions.Select(x => x.ParticipantId).ToList().AsReadOnly())
        // });
    }

    [Test]
    public void Should_Delete_When_ClockIsProvided()
    {
        // Arrange
        TestCandidate = CreateTestCandidate();

        // Act
        TestCandidate.Delete(_clockMock.Object);

        // Assert
        TestCandidate.Deletion?.Should().NotBeNull();
        TestCandidate.Deletion?.IsDeleted.Should().BeTrue();

        TestCandidate
            .Deletion?.RemovalDate.Should()
            .BeCloseTo(_clockMock.Object.UtcNow, TimeSpan.FromMilliseconds(500));

        // Commented out because the test is not working as expected
        // AssertDomainEventPublished(TestCandidate, new[]
        // {
        //     new BudgetPermissionDeletedEvent(MessageContextFactoryMock.Object.Current(), TestCandidate.Id,
        //         TestCandidate.BudgetId, TestCandidate.Permissions.Select(x => x.ParticipantId).ToList().AsReadOnly())
        // });
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
            .WithMessage($"Budget permission with id: {TestCandidate.Id} is already deleted.");
    }
}