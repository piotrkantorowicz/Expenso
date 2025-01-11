using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.ArchTests.AccessModifiers;

[TestFixture]
internal sealed class AccessModifierTests : AccessModifierTestBase
{
    public AccessModifierTests() : base(notInternalTypes:
        [
            "Module",
            "Extensions",
            "Request",
            "Response",
            "Query",
            "Command",
            "IntegrationEvent",
            "AllowedEvents"
        ], notSealedTypes:
        [
            "TestBase",
            "Program"
        ], notAbstractTypes:
        [
            "Program"
        ], excludedNamespaces:
        [
            "Expenso.TimeManagement.Core.Persistence.EfCore.Migrations",
            "Expenso.TimeManagement.Core.Application.Shared.Settings"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}