using System.Linq.Expressions;

namespace Expenso.Shared.System.Expressions.And;

public static class AndExpression<T>
{
    public static Expression<Func<T, bool>> And(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));
        ReplaceExpressionVisitor leftVisitor = new ReplaceExpressionVisitor(leftExpression.Parameters[0], parameter);
        Expression left = leftVisitor.Visit(leftExpression.Body)!;
        ReplaceExpressionVisitor rightVisitor = new ReplaceExpressionVisitor(rightExpression.Parameters[0], parameter);
        Expression right = rightVisitor.Visit(rightExpression.Body)!;

        return Expression.Lambda<Func<T, bool>>(Expression.And(left, right), parameter);
    }
}