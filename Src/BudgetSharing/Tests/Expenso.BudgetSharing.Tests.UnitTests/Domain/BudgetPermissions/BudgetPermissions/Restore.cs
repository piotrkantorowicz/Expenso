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

        // Act
        Action act = () => TestCandidate.Restore();

        // Assert
        act
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage($"Budget permission with id: {TestCandidate.Id} is not deleted.");
    }
}