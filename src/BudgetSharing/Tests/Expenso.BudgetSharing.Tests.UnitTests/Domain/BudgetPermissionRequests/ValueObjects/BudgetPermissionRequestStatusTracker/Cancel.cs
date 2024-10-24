using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal sealed class Cancel : BudgetPermissionRequestStatusTrackerTestBase
{
    [Test]
    public void Should_SetCancelledStatus()
    {
        // Arrange
        DateTimeOffset submissionDate = DateTimeOffset.UtcNow.AddMinutes(minutes: -30);
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        DateTimeOffset cancellationDate = _clockMock.Object.UtcNow;

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, submissionDate: submissionDate,
                expirationDate: submissionDate.AddDays(days: 1),
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        // Act
        statusTracker.Cancel(cancellationDate: cancellationDate);

        // Assert
        statusTracker
            .Status.Should()
            .Be(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                .Cancelled);

        statusTracker.CancellationDate.Should().NotBeNull();
        statusTracker.CancellationDate!.Value.Should().Be(expected: cancellationDate);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Confirmed()
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
        Action action = () => statusTracker.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_AlreadyCancelled()
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
        Action action = () => statusTracker.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled.");
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
        Action action = () => statusTracker.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled.");
    }
}