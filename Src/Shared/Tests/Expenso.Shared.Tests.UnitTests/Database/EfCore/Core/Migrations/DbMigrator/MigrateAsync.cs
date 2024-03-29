using System.Reflection;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Core.Migrations.DbMigrator;

internal sealed class MigrateAsync : DbMigratorTestBase
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
        await TestCandidate.MigrateAsync(_serviceScopeMock.Object, assemblies,
            It.IsAny<CancellationToken>());

        // Assert
        _testDbContextMigrateMock.Verify(x => x.MigrateAsync(It.IsAny<CancellationToken>()), Times.Once);
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
        await TestCandidate.MigrateAsync(_serviceScopeMock.Object, assemblies,
            It.IsAny<CancellationToken>());

        // Assert
        _testDbContextNoMigrateMock.Verify(x => x.MigrateAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}