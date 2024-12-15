using System.Reflection;

using Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;
using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.ArchTests.AccessModifiers;

[TestFixture]
internal sealed class AccessModifierTests : AccessModifierTestBase
{
    public AccessModifierTests() : base(notInternal:
        [
            "TestBase",
            "InMemoryFakeLogger",
            "Assertions",
            "Extensions"
        ], notSealed:
        [
            "TestBase",
            "Program",
            "Exception",
            "RichTestObject"
        ], notAbstract:
        [
            "Program",
            "Exception",
            "RichTestObject"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = AllAssemblies.GetAssemblies();
}