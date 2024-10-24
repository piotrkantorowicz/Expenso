using Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;

namespace Expenso.Shared.Tests.ArchTests;

[TestFixture]
internal sealed class AccessModifierTests : ArchTestTestBase
{
    private static readonly string[] NotInternal =
    [
        "TestBase",
        "InMemoryFakeLogger",
        "Assertions"
    ];

    private static readonly string[] NotSealed =
    [
        "TestBase",
        "Program",
        "Exception",
        "RichTestObject"
    ];

    private static readonly string[] NotAbstract =
    [
        "Program",
        "Exception",
        "RichTestObject"
    ];

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternalInTestAssemblies()
    {
        ConditionList? types = NetArchTypes
            .InAssemblies(assemblies: AllAssemblies
                .GetAssembliesCollection()
                .Where(predicate: x => x.FullName?.Contains(value: "Tests") is true))
            .Should()
            .BePublic();

        types = NotInternal.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        AssertFailingTypes(result: types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = NetArchTypes
            .InAssemblies(assemblies: AllAssemblies.GetAssembliesCollection())
            .Should()
            .BeClasses()
            .And()
            .NotBeStatic()
            .And()
            .NotBeAbstract()
            .And()
            .NotBeSealed();

        types = NotSealed.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        AssertFailingTypes(result: types);
    }

    [Test]
    public void Should_Passed_When_AllNotSealedClassesAreAbstract()
    {
        ConditionList? types = NetArchTypes
            .InAssemblies(assemblies: AllAssemblies.GetAssembliesCollection())
            .Should()
            .NotBeSealed()
            .And()
            .NotBeAbstract();

        types = NotAbstract.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        AssertFailingTypes(result: types);
    }
}