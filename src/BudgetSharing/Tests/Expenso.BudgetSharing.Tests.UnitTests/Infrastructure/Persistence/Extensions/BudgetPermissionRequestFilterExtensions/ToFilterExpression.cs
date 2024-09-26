using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.
    BudgetPermissionRequestFilterExtensions;

internal sealed class ToFilterExpression : BudgetPermissionRequestFilterExtensionsTestBase
{
    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingId()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            Id = _budgetPermissionRequestId
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            Id = BudgetPermissionRequestId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingBudgetId()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            BudgetId = _budgetId
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterBudgetIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            BudgetId = BudgetId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingOwnerId()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            OwnerId = _ownerId
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterOwnerIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            OwnerId = PersonId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingParticipantId()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            ParticipantId = _participantId
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterParticipantIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            ParticipantId = PersonId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingPermissionType()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            PermissionType = _permissionType
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterPermissionTypeDoesNotMatch()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            PermissionType = PermissionType.Owner
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingStatus()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            Status = _status
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterStatusDoesNotMatch()
    {
        // Arrange
        BudgetPermissionRequestFilter filter = new()
        {
            Status = BudgetPermissionRequestStatus.Confirmed
        };

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().BeFalse();
    }
}