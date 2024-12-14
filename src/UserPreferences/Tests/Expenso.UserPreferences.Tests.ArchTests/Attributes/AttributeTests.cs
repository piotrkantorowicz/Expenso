using Expenso.Shared.Tests.Utils.ArchTests;

namespace Expenso.UserPreferences.Tests.ArchTests.Attributes;

[TestFixture]
internal sealed class AttributeTests : AttributesTestBase
{
    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}