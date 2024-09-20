using Expenso.Shared.Tests.Utils.ArchTests;

using NetArchTest.Rules;

namespace Expenso.TimeManagement.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : TestBase
{
    private static readonly string[] NotInternal =
    [
        "Module",
        "Extensions",
        "Request",
        "Response",
        "Query",
        "Command",
        "Contract",
        "IntegrationEvent"
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

    private static readonly string[] NamespacesToExclude =
    [
        "Expenso.TimeManagement.Core.Persistence.EfCore.Migrations"
    ];

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternal()
    {
        ConditionList? types =
            Types.InAssemblies(assemblies: Assemblies.GetAssemblies()).Should().BeClasses().And().BePublic();

        types = NotInternal.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        types = NamespacesToExclude.Aggregate(seed: types,
            func: (current, skippedNamespace) => current.And().NotResideInNamespace(name: skippedNamespace));

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

        types = NamespacesToExclude.Aggregate(seed: types,
            func: (current, skippedNamespace) => current.And().NotResideInNamespace(name: skippedNamespace));

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

        types = NamespacesToExclude.Aggregate(seed: types,
            func: (current, skippedNamespace) => current.And().NotResideInNamespace(name: skippedNamespace));

        AssertFailingTypes(result: types);
    }
}