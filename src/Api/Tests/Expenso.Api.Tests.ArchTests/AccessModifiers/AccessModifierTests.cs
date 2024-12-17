using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.Api.Tests.ArchTests.AccessModifiers;

[TestFixture]
internal sealed class AccessModifierTests : AccessModifierTestBase
{
    public AccessModifierTests() : base(notInternal:
        [
            "Exception"
        ], notSealed:
        [
            "TestBase",
            "Program"
        ], notAbstract:
        [
            "Program"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}