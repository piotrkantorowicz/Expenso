using System.Text;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Npsql.DbContexts.Cases;

internal sealed class CreateDbContext : NpsqlDbContextFactoryTestBase
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("Path not exists not this machine")]
    public void Should_ThrowArgumentException_When_PathIsNotValid(string projectPath)
    {
        // Arrange
        // Act
        // Assert
        ArgumentException? exception =
            Assert.Throws<ArgumentException>(() => TestCandidate.CreateDbContext([projectPath]));

        string expectedExceptionMessage = new StringBuilder()
            .Append(
                "Startup project path parameter must be provided and must exists on current machine. Actual value: ")
            .Append(projectPath)
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_Passed_When_CreateDbContext()
    {
        // Arrange
        const string relativePath = @"..\..\..\..\..\..\..\Src\Api\Expenso.Api";
        string startupProjectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, relativePath));
        
        // Act
        TestDbContext dbContext = TestCandidate.CreateDbContext([startupProjectPath]);

        // Assert
        dbContext.Should().NotBeNull();
    }
}