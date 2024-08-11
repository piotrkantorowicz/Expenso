using System.Linq.Expressions;

namespace Expenso.Shared.System.Expressions.And;

public static class AndExpression<T>
{
    public static Expression<Func<T, bool>> And(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        ParameterExpression parameter = Expression.Parameter(type: typeof(T));
        ReplaceExpressionVisitor leftVisitor = new(oldValue: leftExpression.Parameters[index: 0], newValue: parameter);
        Expression left = leftVisitor.Visit(node: leftExpression.Body)!;

        ReplaceExpressionVisitor rightVisitor =
            new(oldValue: rightExpression.Parameters[index: 0], newValue: parameter);

        Expression right = rightVisitor.Visit(node: rightExpression.Body)!;

        return Expression.Lambda<Func<T, bool>>(body: Expression.And(left: left, right: right), parameter);
    }
}