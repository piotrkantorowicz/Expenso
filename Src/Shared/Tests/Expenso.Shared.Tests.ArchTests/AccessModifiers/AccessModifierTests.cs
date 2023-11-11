namespace Expenso.Shared.Tests.ArchTests.AccessModifiers;

internal sealed class AccessModifierTests : ArchTestTestBase
{
    private static readonly string[] NotInternal = { "TestBase" };
    private static readonly string[] NotSealed = { "TestBase", "Program" };
    private static readonly string[] NotAbstract = { "Program" };

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternalInTestAssemblies()
    {
        ConditionList? types = NetArchTypes
            .InAssemblies(Assemblies.ToArray().Where(x => x.FullName?.Contains("Tests") == true)).Should().BePublic();
        types = NotInternal.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));
        AssertArchTestResult(types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = NetArchTypes.InAssemblies(Assemblies.ToArray()).Should().BeClasses().And().NotBeStatic()
            .And().NotBeAbstract().And().NotBeSealed();
        types = NotSealed.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));
        AssertArchTestResult(types);
    }

    [Test]
    public void Should_Passed_When_AllNotSealedClassesAreAbstract()
    {
        ConditionList? types = NetArchTypes.InAssemblies(Assemblies.ToArray()).Should().NotBeSealed().And()
            .NotBeAbstract();
        types = NotAbstract.Aggregate(types,
            (current, skippedTypeName) => current.And().NotHaveNameMatching(skippedTypeName));

        AssertArchTestResult(types);
    }
}