using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.BudgetPermissionFilterExtensions;

[TestFixture]
internal sealed class ToFilterExpression : BudgetPermissionFilterExtensionsTestBase
{
    [TestCase(arg1: nameof(BudgetPermissionFilter.BudgetId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.BudgetId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.BudgetId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.BudgetCode), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.BudgetCode), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.BudgetCode), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.Id), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.Id), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.Id), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.OwnerId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.OwnerId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.OwnerId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.ParticipantId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.ParticipantId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.ParticipantId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.PermissionTypes), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionFilter.PermissionTypes), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionFilter.PermissionTypes), arg2: true, arg3: false)]
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
            nameof(BudgetPermissionFilter.BudgetCode) when expectedResult => new BudgetPermissionFilter
            {
                BudgetCode = _budgetCode
            },
            nameof(BudgetPermissionFilter.BudgetCode) when expectedResult is false => new BudgetPermissionFilter
            {
                BudgetCode = BudgetCode.New(value: "BDGT/65/12/2024")
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
            nameof(BudgetPermissionFilter.PermissionTypes) when expectedResult => new BudgetPermissionFilter
            {
                PermissionTypes = [_permissionType]
            },
            nameof(BudgetPermissionFilter.PermissionTypes)when expectedResult is false => new BudgetPermissionFilter
            {
                PermissionTypes = [PermissionType.Owner]
            },
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(propertyName), actualValue: propertyName,
                message: "Unsupported property.")
        };
    }
}