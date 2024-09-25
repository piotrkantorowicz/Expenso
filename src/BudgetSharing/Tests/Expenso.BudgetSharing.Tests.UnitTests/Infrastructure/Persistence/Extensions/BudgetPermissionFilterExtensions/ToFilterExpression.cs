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
    public void Should_ReturnTrue_When_IdIsProvided()
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
    public void Should_ReturnFalse_When_WrongIdIsProvided()
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
    public void Should_ReturnTrue_When_BudgetIdIsProvided()
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
    public void Should_ReturnFalse_When_WrongBudgetIdIsProvided()
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
    public void Should_ReturnTrue_When_OwnerIdIsProvided()
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
    public void Should_ReturnFalse_When_WrongOwnerIdIsProvided()
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
    public void Should_ReturnTrue_When_ParticipantIdIsProvided()
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
    public void Should_ReturnFalse_When_WrongParticipantIdIsProvided()
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
    public void Should_ReturnTrue_When_PermissionTypeIsProvided()
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
    public void Should_ReturnFalse_When_WrongPermissionTypeIsProvided()
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
    public void Should_ReturnTrue_When_BlockerIsNull()
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
    public void Should_ReturnTrue_When_BlockerIsNotBlocked()
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
    public void Should_ReturnFalse_When_BlockerIsBlocked()
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