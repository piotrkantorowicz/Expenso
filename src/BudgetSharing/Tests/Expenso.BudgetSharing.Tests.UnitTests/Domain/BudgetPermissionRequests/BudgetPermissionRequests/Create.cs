using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.ValueObjects;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

internal sealed class Create : BudgetPermissionRequestTestBase
{
    [Test]
    public void Should_CreateBudgetPermissionRequest()
    {
        // Arrange
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: new DateTime(year: 2021, month: 1, day: 1));

        // Act
        TestCandidate = CreateTestCandidate(emitDomainEvents: true);

        // Assert
        TestCandidate.Id.Should().NotBeNull();
        TestCandidate.BudgetId.Should().Be(expected: _defaultBudgetId);
        TestCandidate.ParticipantId.Should().Be(expected: _defaultPersonId);
        TestCandidate.PermissionType.Should().Be(expected: _defaultPermissionType);
        TestCandidate.StatusTracker.Status.Should().Be(expected: BudgetPermissionRequestStatus.Pending);

        TestCandidate
            .StatusTracker.ExpirationDate.Should()
            .Be(expected: DateAndTime.New(value: _clockMock.Object.UtcNow.AddDays(days: Expiration)));

        AssertDomainEventPublished(aggregateRoot: TestCandidate, expectedDomainEvents:
        [
            new BudgetPermissionRequestedEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                OwnerId: TestCandidate.OwnerId, ParticipantId: TestCandidate.ParticipantId,
                PermissionType: TestCandidate.PermissionType,
                SubmissionDate: TestCandidate.StatusTracker.SubmissionDate)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsUnknown()
    {
        // Arrange
        // Act
        DomainRuleValidationException? exception = Assert.Throws<DomainRuleValidationException>(code: () =>
            BudgetPermissionRequest.Create(budgetId: _defaultBudgetId, ownerId: _defaultOwnerId,
                personId: _defaultPersonId, permissionType: PermissionType.Unknown, expirationDays: Expiration,
                clock: _clockMock.Object));

        // Assert
        string expectedExceptionMessage = $"Unknown permission type {PermissionType.Unknown.Value} cannot be processed";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ExpirationDateIsLessThanOneDay()
    {
        // Arrange
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: new DateTime(year: 2021, month: 1, day: 1));

        // Act
        DomainRuleValidationException? exception = Assert.Throws<DomainRuleValidationException>(code: () =>
            BudgetPermissionRequest.Create(budgetId: _defaultBudgetId, ownerId: _defaultOwnerId,
                personId: _defaultPersonId, permissionType: _defaultPermissionType, expirationDays: 0,
                clock: _clockMock.Object));

        // Assert
        string expectedExceptionMessage = $"Expiration date {_clockMock.Object.UtcNow} must be greater than one day";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}