using System.Reflection;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.Migrations.DbMigrator;

internal sealed class EnsureDatabaseCreated : DbMigratorTestBase
{
    [Test]
    public async Task Should_RunMigrations_When_DbContextIsMigratable()
    {
        // Arrange
        List<Assembly> assemblies = new()
        {
            typeof(TestDbContextMigrate).Assembly
        };

        _serviceScopeMock
            .Setup(x => x.ServiceProvider.GetService(typeof(TestDbContextMigrate)))
            .Returns(_testDbContextMigrateMock.Object);

        // Act
        await TestCandidate.EnsureDatabaseCreatedAsync(_serviceScopeMock.Object, assemblies);

        // Assert
        _testDbContextMigrateMock.Verify(x => x.MigrateAsync(), Times.Once);
    }

    [Test]
    public async Task Should_NotRunMigrations_When_DbContextIsNotMigratable()
    {
        // Arrange
        List<Assembly> assemblies = new()
        {
            typeof(TestDbContextMigrate).Assembly
        };

        _serviceScopeMock
            .Setup(x => x.ServiceProvider.GetService(typeof(TestDbContextNoMigrate)))
            .Returns(_testDbContextNoMigrateMock.Object);

        // Act
        await TestCandidate.EnsureDatabaseCreatedAsync(_serviceScopeMock.Object, assemblies);

        // Assert
        _testDbContextNoMigrateMock.Verify(x => x.MigrateAsync(), Times.Never);
    }
}