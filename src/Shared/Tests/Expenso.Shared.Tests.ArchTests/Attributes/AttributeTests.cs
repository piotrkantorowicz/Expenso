using System.Reflection;

using Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;
using Expenso.Shared.Tests.Utils.ArchTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.ArchTests.Attributes;

[TestFixture]
internal sealed class AttributeTests : AttributesTestBase
{
    protected override IReadOnlyCollection<Assembly> TestClassesAssemblies { get; } = AllAssemblies
        .GetAssemblies()
        .Except(second: [typeof(AttributesTestBase).Assembly])
        .ToArray();
}