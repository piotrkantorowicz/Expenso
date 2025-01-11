using System.Reflection;

using Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;
using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.ArchTests.AccessModifiers;

[TestFixture]
internal sealed class AccessModifierTests : AccessModifierTestBase
{
    public AccessModifierTests() : base(notInternalTypes:
        [
            "TestBase",
            "InMemoryFakeLogger",
            "Assertions",
            "Extensions",
            "MessageContext",
            "OrExpression",
            "AndExpression",
            "NpsqlDbContextFactory",
            "Paged",
            "Paging",
            "Clock",
            "ProviderTimeZoneResult",
            "RequestTimeZone"
        ], notSealedTypes:
        [
            "TestBase",
            "Program",
            "Exception",
            "RichTestObject"
        ], notAbstractTypes:
        [
            "Program",
            "Exception",
            "RichTestObject"
        ], publicNamespaces:
        [
            "DTO",
            "Settings",
            "Validators",
            "Exceptions",
            "Constants",
            "Domain",
            "Modules",
            "Helpers",
            "Converters"
        ])
    {
    }

    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = AllAssemblies.GetAssemblies();
}