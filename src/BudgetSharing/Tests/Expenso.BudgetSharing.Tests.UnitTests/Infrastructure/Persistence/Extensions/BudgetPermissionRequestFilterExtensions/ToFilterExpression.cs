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
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.OwnerId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.ParticipantId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.PermissionType), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.Status), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.Id), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.BudgetId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.OwnerId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.ParticipantId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.PermissionType), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestFilter.Status), arg2: false)]
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
            nameof(BudgetPermissionRequestFilter.PermissionType) when expectedResult => new
                BudgetPermissionRequestFilter
                {
                    PermissionType = _permissionType
                },
            nameof(BudgetPermissionRequestFilter.PermissionType)when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    PermissionType = PermissionType.Owner
                },
            nameof(BudgetPermissionRequestFilter.Status)when expectedResult => new BudgetPermissionRequestFilter
            {
                Status = _status
            },
            nameof(BudgetPermissionRequestFilter.Status)when expectedResult is false => new
                BudgetPermissionRequestFilter
                {
                    Status = BudgetPermissionRequestStatus.Confirmed
                },
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(propertyName), actualValue: propertyName,
                message: "Unsupported property.")
        };
    }
}