using System.Linq.Expressions;

using Expenso.Shared.System.Expressions.Or;
using Expenso.Shared.Tests.UnitTests.System.Expressions.TestData;

namespace Expenso.Shared.Tests.UnitTests.System.Expressions.OrExpression;

internal sealed class Or
{
    [Test]
    public void OrExpression_CombinesTwoExpressionsWithOr()
    {
        // Arrange
        Expression<Func<TestClass, bool>> leftExpression = testClass => testClass.Value > 5;
        Expression<Func<TestClass, bool>> rightExpression = testClass => testClass.Value < 3;

        // Act
        Expression<Func<TestClass, bool>>
            resultExpression = OrExpression<TestClass>.Or(leftExpression, rightExpression);

        // Assert
        Func<TestClass, bool> compiledExpression = resultExpression.Compile();

        compiledExpression(new TestClass
            {
                Value = 4
            })
            .Should()
            .BeFalse();

        compiledExpression(new TestClass
            {
                Value = 2
            })
            .Should()
            .BeTrue();

        compiledExpression(new TestClass
            {
                Value = 6
            })
            .Should()
            .BeTrue();
    }
}