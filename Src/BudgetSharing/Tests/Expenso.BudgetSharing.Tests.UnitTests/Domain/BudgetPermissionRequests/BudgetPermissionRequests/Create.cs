using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
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
        _clockMock.Setup(x => x.UtcNow).Returns(new DateTime(2021, 1, 1));

        // Act
        TestCandidate = CreateTestCandidate(true);

        // Assert
        TestCandidate.Id.Should().NotBeNull();
        TestCandidate.BudgetId.Should().Be(_defaultBudgetId);
        TestCandidate.ParticipantId.Should().Be(_defaultPersonId);
        TestCandidate.PermissionType.Should().Be(_defaultPermissionType);
        TestCandidate.Status.Should().Be(BudgetPermissionRequestStatus.Pending);
        TestCandidate.ExpirationDate.Should().Be(DateAndTime.New(_clockMock.Object.UtcNow.AddDays(Expiration)));

        AssertDomainEventPublished(TestCandidate, [
            new BudgetPermissionRequestedEvent(MessageContextFactoryMock.Object.Current(), TestCandidate.BudgetId,
                TestCandidate.ParticipantId, TestCandidate.PermissionType)
        ]);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_PermissionTypeIsUnknown()
    {
        // Arrange
        // Act
        DomainRuleValidationException? exception = Assert.Throws<DomainRuleValidationException>(() =>
            BudgetPermissionRequest.Create(_defaultBudgetId, _defaultPersonId, PermissionType.Unknown, Expiration,
                _clockMock.Object));

        // Assert
        string expectedExceptionMessage =
            $"Unknown permission type {PermissionType.Unknown.Value} cannot be processed.";

        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ExpirationDateIsLessThanOneDay()
    {
        // Arrange
        _clockMock.Setup(x => x.UtcNow).Returns(new DateTime(2021, 1, 1));

        // Act
        DomainRuleValidationException? exception = Assert.Throws<DomainRuleValidationException>(() =>
            BudgetPermissionRequest.Create(_defaultBudgetId, _defaultPersonId, _defaultPermissionType, 0,
                _clockMock.Object));

        // Assert
        string expectedExceptionMessage = $"Expiration date {_clockMock.Object.UtcNow} must be greater than one day.";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}