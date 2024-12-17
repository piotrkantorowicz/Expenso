using Expenso.Shared.Database.EfCore.DbContexts;
using Expenso.Shared.Database.EfCore.Migrations;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Core.Migrations.DbMigrator;

[TestFixture]
internal abstract class DbMigratorTestBase : TestBase<Shared.Database.EfCore.Migrations.DbMigrator>
{
    [SetUp]
    public void Setup()
    {
        _serviceScopeMock = new Mock<IServiceScope>();
        _testDbContextMigrateMock = new Mock<ITestDbContextMigrate>();
        _testDbContextNoMigrateMock = new Mock<ITestDbContextNoMigrate>();
        _testDbContextNoSeedMock = new Mock<ITestDbContextNoSeed>();
        TestCandidate = new Shared.Database.EfCore.Migrations.DbMigrator();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceScopeMock = null!;
        _testDbContextMigrateMock = null!;
        _testDbContextNoMigrateMock = null!;
        _testDbContextNoSeedMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IServiceScope> _serviceScopeMock = null!;
    protected Mock<ITestDbContextMigrate> _testDbContextMigrateMock = null!;
    protected Mock<ITestDbContextNoMigrate> _testDbContextNoMigrateMock = null!;
    protected Mock<ITestDbContextNoSeed> _testDbContextNoSeedMock = null!;
}

internal interface ITestDbContextMigrate : IDbContext;

internal interface ITestDbContextNoMigrate : IDbContext, IDoNotMigrate;

internal interface ITestDbContextNoSeed : IDbContext, IDoNotSeed;

internal sealed class TestDbContextMigrate : DbContext, ITestDbContextMigrate
{
    public TestDbContextMigrate(DbContextOptions<TestDbContextMigrate> options) : base(options: options)
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

internal sealed class TestDbContextNoMigrate : DbContext, ITestDbContextNoMigrate
{
    public TestDbContextNoMigrate(DbContextOptions<TestDbContextNoMigrate> options) : base(options: options)
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

internal sealed class TestDbContextNoSeed : DbContext, ITestDbContextNoSeed
{
    public TestDbContextNoSeed(DbContextOptions<TestDbContextNoSeed> options) : base(options: options)
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