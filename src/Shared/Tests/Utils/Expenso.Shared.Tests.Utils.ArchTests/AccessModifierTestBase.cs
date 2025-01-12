using System.Reflection;

using NetArchTest.Rules;

using NUnit.Framework;

namespace Expenso.Shared.Tests.Utils.ArchTests;

public abstract class AccessModifierTestBase : TestBase
{
    private readonly string[] _notInternalTypes;
    private readonly string[] _notSealedTypes;
    private readonly string[] _notAbstractTypes;
    private readonly string[]? _publicNamespaces;
    private readonly string[]? _excludedNamespaces;

    protected AccessModifierTestBase(string[] notInternalTypes, string[] notSealedTypes, string[] notAbstractTypes,
        string[]? publicNamespaces = null, string[]? excludedNamespaces = null)
    {
        _notInternalTypes = notInternalTypes;
        _notSealedTypes = notSealedTypes;
        _notAbstractTypes = notAbstractTypes;
        _publicNamespaces = publicNamespaces;
        _excludedNamespaces = excludedNamespaces;
    }

    protected abstract IReadOnlyCollection<Assembly> TestClassesAssemblies { get; }

    [Test]
    public void Should_Passed_When_AllExpectedTypesAreInternal()
    {
        ConditionList? types =
            Types.InAssemblies(assemblies: TestClassesAssemblies).Should().BeClasses().And().BePublic();

        types = _notInternalTypes.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        if (_publicNamespaces is not null or [])
        {
            types = _publicNamespaces?.Aggregate(seed: types,
                func: (current, skippedTypeName) =>
                    current.And().NotResideInNamespaceContaining(name: skippedTypeName));
        }

        if (_excludedNamespaces is not null or [])
        {
            types = _excludedNamespaces?.Aggregate(seed: types,
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

        types = _notSealedTypes.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        if (_excludedNamespaces is not null or [])
        {
            types = _excludedNamespaces?.Aggregate(seed: types,
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

        types = _notAbstractTypes.Aggregate(seed: types,
            func: (current, skippedTypeName) => current.And().NotHaveNameMatching(pattern: skippedTypeName));

        if (_excludedNamespaces is not null or [])
        {
            types = _excludedNamespaces?.Aggregate(seed: types,
                func: (current, skippedNamespace) => current.And().NotResideInNamespace(name: skippedNamespace));
        }

        AssertFailingTypes(result: types);
    }
}