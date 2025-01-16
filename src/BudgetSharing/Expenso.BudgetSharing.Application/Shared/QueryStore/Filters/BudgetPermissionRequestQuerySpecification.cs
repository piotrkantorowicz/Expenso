using System.Linq.Expressions;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Expressions.And;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;

public sealed record BudgetPermissionRequestQuerySpecification(
    BudgetPermissionRequestId? Id = null,
    BudgetId? BudgetId = null,
    BudgetCode? BudgetCode = null,
    PersonId? ParticipantId = null,
    PersonId? OwnerId = null,
    BudgetPermissionRequestStatus[]? Statuses = null,
    PermissionType[]? PermissionTypes = null,
    Paging? Pagination = null)
{
    public Expression<Func<BudgetPermissionRequest, bool>> Filter()
    {
        Expression<Func<BudgetPermissionRequest, bool>> predicate = p => true;

        if (Id is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.Id == Id);
        }

        if (BudgetId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetId == BudgetId);
        }

        if (BudgetCode is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.BudgetCode == BudgetCode);
        }

        if (ParticipantId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.ParticipantId == ParticipantId);
        }

        if (OwnerId is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => x.OwnerId == OwnerId);
        }

        if (Statuses is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => Statuses.Contains(x.StatusTracker.Status));
        }

        if (PermissionTypes is not null)
        {
            predicate = AndExpression<BudgetPermissionRequest>.And(leftExpression: predicate,
                rightExpression: x => PermissionTypes.Contains(x.PermissionType));
        }

        return predicate;
    }
}