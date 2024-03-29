namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.DbContexts.NpsqlDbContextFactory;

internal sealed class CreateDbContext : NpsqlDbContextFactoryTestBase
{
    [Test, TestCase(null), TestCase(""), TestCase("Path not exists not this machine")]
    public void Should_ThrowArgumentException_When_PathIsNotValid(string projectPath)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException? exception =
            Assert.Throws<ArgumentException>(() => TestCandidate.CreateDbContext([projectPath]));

        string expectedExceptionMessage =
            $"Startup project path parameter must be provided and must exists on current machine. Actual value: {projectPath}";

        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_Passed_When_CreateDbContext()
    {
        // Arrange
        string relativePath = Path.Combine([
            "..",
            "..",
            "..",
            "..",
            "..",
            "..",
            "..",
            "Src",
            "Api",
            "Expenso.Api"
        ]);

        string startupProjectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, relativePath));

        // Act
        TestDbContext dbContext = TestCandidate.CreateDbContext([startupProjectPath]);

        // Assert
        dbContext.Should().NotBeNull();
    }
}