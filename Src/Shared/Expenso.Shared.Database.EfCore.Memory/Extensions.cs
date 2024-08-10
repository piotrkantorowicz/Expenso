using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.Memory;

public static class Extensions
{
    public static void AddMemoryDatabase<T>(this IServiceCollection services, string moduleName) where T : DbContext
    {
        services.AddDbContext<T>(optionsAction: x => x.UseInMemoryDatabase(databaseName: moduleName));
    }
}