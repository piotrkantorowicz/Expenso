namespace Expenso.Shared.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : ArchTestTestBase
{
    private static readonly string[] NotInternal =
    [
        "TestBase",
        "InMemoryFakeLogger"
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
            .InAssemblies(assemblies: Assemblies
                .ToArray()
                .Where(predicate: x => x.FullName?.Contains(value: "Tests") == true))
            .Should()
            .BePublic();

        types = NotInternal.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        AssertArchTestResult(result: types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = NetArchTypes
            .InAssemblies(assemblies: Assemblies.ToArray())
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

        AssertArchTestResult(result: types);
    }

    [Test]
    public void Should_Passed_When_AllNotSealedClassesAreAbstract()
    {
        ConditionList? types = NetArchTypes
            .InAssemblies(assemblies: Assemblies.ToArray())
            .Should()
            .NotBeSealed()
            .And()
            .NotBeAbstract();

        types = NotAbstract.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        AssertArchTestResult(result: types);
    }
}