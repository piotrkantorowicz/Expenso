using Expenso.Shared.Database.EfCore.DbContexts;
using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.DbContexts.NpsqlDbContextFactory;

internal abstract class NpsqlDbContextFactoryTestBase : TestBase<NpsqlDbContextFactory<TestDbContext>>
{
    [SetUp]
    public void Setup()
    {
        TestCandidate = new TestDbContextFactory();
    }
}

internal sealed class TestDbContextFactory : NpsqlDbContextFactory<TestDbContext>;

internal interface ITestDbContext : IDbContext;

internal sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options), ITestDbContext
{
    public Task MigrateAsync(CancellationToken cancellationToken)
    {
        return Database.MigrateAsync(cancellationToken);
    }

    public Task SeedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}