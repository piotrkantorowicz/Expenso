using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

namespace Expenso.BudgetSharing.Tests.ArchTests.AccessModifiers;

[TestFixture]
internal sealed class AccessModifierTests : AccessModifierTestBase
{
    public AccessModifierTests() : base(notInternal:
        [
            "Module",
            "Extensions"
        ], notSealed:
        [
            "TestBase",
            "Program"
        ], notAbstract:
        [
            "Program"
        ], publicTypes:
        [
            "DTO",
            "Domain",
            "ValueObjects"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}