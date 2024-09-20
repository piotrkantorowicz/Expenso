using Expenso.Shared.Tests.Utils.ArchTests;

namespace Expenso.IAM.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : TestBase
{
    private static readonly string[] PublicTypes =
    [
        "DTO"
    ];

    private static readonly string[] NotInternal =
    [
        "Module",
        "Extensions"
    ];

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
        ConditionList? types =
            Types.InAssemblies(assemblies: Assemblies.GetAssemblies()).Should().BeClasses().And().BePublic();

        types = NotInternal.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        types = PublicTypes.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().ResideInNamespaceEndingWith(name: skippedTypeName));

        AssertFailingTypes(result: types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = Types
            .InAssemblies(assemblies: Assemblies.GetAssemblies())
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
        ConditionList? types = Types
            .InAssemblies(assemblies: Assemblies.GetAssemblies())
            .Should()
            .NotBeSealed()
            .And()
            .NotBeAbstract();

        types = NotAbstract.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        AssertFailingTypes(result: types);
    }
}