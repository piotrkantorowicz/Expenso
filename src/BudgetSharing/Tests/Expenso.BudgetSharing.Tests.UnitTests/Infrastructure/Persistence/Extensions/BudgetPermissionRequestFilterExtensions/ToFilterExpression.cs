using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.
    BudgetPermissionRequestFilterExtensions;

[TestFixture]
internal sealed class ToFilterExpression : BudgetPermissionRequestFilterExtensionsTestBase
{
    [TestCase(arg1: nameof(BudgetPermissionRequestFilter.Id), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.BudgetId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.BudgetCode), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.OwnerId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.ParticipantId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.PermissionTypes), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.Statuses), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.Id), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.BudgetId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.BudgetCode), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.OwnerId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.ParticipantId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.PermissionTypes), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.Statuses), arg2: false)]
    public void Should_ReturnExpectedResult_When_FilterPropertyMatches(string propertyName, bool expectedResult)
    {
        // Arrange
        BudgetPermissionRequestFilter filter =
            CreateFilterWithProperty(propertyName: propertyName, expectedResult: expectedResult);

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = filter.ToFilterExpression();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.Should().Be(expected: expectedResult);
    }

    private BudgetPermissionRequestFilter CreateFilterWithProperty(string propertyName, bool expectedResult)
    {
        return propertyName switch
        {
            nameof(BudgetPermissionRequestFilter.Id) when expectedResult => new BudgetPermissionRequestFilter
            {
                Id = _budgetPermissionRequestId
            },
            nameof(BudgetPermissionRequestFilter.Id) when expectedResult is false => new BudgetPermissionRequestFilter
            {
                Id = BudgetPermissionRequestId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionRequestFilter.BudgetId) when expectedResult => new BudgetPermissionRequestFilter
            {
                BudgetId = _budgetId
            },
            nameof(BudgetPermissionRequestFilter.BudgetId) when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    BudgetId = BudgetId.New(value: Guid.NewGuid())
                },
            nameof(BudgetPermissionRequestFilter.BudgetCode) when expectedResult => new BudgetPermissionRequestFilter
            {
                BudgetCode = _budgetCode
            },
            nameof(BudgetPermissionRequestFilter.BudgetCode) when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    BudgetCode = BudgetCode.New(value: "BUDGET_CODE_OTHER")
                },
            nameof(BudgetPermissionRequestFilter.OwnerId)when expectedResult => new BudgetPermissionRequestFilter
            {
                OwnerId = _ownerId
            },
            nameof(BudgetPermissionRequestFilter.OwnerId)when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    OwnerId = PersonId.New(value: Guid.NewGuid())
                },
            nameof(BudgetPermissionRequestFilter.ParticipantId)when expectedResult => new BudgetPermissionRequestFilter
            {
                ParticipantId = _participantId
            },
            nameof(BudgetPermissionRequestFilter.ParticipantId)when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    ParticipantId = PersonId.New(value: Guid.NewGuid())
                },
            nameof(BudgetPermissionRequestFilter.PermissionTypes) when expectedResult => new
                BudgetPermissionRequestFilter
                {
                    PermissionTypes = [_permissionType]
                },
            nameof(BudgetPermissionRequestFilter.PermissionTypes)when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    PermissionTypes = [PermissionType.Owner]
                },
            nameof(BudgetPermissionRequestFilter.Statuses)when expectedResult => new BudgetPermissionRequestFilter
            {
                Statuses = [_status]
            },
            nameof(BudgetPermissionRequestFilter.Statuses)when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    Statuses = [BudgetPermissionRequestStatus.Confirmed]
                },
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(propertyName), actualValue: propertyName,
                message: "Unsupported property.")
        };
    }
}