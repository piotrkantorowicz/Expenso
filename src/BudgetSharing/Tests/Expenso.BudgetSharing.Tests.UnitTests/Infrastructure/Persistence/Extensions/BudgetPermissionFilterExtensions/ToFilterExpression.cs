using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.BudgetPermissionFilterExtensions;

internal sealed class ToFilterExpression : BudgetPermissionFilterExtensionsTestBase
{
    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingId()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            Id = _budgetPermissionId
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            Id = BudgetPermissionId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingBudgetId()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            BudgetId = _budgetId
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterBudgetIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            BudgetId = BudgetId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingOwnerId()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            OwnerId = _ownerId
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterOwnerIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            OwnerId = PersonId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingParticipantId()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            ParticipantId = _participantId
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterParticipantIdDoesNotMatch()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            ParticipantId = PersonId.New(value: Guid.NewGuid())
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_FilterHasMatchingPermissionType()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            PermissionType = _permissionType
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_FilterPermissionTypeDoesNotMatch()
    {
        // Arrange
        BudgetPermissionFilter filter = new()
        {
            PermissionType = PermissionType.Owner
        };

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnTrue_When_NoBlockerIsPresent()
    {
        // Arrange
        BudgetPermissionFilter filter = new();

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnTrue_When_BlockerIsNotBlocking()
    {
        // Arrange
        BudgetPermissionFilter filter = new();

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_BlockerIsBlocking()
    {
        // Arrange
        BudgetPermissionFilter filter = new();
        _budgetPermission.Block();

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().BeFalse();
    }
}