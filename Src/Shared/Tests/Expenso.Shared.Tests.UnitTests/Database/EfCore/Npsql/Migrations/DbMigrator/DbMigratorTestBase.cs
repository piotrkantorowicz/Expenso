using Expenso.Shared.Database.EfCore.NpSql.DbContexts;
using Expenso.Shared.Database.EfCore.NpSql.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using TestCandidate = Expenso.Shared.Database.EfCore.NpSql.Migrations.DbMigrator;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.Migrations.DbMigrator;

internal abstract class DbMigratorTestBase : TestBase<TestCandidate>
{
    protected Mock<IServiceScope> _serviceScopeMock = null!;
    protected Mock<ITestDbContextMigrate> _testDbContextMigrateMock = null!;
    protected Mock<ITestDbContextNoMigrate> _testDbContextNoMigrateMock = null!;

    [SetUp]
    public void Setup()
    {
        _serviceScopeMock = new Mock<IServiceScope>();
        _testDbContextMigrateMock = new Mock<ITestDbContextMigrate>();
        _testDbContextNoMigrateMock = new Mock<ITestDbContextNoMigrate>();
        TestCandidate = new TestCandidate();
    }
}

internal interface ITestDbContextMigrate : IDbContext;

internal interface ITestDbContextNoMigrate : IDbContext, IDoNotMigrate;

internal sealed class TestDbContextMigrate(DbContextOptions<TestDbContextMigrate> options)
    : DbContext(options), ITestDbContextMigrate
{
    public Task MigrateAsync()
    {
        return Database.MigrateAsync();
    }
}

internal sealed class TestDbContextNoMigrate(DbContextOptions<TestDbContextNoMigrate> options)
    : DbContext(options), ITestDbContextNoMigrate
{
    public Task MigrateAsync()
    {
        return Database.MigrateAsync();
    }
}