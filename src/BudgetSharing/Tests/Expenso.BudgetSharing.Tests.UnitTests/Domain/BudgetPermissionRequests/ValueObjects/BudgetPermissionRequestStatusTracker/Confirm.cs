using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal sealed class Confirm : BudgetPermissionRequestStatusTrackerTestBase
{
    [Test]
    public void Should_SetConfirmedStatus()
    {
        // Arrange
        DateTimeOffset submissionDate = DateTimeOffset.UtcNow.AddMinutes(minutes: -30);
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        DateTimeOffset confirmationDate = _clockMock.Object.UtcNow;

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: submissionDate,
                expirationDate: submissionDate.AddDays(days: 1),
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        // Act
        statusTracker.Confirm(confirmationDate: confirmationDate);

        // Assert
        statusTracker
            .Status.Should()
            .Be(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                .Confirmed);

        statusTracker.ConfirmationDate.Should().NotBeNull();
        statusTracker.ConfirmationDate!.Value.Should().Be(expected: confirmationDate);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_AlreadyConfirmed()
    {
        // Arrange
        DateTimeOffset submissionDate = DateTimeOffset.UtcNow.AddMinutes(minutes: -30);
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: submissionDate,
                expirationDate: submissionDate.AddDays(days: 1),
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        statusTracker.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => statusTracker.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made confirmed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Cancelled()
    {
        // Arrange
        DateTimeOffset submissionDate = DateTimeOffset.UtcNow.AddMinutes(minutes: -30);
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: submissionDate,
                expirationDate: submissionDate.AddDays(days: 1),
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        statusTracker.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => statusTracker.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made confirmed.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Expired()
    {
        // Arrange
        DateTimeOffset submissionDate = DateTimeOffset.UtcNow.AddMinutes(minutes: -30);
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: submissionDate,
                expirationDate: submissionDate.AddDays(days: 1),
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        statusTracker.Expire();

        // Act
        Action action = () => statusTracker.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made confirmed.");
    }
}