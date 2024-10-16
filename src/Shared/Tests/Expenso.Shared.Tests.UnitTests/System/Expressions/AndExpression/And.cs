using System.Linq.Expressions;

using Expenso.Shared.System.Expressions.And;
using Expenso.Shared.Tests.UnitTests.System.Expressions.TestData;

namespace Expenso.Shared.Tests.UnitTests.System.Expressions.AndExpression;

[TestFixture]
internal sealed class And
{
    [Test]
    public void AndExpression_CombinesTwoExpressionsWithAnd()
    {
        // Arrange
        Expression<Func<TestClass, bool>> leftExpression = testClass => testClass.Value > 5;
        Expression<Func<TestClass, bool>> rightExpression = testClass => testClass.Value < 10;

        // Act
        Expression<Func<TestClass, bool>> resultExpression =
            AndExpression<TestClass>.And(leftExpression: leftExpression, rightExpression: rightExpression);

        // Assert
        Func<TestClass, bool> compiledExpression = resultExpression.Compile();

        compiledExpression(arg: new TestClass
            {
                Value = 4
            })
            .Should()
            .BeFalse();

        compiledExpression(arg: new TestClass
            {
                Value = 7
            })
            .Should()
            .BeTrue();

        compiledExpression(arg: new TestClass
            {
                Value = 11
            })
            .Should()
            .BeFalse();
    }
}