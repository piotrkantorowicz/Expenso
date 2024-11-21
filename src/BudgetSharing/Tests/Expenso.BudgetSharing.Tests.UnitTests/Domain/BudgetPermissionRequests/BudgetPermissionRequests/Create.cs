using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.ValueObjects;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

[TestFixture]
internal sealed class Create : BudgetPermissionRequestTestBase
{
    [Test]
    public void Should_CreateBudgetPermissionRequest()
    {
        // Arrange
        // Act
        TestCandidate = CreateTestCandidate(emitDomainEvents: true, delay: 0);

        // Assert
        TestCandidate.Id.Should().NotBeNull();
        TestCandidate.BudgetId.Should().Be(expected: _defaultBudgetId);
        TestCandidate.ParticipantId.Should().Be(expected: _defaultPersonId);
        TestCandidate.PermissionType.Should().Be(expected: _defaultPermissionType);
        TestCandidate.StatusTracker.Status.Should().Be(expected: BudgetPermissionRequestStatus.Pending);

        TestCandidate
            .StatusTracker.ExpirationDate.Value.Should()
            .BeCloseTo(nearbyTime: DateAndTime.New(value: _clockMock.Object.UtcNow.AddDays(days: Expiration)).Value,
                precision: TimeSpan.FromMilliseconds(value: 1000));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                BudgetCode: TestCandidate.BudgetCode,
                PermissionType: TestCandidate.PermissionType,
                SubmissionDate: TestCandidate.StatusTracker.SubmissionDate)
        ]);
    }

    [Test]
    public async Task Should_ThrowDomainRuleValidationException_When_ExpirationDateIsLessThanOneDay()
    {
        // Arrange
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: new DateTime(year: 2021, month: 1, day: 1));

        // Act
        Func<Task> action = () => Task.FromResult(result: BudgetPermissionRequest.Create(budgetId: _defaultBudgetId,
            ownerId: _defaultOwnerId, personId: _defaultPersonId, budgetCode: _budgetCode,
            permissionType: _defaultPermissionType, expirationDate: _clockMock.Object.UtcNow.AddDays(days: 0),
            submissionDate: _clockMock.Object.UtcNow));

        // Assert
        await action
            .Should()
            .ThrowAsync<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetailsAsync(
                expectedWildcardPattern: $"Expiration date {_clockMock.Object.UtcNow} must be greater than one day.");
    }
}