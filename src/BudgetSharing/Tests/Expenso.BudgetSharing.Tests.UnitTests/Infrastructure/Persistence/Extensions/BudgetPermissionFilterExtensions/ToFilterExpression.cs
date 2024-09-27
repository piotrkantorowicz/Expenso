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
    [TestCase(arg1: nameof(BudgetPermissionFilter.BudgetId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.BudgetId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.BudgetId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.Id), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.Id), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.Id), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.OwnerId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.OwnerId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.OwnerId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.ParticipantId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.ParticipantId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.ParticipantId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.PermissionType), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.PermissionType), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.PermissionType), arg2: true, arg3: false)]
    public void Should_ReturnExpectedResult_When_FilterPropertyMatches(string propertyName, bool expectedResult,
        bool blocked)
    {
        // Arrange
        BudgetPermissionFilter filter =
            CreateFilterWithProperty(propertyName: propertyName, expectedResult: expectedResult);

        // Act
        Expression<Func<BudgetPermission, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.Should().Be(expected: expectedResult);
    }

    private BudgetPermissionFilter CreateFilterWithProperty(string propertyName, bool expectedResult)
    {
        return propertyName switch
        {
            nameof(BudgetPermissionFilter.Id) when expectedResult => new BudgetPermissionFilter
            {
                Id = _budgetPermissionId
            },
            nameof(BudgetPermissionFilter.Id) when expectedResult is false => new BudgetPermissionFilter
            {
                Id = BudgetPermissionId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionFilter.BudgetId) when expectedResult => new BudgetPermissionFilter
            {
                BudgetId = _budgetId
            },
            nameof(BudgetPermissionFilter.BudgetId) when expectedResult is false => new BudgetPermissionFilter
            {
                BudgetId = BudgetId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionFilter.OwnerId)when expectedResult => new BudgetPermissionFilter
            {
                OwnerId = _ownerId
            },
            nameof(BudgetPermissionFilter.OwnerId)when expectedResult is false => new BudgetPermissionFilter
            {
                OwnerId = PersonId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionFilter.ParticipantId)when expectedResult => new BudgetPermissionFilter
            {
                ParticipantId = _participantId
            },
            nameof(BudgetPermissionFilter.ParticipantId)when expectedResult is false => new BudgetPermissionFilter
            {
                ParticipantId = PersonId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionFilter.PermissionType) when expectedResult => new BudgetPermissionFilter
            {
                PermissionType = _permissionType
            },
            nameof(BudgetPermissionFilter.PermissionType)when expectedResult is false => new BudgetPermissionFilter
            {
                PermissionType = PermissionType.Owner
            },
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(propertyName), actualValue: propertyName,
                message: "Unsupported property.")
        };
    }
}