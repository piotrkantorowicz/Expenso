using System.Linq.Expressions;

namespace Expenso.Shared.System.Expressions.Or;

public static class OrExpression<T>
{
    public static Expression<Func<T, bool>> Or(Expression<Func<T, bool>> leftExpression,
        Expression<Func<T, bool>> rightExpression)
    {
        ParameterExpression parameter = Expression.Parameter(type: typeof(T));
        ReplaceExpressionVisitor leftVisitor = new(oldValue: leftExpression.Parameters[index: 0], newValue: parameter);
        Expression left = leftVisitor.Visit(node: leftExpression.Body)!;

        ReplaceExpressionVisitor rightVisitor =
            new(oldValue: rightExpression.Parameters[index: 0], newValue: parameter);

        Expression right = rightVisitor.Visit(node: rightExpression.Body)!;

        return Expression.Lambda<Func<T, bool>>(body: Expression.Or(left: left, right: right), parameter);
    }
}