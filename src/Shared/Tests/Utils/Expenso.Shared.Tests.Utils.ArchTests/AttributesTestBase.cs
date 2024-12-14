using System.Reflection;

using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.Utils.ArchTests;

[TestFixture]
public abstract class AttributesTestBase : TestBase
{
    protected abstract IReadOnlyCollection<Assembly> TestClassesAssemblies { get; }

    [Test]
    public void Should_Passed_When_AllUnitTestClassesHaveTestFixtureAttribute()
    {
        ConditionList? unitTestTypes = Types
            .InAssemblies(assemblies: TestClassesAssemblies
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
            .InAssemblies(assemblies: TestClassesAssemblies
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

    [Test]
    public void Should_Passed_When_AllUnitTestClassesContainTearDownMethod()
    {
        IEnumerable<Type>? unitTestTypes = Types
            .InAssemblies(assemblies: TestClassesAssemblies
                .Where(predicate: x => x.FullName?.Contains(value: "Unit") is true)
                .ToArray())
            .Should()
            .BeClasses()
            .And()
            .Inherit(type: typeof(TestBase<>))
            .GetTypes();

        AssertAllClassesContainTearDownMethod(testBaseClasses: unitTestTypes);
    }

    [Test]
    public void Should_Passed_When_AllArchTestClassesContainTearDownMethod()
    {
        IEnumerable<Type>? archTestTypes = Types
            .InAssemblies(assemblies: TestClassesAssemblies
                .Where(predicate: x => x.FullName?.Contains(value: "Arch") is true)
                .ToArray())
            .Should()
            .BeClasses()
            .And()
            .Inherit(type: typeof(TestBase))
            .GetTypes();

        AssertAllClassesContainTearDownMethod(testBaseClasses: archTestTypes);
    }

    private static void AssertAllClassesContainTearDownMethod(IEnumerable<Type> testBaseClasses)
    {
        IList<Type> failingTypes = [];

        foreach (Type testBaseClass in testBaseClasses)
        {
            IEnumerable<MethodInfo> tearDownMethods = testBaseClass
                .GetMethods()
                .Where(predicate: m =>
                    m.GetCustomAttributes(attributeType: typeof(TearDownAttribute), inherit: false).Length != 0);

            if (!tearDownMethods.Any())
            {
                failingTypes.Add(item: testBaseClass);
            }
        }

        AssertFailingTypes(result: failingTypes);
    }
}