using System.Reflection;

using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.ArchTests.Attributes;

[TestFixture]
internal sealed class AttributeTests : AttributesTestBase
{
    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = Assemblies.GetAssemblies();
}