using Expenso.Shared.Tests.ArchTests.Utils;

namespace Expenso.IAM.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : TestBase
{
    private static readonly string[] NotInternal = { "Module", "Registration", "Dto" };
    private static readonly string[] NotSealed = { "TestBase", "Program" };
    private static readonly string[] NotAbstract = { "Program" };

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternal()
    {
        ConditionList? types = Types.InAssemblies(Assemblies.ToArray()).Should().BeClasses().And().BePublic();
        types = NotInternal.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));

        AssertArchTestResult(types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = Types.InAssemblies(Assemblies.ToArray()).Should().BeClasses().And().NotBeStatic().And()
            .NotBeAbstract().And().NotBeSealed();
        types = NotSealed.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));
        AssertArchTestResult(types);
    }

    [Test]
    public void Should_Passed_When_AllNotSealedClassesAreAbstract()
    {
        ConditionList? types = Types.InAssemblies(Assemblies.ToArray()).Should().NotBeSealed().And().NotBeAbstract();
        types = NotAbstract.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));

        AssertArchTestResult(types);
    }
}