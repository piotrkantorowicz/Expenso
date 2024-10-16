﻿using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.Domain.Types.ValueObjects;

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
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: 1));

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, clock: _clockMock.Object,
                expirationDate: expirationDate,
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        // Act
        statusTracker.Expire();

        // Assert
        statusTracker
            .Status.Should()
            .Be(expected: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                .Expired);
    }

    [Test]
    public void Should_ThrowDomainRuleValidationException_When_AlreadyConfirmed()
    {
        // Arrange
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: 1));

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, clock: _clockMock.Object,
                expirationDate: expirationDate,
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        statusTracker.Confirm(clock: _clockMock.Object);

        // Act
        Action action = () => statusTracker.Expire();

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
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: 1));

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, clock: _clockMock.Object,
                expirationDate: expirationDate,
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        statusTracker.Cancel(clock: _clockMock.Object);

        // Act
        Action action = () => statusTracker.Expire();

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
        DateTimeOffset currentTime = DateTimeOffset.UtcNow;
        _clockMock.Setup(expression: c => c.UtcNow).Returns(value: currentTime);
        DateAndTime expirationDate = DateAndTime.New(value: currentTime.AddDays(days: 1));

        BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker statusTracker =
            BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatusTracker.Start(
                budgetPermissionRequestId: _budgetPermissionRequestId, clock: _clockMock.Object,
                expirationDate: expirationDate,
                status: BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects.BudgetPermissionRequestStatus
                    .Pending);

        statusTracker.Expire();

        // Act
        Action action = () => statusTracker.Expire();

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