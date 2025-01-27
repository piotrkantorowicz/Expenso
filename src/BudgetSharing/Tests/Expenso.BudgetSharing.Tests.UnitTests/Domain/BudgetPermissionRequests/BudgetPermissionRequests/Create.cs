using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

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
        TestCandidate.Id.ShouldNotBeNull();
        TestCandidate.BudgetId.ShouldBe(expected: _defaultBudgetId);
        TestCandidate.ParticipantId.ShouldBe(expected: _defaultPersonId);
        TestCandidate.PermissionType.ShouldBe(expected: _defaultPermissionType);
        TestCandidate.StatusTracker.Status.ShouldBe(expected: BudgetPermissionRequestStatus.Pending);

        TestCandidate.StatusTracker.ExpirationDate.Value.ShouldBeCloseTo(
            expected: DateAndTime.New(value: _clockMock.Object.UtcNow.AddDays(days: Expiration)).Value,
            precision: TimeSpan.FromMilliseconds(value: 1000));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                BudgetCode: TestCandidate.BudgetCode, PermissionType: TestCandidate.PermissionType,
                SubmissionDate: TestCandidate.StatusTracker.SubmissionDate)
        ]);
    }

    [Test]
    public async Task Should_ThrowDomainRuleValidationException_When_ExpirationDateIsLessThanOneDay()
    {
        // Arrange
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: new DateTime(year: 2021, month: 1, day: 1));

        // Act
        Func<Task> action = () => Task.FromResult(result: BudgetPermissionRequest.Create(
            budgetPermissionRequestId: _defaultBudgetPermissionId, budgetId: _defaultBudgetId, ownerId: _defaultOwnerId,
            personId: _defaultPersonId, budgetCode: _budgetCode, permissionType: _defaultPermissionType,
            expirationDate: _clockMock.Object.UtcNow.AddDays(days: 0), submissionDate: _clockMock.Object.UtcNow));

        // Assert
        await action.AssertDomainRuleValidationExceptionAsync(
            expectedDetails:
            $"Expiration date {(DateAndTime)_clockMock.Object.UtcNow} must be greater than Submission date: {(DateAndTime)_clockMock.Object.UtcNow}.");
    }
}