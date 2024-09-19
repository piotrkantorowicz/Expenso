using System.Reflection;

using Expenso.DocumentManagement.Api;
using Expenso.DocumentManagement.Core.Application.Shared.Services;
using Expenso.DocumentManagement.Proxy;
using Expenso.DocumentManagement.Tests.ArchTests.AccessModifiers;
using Expenso.DocumentManagement.Tests.UnitTests.Application.Shared.FileStorage;

namespace Expenso.DocumentManagement.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(DocumentManagementModule).Assembly;
    private static readonly Assembly Core = typeof(IFileStorage).Assembly;
    private static readonly Assembly Proxy = typeof(IDocumentManagementProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(FileStorageTestBase).Assembly;
    private static readonly Assembly ArchTests = typeof(AccessModifierTests).Assembly;

    public static IReadOnlyCollection<Assembly> GetAssemblies()
    {
        List<Assembly> assemblies =
        [
            Api,
            Core,
            Proxy,
            UnitTests,
            ArchTests
        ];

        return assemblies;
    }
}