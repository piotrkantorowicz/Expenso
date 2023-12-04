namespace Expenso.Shared.Tests.Utils.ArchTests;

public abstract class TestBase
{
    protected static void AssertArchTestResult(ConditionList? result)
    {
        AssertFailingTypes(result?.GetTypes());
    }

    private static void AssertFailingTypes(IEnumerable<Type>? result)
    {
        result
            ?.Should()
            .BeNullOrEmpty();
    }
}