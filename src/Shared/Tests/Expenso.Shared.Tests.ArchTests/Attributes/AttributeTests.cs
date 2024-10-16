using Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Shared.Tests.ArchTests.Attributes;

[TestFixture]
internal sealed class AttributeTests : ArchTestTestBase
{
    [Test]
    public void Should_Passed_When_AllUnitTestClassesHaveTestFixtureAttribute()
    {
        ConditionList? unitTestTypes = NetArchTypes
            .InAssemblies(assemblies: TestsAssemblies
                .GetAssemblies()
                .Select(selector: x => x.Value)
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
        ConditionList? archTestTypes = NetArchTypes
            .InAssemblies(assemblies: TestsAssemblies
                .GetAssemblies()
                .Select(selector: x => x.Value)
                .Where(predicate: x => x.FullName?.Contains(value: "Arch") is true)
                .ToArray())
            .Should()
            .BeClasses()
            .And()
            .Inherit(type: typeof(ArchTestTestBase))
            .And()
            .NotHaveCustomAttribute(attribute: typeof(TestFixtureAttribute));

        AssertFailingTypes(result: archTestTypes);
    }
}