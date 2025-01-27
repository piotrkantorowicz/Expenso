using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
internal sealed class Create : BudgetPermissionTestBase
{
    [Test]
    public void Should_Create()
    {
        // Arrange
        // Act
        TestCandidate = CreateTestCandidate(emitDomainEvents: true);

        // Assert
        TestCandidate.Id.ShouldBe(expected: _defaultBudgetPermissionId);
        TestCandidate.BudgetId.ShouldBe(expected: _defaultBudgetId);
        TestCandidate.OwnerId.ShouldBe(expected: _defaultOwnerId);

        TestCandidate.Permissions.ShouldContainSingle(predicate: x =>
            x.ParticipantId == _defaultOwnerId && x.PermissionType == PermissionType.Owner);

        TestCandidate.Blocker.ShouldBeNull();

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionGrantedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                BudgetCode: TestCandidate.BudgetCode, OwnerId: TestCandidate.OwnerId,
                ParticipantId: TestCandidate.OwnerId, PermissionType: PermissionType.Owner)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_BudgetPermissionIsNotUnique()
    {
        // Arrange
        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.IsUnique(_defaultBudgetPermissionId, _defaultBudgetId, _defaultOwnerId,
                _budgetCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: false);

        // Act
        Action action = () => CreateTestCandidate(emitDomainEvents: true);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails:
            $"A budget permission must be uniquely identified by its ID {_defaultBudgetPermissionId} and Budget ID {_defaultBudgetId} and combination of Owner ID {_defaultOwnerId} and Budget Code {_budgetCode}.");
    }
}