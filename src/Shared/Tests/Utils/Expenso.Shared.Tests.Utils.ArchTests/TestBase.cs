namespace Expenso.Shared.Tests.Utils.ArchTests;

public abstract class TestBase
{
    protected static void AssertFailingTypes(ConditionList? result)
    {
        AssertFailingTypes(result: result?.GetTypes());
    }

    private static void AssertFailingTypes(IEnumerable<Type>? result)
    {
        result?.Should().BeNullOrEmpty();
    }
}