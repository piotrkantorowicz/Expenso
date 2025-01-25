using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.
    BudgetPermissionRequestFilterExtensions;

[TestFixture]
internal sealed class ToFilterExpression : BudgetPermissionRequestFilterExtensionsTestBase
{
    [TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.Id), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.BudgetId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.BudgetCode), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.OwnerId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.ParticipantId), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.PermissionTypes), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.Statuses), arg2: true),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.Id), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.BudgetId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.BudgetCode), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.OwnerId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.ParticipantId), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.PermissionTypes), arg2: false),
     TestCase(arg1: nameof(BudgetPermissionRequestQuerySpecification.Statuses), arg2: false)]
    public void Should_ReturnExpectedResult_When_FilterPropertyMatches(string propertyName, bool expectedResult)
    {
        // Arrange
        BudgetPermissionRequestQuerySpecification querySpecification =
            CreateFilterWithProperty(propertyName: propertyName, expectedResult: expectedResult);

        // Act
        Expression<Func<BudgetPermissionRequest, bool>> expression = querySpecification.Filter();
        bool result = expression.Compile().Invoke(arg: _budgetPermissionRequest);

        // Assert
        result.ShouldBe(expected: expectedResult);
    }

    private BudgetPermissionRequestQuerySpecification CreateFilterWithProperty(string propertyName, bool expectedResult)
    {
        return propertyName switch
        {
            nameof(BudgetPermissionRequestQuerySpecification.Id) when expectedResult => new
                BudgetPermissionRequestQuerySpecification
            {
                Id = _budgetPermissionRequestId
            },
            nameof(BudgetPermissionRequestQuerySpecification.Id) when expectedResult is false => new
                BudgetPermissionRequestQuerySpecification
            {
                Id = BudgetPermissionRequestId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionRequestQuerySpecification.BudgetId) when expectedResult => new
                BudgetPermissionRequestQuerySpecification
            {
                BudgetId = _budgetId
            },
            nameof(BudgetPermissionRequestQuerySpecification.BudgetId) when expectedResult is false => new
                BudgetPermissionRequestQuerySpecification
                {
                    BudgetId = BudgetId.New(value: Guid.NewGuid())
                },
            nameof(BudgetPermissionRequestQuerySpecification.BudgetCode) when expectedResult => new
                BudgetPermissionRequestQuerySpecification
            {
                BudgetCode = _budgetCode
            },
            nameof(BudgetPermissionRequestQuerySpecification.BudgetCode) when expectedResult is false => new
                BudgetPermissionRequestQuerySpecification
                {
                    BudgetCode = BudgetCode.New(value: "BDGT/151/12/2024")
                },
            nameof(BudgetPermissionRequestQuerySpecification.OwnerId)when expectedResult => new
                BudgetPermissionRequestQuerySpecification
            {
                OwnerId = _ownerId
            },
            nameof(BudgetPermissionRequestQuerySpecification.OwnerId)when expectedResult is false => new
                BudgetPermissionRequestQuerySpecification
                {
                    OwnerId = PersonId.New(value: Guid.NewGuid())
                },
            nameof(BudgetPermissionRequestQuerySpecification.ParticipantId)when expectedResult => new
                BudgetPermissionRequestQuerySpecification
            {
                ParticipantId = _participantId
            },
            nameof(BudgetPermissionRequestQuerySpecification.ParticipantId)when expectedResult is false => new
                BudgetPermissionRequestQuerySpecification
                {
                    ParticipantId = PersonId.New(value: Guid.NewGuid())
                },
            nameof(BudgetPermissionRequestQuerySpecification.PermissionTypes) when expectedResult => new
                BudgetPermissionRequestQuerySpecification
                {
                    PermissionTypes = [_permissionType]
                },
            nameof(BudgetPermissionRequestQuerySpecification.PermissionTypes)when expectedResult is false => new
                BudgetPermissionRequestQuerySpecification
                {
                    PermissionTypes = [PermissionType.Owner]
                },
            nameof(BudgetPermissionRequestQuerySpecification.Statuses)when expectedResult => new
                BudgetPermissionRequestQuerySpecification
            {
                Statuses = [_status]
            },
            nameof(BudgetPermissionRequestQuerySpecification.Statuses)when expectedResult is false => new
                BudgetPermissionRequestQuerySpecification
                {
                    Statuses = [BudgetPermissionRequestStatus.Confirmed]
                },
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(propertyName), actualValue: propertyName,
                message: "Unsupported property.")
        };
    }
}