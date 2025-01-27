using NetArchTest.Rules;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.Utils.ArchTests;

[TestFixture]
public abstract class TestBase
{
    protected static void AssertFailingTypes(ConditionList? result)
    {
        AssertFailingTypes(result: result?.GetTypes());
    }

    protected static void AssertFailingTypes(IEnumerable<Type>? result)
    {
        result?.ShouldBeEmpty();
    }
}