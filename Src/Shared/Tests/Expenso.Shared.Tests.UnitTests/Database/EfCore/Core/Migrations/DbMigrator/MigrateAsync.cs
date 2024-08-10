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
            .Setup(expression: x => x.ServiceProvider.GetService(typeof(TestDbContextMigrate)))
            .Returns(value: _testDbContextMigrateMock.Object);

        // Act
        await TestCandidate.MigrateAsync(scope: _serviceScopeMock.Object, assemblies: assemblies,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _testDbContextMigrateMock.Verify(expression: x => x.MigrateAsync(It.IsAny<CancellationToken>()),
            times: Times.Once);
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
            .Setup(expression: x => x.ServiceProvider.GetService(typeof(TestDbContextNoMigrate)))
            .Returns(value: _testDbContextNoMigrateMock.Object);

        // Act
        await TestCandidate.MigrateAsync(scope: _serviceScopeMock.Object, assemblies: assemblies,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _testDbContextNoMigrateMock.Verify(expression: x => x.MigrateAsync(It.IsAny<CancellationToken>()),
            times: Times.Never);
    }
}