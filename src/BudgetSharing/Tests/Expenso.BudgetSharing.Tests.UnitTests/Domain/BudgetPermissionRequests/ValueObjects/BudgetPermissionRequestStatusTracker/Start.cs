using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.ValueObjects;

using FluentAssertions;

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

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus status =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Pending;

        // Act
        TestCandidate =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: _clockMock.Object.UtcNow,
                expirationDate: expirationDate, status: status);

        // Assert
        TestCandidate.Should().NotBeNull();
        TestCandidate.ExpirationDate.Should().Be(expected: expirationDate);
        TestCandidate.SubmissionDate.Value.Should().Be(expected: currentTime);

        TestCandidate
            .Status.Should()
            .Be(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                .Pending);

        TestCandidate.ConfirmationDate.Should().BeNull();
        TestCandidate.CancellationDate.Should().BeNull();
    }

    [Test, TestCase(arg: "None"), TestCase(arg: "Confirmed"), TestCase(arg: "Cancelled"), TestCase(arg: "Expired")]
    public void Should_ThrowDomainRuleValidationException_When_StatusIsOtherThanPending(string statusDisplayName)
    {
        // Arrange
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: 1));

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus status =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.FromDisplayName(
                displayName: statusDisplayName);

        Action action = () =>
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: _clockMock.Object.UtcNow,
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

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus status =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Pending;

        Action action = () =>
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: _clockMock.Object.UtcNow,
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