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
            .Setup(expression: x => x.ServiceProvider.GetService(typeof(TestDbContextMigrate)))
            .Returns(value: _testDbContextMigrateMock.Object);

        // Act
        await TestCandidate.SeedAsync(scope: _serviceScopeMock.Object, assemblies: assemblies,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _testDbContextMigrateMock.Verify(expression: x => x.SeedAsync(It.IsAny<CancellationToken>()),
            times: Times.Once);
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
            .Setup(expression: x => x.ServiceProvider.GetService(typeof(TestDbContextNoSeed)))
            .Returns(value: _testDbContextNoSeedMock.Object);

        // Act
        await TestCandidate.SeedAsync(scope: _serviceScopeMock.Object, assemblies: assemblies,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _testDbContextNoMigrateMock.Verify(expression: x => x.SeedAsync(It.IsAny<CancellationToken>()),
            times: Times.Never);
    }
}