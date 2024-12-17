using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.ArchTests.Attributes;

[TestFixture]
internal sealed class AttributeTests : AttributesTestBase
{
    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}