using System.Reflection;

using NetArchTest.Rules;

using NUnit.Framework;

namespace Expenso.Shared.Tests.Utils.ArchTests;

public abstract class AccessModifierTestBase : TestBase
{
    private readonly string[] _notInternal;
    private readonly string[] _notSealed;
    private readonly string[] _notAbstract;
    private readonly string[]? _publicTypes;
    private readonly string[]? _namespacesToExclude;

    protected AccessModifierTestBase(string[] notInternal, string[] notSealed, string[] notAbstract,
        string[]? publicTypes = null, string[]? namespacesToExclude = null)
    {
        _notInternal = notInternal;
        _notSealed = notSealed;
        _notAbstract = notAbstract;
        _publicTypes = publicTypes;
        _namespacesToExclude = namespacesToExclude;
    }

    protected abstract IReadOnlyCollection<Assembly> TestClassesAssemblies { get; }

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternal()
    {
        ConditionList? types =
            Types.InAssemblies(assemblies: TestClassesAssemblies).Should().BeClasses().And().BePublic();

        types = _notInternal.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        if (_publicTypes is not null or [])
        {
            types = _publicTypes?.Aggregate(seed: types,
                func: (current, skippedTypeName) =>
                    current.And().NotResideInNamespaceContaining(name: skippedTypeName));
        }

        if (_namespacesToExclude is not null or [])
        {
            types = _namespacesToExclude?.Aggregate(seed: types,
                func: (current, skippedNamespace) => current?.And().NotResideInNamespace(name: skippedNamespace));
        }

        AssertFailingTypes(result: types);
    }

    [Test]
    public void Should_Passed_When_AllExpectedClassesAreSealed()
    {
        ConditionList? types = Types
            .InAssemblies(assemblies: TestClassesAssemblies)
            .Should()
            .BeClasses()
            .And()
            .NotBeStatic()
            .And()
            .NotBeAbstract()
            .And()
            .NotBeSealed();

        types = _notSealed.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        if (_namespacesToExclude is not null or [])
        {
            types = _namespacesToExclude?.Aggregate(seed: types,
                func: (current, skippedNamespace) => current.And().NotResideInNamespace(name: skippedNamespace));
        }

        AssertFailingTypes(result: types);
    }

    [Test]
    public void Should_Passed_When_AllNotSealedClassesAreAbstract()
    {
        ConditionList? types = Types
            .InAssemblies(assemblies: TestClassesAssemblies)
            .Should()
            .NotBeSealed()
            .And()
            .NotBeAbstract();

        types = _notAbstract.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        if (_namespacesToExclude is not null or [])
        {
            types = _namespacesToExclude?.Aggregate(seed: types,
                func: (current, skippedNamespace) => current.And().NotResideInNamespace(name: skippedNamespace));
        }

        AssertFailingTypes(result: types);
    }
}