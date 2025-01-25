using System.Linq.Expressions;

using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Infrastructure.Persistence.Extensions.BudgetPermissionFilterExtensions;

[TestFixture]
internal sealed class ToFilterExpression : BudgetPermissionFilterExtensionsTestBase
{
    [TestCase(arg1: nameof(BudgetPermissionQuerySpecification.BudgetId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.BudgetId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.BudgetId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.BudgetCode), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.BudgetCode), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.BudgetCode), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.Id), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.Id), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.Id), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.OwnerId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.OwnerId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.OwnerId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.ParticipantId), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.ParticipantId), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.ParticipantId), arg2: true, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.PermissionTypes), arg2: false, arg3: true),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.PermissionTypes), arg2: false, arg3: false),
     TestCase(arg1: nameof(BudgetPermissionQuerySpecification.PermissionTypes), arg2: true, arg3: false)]
    public void Should_ReturnExpectedResult_When_FilterPropertyMatches(string propertyName, bool expectedResult,
        bool blocked)
    {
        // Arrange
        BudgetPermissionQuerySpecification querySpecification =
            CreateFilterWithProperty(propertyName: propertyName, expectedResult: expectedResult);

        // Act
        Expression<Func<BudgetPermission, bool>> expression = querySpecification.Filter();
        bool result = expression.Compile().Invoke(arg: _budgetPermission);

        // Assert
        result.ShouldBe(expected: expectedResult);
    }

    private BudgetPermissionQuerySpecification CreateFilterWithProperty(string propertyName, bool expectedResult)
    {
        return propertyName switch
        {
            nameof(BudgetPermissionQuerySpecification.Id) when expectedResult => new BudgetPermissionQuerySpecification
            {
                Id = _budgetPermissionId
            },
            nameof(BudgetPermissionQuerySpecification.Id) when expectedResult is false => new
                BudgetPermissionQuerySpecification
            {
                Id = BudgetPermissionId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionQuerySpecification.BudgetId) when expectedResult => new
                BudgetPermissionQuerySpecification
            {
                BudgetId = _budgetId
            },
            nameof(BudgetPermissionQuerySpecification.BudgetId) when expectedResult is false => new
                BudgetPermissionQuerySpecification
            {
                BudgetId = BudgetId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionQuerySpecification.BudgetCode) when expectedResult => new
                BudgetPermissionQuerySpecification
            {
                BudgetCode = _budgetCode
            },
            nameof(BudgetPermissionQuerySpecification.BudgetCode) when expectedResult is false => new
                BudgetPermissionQuerySpecification
            {
                BudgetCode = BudgetCode.New(value: "BDGT/65/12/2024")
            },
            nameof(BudgetPermissionQuerySpecification.OwnerId)when expectedResult => new
                BudgetPermissionQuerySpecification
            {
                OwnerId = _ownerId
            },
            nameof(BudgetPermissionQuerySpecification.OwnerId)when expectedResult is false => new
                BudgetPermissionQuerySpecification
            {
                OwnerId = PersonId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionQuerySpecification.ParticipantId)when expectedResult => new
                BudgetPermissionQuerySpecification
            {
                ParticipantId = _participantId
            },
            nameof(BudgetPermissionQuerySpecification.ParticipantId)when expectedResult is false => new
                BudgetPermissionQuerySpecification
            {
                ParticipantId = PersonId.New(value: Guid.NewGuid())
            },
            nameof(BudgetPermissionQuerySpecification.PermissionTypes) when expectedResult => new
                BudgetPermissionQuerySpecification
            {
                PermissionTypes = [_permissionType]
            },
            nameof(BudgetPermissionQuerySpecification.PermissionTypes)when expectedResult is false => new
                BudgetPermissionQuerySpecification
            {
                PermissionTypes = [PermissionType.Owner]
            },
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(propertyName), actualValue: propertyName,
                message: "Unsupported property.")
        };
    }
}