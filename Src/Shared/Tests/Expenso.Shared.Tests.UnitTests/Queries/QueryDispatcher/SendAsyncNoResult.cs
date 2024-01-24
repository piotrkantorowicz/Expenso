using Expenso.Shared.Tests.UnitTests.Queries.TestData;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryDispatcher;

internal sealed class QueryAsync : QueryDispatcherTestBase
{
    [Test]
    public async Task Should_SendQuery()
    {
        // Arrange
        TestQuery testQuery = new(Guid.NewGuid());

        // Act
        TestResponse? queryResult = await TestCandidate.QueryAsync(testQuery);

        // Assert
        queryResult?.Should().NotBeNull();
        queryResult?.Id.Should().NotBeEmpty();
        queryResult?.Id.Should().Be(testQuery.Id);
        queryResult?.Name.Should().NotBeNullOrEmpty();
        queryResult?.Name.Should().Be("vWdGYZaiMz9cex");
    }
}
