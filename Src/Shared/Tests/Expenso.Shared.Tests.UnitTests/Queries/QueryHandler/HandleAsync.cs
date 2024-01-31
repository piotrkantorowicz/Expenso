using Expenso.Shared.Tests.UnitTests.Queries.TestData;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandler;

internal sealed class HandleAsync : QueryHandlerResultTestBase
{
    [Test]
    public async Task Should_HandleCommand()
    {
        // Arrange
        // Act
        TestResponse? queryResult = await TestCandidate.HandleAsync(_testQuery);

        // Assert
        queryResult?.Should().NotBeNull();
        queryResult?.Id.Should().NotBeEmpty();
        queryResult?.Id.Should().Be(_testQuery.Id);
        queryResult?.Name.Should().NotBeNullOrEmpty();
        queryResult?.Name.Should().Be("vWdGYZaiMz9cex");
    }
}