using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.UserPreferences.Tests.ArchTests.AccessModifiers;

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
            "Payload"
        ], notSealedTypes:
        [
            "TestBase",
            "Program"
        ], notAbstractTypes:
        [
            "Program"
        ], excludedNamespaces:
        [
            "Expenso.UserPreferences.Core.Persistence.EfCore.Migrations"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}