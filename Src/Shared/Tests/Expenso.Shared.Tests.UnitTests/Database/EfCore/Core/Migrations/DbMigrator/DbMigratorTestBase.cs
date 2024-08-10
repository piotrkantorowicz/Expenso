using Expenso.Shared.Database.EfCore.DbContexts;
using Expenso.Shared.Database.EfCore.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using TestCandidate = Expenso.Shared.Database.EfCore.Migrations.DbMigrator;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Core.Migrations.DbMigrator;

internal abstract class DbMigratorTestBase : TestBase<TestCandidate>
{
    protected Mock<IServiceScope> _serviceScopeMock = null!;
    protected Mock<ITestDbContextMigrate> _testDbContextMigrateMock = null!;
    protected Mock<ITestDbContextNoMigrate> _testDbContextNoMigrateMock = null!;
    protected Mock<ITestDbContextNoSeed> _testDbContextNoSeedMock = null!;

    [SetUp]
    public void Setup()
    {
        _serviceScopeMock = new Mock<IServiceScope>();
        _testDbContextMigrateMock = new Mock<ITestDbContextMigrate>();
        _testDbContextNoMigrateMock = new Mock<ITestDbContextNoMigrate>();
        _testDbContextNoSeedMock = new Mock<ITestDbContextNoSeed>();
        TestCandidate = new TestCandidate();
    }
}

internal interface ITestDbContextMigrate : IDbContext;

internal interface ITestDbContextNoMigrate : IDbContext, IDoNotMigrate;

internal interface ITestDbContextNoSeed : IDbContext, IDoNotSeed;

internal sealed class TestDbContextMigrate(DbContextOptions<TestDbContextMigrate> options)
    : DbContext(options), ITestDbContextMigrate
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

internal sealed class TestDbContextNoMigrate(DbContextOptions<TestDbContextNoMigrate> options)
    : DbContext(options), ITestDbContextNoMigrate
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

internal sealed class TestDbContextNoSeed(DbContextOptions<TestDbContextNoSeed> options)
    : DbContext(options), ITestDbContextNoSeed
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