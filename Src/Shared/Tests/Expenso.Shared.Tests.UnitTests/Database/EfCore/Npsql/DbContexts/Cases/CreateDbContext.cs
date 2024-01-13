namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.DbContexts.Cases;

internal sealed class CreateDbContext : NpsqlDbContextFactoryTestBase
{
    [Test]
    public void Should_CreateDbContext()
    {
        // Arrange
        
        
        // Act
        var dbContext = TestCandidate.CreateDbContext([ "path" ]);

        // Assert
    }
}