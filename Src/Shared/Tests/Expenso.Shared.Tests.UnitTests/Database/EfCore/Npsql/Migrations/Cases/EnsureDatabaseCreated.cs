using Moq;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.Migrations.Cases;

internal sealed class EnsureDatabaseCreated : DbMigratorTestBase
{
    [Test]
    public async Task Should_RunMigrations_When_DbContextIsMigratable()
    {
        // Arrange
        _serviceScopeMock
            .Setup(x => x.ServiceProvider.GetService(typeof(TestDbContextMigrate)))
            .Returns(_testDbContextMigrateMock.Object);
        
        // Act
        await TestCandidate.EnsureDatabaseCreatedAsync(_serviceScopeMock.Object);

        // Assert
        _testDbContextMigrateMock.Verify(x => x.MigrateAsync(), Times.Once);
    }
    
    [Test]
    public async Task Should_NotRunMigrations_When_DbContextIsNotMigratable()
    {
        // Arrange
        _serviceScopeMock
            .Setup(x => x.ServiceProvider.GetService(typeof(TestDbContextNoMigrate)))
            .Returns(_testDbContextNoMigrateMock.Object);
        
        // Act
        await TestCandidate.EnsureDatabaseCreatedAsync(_serviceScopeMock.Object);

        // Assert
        _testDbContextNoMigrateMock.Verify(x => x.MigrateAsync(), Times.Never);
    }
}