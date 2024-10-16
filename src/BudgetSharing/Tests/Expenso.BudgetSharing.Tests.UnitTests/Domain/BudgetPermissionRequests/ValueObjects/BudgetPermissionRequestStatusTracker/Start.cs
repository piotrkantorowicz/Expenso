using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.ValueObjects;

using FluentAssertions;

using Status = Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal sealed class Start : BudgetPermissionRequestStatusTrackerTestBase
{
    [Test]
    public void Should_CreateStatusTracker()
    {
        // Arrange
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: 1));
        Status status = Status.Pending;

        // Act
        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker result =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, clock: _clockMock.Object,
                expirationDate: expirationDate, status: status);

        // Assert
        result.Should().NotBeNull();
        result.ExpirationDate.Should().Be(expected: expirationDate);
        result.SubmissionDate.Value.Should().Be(expected: currentTime);
        result.Status.Should().Be(expected: Status.Pending);
        result.ConfirmationDate.Should().BeNull();
        result.CancellationDate.Should().BeNull();
    }

    [Test, TestCase(arg: "Unknown"), TestCase(arg: "Confirmed"), TestCase(arg: "Cancelled"), TestCase(arg: "Expired")]
    public void Should_ThrowDomainRuleValidationException_When_StatusIsOtherThanPending(string statusDisplayName)
    {
        // Arrange
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: 1));
        Status status = Status.FromDisplayName(displayName: statusDisplayName);

        Action action = () =>
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, clock: _clockMock.Object,
                expirationDate: expirationDate, status: status);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern: $"Budget permission request status must be 'Pending' but was '{status}'.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ExpirationDateIsLessThanSubmissionDate()
    {
        // Arrange
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: -1));
        Status status = Status.Pending;

        Action action = () =>
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, clock: _clockMock.Object,
                expirationDate: expirationDate, status: status);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Expiration date {expirationDate.Value} must be greater than Submission date: {currentTime}.");
    }
}