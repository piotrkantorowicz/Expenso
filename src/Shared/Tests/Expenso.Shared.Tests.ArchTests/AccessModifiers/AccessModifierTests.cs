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
            "Extensions",
            "MessageContext",
            "OrExpression",
            "AndExpression",
            "NpsqlDbContextFactory",
            "Paged",
            "Paging"
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
        ], publicTypes:
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