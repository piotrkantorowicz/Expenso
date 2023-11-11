using FluentAssertions;
using NetArchTest.Rules;

namespace Expenso.Shared.Tests.ArchTests.Utils;

public abstract class TestBase
{
    protected void AssertArchTestResult(TestResult result)
    {
        AssertFailingTypes(result.FailingTypes);
    }

    protected void AssertFailingTypes(IEnumerable<Type> types)
    {
        types.Should().BeNullOrEmpty();
    }
}