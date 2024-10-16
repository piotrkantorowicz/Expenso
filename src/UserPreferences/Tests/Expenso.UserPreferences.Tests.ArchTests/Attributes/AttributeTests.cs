using Expenso.Shared.Tests.Utils.ArchTests;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.UserPreferences.Tests.ArchTests.Attributes;

[TestFixture]
internal sealed class AttributeTests : TestBase
{
    [Test]
    public void Should_Passed_When_AllUnitTestClassesHaveTestFixtureAttribute()
    {
        ConditionList? unitTestTypes = Types
            .InAssemblies(assemblies: Assemblies
                .GetAssemblies()
                .Where(predicate: x => x.FullName?.Contains(value: "Unit") is true)
                .ToArray())
            .Should()
            .BeClasses()
            .And()
            .Inherit(type: typeof(TestBase<>))
            .And()
            .NotHaveCustomAttribute(attribute: typeof(TestFixtureAttribute));

        AssertFailingTypes(result: unitTestTypes);
    }

    [Test]
    public void Should_Passed_When_AllArchTestClassesHaveTestFixtureAttribute()
    {
        ConditionList? archTestTypes = Types
            .InAssemblies(assemblies: Assemblies
                .GetAssemblies()
                .Where(predicate: x => x.FullName?.Contains(value: "Arch") is true)
                .ToArray())
            .Should()
            .BeClasses()
            .And()
            .Inherit(type: typeof(TestBase))
            .And()
            .NotHaveCustomAttribute(attribute: typeof(TestFixtureAttribute));

        AssertFailingTypes(result: archTestTypes);
    }
}