using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

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
        TestCandidate.ShouldNotBeNull();
        TestCandidate.ExpirationDate.ShouldBe(expected: expirationDate);
        TestCandidate.SubmissionDate.Value.ShouldBe(expected: currentTime);

        TestCandidate.Status.ShouldBe(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Pending);

        TestCandidate.ConfirmationDate.ShouldBeNull();
        TestCandidate.CancellationDate.ShouldBeNull();
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
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Budget permission request status must be 'Pending' but was '{status}'.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_ExpirationDateIsLessThanSubmissionDate()
    {
        // Arrange
        DateTimeOffset now = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: now);
        DateAndTime currentTime = DateAndTime.New(value: now);
        DateAndTime expirationDate = DateAndTime.New(value: now.AddDays(days: -1));

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus status =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus.Pending;

        Action action = () =>
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: _clockMock.Object.UtcNow,
                expirationDate: expirationDate, status: status);

        // Assert
        action.AssertDomainRuleValidationException(
            expectedDetails: $"Expiration date {expirationDate} must be greater than Submission date: {currentTime}.");
    }
}