using Expenso.Shared.Tests.ArchTests.Utils;

namespace Expenso.Api.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : TestBase
{
    private static readonly string[] NotSealed = { "TestBase", "Program" };
    private static readonly string[] NotAbstract = { "Program" };

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternal()
    {
        IEnumerable<Type>? types = Types.InAssemblies(Assemblies.ToArray()).Should().BePublic().GetTypes();

        AssertFailingTypes(types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = Types.InAssemblies(Assemblies.ToArray()).Should().BeClasses().And().NotBeStatic().And()
            .NotBeAbstract().And().NotBeSealed();
        types = NotSealed.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));
        AssertFailingTypes(types.GetTypes());
    }

    [Test]
    public void Should_Passed_When_AllNotSealedClassesAreAbstract()
    {
        ConditionList? types = Types.InAssemblies(Assemblies.ToArray()).Should().NotBeSealed().And().NotBeAbstract();
        types = NotAbstract.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));

        AssertFailingTypes(types.GetTypes());
    }
}