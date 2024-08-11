namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.DbContexts.NpsqlDbContextFactory;

internal sealed class CreateDbContext : NpsqlDbContextFactoryTestBase
{
    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "Path not exists not this machine")]
    public void Should_ThrowArgumentException_When_PathIsNotValid(string projectPath)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException? exception =
            Assert.Throws<ArgumentException>(code: () => TestCandidate.CreateDbContext(args: [projectPath]));

        string expectedExceptionMessage =
            $"Startup project path parameter must be provided and must exists on current machine. Actual value: {projectPath}";

        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }

    [Test]
    public void Should_Passed_When_CreateDbContext()
    {
        // Arrange
        string relativePath = Path.Combine(paths:
        [
            "..",
            "..",
            "..",
            "..",
            "..",
            "..",
            "..",
            "src",
            "Api",
            "Expenso.Api"
        ]);

        string startupProjectPath =
            Path.GetFullPath(path: Path.Combine(path1: Environment.CurrentDirectory, path2: relativePath));

        // Act
        TestDbContext dbContext = TestCandidate.CreateDbContext(args: [startupProjectPath]);

        // Assert
        dbContext.Should().NotBeNull();
    }
}