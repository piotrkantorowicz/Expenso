using Expenso.Shared.Domain.Types.Exceptions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.ValueObjects.
    BudgetPermissionRequestStatusTracker;

[TestFixture]
internal sealed class Expire : BudgetPermissionRequestStatusTrackerTestBase
{
    [Test]
    public void Should_SetExpireStatus()
    {
        // Arrange


        // Act
        TestCandidate.Expire();

        // Assert
        TestCandidate
            .Status.Should()
            .Be(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                .Expired);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_AlreadyConfirmed()
    {
        // Arrange
        TestCandidate.Confirm(confirmationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made expired.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Cancelled()
    {
        // Arrange
        TestCandidate.Cancel(cancellationDate: _clockMock.Object.UtcNow);

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made expired.");
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_Expired()
    {
        // Arrange
        TestCandidate.Expire();

        // Act
        Action action = () => TestCandidate.Expire();

        // Assert
        action
            .Should()
            .Throw<DomainRuleValidationException>()
            .WithMessage(expectedWildcardPattern: "Business rule validation failed.")
            .WithDetails(
                expectedWildcardPattern:
                $"Only pending budget permission request {_budgetPermissionRequestId} can be made expired.");
    }
}