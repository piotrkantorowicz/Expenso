using System.Linq.Expressions;

using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Expressions.And;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;

public sealed record BudgetPermissionQuerySpecification(
    BudgetPermissionId? Id = null,
    BudgetId? BudgetId = null,
    BudgetCode? BudgetCode = null,
    PersonId? OwnerId = null,
    PersonId? ParticipantId = null,
    PermissionType[]? PermissionTypes = null,
    Paging? Pagination = null)
{
    public Expression<Func<BudgetPermission, bool>> Filter()
    {
        // In expressions, it is necessary to use '==' instead of 'is.' While this creates some inconsistency with the rest of the code, I don't see any viable workarounds.
        Expression<Func<BudgetPermission, bool>> predicate = p => p.Blocker == null || p.Blocker.IsBlocked == false;

        if (Id is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.Id == Id);
        }

        if (BudgetId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetId == BudgetId);
        }

        if (BudgetCode is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetCode == BudgetCode);
        }

        if (OwnerId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.OwnerId == OwnerId);
        }

        if (ParticipantId is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x => x.Permissions.Any(p => p.ParticipantId == PersonId.New(ParticipantId.Value)));
        }

        if (PermissionTypes is not null)
        {
            predicate = AndExpression<BudgetPermission>.And(leftExpression: predicate,
                rightExpression: x =>
                    x.Permissions.Select(y => y.PermissionType).Any(y => PermissionTypes.Contains(y)));
        }

        return predicate;
    }
}