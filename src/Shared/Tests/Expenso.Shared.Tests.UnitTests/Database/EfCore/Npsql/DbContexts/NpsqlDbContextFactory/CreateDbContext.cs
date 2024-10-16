namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.DbContexts.NpsqlDbContextFactory;

[TestFixture]
internal sealed class CreateDbContext : NpsqlDbContextFactoryTestBase
{
    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "Path not exists not this machine")]
    public async Task Should_ThrowArgumentException_When_PathIsNotValid(string projectPath)
    {
        // Arrange
        // Act
        Func<Task> action = async () =>
            await Task.FromResult(result: TestCandidate.CreateDbContext(args: [projectPath]));

        // Assert
        await action
            .Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage(
                expectedWildcardPattern:
                $"Startup project path parameter must be provided and must exists on current machine. Actual value: {projectPath}.");
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