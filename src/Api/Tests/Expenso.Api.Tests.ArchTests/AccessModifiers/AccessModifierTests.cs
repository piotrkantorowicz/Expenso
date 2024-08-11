using Expenso.Shared.Tests.Utils.ArchTests;

namespace Expenso.Api.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : TestBase
{
    private static readonly string[] NotSealed =
    [
        "TestBase",
        "Program"
    ];

    private static readonly string[] NotAbstract =
    [
        "Program"
    ];

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternal()
    {
        ConditionList? types = Types.InAssemblies(assemblies: Assemblies.ToArray()).Should().BePublic();
        AssertArchTestResult(result: types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = Types
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
        ConditionList? types =
            Types.InAssemblies(assemblies: Assemblies.ToArray()).Should().NotBeSealed().And().NotBeAbstract();

        types = NotAbstract.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        AssertArchTestResult(result: types);
    }
}