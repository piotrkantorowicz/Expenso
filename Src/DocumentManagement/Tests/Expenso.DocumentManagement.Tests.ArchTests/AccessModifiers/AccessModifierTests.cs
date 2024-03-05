using Expenso.Shared.Tests.Utils.ArchTests;

using NetArchTest.Rules;

namespace Expenso.DocumentManagement.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : TestBase
{
    private static readonly string[] PublicTypes =
    [
        "DTO"
    ];

    private static readonly string[] NotInternal =
    [
        "Module"
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
        ConditionList? types = Types.InAssemblies(Assemblies.ToArray()).Should().BePublic();

        types = NotInternal.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));

        types = PublicTypes.Aggregate(types,
            (current, skippedTypeName) => current.And().ResideInNamespaceEndingWith(skippedTypeName));

        AssertArchTestResult(types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreSealed()
    {
        ConditionList? types = Types
            .InAssemblies(Assemblies.ToArray())
            .Should()
            .NotBeStatic()
            .And()
            .NotBeAbstract()
            .And()
            .NotBeSealed();

        types = NotSealed.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));

        AssertArchTestResult(types);
    }

    [Test]
    public void Should_Passed_When_AllNotSealedAreAbstract()
    {
        ConditionList? types = Types.InAssemblies(Assemblies.ToArray()).Should().NotBeSealed().And().NotBeAbstract();

        types = NotAbstract.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));

        AssertArchTestResult(types);
    }
}