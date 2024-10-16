using NUnit.Framework;

namespace Expenso.Shared.Tests.Utils.ArchTests;

[TestFixture]
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