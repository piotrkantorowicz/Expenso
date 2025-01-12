using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.DocumentManagement.Tests.ArchTests.AccessModifiers;

[TestFixture]
internal sealed class AccessModifierTests : AccessModifierTestBase
{
    public AccessModifierTests() : base(notInternalTypes:
        [
            "Module",
            "Extensions"
        ], notSealedTypes:
        [
            "TestBase",
            "Program"
        ], notAbstractTypes:
        [
            "Program"
        ], publicNamespaces:
        [
            "DTO"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}