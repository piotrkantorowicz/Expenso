using Expenso.Shared.Database;
using Expenso.Shared.Database.EfCore.Npsql.DbContexts;
using Expenso.Shared.Database.EfCore.Settings;

using DatabaseExtensions = Expenso.Shared.Database.EfCore.Memory.Extensions;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class DatabaseAssemblies
{
    private static readonly Assembly Database = typeof(IUnitOfWork).Assembly;
    private static readonly Assembly DatabaseEfCore = typeof(EfCoreSettings).Assembly;
    private static readonly Assembly DatabaseEfCoreMemory = typeof(DatabaseExtensions).Assembly;
    private static readonly Assembly DatabaseEfCoreNpSql = typeof(NpsqlDbContextFactory<>).Assembly;

    private static readonly Dictionary<string, Assembly> Assemblies = new()
    {
        [key: nameof(Database)] = typeof(IUnitOfWork).Assembly,
        [key: nameof(DatabaseEfCore)] = typeof(EfCoreSettings).Assembly,
        [key: nameof(DatabaseEfCoreMemory)] = typeof(DatabaseExtensions).Assembly,
        [key: nameof(DatabaseEfCoreNpSql)] = typeof(NpsqlDbContextFactory<>).Assembly
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}
