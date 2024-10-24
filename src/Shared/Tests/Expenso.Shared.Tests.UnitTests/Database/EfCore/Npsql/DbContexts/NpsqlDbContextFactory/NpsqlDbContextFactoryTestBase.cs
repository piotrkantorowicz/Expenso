using Expenso.Shared.Database.EfCore.DbContexts;
using Expenso.Shared.Database.EfCore.Npsql.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.DbContexts.NpsqlDbContextFactory;

[TestFixture]
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

internal sealed class TestDbContext : DbContext, ITestDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options: options)
    {
    }

    public Task MigrateAsync(CancellationToken cancellationToken)
    {
        return Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public Task SeedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}