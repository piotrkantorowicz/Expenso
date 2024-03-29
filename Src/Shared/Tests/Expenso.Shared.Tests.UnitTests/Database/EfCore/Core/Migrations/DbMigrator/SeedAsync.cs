using System.Reflection;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Core.Migrations.DbMigrator;

internal sealed class SeedAsync : DbMigratorTestBase
{
    [Test]
    public async Task Should_RunSeeding_When_DbContextIsSeedable()
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
        await TestCandidate.SeedAsync(_serviceScopeMock.Object, assemblies, It.IsAny<CancellationToken>());

        // Assert
        _testDbContextMigrateMock.Verify(x => x.SeedAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Should_notRunSeeding_When_DbContextIsNotSeedable()
    {
        // Arrange
        List<Assembly> assemblies = new()
        {
            typeof(TestDbContextMigrate).Assembly
        };

        _serviceScopeMock
            .Setup(x => x.ServiceProvider.GetService(typeof(TestDbContextNoSeed)))
            .Returns(_testDbContextNoSeedMock.Object);

        // Act
        await TestCandidate.SeedAsync(_serviceScopeMock.Object, assemblies, It.IsAny<CancellationToken>());

        // Assert
        _testDbContextNoMigrateMock.Verify(x => x.SeedAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}