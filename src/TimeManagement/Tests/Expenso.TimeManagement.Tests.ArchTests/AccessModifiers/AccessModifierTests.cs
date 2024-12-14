using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

namespace Expenso.TimeManagement.Tests.ArchTests.AccessModifiers;

[TestFixture]
internal sealed class AccessModifierTests : AccessModifierTestBase
{
    public AccessModifierTests() : base(notInternal:
        [
            "Module",
            "Extensions",
            "Request",
            "Response",
            "Query",
            "Command",
            "IntegrationEvent",
            "AllowedEvents"
        ], notSealed:
        [
            "TestBase",
            "Program"
        ], notAbstract:
        [
            "Program"
        ], namespacesToExclude:
        [
            "Expenso.TimeManagement.Core.Persistence.EfCore.Migrations",
            "Expenso.TimeManagement.Core.Application.Shared.Settings"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}